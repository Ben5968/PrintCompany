using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PrintCompany.Core;
using PrintCompany.Data;
using PrintCompany.ViewModels;

namespace PrintCompany.Controllers
{
    public class CustomersController : Controller
    {
        private readonly PrintCompanyDbContext _context;
        private readonly IMapper _mapper;

        public CustomersController(PrintCompanyDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: Customers
        public async Task<IActionResult> Index()
        {
            //return View(await _context.Customers.ToListAsync());
            return View();
        }

        // GET: Customers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Customers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CustomerViewModel customerViewModel)
        {
            if (ModelState.IsValid)
            {
                var customer = _mapper.Map<Customer>(customerViewModel);

                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Customers");
            }
            return View(customerViewModel);
        }

        // GET: Customers/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var customer = await _context.Customers.FindAsync(id);           
            var customer = _context.Customers.Find(id);

            if (customer == null)
            {
                return NotFound();
            }

            var customerViewModel = _mapper.Map<CustomerViewModel>(customer);

            return View(customerViewModel);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CustomerViewModel customerViewModel)
        {
            if (id != customerViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var customer = _context.Customers.SingleOrDefault(e => e.Id == id);

                _mapper.Map(customerViewModel, customer);

                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customerViewModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Customers");
            }
            return View(customerViewModel);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Customers");
        }

        public JsonResult GetCustomerList(string searchTerm)
        {

            var customerList = _context.Customers.OrderBy(m => m.Name).ToList();

            if (searchTerm != null)
            {
                customerList = _context.Customers.Where(c => c.Name.Contains(searchTerm)).OrderBy(m => m.Name).ToList();
            }

            var modifiedData = customerList.Select(x => new
            {
                id = x.Id,
                text = x.Name
            });

            return Json(modifiedData);
        }

        public JsonResult GetCustomer(int id)
        {

            var customer = _context.Customers.SingleOrDefault(i => i.Id == id);

            var modifiedData = new {
                id = customer.Id,
                text = customer.Name
            };

            return Json(modifiedData);
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.Id == id);
        }

        [HttpPost]
        public async Task<IActionResult> LoadTable([FromBody]DTParameters dtParameters)
        {
            var searchBy = dtParameters.Search?.Value;

            var orderCriteria = string.Empty;
            var orderAscendingDirection = true;

            if (dtParameters.Order != null)
            {
                // in this example we just default sort on the 1st column
                orderCriteria = dtParameters.Columns[dtParameters.Order[0].Column].Data;
                orderAscendingDirection = dtParameters.Order[0].Dir.ToString().ToLower() == "asc";
            }
            else
            {
                // if we have an empty search then just order the results by Id ascending
                orderCriteria = "Name";
                orderAscendingDirection = true;
            }

            var result = await _context.Customers.ToListAsync();

            if (!string.IsNullOrEmpty(searchBy))
            {
                result = result.Where(r => r.Name != null && r.Name.ToUpper().Contains(searchBy.ToUpper()) ||
                                           r.BillingAddress != null && r.BillingAddress.ToUpper().Contains(searchBy.ToUpper()) ||
                                           r.MainContact != null && r.MainContact.ToUpper().Contains(searchBy.ToUpper()) ||
                                           r.ContactPhone != null && r.ContactPhone.ToUpper().Contains(searchBy.ToUpper()) ||
                                           r.ContactEmail != null && r.ContactEmail.ToUpper().Contains(searchBy.ToUpper()))
                    .ToList();
            }




            // now just get the count of items (without the skip and take) - eg how many could be returned with filtering
            var filteredResultsCount = result.Count();
            var totalResultsCount = await _context.Customers.CountAsync();


            return Json(new
            {
                draw = dtParameters.Draw,
                recordsTotal = totalResultsCount,
                recordsFiltered = filteredResultsCount,
                data = result
                      .Skip(dtParameters.Start)
                      .Take(dtParameters.Length)
                      .ToList()
            });
        }
    }
}

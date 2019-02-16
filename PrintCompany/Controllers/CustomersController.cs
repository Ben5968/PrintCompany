using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public CustomersController(PrintCompanyDbContext context)
        {
            _context = context;
        }

        // GET: Customers
        public async Task<IActionResult> Index()
        {
            return View(await _context.Customers.ToListAsync());
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
                Customer customer = new Customer
                {
                    Id = customerViewModel.Id,
                    Name = customerViewModel.Name,
                    BillingAddress = customerViewModel.BillingAddress,
                    ShippingAddress = customerViewModel.ShippingAddress,
                    MainContact = customerViewModel.MainContact
                };               

                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Customers");
            }
            return View(customerViewModel);
        }

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.FindAsync(id);           

            if (customer == null)
            {
                return NotFound();
            }

            CustomerViewModel customerViewModel = new CustomerViewModel
            {
                Id = customer.Id,
                Name = customer.Name,
                BillingAddress = customer.BillingAddress,
                ShippingAddress = customer.ShippingAddress,
                MainContact = customer.MainContact
            };

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
                customer.BillingAddress = customerViewModel.BillingAddress;
                customer.MainContact = customerViewModel.MainContact;
                customer.Name = customerViewModel.Name;
                customer.ShippingAddress = customerViewModel.ShippingAddress;

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

            var customerList = _context.Customers.ToList();

            if(searchTerm != null)
            {
                customerList = _context.Customers.Where(c => c.Name.Contains(searchTerm)).ToList();
            }

            var modifiedData = customerList.Select(x => new
            {
                id = x.Id,
                text = x.Name
            });

            return Json(modifiedData);
        }

        // GET: Customers/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var customer = await _context.Customers
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (customer == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(customer);
        //}

        // POST: Customers/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var customer = await _context.Customers.FindAsync(id);
        //    _context.Customers.Remove(customer);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.Id == id);
        }
    }
}

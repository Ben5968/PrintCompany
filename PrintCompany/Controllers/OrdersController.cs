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
    public class OrdersController : Controller
    {
        private readonly PrintCompanyDbContext _context;

        public OrdersController(PrintCompanyDbContext context)
        {
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            List<OrderViewModel> orderViewModel = new List<OrderViewModel>();
            var orders = await _context.Orders.Include("Customer").ToListAsync();

            foreach (var order in orders)
            {
                orderViewModel.Add(
                    new OrderViewModel
                    {
                        Id = order.Id,
                        CustomerId = order.CustomerId,
                        CustomerName = order.Customer.Name,
                        OrderNumber = order.OrderNumber,
                        DueDate = order.DueDate,
                        OrderDate = order.OrderDate,
                        //EmbroideryRequired = order.EmbroideryRequired,
                        //PrintRequired = order.PrintRequired
                    });
            }
            return View(orderViewModel);
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrderViewModel orderViewModel)
        {
            if (ModelState.IsValid)
            {
                Order order = new Order
                {
                    Id = orderViewModel.Id,
                    DueDate = orderViewModel.DueDate,
                    OrderDate = orderViewModel.OrderDate,
                    //EmbroideryRequired = orderViewModel.EmbroideryRequired,
                    //PrintRequired = orderViewModel.PrintRequired,
                    CustomerId = orderViewModel.CustomerId,
                    OrderNumber = orderViewModel.OrderNumber
                };
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction("Edit", "Orders", new { id = order.Id });
            }
            return View(orderViewModel);
        }

        // GET: Orders/Edit/5
        public  IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order =  _context.Orders.Include(x=>x.Customer).SingleOrDefault(i => i.Id == id.Value);
            if (order == null)
            {
                return NotFound();
            }

            OrderViewModel orderViewModel = new OrderViewModel
            {
                CustomerId = order.CustomerId,
                CustomerName = order.Customer.Name,
                DueDate = order.DueDate,
                //EmbroideryRequired = order.EmbroideryRequired,
                Id = order.Id,
                OrderDate = order.OrderDate,
                OrderNumber = order.OrderNumber,
                //PrintRequired = order.PrintRequired
                //OrderLines = order.OrderLines.Where(x => x.OrderId == id).ToList(),
                ItemTypes = GetItemTypes(),
                ItemColors = GetItemColors(),
                ItemSizes = GetItemSizes()
            };

            return View(orderViewModel);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, OrderViewModel orderViewModel)
        {
            if (id != orderViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var order = _context.Orders.Include(x => x.OrderLines).SingleOrDefault(e => e.Id == id);                    
                    order.CustomerId = orderViewModel.CustomerId;
                    order.DueDate = orderViewModel.DueDate;
                    //order.EmbroideryRequired = orderViewModel.EmbroideryRequired;
                    order.OrderDate = orderViewModel.OrderDate;
                    order.OrderNumber = orderViewModel.OrderNumber;
                //order.PrintRequired = orderViewModel.PrintRequired;                
                try
                {                    
                    //_context.Entry(order).State = EntityState.Modified;
                    _context.Update(order);
                    //_context.Orders.Update(order);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(orderViewModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Orders");
            }
            return View(orderViewModel);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Orders");
            {
                //return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var order = await _context.Orders.FindAsync(id);
        //    _context.Orders.Remove(order);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }

        private SelectList GetItemTypes()
        {
            var types = _context.ItemTypes
                        .Select(x =>
                                new SelectListItem
                                {
                                    Value = x.Id.ToString(),
                                    Text = x.Type
                                });
            return new SelectList(types, "Value", "Text");
        }

        private SelectList GetItemSizes()
        {
            var types = _context.ItemSizes
                        .Select(x =>
                                new SelectListItem
                                {
                                    Value = x.Id.ToString(),
                                    Text = x.Size
                                });
            return new SelectList(types, "Value", "Text");
        }

        private SelectList GetItemColors()
        {
            var types = _context.ItemColors
                        .Select(x =>
                                new SelectListItem
                                {
                                    Value = x.Id.ToString(),
                                    Text = x.Color
                                });
            return new SelectList(types, "Value", "Text");
        }

    }
}

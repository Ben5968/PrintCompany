using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PrintCompany.Core;
using PrintCompany.Data;
using PrintCompany.ViewModels;

namespace PrintCompany.Controllers
{
    public class OrderLinesController : Controller
    {

        private readonly PrintCompanyDbContext _context;

        public OrderLinesController(PrintCompanyDbContext context)
        {
            _context = context;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(OrderLineViewModel orderLineViewModel )
        {
            if (ModelState.IsValid)
            {
                OrderLine orderLine = new OrderLine
                {                    
                    EmbroideryRequired = orderLineViewModel.EmbroideryRequired,
                    Id = orderLineViewModel.Id,
                    ItemColorId = orderLineViewModel.ItemColorId,
                    ItemSizeId = orderLineViewModel.ItemSizeId,
                    ItemTypeId = orderLineViewModel.ItemTypeId,
                    OrderId = orderLineViewModel.OrderId,
                    PrintRequired = orderLineViewModel.PrintRequired                    
                };
                _context.OrderLines.Add(orderLine);
                await _context.SaveChangesAsync();
                return RedirectToAction("Edit", "Orders");
            }
            return View(orderLineViewModel);
        }

        public IActionResult ListOrderLinesByOrderId(int id)
        {
            var orderLinesInOrder = _context.OrderLines.Where(x => x.OrderId == id).ToList();

            List<OrderLineViewModel> orderLineViewModels = new List<OrderLineViewModel>();

            foreach (var orderLine in orderLinesInOrder)
            {
                orderLineViewModels.Add(new OrderLineViewModel
                {
                    Id = orderLine.Id,
                    EmbroideryRequired = orderLine.EmbroideryRequired,
                    ItemColorId = orderLine.ItemColorId,
                    ItemSizeId = orderLine.ItemSizeId,
                    ItemTypeId = orderLine.ItemTypeId,
                    OrderId = orderLine.OrderId,
                    PrintRequired = orderLine.PrintRequired,
                    Quantity = orderLine.Quantity
                });
            }

            return View();
        }



        public IActionResult Edit()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Edit(int id)
        {
            return View();
        }
    }
}
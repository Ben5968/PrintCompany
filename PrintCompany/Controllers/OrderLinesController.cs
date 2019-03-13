using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
                    PrintRequired = orderLineViewModel.PrintRequired,
                    ItemType = orderLineViewModel.ItemType,
                    ItemColor = orderLineViewModel.ItemColor,
                    ItemSize = orderLineViewModel.ItemSize,
                    Quantity = orderLineViewModel.Quantity
                };
                _context.OrderLines.Add(orderLine);
                await _context.SaveChangesAsync();
                return RedirectToAction("Edit", "Orders", new { id = orderLineViewModel.OrderId });
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

        public PartialViewResult Edit(int id)
        {
            var orderLine = _context.OrderLines.Include("ItemType").Include("ItemColor").Include("ItemSize").SingleOrDefault(x => x.Id == id);
            OrderLineViewModel orderLineViewModel = new OrderLineViewModel
            {
                EmbroideryRequired = orderLine.EmbroideryRequired,
                Id = orderLine.Id,
                ItemColor = orderLine.ItemColor,
                ItemColorId = orderLine.ItemColorId,
                ItemSize = orderLine.ItemSize,
                ItemSizeId = orderLine.ItemSizeId,
                ItemType = orderLine.ItemType,
                ItemTypeId = orderLine.ItemTypeId,
                OrderId = orderLine.OrderId,
                PrintRequired = orderLine.PrintRequired,
                Quantity = orderLine.Quantity
            };
            return PartialView("_PartialModalOrderLine-Edit", orderLineViewModel);           
        }

        [HttpPost]
        public IActionResult Edit(int id, OrderLineViewModel orderLineViewModel)
        {
            if (id != orderLineViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var orderLine = _context.OrderLines.SingleOrDefault(e => e.Id == id);
                orderLine.EmbroideryRequired = orderLineViewModel.EmbroideryRequired;
                orderLine.ItemColorId = orderLineViewModel.ItemColorId;
                orderLine.ItemSizeId = orderLineViewModel.ItemSizeId;
                orderLine.ItemTypeId = orderLineViewModel.ItemTypeId;
                orderLine.OrderId = orderLineViewModel.OrderId;
                orderLine.PrintRequired = orderLineViewModel.PrintRequired;
                orderLine.Quantity = orderLineViewModel.Quantity;

                try
                {
                    _context.Update(orderLine);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderLineExists(orderLineViewModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return Json("Success");
            }
            return View(orderLineViewModel);
        }

        private bool OrderLineExists(int id)
        {
            return _context.OrderLines.Any(e => e.Id == id);
        }


        public JsonResult GetItemList(string searchTerm)
        {

            var itemList = _context.ItemTypes.ToList();

            if (searchTerm != null)
            {
                itemList = _context.ItemTypes.Where(c => c.Type.Contains(searchTerm)).ToList();
            }

            var modifiedData = itemList.Select(x => new
            {
                id = x.Id,
                text = x.Type
            });

            return Json(modifiedData);
        }

        public JsonResult GetSizeList(string searchTerm)
        {

            var sizeList = _context.ItemSizes.ToList();

            if (searchTerm != null)
            {
                sizeList = _context.ItemSizes.Where(c => c.Size.Contains(searchTerm)).ToList();
            }

            var modifiedData = sizeList.Select(x => new
            {
                id = x.Id,
                text = x.Size
            });

            return Json(modifiedData);
        }

        public JsonResult GetColorList(string searchTerm)
        {

            var colorList = _context.ItemColors.ToList();

            if (searchTerm != null)
            {
                colorList = _context.ItemColors.Where(c => c.Color.Contains(searchTerm)).ToList();
            }

            var modifiedData = colorList.Select(x => new
            {
                id = x.Id,
                text = x.Color
            });

            return Json(modifiedData);
        }

    }
}
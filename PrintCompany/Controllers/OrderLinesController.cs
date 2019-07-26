using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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
        private readonly IMapper _mapper;

        public OrderLinesController(PrintCompanyDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(OrderLineViewModel orderLineViewModel)
        {
            if (ModelState.IsValid)
            {
                var orderLine = _mapper.Map<OrderLine>(orderLineViewModel);
              
                _context.OrderLines.Add(orderLine);
                await _context.SaveChangesAsync();
                return RedirectToAction("Edit", "Orders", new { id = orderLineViewModel.OrderId });
            }
            return View(orderLineViewModel);
        }
        
        public PartialViewResult Edit(int id)
        {
            var orderLine = _context.OrderLines.Include("ItemType").
                Include("ItemColor").Include("ItemSize").
                Include("Supplier").
                SingleOrDefault(x => x.Id == id);

            var orderLineViewModel = _mapper.Map<OrderLineViewModel>(orderLine);
           
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

                _mapper.Map(orderLineViewModel, orderLine);     

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

        public async Task<IActionResult> Delete(int id)
        {
            var file = await _context.OrderLines.FindAsync(id);
            if (file == null)
                return NotFound();
            _context.OrderLines.Remove(file);
            await _context.SaveChangesAsync();
            return Json("Success");
        }

        private bool OrderLineExists(int id)
        {
            return _context.OrderLines.Any(e => e.Id == id);
        }


        public JsonResult GetItemList(string searchTerm)
        {

            var itemList = _context.ItemTypes.OrderBy(m=>m.Type).ToList();

            if (searchTerm != null)
            {
                itemList = _context.ItemTypes.Where(c => c.Type.Contains(searchTerm)).OrderBy(m => m.Type).ToList();
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

            var sizeList = _context.ItemSizes.OrderBy(m => m.Size).ToList();

            if (searchTerm != null)
            {
                sizeList = _context.ItemSizes.Where(c => c.Size.Contains(searchTerm)).OrderBy(m => m.Size).ToList();
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

            var colorList = _context.ItemColors.OrderBy(m => m.Color).ToList();

            if (searchTerm != null)
            {
                colorList = _context.ItemColors.Where(c => c.Color.Contains(searchTerm)).OrderBy(m => m.Color).ToList();
            }

            var modifiedData = colorList.Select(x => new
            {
                id = x.Id,
                text = x.Color
            });

            return Json(modifiedData);
        }

        public JsonResult GetSuppliersList(string searchTerm)
        {

            var supplierList = _context.Suppliers.OrderBy(m => m.Name).ToList();

            if (searchTerm != null)
            {
                supplierList = _context.Suppliers.Where(c => c.Name.Contains(searchTerm)).OrderBy(m => m.Name).ToList();
            }

            var modifiedData = supplierList.Select(x => new
            {
                id = x.Id,
                text = x.Name
            });

            return Json(modifiedData);
        }

    }
}
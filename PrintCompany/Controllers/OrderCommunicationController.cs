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
    public class OrderCommunicationController : Controller
    {
        private readonly PrintCompanyDbContext _context;
        private readonly IMapper _mapper;

        public OrderCommunicationController(PrintCompanyDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        //public async Task<IActionResult> Index(int Id)
        //{
        //    var order = _context.Orders.Find(Id);
        //    if (order == null)
        //    {
        //        return NotFound();
        //    }

        //    List<ItemColor> colors =
        //            await _context.ItemColors.ToListAsync();
        //    return View(colors);
        //}

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrderCustomerContactViewModel orderCustomerContactViewModel)
        {
            if (ModelState.IsValid)
            {
                var orderCustomerContact = _mapper.Map<OrderCustomerContact>(orderCustomerContactViewModel);

                _context.OrderCustomerContacts.Add(orderCustomerContact);
                await _context.SaveChangesAsync();
                return RedirectToAction("Edit", "Orders", new { id = orderCustomerContactViewModel.OrderId });
            }
            return View(orderCustomerContactViewModel);
        }

        //public IActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    //var type = await _context.types.FindAsync(id);           
        //    var color = _context.ItemColors.Find(id);

        //    if (color == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(color);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, ItemColor itemColor)
        //{
        //    if (id != itemColor.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        var color = _context.ItemColors.SingleOrDefault(e => e.Id == id);
        //        color.Color = itemColor.Color;

        //        try
        //        {
        //            _context.Update(color);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!ColorExists(itemColor.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction("Index", "Colors", new { area = "Admin" });
        //    }
        //    return View(itemColor);
        //}

        //public async Task<IActionResult> Delete(int id)
        //{
        //    var color = await _context.ItemColors.FindAsync(id);
        //    _context.ItemColors.Remove(color);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction("Index", "Colors", new { area = "Admin" });
        //}

        //private bool ColorExists(int id)
        //{
        //    return _context.ItemColors.Any(e => e.Id == id);
        //}


        public JsonResult GetContactTypeList(string searchTerm)
        {

            var contactTypeList = _context.ContactTypes.OrderBy(m => m.ContactTypeName).ToList();

            if (searchTerm != null)
            {
                contactTypeList = _context.ContactTypes.Where(c => c.ContactTypeName.Contains(searchTerm)).OrderBy(m => m.ContactTypeName).ToList();
            }

            var modifiedData = contactTypeList.Select(x => new
            {
                id = x.Id,
                text = x.ContactTypeName
            });

            return Json(modifiedData);
        }
    }
}
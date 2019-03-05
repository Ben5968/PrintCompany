using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrintCompany.Core;
using PrintCompany.Data;
using PrintCompany.ViewModels;

namespace PrintCompany.Areas.Admin.Controllers
{
    [Area("Admin")]    
    public class TypesController : Controller
    {
        private readonly PrintCompanyDbContext _context;

        public TypesController(PrintCompanyDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            List<ItemType> types =
                await _context.ItemTypes.ToListAsync();
            return View(types);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ItemType itemType )
        {
            if (ModelState.IsValid)
            {
                _context.ItemTypes.Add(itemType);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Types", new { area = "Admin" });               
            }
            return View(itemType);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var type = await _context.types.FindAsync(id);           
            var type = _context.ItemTypes.Find(id);

            if (type == null)
            {
                return NotFound();
            }            

            return View(type);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ItemType itemType)
        {
            if (id != itemType.Id)
            {
                return NotFound();
            }  

            if (ModelState.IsValid)
            {
                var type = _context.ItemTypes.SingleOrDefault(e => e.Id == id);
                type.Type = itemType.Type;

                try
                {
                    _context.Update(type);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TypeExists(itemType.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Types", new { area = "Admin" });
            }
            return View(itemType);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var type = await _context.ItemTypes.FindAsync(id);
            _context.ItemTypes.Remove(type);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Types", new { area = "Admin" });
        }

        private bool TypeExists(int id)
        {
            return _context.ItemTypes.Any(e => e.Id == id);
        }
    }
}
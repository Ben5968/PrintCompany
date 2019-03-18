using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrintCompany.Core;
using PrintCompany.Data;
using PrintCompany.ViewModels;

namespace PrintCompany.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]    
    public class ColorsController : Controller
    {
        private readonly PrintCompanyDbContext _context;

        public ColorsController(PrintCompanyDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            List<ItemColor> colors =
                await _context.ItemColors.ToListAsync();
            return View(colors);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ItemColor itemColor )
        {
            if (ModelState.IsValid)
            {
                _context.ItemColors.Add(itemColor);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Colors", new { area = "Admin" });               
            }
            return View(itemColor);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var type = await _context.types.FindAsync(id);           
            var color = _context.ItemColors.Find(id);

            if (color == null)
            {
                return NotFound();
            }            

            return View(color);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ItemColor itemColor)
        {
            if (id != itemColor.Id)
            {
                return NotFound();
            }  

            if (ModelState.IsValid)
            {
                var color = _context.ItemColors.SingleOrDefault(e => e.Id == id);
                color.Color = itemColor.Color;

                try
                {
                    _context.Update(color);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ColorExists(itemColor.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Colors", new { area = "Admin" });
            }
            return View(itemColor);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var color = await _context.ItemColors.FindAsync(id);
            _context.ItemColors.Remove(color);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Colors", new { area = "Admin" });
        }

        private bool ColorExists(int id)
        {
            return _context.ItemColors.Any(e => e.Id == id);
        }
    }
}
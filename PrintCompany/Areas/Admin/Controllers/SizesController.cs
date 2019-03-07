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
    public class SizesController : Controller
    {
        private readonly PrintCompanyDbContext _context;

        public SizesController(PrintCompanyDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            List<ItemSize> sizes =
                await _context.ItemSizes.ToListAsync();
            return View(sizes);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ItemSize itemSize )
        {
            if (ModelState.IsValid)
            {
                _context.ItemSizes.Add(itemSize);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Sizes", new { area = "Admin" });               
            }
            return View(itemSize);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var type = await _context.types.FindAsync(id);           
            var size = _context.ItemSizes.Find(id);

            if (size == null)
            {
                return NotFound();
            }            

            return View(size);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ItemSize itemSize)
        {
            if (id != itemSize.Id)
            {
                return NotFound();
            }  

            if (ModelState.IsValid)
            {
                var size = _context.ItemSizes.SingleOrDefault(e => e.Id == id);
                size.Size = itemSize.Size;

                try
                {
                    _context.Update(size);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SizeExists(itemSize.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Sizes", new { area = "Admin" });
            }
            return View(itemSize);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var size = await _context.ItemSizes.FindAsync(id);
            _context.ItemSizes.Remove(size);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Sizes", new { area = "Admin" });
        }

        private bool SizeExists(int id)
        {
            return _context.ItemSizes.Any(e => e.Id == id);
        }
    }
}
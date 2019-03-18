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
    public class SuppliersController : Controller
    {
        private readonly PrintCompanyDbContext _context;

        public SuppliersController(PrintCompanyDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            List<Supplier> suppliers =
                await _context.Suppliers.ToListAsync();
            return View(suppliers);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Supplier Supplier )
        {
            if (ModelState.IsValid)
            {
                _context.Suppliers.Add(Supplier);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Suppliers", new { area = "Admin" });               
            }
            return View(Supplier);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var type = await _context.types.FindAsync(id);           
            var supplier = _context.Suppliers.Find(id);

            if (supplier == null)
            {
                return NotFound();
            }            

            return View(supplier);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Supplier Supplier)
        {
            if (id != Supplier.Id)
            {
                return NotFound();
            }  

            if (ModelState.IsValid)
            {
                var supplier = _context.Suppliers.SingleOrDefault(e => e.Id == id);
                supplier.Address = Supplier.Address;
                supplier.CompanyEmail = Supplier.CompanyEmail;
                supplier.CompanyWebSite = Supplier.CompanyWebSite;
                supplier.MainContact = Supplier.MainContact;
                supplier.MainContactEmail = Supplier.MainContactEmail;
                supplier.Name = Supplier.Name;

                try
                {
                    _context.Update(supplier);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SupplierExists(Supplier.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Suppliers", new { area = "Admin" });
            }
            return View(Supplier);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var supplier = await _context.Suppliers.FindAsync(id);
            _context.Suppliers.Remove(supplier);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Suppliers", new { area = "Admin" });
        }

        private bool SupplierExists(int id)
        {
            return _context.Suppliers.Any(e => e.Id == id);
        }
    }
}
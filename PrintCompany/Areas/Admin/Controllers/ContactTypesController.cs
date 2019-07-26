using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrintCompany.Core;
using PrintCompany.Data;

namespace PrintCompany.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class ContactTypesController : Controller
    {
        private readonly PrintCompanyDbContext _context;

        public ContactTypesController(PrintCompanyDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            List<ContactType> contactTypes =
                await _context.ContactTypes.ToListAsync();
            return View(contactTypes);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ContactType contactType)
        {
            if (ModelState.IsValid)
            {
                _context.ContactTypes.Add(contactType);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "contactTypes", new { area = "Admin" });
            }
            return View(contactType);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
                    
            var contactType = _context.ContactTypes.Find(id);

            if (contactType == null)
            {
                return NotFound();
            }

            return View(contactType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ContactType contactType)
        {
            if (id != contactType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var type = _context.ContactTypes.SingleOrDefault(e => e.Id == id);
                type.ContactTypeName = contactType.ContactTypeName;

                try
                {
                    _context.Update(contactType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!contactTypeExists(contactType.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "contactTypes", new { area = "Admin" });
            }
            return View(contactType);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var contactType = await _context.ContactTypes.FindAsync(id);
            _context.ContactTypes.Remove(contactType);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "contactTypes", new { area = "Admin" });
        }

        private bool contactTypeExists(int id)
        {
            return _context.ContactTypes.Any(e => e.Id == id);
        }

        public JsonResult GetContactTypeList(string searchTerm)
        {

            var contactTypeList = _context.ContactTypes.OrderBy(m=>m.ContactTypeName).ToList();

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

        public JsonResult GetContactType(int id)
        {

            var contactType = _context.ContactTypes.SingleOrDefault(i => i.Id == id);


            if (contactType != null) { 
            var modifiedData = new
            {
                id = contactType.Id,
                text = contactType.ContactTypeName
            };
            return Json(modifiedData);
            }
            else
            {
                return Json(new object {});
            }
        }

    }
}
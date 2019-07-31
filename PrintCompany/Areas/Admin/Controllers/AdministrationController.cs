using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrintCompany.ViewModels;

namespace PrintCompany.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class AdministrationController : Controller
    {
        private readonly RoleManager<IdentityRole> _rolemanager;
        private readonly UserManager<IdentityUser> _userManager;

        public AdministrationController(RoleManager<IdentityRole> rolemanager, UserManager<IdentityUser> userManager)
        {
            _rolemanager = rolemanager;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityRole identityRole = new IdentityRole
                {
                    Name = model.RoleName
                };
                IdentityResult result = await _rolemanager.CreateAsync(identityRole);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles", "Administration", new { area = "Admin" });
                }

                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult ListRoles()
        {
            var roles = _rolemanager.Roles;
            return View(roles);
        }

        public async Task<IActionResult> EditRole(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var role = await _rolemanager.FindByIdAsync(id);

            if (role == null)
            {
                return NotFound();
            }

            var model = new EditRoleViewModel
            {
                Id = role.Id,
                RoleName = role.Name
            };

            foreach (var user in _userManager.Users)
            {
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    model.Users.Add(user.UserName);
                }
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRole(string id, EditRoleViewModel editRoleViewModel)
        {
            if (id != editRoleViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var role = await _rolemanager.FindByIdAsync(id);
                role.Name = editRoleViewModel.RoleName;

                try
                {
                    await _rolemanager.UpdateAsync(role);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoleExists(role.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("ListRoles", "Administration", new { area = "Admin" });
            }
            return View(editRoleViewModel);
        }

        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await _rolemanager.FindByIdAsync(id);

            if (role.Name == "Admin")
            {
                return RedirectToAction("ListRoles", "Administration", new { area = "Admin" });
            }

            await _rolemanager.DeleteAsync(role);
            return RedirectToAction("ListRoles", "Administration", new { area = "Admin" });
        }

        private bool RoleExists(string id)
        {
            return _rolemanager.Roles.Any(e => e.Id == id);
        }
    }
}
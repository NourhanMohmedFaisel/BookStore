using BookStore.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.Intrinsics.Arm;

namespace BookStore.Controllers
{
    [Authorize(Roles ="Admin")]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole<int>> roleManager;
        public RoleController(RoleManager<IdentityRole<int>> roleManager) 
        { 
            this.roleManager = roleManager;
        
        }
        public IActionResult AddRole()
        {
            return View("AddRole");
        }


        [HttpPost]
        public async Task <IActionResult> SaveRole(RoleViewModel roleViewModel)
        {
            if (ModelState.IsValid)
            {
                IdentityRole<int> role = new IdentityRole<int>();
                role.Name = roleViewModel.Name;

                IdentityResult res = await roleManager.CreateAsync(role);

                if (res.Succeeded)
                {
                    ViewBag.success = true;
                    return View("AddRole");
                }

                foreach (var e in res.Errors)
                {
                    ModelState.AddModelError("", e.Description);
                }
            }

            return View("AddRole", roleViewModel);
        }
    }
}

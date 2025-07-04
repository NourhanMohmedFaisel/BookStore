using BookStore.IRepository;
using BookStore.Models;
using BookStore.Repository;
using BookStore.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookStore.Controllers
{
    public class ApplicationUserController : Controller
    {
        private readonly IOrderRepo _orderRepository;
        private readonly IApplicationUserRepo _applicationUserRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public ApplicationUserController(IOrderRepo orderRepository, IApplicationUserRepo applicationUserRepository, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _applicationUserRepository = applicationUserRepository;
            _orderRepository = orderRepository;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Management()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> ListUsers()
        {
            var users = _applicationUserRepository.GetAll();

            var userViewModels = new List<UserWithRolesViewModel>();

            foreach (var user in users)
            {
                var roles = await _applicationUserRepository.GetUserRolesAsync(user);
                userViewModels.Add(new UserWithRolesViewModel
                {
                    User = user,
                    Roles = roles
                });
            }

            return View(userViewModels);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> MakeAdmin(string id)
        {
            var currentUserEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            var superAdminEmail = "nourhan231@gmail.com"; 
            if (currentUserEmail != superAdminEmail)
            {
                TempData["Message"] = "❌ You are not authorized to assign Admin roles.";
                return RedirectToAction("ListUsers");
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                TempData["Message"] = "⚠️ User not found.";
                return RedirectToAction("ListUsers");
            }

            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Contains("Admin"))
            {
                TempData["Message"] = "🚫 This user is already an Admin.";
                return RedirectToAction("ListUsers");
            }

            await _userManager.AddToRoleAsync(user, "Admin");
            TempData["SuccessMessage"] = $"✅ {user.UserName} promoted to Admin successfully.";
            return RedirectToAction("ListUsers");
        }


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RemoveAdmin(string id)
        {
            var currentUserEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            var superAdminEmail = "nourhan231@gmail.com";

            if (currentUserEmail != superAdminEmail)
            {
                TempData["Message"] = "❌ You are not authorized to remove an Admin.";
                return RedirectToAction("ListUsers");
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                TempData["Message"] = "⚠️ User not found.";
                return RedirectToAction("ListUsers");
            }

            var roles = await _userManager.GetRolesAsync(user);
            if (!roles.Contains("Admin"))
            {
                TempData["Message"] = "🚫 This user is not an Admin.";
                return RedirectToAction("ListUsers");
            }

            await _userManager.RemoveFromRoleAsync(user, "Admin");
            TempData["SuccessMessage"] = $"✅ Admin role removed from {user.UserName}.";
            return RedirectToAction("ListUsers");
        }




        public IActionResult UserOrders()
        {
            var users = _applicationUserRepository.GetAll(); 
            var viewModels = new List<UserOrdersViewModel>();

            foreach (var user in users)
            {
                var orders = _orderRepository.GetOrdersByUserId(user.Id);
                viewModels.Add(new UserOrdersViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    Orders = orders
                });
            }

            return View(viewModels);
        }

        [Authorize]
        public IActionResult Profile()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var user = _applicationUserRepository.GetById(userId);
            if (user == null)
                return NotFound();

            var orders = _orderRepository.GetOrdersByUserId(userId);

            var viewModel = new UserProfileViewModel
            {
                Id = user.Id,
                Name = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Orders = orders
            };

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult EditProfile(int id)
        {
            var user = _applicationUserRepository.GetById(id);
            if (user == null)
                return NotFound();

            var viewModel = new UserProfileViewModel
            {
                Id = user.Id,
                Name = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(UserProfileViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByIdAsync(model.Id.ToString());
            if (user == null)
                return NotFound();

            user.UserName = model.Name;
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(model);
            }

          
            await _signInManager.RefreshSignInAsync(user);

            TempData["SuccessMessage"] = "Profile updated successfully!";
            return RedirectToAction("Profile");
        }

        [HttpGet]
        [Authorize]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound();

            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
                return View(model);
            }

            await _signInManager.RefreshSignInAsync(user);
            TempData["SuccessMessage"] = "Password changed successfully!";
            return RedirectToAction("Profile");
        }








    }
}

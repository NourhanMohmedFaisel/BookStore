using BookStore.Models;
using BookStore.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookStore.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public AccountController(UserManager<ApplicationUser> userManager,
                                 SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

       
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SaveRegister
            (RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
               
                ApplicationUser applicationUser = new ApplicationUser();
                applicationUser.Email = registerViewModel.Email;
                applicationUser.UserName = registerViewModel.UserName;
                applicationUser.PasswordHash = registerViewModel.Password;
                applicationUser.PhoneNumber = registerViewModel.phoneNumber;

                
                IdentityResult result =
                    await userManager.CreateAsync(applicationUser, registerViewModel.Password);
                if (result.Succeeded)
                {

                   
                    await signInManager.SignInAsync(applicationUser, false);
                    return RedirectToAction("Index", "Home");
                }
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
            }
            return View("Register", registerViewModel);
        }

       

        [HttpGet]
        public IActionResult RegisterAdmin()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SaveRegisterAdmin
            (RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser applicationUser = new ApplicationUser();
                applicationUser.Email = registerViewModel.Email;
                applicationUser.UserName = registerViewModel.UserName;
                applicationUser.PasswordHash = registerViewModel.Password;
                applicationUser.PhoneNumber = registerViewModel.phoneNumber;

               
                IdentityResult result =
                    await userManager.CreateAsync(applicationUser, registerViewModel.Password);
                if (result.Succeeded)
                {
                    
                    await userManager.AddToRoleAsync(applicationUser, "Admin");
                   
                    await signInManager.SignInAsync(applicationUser, false);
                    return RedirectToAction("Index", "Home");
                }
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
            }
            return View("Register", registerViewModel);
        }


       
        public IActionResult Login()
        {
            return View("Login");
        }

        [HttpPost]
        public async Task<IActionResult> SaveLogin(LoginUserViewModel userViewModel)
        {
            if (ModelState.IsValid == true)
            {
              
                ApplicationUser appUser =
                    await userManager.FindByNameAsync(userViewModel.Name);
                if (appUser != null)
                {
                    bool found =
                         await userManager.CheckPasswordAsync(appUser, userViewModel.Password);
                    if (found == true)
                    {
                        List<Claim> Claims = new List<Claim>();
                        Claims.Add(new Claim("UserEmail", appUser.Email));

                        await signInManager.SignInWithClaimsAsync(appUser, userViewModel.RememberMe, Claims);
                       
                        return RedirectToAction("Index", "Home");
                    }

                }
                ModelState.AddModelError("", "Username OR PAssword wrong");
               
            }
            return View("Login", userViewModel);
        }



       
        public async Task<IActionResult> SignOut()
        {
            await signInManager.SignOutAsync(); 
            return RedirectToAction("Login", "Account");
        }
    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.DTO;
using Talabat.APIs.Errors;
using Talabat.Core.Entities.Identity;

namespace AdminDashboard.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AdminController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }


        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto login)
        {
            if(ModelState.IsValid)
            {
				var user = await _userManager.FindByEmailAsync(login.Email);

				if (user == null)
				{
					Unauthorized(new ApiResponse(401, "Invalid Email Login"));
					return RedirectToAction(nameof(Login));
				}

				var result = await _signInManager.CheckPasswordSignInAsync(user, login.Password, false);
				if (!result.Succeeded) return Unauthorized(new ApiResponse(401, "Invalid Password Login"));

                if (!result.Succeeded || !await _userManager.IsInRoleAsync(user, "Admin"))
                {
                    Unauthorized(new ApiResponse(401, "Invalid Password Login"));
                    return RedirectToAction(nameof(Login));
                }
			}
            return RedirectToAction("Index", "Home");

        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }
    }
}

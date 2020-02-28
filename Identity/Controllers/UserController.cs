using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Identity.Data;
using Identity.Models;
using Identity.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Identity.Controllers
{
    public class UserController : Controller
    {
        private DataContext _context;
        private SignInManager<AppUser> _signInManager;
        private UserManager<AppUser> _userManager;
        private ILogger<HomeController> _logger;

        public UserController(DataContext Context, SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, ILogger<HomeController> logger)
        {
            _context = Context;
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            if (_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Index", "Home");
            }
            return View(new RegisterViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if(!ModelState.IsValid)
            {
                return View(registerViewModel);
            }

            AppUser appUser = new AppUser
            {
                FirstName = registerViewModel.FirstName,
                LastName = registerViewModel.LastName,
                UserName = registerViewModel.Username
            };

            var result = await _userManager.CreateAsync(appUser, registerViewModel.Password);
            if(!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Registration failed");
                _logger.LogInformation("Register Failed");
                return View(registerViewModel);
            }

            var subscriberRole = await _userManager.AddToRoleAsync(appUser, "Subscriber");
            if(!subscriberRole.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Role assignment failed! Contact with authority");
                _logger.LogInformation("Role assignment failed! Contact with authority");
                return View(registerViewModel);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Index", "Home");
            }
            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if(!ModelState.IsValid)
            {
                return View(loginViewModel);
            }

            var result = await _signInManager.PasswordSignInAsync(loginViewModel.Username, loginViewModel.Password, loginViewModel.RememberMe, true);
            if(result.Succeeded)
            {
               return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            _logger.LogInformation("Invalid login attempt.");
            return View(loginViewModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> LogOut(string returnUrl = null)
        {
            await _signInManager.SignOutAsync();
            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            var roles = await _userManager.GetRolesAsync(await _userManager.GetUserAsync(User));
            UserViewModel userViewModel = new UserViewModel
            {
                FirstName   = user.FirstName,
                LastName    = user.LastName,
                Username    = user.UserName,
                Email       = user.Email,
                Roles       = roles
            };
            return View(userViewModel);
        }


    }
}
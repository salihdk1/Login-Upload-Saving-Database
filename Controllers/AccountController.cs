using LDap.Helpers;
using LDap.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace LDap.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly LdapAuthenticationService _ldapService;
        private readonly ILogger<AccountController> _logger;

        public AccountController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            LdapAuthenticationService ldapService,
            ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _ldapService = ldapService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (_ldapService.ValidateCredentials(model.username, model.password))
            {
                var user = await _userManager.FindByNameAsync(model.username);
                if (user == null)
                {
                    // Kullanıcıyı otomatik olarak oluştur
                    user = new AppUser { UserName = model.username };
                    var result = await _userManager.CreateAsync(user);
                    if (!result.Succeeded)
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                        return View(model);
                    }
                }

                await _signInManager.SignInAsync(user, model.RememberMe);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Geçersiz kullanıcı adı veya şifre.");
                return View(model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AdminLogin()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AdminLogin(string username)
        {
            // Sadece belirli bir kullanıcı adı ile giriş yapılabilir
            if (username == "191005035")
            {
                var user = await _userManager.FindByNameAsync(username);
                if (user == null)
                {
                    // Kullanıcıyı otomatik olarak oluştur
                    user = new AppUser { UserName = username };
                    var result = await _userManager.CreateAsync(user);
                    if (!result.Succeeded)
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                        return View();
                    }
                }

                // Admin yetkisini kontrol edin
                if (!await _userManager.IsInRoleAsync(user, "Admin"))
                {
                    await _userManager.AddToRoleAsync(user, "Admin");
                }

                // Admin rolüne sahip olan kullanıcıyı oturum açma
                await _signInManager.SignInAsync(user, false);
                return RedirectToAction("Admin", "Home");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Geçersiz kullanıcı adı.");
                return View();
            }
        }

    }
}

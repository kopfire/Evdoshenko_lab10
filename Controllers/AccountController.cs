using Evdoshenko_lab10.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using static System.Reflection.Metadata.BlobBuilder;

namespace Evdoshenko_lab10.Controllers
{
    public class AccountController : Controller
    {
        private ApplicationContext db;

        public AccountController(ApplicationContext context)
        {
            db = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var salt = ((await db.Users.FirstOrDefaultAsync(u => u.Username == model.Username)) ?? new User { Salt = "" }).Salt;
                User user = await db.Users.FirstOrDefaultAsync(
                    u => u.Username == model.Username && GetHash(model.Password, salt) == u.Password);
                if (user != null)
                {
                    await Authenticate(model.Username); // аутентификация

                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await db.Users.FirstOrDefaultAsync(u => u.Username == model.Username);
                if (user == null)
                {
                    // добавляем пользователя в бд
                    var salt = GenerationSalt();
                    db.Users.Add(new User { Username = model.Username, Password = GetHash(model.Password, salt), Salt = salt, Role = "USER" });
                    await db.SaveChangesAsync();

                    await Authenticate(model.Username); // аутентификация

                    return RedirectToAction("Index", "Home");
                }
                else
                    ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            return View(model);
        }

        private async Task Authenticate(string userName)
        {
            // создаем один claim
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
            };
            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            await HttpContext.SignInAsync("Cookie", new ClaimsPrincipal(id));
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("Cookie");
            return RedirectToAction("Login", "Account");
        }

        private string GetHash(string password, string salt)
        {
            var md5 = MD5.Create();
            var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(password + salt + "KSA~VJC@C8"));
            Console.WriteLine(Convert.ToBase64String(hash));
            return Convert.ToBase64String(hash);
        }

        private string GenerationSalt()
        {
            var salt = "";
            Random random = new();
            for (int i = 0; i < 10; i++)
            {
                char value = (char)random.Next(33, 125);
                if (value == '\\' || value == '/')
                    value = (char)random.Next(33, 91);
                salt += value;
            }
            return salt;
        }
    }
}

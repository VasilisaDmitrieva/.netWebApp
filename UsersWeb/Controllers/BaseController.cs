using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UsersWeb.Models;

namespace UsersWeb.Controllers
{
    public class BaseController : Controller
    {
        protected readonly UserContext db;
        public BaseController(UserContext context)
        {
            db = context;
        }

        public async Task<IActionResult> Logout()
        {
            await db.SaveChangesAsync();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

        public async Task<IActionResult> RedirectToIndex(User? user)
        {
            await db.SaveChangesAsync();
            if (user != null)
            {
                await Authenticate(user);
            }
            return RedirectToAction("Index", "Home");
        }

        public async Task Authenticate(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Mail ?? "")
            };
            ClaimsIdentity id = new(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        public User? GetUserByMail(string? mail)
        {
            return db.Users.FirstOrDefault(u => u.Mail == mail);
        }

        public User? GetAuthenticatedUser()
        {
            if (User.Identity == null)
            {
                return null;
            }
            return GetUserByMail(User.Identity.Name);
        }
    }
}

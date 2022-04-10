using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UsersWeb.Models.View;
using UsersWeb.Models;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace UsersWeb.Controllers
{
    public class AccountController : BaseController
    {
        public AccountController(UserContext context) : base(context)
        {
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid && model.Password != null)
            {
                User? user = db.Users.AsEnumerable().FirstOrDefault(u => u.Mail == model.Mail &&
                    u.PasswordSalt != null &&
                    u.PasswordHash == PasswordHash.GetPasswordHash(u.PasswordSalt, model.Password));

                if (user != null)
                {
                    if (user.Status == Status.Blocked)
                    {
                        ModelState.AddModelError("", "Blocked user");
                    }
                    else
                    {
                        user.LastLoginDate = DateTime.Now;
                        db.Update(user);
                        return await RedirectToIndex(user);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Wrong password or email");
                }
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
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid && model.Password != null)
            {
                User? user = GetUserByMail(model.Mail);
                if (user == null)
                {
                    string passwordSalt = PasswordHash.GenerateSalt();
                    EntityEntry<User> entityUser = db.Users.Add(new User(model.Name, model.Mail, Status.Avaliable, passwordSalt, PasswordHash.GetPasswordHash(passwordSalt, model.Password)));
                    return await RedirectToIndex(entityUser.Entity);
                } else
                {
                    ModelState.AddModelError("", "User with such email already exists.");
                }
            }

            return View(model);
        }
    }
}

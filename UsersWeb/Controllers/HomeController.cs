using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using UsersWeb.Models;
using UsersWeb.Models.View;

namespace UsersWeb.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(UserContext context) : base(context)
        {
        }

        [Authorize]
        [HttpGet]
        public IActionResult Index()
        {
            return View(db.Users.ToList());
        }

        private void DoAction(User u, string action)
        {
            switch (action)
            {
                case "Delete":
                    db.Users.Remove(u);
                    break;
                case "Block":
                case "Unblock":
                    u.Status = action == "Block" ? Status.Blocked : Status.Avaliable;
                    db.Users.Update(u);
                    break;
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Index(List<User> users, string? action)
        {
            User? loginedUser = GetAuthenticatedUser();
            if (users == null || action == null || loginedUser == null) return View("Error");
            
            foreach (User user in users.Where(x => x.IsChecked).ToList()) {
                User? u = GetUserByMail(user.Mail);
                if (u == null)
                {
                    return View("Error");
                }
                DoAction(u, action);
            }

            if (action != "Unblock" && users.Where(x => x.IsChecked).ToList().Exists(x => x.Mail == loginedUser.Mail))
            {
                return await Logout();
            }
            
            return await RedirectToIndex(loginedUser);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
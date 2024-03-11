using LawetaWeb.Credentials;
using Microsoft.AspNetCore.Mvc;

namespace LawetaWeb.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        
        [HttpPost]
        public IActionResult Index(string password)
        {
            if (password == AdminCredentials.pswd)
            {
                HttpContext.Session.SetString("IsAdmin", "true");
                return RedirectToAction("Index", "EditData");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Nieprawidłowe hasło.");
                return View();
            }
        }
    }
}

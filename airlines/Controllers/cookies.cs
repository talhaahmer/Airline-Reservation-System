using airlines.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace airlines.Controllers
{
    public class cookies : Controller
    {
        AirlinesContext db = new AirlinesContext();

        public IActionResult index()
        {

            return View(db.Logins.ToList());
        }

        public IActionResult registration()
        {

            return View();
        }


        [HttpPost]
        public IActionResult registration(Login lg)
        {
            if (ModelState.IsValid)
            {
                db.Logins.Add(lg);
                db.SaveChanges();
            }
            return View("login");
        }

        
        [HttpGet]
        public ActionResult regEdit(int id)
        {
            var login = db.Logins.Find(id);
           
            return View(login);
        }
        [HttpPost]
        public ActionResult regEdit(int id, Login login)
        {

            if (ModelState.IsValid)
            {

                db.Update(login);
                db.SaveChanges();

            }

            return RedirectToAction("login");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(Login lg)
        {
            ClaimsIdentity identity = null;
            bool isAuthenticated = false;

            var res = db.Logins.FirstOrDefault(x => x.Email == lg.Email && x.Password == lg.Password);
            if (res != null)
            {


                if (res.RoleId == 1)
                {

                    //Create the identity for the user
                    identity = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Name, lg.Email),
                    new Claim(ClaimTypes.Sid, res.Id.ToString()),
                    new Claim(ClaimTypes.Role, "Admin")
                }, CookieAuthenticationDefaults.AuthenticationScheme);

                    isAuthenticated = true;
                }

                if (res.RoleId == 2)
                {
                    //Create the identity for the user
                    identity = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Name, lg.Email),
                     new Claim(ClaimTypes.Sid, res.Id.ToString()),
                    new Claim(ClaimTypes.Role, "User")
                }, CookieAuthenticationDefaults.AuthenticationScheme);

                    isAuthenticated = true;
                }

                if (isAuthenticated && res.RoleId==1)
                {
                    var principal = new ClaimsPrincipal(identity);

                    var login = HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    return RedirectToAction("Index", "Admin");
                }


               else
                {
                    var principal = new ClaimsPrincipal(identity);

                    var login = HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    return RedirectToAction("Index", "Home");
                }


            }


            return View();
        }




        public IActionResult Logout()
        {
            var login = HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace WebApplication1.Controllers
{
    [Authorize]
    public class AuthController : Controller
    {
        private readonly AuthService _authService;

        public AuthController()
        {
            _authService = new AuthService();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            if (_authService.ValidateUser(username, password, out var role, out var userId))
            {
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Role, role),
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()), // Include the user ID as NameIdentifier
        };

                var identity = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);

                var owinContext = HttpContext.Request.GetOwinContext();
                var authenticationManager = owinContext.Authentication;

                authenticationManager.SignIn(new AuthenticationProperties { IsPersistent = false }, identity);

                return role == "Admin" ? RedirectToAction("AdminView") : RedirectToAction("NormalUserView");
            }

            return View();
        }


        public ActionResult Logout()
        {
            var owinContext = HttpContext.Request.GetOwinContext();
            var authenticationManager = owinContext.Authentication;

            authenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

            return RedirectToAction("Login", "Auth");
        }

        [Authorize]
        public ActionResult AdminView()
        {
            var allUsers = _authService.GetNonAdminUsers();
            var nonAdminUsers = allUsers.Where(u => u.Role != "Admin").ToList();
            var adminUsers = allUsers.Where(u => u.Role == "Admin").ToList();

            ViewBag.NonAdminUsers = nonAdminUsers; // Pass non-admin users to the view
            ViewBag.AdminUsers = adminUsers;       // Pass admin users to the view

            return View();
        }




        [Authorize(Roles = "NormalUser")]
        public ActionResult NormalUserView()
        {
            return View();
        }

        public ActionResult MakeAdmin(int id)
        {
            _authService.MakeUserAdmin(id);
            return RedirectToAction("AdminView");
        }

        public ActionResult DeleteUser(int id)
        {
            _authService.DeleteUser(id);
            return RedirectToAction("AdminView");
        }


    }
}

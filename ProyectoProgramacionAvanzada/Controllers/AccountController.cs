using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ProyectoProgramacionAvanzada.Models;

namespace ProyectoProgramacionAvanzada.Controllers
{
    public class AccountController : Controller
    {
        private static List<User> users = new List<User>
        {
            new User { id = 1, username = "admin", hashed_password = "admin123", enabled = true, last_connection = DateTime.Now },
            new User { id = 2, username = "usuario", hashed_password = "usuario123", enabled = true, last_connection = DateTime.Now }
        };

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ViewBag.Error = "Debe ingresar usuario y contraseña.";
                return View();
            }

            var user = users.FirstOrDefault(u => u.username == username && u.hashed_password == password && u.enabled);

            if (user != null)
            {
                user.last_connection = DateTime.Now;
                Session["UserId"] = user.id;
                Session["Username"] = user.username;
                return RedirectToAction("Index", "Dashboard");
            }

            ViewBag.Error = "Usuario o contraseña incorrectos.";
            return View();
        }

        public ActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Signup(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ViewBag.Error = "Debe completar todos los campos.";
                return View();
            }

            if (users.Any(u => u.username == username))
            {
                ViewBag.Error = "El nombre de usuario ya existe.";
                return View();
            }

            var newUser = new User
            {
                id = users.Any() ? users.Max(u => u.id) + 1 : 1,
                username = username,
                hashed_password = password,
                enabled = true,
                last_connection = DateTime.Now
            };

            users.Add(newUser);
            return RedirectToAction("Login");
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }

        public ActionResult AccessDenied()
        {
            return View();
        }
    }
}

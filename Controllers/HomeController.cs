using System;
using System.Diagnostics;
using System.Runtime.InteropServices.ComTypes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Design.Internal;
using Microsoft.Extensions.Configuration;
using ProjectHelp_Site.Models;

namespace ProjectHelp_Site.Controllers
{
    public class HomeController : Controller
    {            
        
        [HttpGet]
        public IActionResult Index()
        {
            if (Users._IsConnected) return Redirect("/Home/Profil");
            ViewData["NameOfApp"] = App._NameOfApp;
            ViewData["Result"] = "";
            return View();
        }

        [HttpPost]
        public IActionResult Index(string Username, string Password)
        {
            if (Username != null & Password != null)
            {
                if (Users.Login(Username, Password))
                {
                    return Redirect("/Home/Profil");
                }
                ViewData["Result"] = "Indentifiants incorrecte !";
                ViewData["NameOfApp"] = App._NameOfApp;
                return View();
            }

            ViewData["Result"] = "Merci de compléter tout les champs !";
            ViewData["NameOfApp"] = App._NameOfApp;
            return View();

        }

        public IActionResult Profil()
        {
            Console.WriteLine(Users._Age);
            if (Users._IsConnected != true) return Redirect("/");
            ViewData["Firstname"] = Users._Firstname;
            ViewData["Name"] = Users._Name;
            ViewData["Age"] = Users._Age;
            ViewData["Username"] = Users._Username;
            ViewData["Mail"] = Users._Mail;
            ViewData["NameOfApp"] = App._NameOfApp;
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

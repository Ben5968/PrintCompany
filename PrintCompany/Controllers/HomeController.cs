using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PrintCompany.ViewModels;

namespace PrintCompany.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
           return RedirectToAction("Index", "Order");
        }
    }
}
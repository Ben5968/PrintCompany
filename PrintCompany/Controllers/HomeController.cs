using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PrintCompany.ViewModels;
using Rotativa.AspNetCore;

namespace PrintCompany.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction("Index", "Order");
        }

        //public IActionResult About()
        //{
        //    ViewData["Message"] = "Your application description page.";
        //    var model = new TestModel { Name = "Giorgio" };
        //    return new ViewAsPdf(model, ViewData);
        //}

        //public class TestModel
        //{
        //    public string Name { get; set; }
        //}
    }
}
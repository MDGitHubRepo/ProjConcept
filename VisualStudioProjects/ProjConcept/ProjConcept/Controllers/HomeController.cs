﻿using ProjConcept.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjConcept.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        // Do not cache information as exception details should not be shown to all users.
        public ActionResult Error()
        {
            ErrorDetails model = new ErrorDetails();
            if (ViewBag.ViewControl.Dev)
            {
                model.Exception = (Exception)TempData["Exception"];
                model.ExceptionLoaded = true;
            }
            return View(model);
        }

        [OutputCache(Duration = 360, VaryByParam = "none", NoStore = true)]
        public ActionResult Unauthorized()
        {
            Response.StatusCode = 401;
            return View();
        }
    }
}
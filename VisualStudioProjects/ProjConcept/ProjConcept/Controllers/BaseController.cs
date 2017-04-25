using ProjConcept.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjConcept.Controllers
{
    public class BaseController : Controller
    {
        protected ProjConceptEntities Database { get; set; }

        public BaseController()
        {
            Database = new ProjConceptEntities();
        }
        
        /// <summary>
        /// Overrides the controller class OnException method and redirects to the Error action in the Home controller.
        /// </summary>
        protected override void OnException(ExceptionContext filterContext)
        {
            try
            {
                if (filterContext.ExceptionHandled)
                {
                    return;
                }

                this.LogException(filterContext.Exception);

                if (ViewBag.ViewControl.Dev)
                    TempData["Exception"] = filterContext.Exception;

                filterContext.Result = RedirectToAction("Error", "Home");
                filterContext.ExceptionHandled = true;
            }
            catch (Exception e)
            {
                this.LogException(e);
                filterContext.Result = RedirectToAction("Error", "Home");
            }            
        }

        private void LogException(Exception e)
        {
            ErrorLog errorLog = new ErrorLog
            {
                ErrorMessage = e.Message,
                ErrorSource = e.Source,
                ErrorStackTrace = e.StackTrace,
                ErrorTimestamp = DateTime.Now,
                ErrorUserId = String.IsNullOrWhiteSpace(ViewBag.ViewUser) ? "Anonymous" : ViewBag.ViewUser
            };

            this.Database.ErrorLogs.Add(errorLog);
            this.Database.SaveChanges();
        }
    }
}
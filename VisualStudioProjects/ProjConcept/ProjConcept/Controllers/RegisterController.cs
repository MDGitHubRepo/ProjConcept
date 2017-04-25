using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ProjConcept.Models;

namespace ProjConcept.Controllers
{
    public class RegisterController : BaseController
    {
        private ProjConceptEntities db = new ProjConceptEntities();

        // GET: Register
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        // GET: Register/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }
        
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login([Bind(Include = "UserLoginId,LastName,FirstName,EmailAddress")] User user)
        {
            if (!user.UserAlreadyExists(this.Database))
            {
                ModelState.AddModelError("UserLoginId", "User ID does not exist!");
            }
            else
            {
                CustomAuth.CookieManager.CreateCookie(HttpContext, user.UserLoginId);
                return RedirectToAction("Index", "Home");
            }
            
            return View(user);
        }

        public ActionResult Logoff()
        {
            CustomAuth.CookieManager.DeleteCookie(HttpContext);
            return RedirectToAction("Index", "Home");
        }

        // GET: Register/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Register/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserLoginId,LastName,FirstName,EmailAddress")] User user)
        {
            if (user.UserAlreadyExists(this.Database))
            {
                ModelState.AddModelError("UserLoginId", "User Login ID already exists!");
            }

            if (ModelState.IsValid)
            {
                user.AuthorizationLevel = 20; // User Authorization Level
                db.Users.Add(user);
                db.SaveChanges();
                CustomAuth.CookieManager.CreateCookie(HttpContext, user.UserLoginId);
                return RedirectToAction("Index", "Home");
            }

            return View(user);
        }

        // GET: Register/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            if (CustomAuth.CookieManager.GetUserLoginId(HttpContext.Request.Cookies) == user.UserLoginId)
                return View(user);
            else
            {
                CustomAuth.CookieManager.DeleteCookie(HttpContext);
                return RedirectToAction("Unauthorized", "Home");
            }
        }

        // POST: Register/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserLoginId,LastName,FirstName,EmailAddress")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return View(user);
        }

        // GET: Register/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Register/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

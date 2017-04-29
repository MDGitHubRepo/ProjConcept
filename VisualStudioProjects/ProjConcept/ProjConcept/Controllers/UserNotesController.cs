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
    public class UserNotesController : BaseController
    {
        // GET: UserNotes
        public ActionResult Index()
        {
            // ViewUser is required to provide the correct data
            if (String.IsNullOrWhiteSpace(ViewBag.ViewUser))
                return RedirectToAction("BadRequest", "Home");

            string userId = ViewBag.ViewUser;
            return View(base.Database.UserNotes.Where(n=>n.UserId == userId).OrderBy(n=>n.NoteTitle).ToList());
        }

        // GET: UserNotes
        public ActionResult IndexDataTable()
        {
            // ViewUser is required to provide the correct data
            if (String.IsNullOrWhiteSpace(ViewBag.ViewUser))
                return RedirectToAction("BadRequest", "Home");

            string userId = ViewBag.ViewUser;
            return View(base.Database.UserNotes.Where(n => n.UserId == userId).OrderBy(n => n.NoteTitle).ToList());
        }

        // GET: UserNotes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("BadRequest", "Home");
            }
            UserNote userNote = base.Database.UserNotes.Find(id);
            if (userNote == null)
            {
                return RedirectToAction("NotFound", "Home");
            }
            return View(userNote);
        }

        // GET: UserNotes/Create
        public ActionResult Create()
        {
            // ViewUser is required to provide the correct data
            if (String.IsNullOrWhiteSpace(ViewBag.ViewUser))
                return RedirectToAction("BadRequest", "Home");

            UserNote model = new UserNote() { UserId = ViewBag.ViewUser };
            return View(model);
        }

        // POST: UserNotes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "NoteId,UserId,Note,NoteTitle,NoteLastUpdate")] UserNote userNote)
        {
            if (ModelState.IsValid)
            {
                userNote.NoteLastUpdate = DateTime.Now;
                base.Database.UserNotes.Add(userNote);
                base.Database.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(userNote);
        }

        // GET: UserNotes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("BadRequest", "Home");
            }
            UserNote userNote = base.Database.UserNotes.Find(id);
            if (userNote == null)
            {
                return RedirectToAction("NotFound", "Home");
            }
            return View(userNote);
        }

        // POST: UserNotes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "NoteId,UserId,Note,NoteTitle,NoteLastUpdate")] UserNote userNote)
        {
            if (ModelState.IsValid)
            {
                userNote.NoteLastUpdate = DateTime.Now;
                base.Database.Entry(userNote).State = EntityState.Modified;
                base.Database.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(userNote);
        }

        // GET: UserNotes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("BadRequest", "Home");
            }
            UserNote userNote = base.Database.UserNotes.Find(id);
            if (userNote == null)
            {
                return RedirectToAction("NotFound", "Home");
            }
            return View(userNote);
        }

        // POST: UserNotes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UserNote userNote = base.Database.UserNotes.Find(id);
            base.Database.UserNotes.Remove(userNote);
            base.Database.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                base.Database.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

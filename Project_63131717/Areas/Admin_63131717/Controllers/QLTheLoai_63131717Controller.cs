using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Project_63131717.Models;
using PagedList;
using Project_63131717.Security;

namespace Project_63131717.Areas.Admin_63131717
{
    public class QLTheLoai_63131717Controller : CheckLogin_63131717Controller
    {
        private Project_63131717Entities db = new Project_63131717Entities();
        private const int pageSize = 10;

        // GET: Admin_63131717/QLTheLoai_63131717
        public ActionResult Index(int? page)
        {
            var theloai = db.TheLoais.ToList();
            int pageNumber = (page ?? 1);
            return View(theloai.ToPagedList(pageNumber,pageSize));
        }

        // GET: Admin_63131717/QLTheLoai_63131717/Details/5

        // GET: Admin_63131717/QLTheLoai_63131717/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin_63131717/QLTheLoai_63131717/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IDTheLoai,TenTheLoai")] TheLoai theLoai)
        {
            var existing = db.TheLoais.Find(theLoai.IDTheLoai);
            if (existing != null)
            {
                return Content("ID đã tồn tại. Vui lòng chọn ID khác.");
            }
            if (ModelState.IsValid)
            {
                db.TheLoais.Add(theLoai);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(theLoai);
        }

        // GET: Admin_63131717/QLTheLoai_63131717/Edit/5


        // GET: Admin_63131717/QLTheLoai_63131717/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TheLoai theLoai = db.TheLoais.Find(id);
            if (theLoai == null)
            {
                return HttpNotFound();
            }
            return View(theLoai);
        }

        // POST: Admin_63131717/QLTheLoai_63131717/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            TheLoai theLoai = db.TheLoais.Find(id);
            db.TheLoais.Remove(theLoai);
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

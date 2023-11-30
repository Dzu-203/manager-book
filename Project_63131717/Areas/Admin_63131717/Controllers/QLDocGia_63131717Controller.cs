using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using Project_63131717.Models;
using PagedList;
using Project_63131717.Security;

namespace Project_63131717.Areas.Admin_63131717.Controllers
{
    public class QLDocGia_63131717Controller : CheckLogin_63131717Controller
    {
        private Project_63131717Entities db = new Project_63131717Entities();
        private const int PageSize = 10;

        // GET: Admin_63131717/QLDocGia_63131717
        public ActionResult Index(int? page)
        {
            var docgia = db.DocGias.ToList();
            int pageNumber = (page ?? 1);
            return View(docgia.ToPagedList(pageNumber,PageSize));
        }

        // GET: Admin_63131717/QLDocGia_63131717/Details/5
        public ActionResult Details(int id)
        {
            DocGia docGia = db.DocGias.Find(id);
            if (docGia == null)
            {
                return HttpNotFound();
            }
            return View(docGia);
        }

        // GET: Admin_63131717/QLDocGia_63131717/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin_63131717/QLDocGia_63131717/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IDDG,TenDG,DienThoai,DiaChi,NameUser,Password")] DocGia docGia)
        {
            var existing = db.DocGias.Find(docGia.IDDG);
            if (existing != null)
            {
                return Content("ID đã tồn tại. Vui lòng chọn ID khác.");
            }
            if (ModelState.IsValid)
            {
                db.DocGias.Add(docGia);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(docGia);
        }

        // GET: Admin_63131717/QLDocGia_63131717/Edit/5
        public ActionResult Edit(int id)
        {
            DocGia docGia = db.DocGias.Find(id);
            if (docGia == null)
            {
                return HttpNotFound();
            }
            return View(docGia);
        }

        // POST: Admin_63131717/QLDocGia_63131717/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IDDG,TenDG,DienThoai,DiaChi,NameUser,Password")] DocGia docGia)
        {
            if (ModelState.IsValid)
            {
                db.Entry(docGia).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(docGia);
        }

        // GET: Admin_63131717/QLDocGia_63131717/Delete/5
        public ActionResult Delete(int id)
        {
            DocGia docGia = db.DocGias.Find(id);
            if (docGia == null)
            {
                return HttpNotFound();
            }
            return View(docGia);
        }

        // POST: Admin_63131717/QLDocGia_63131717/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DocGia docGia = db.DocGias.Find(id);
            db.DocGias.Remove(docGia);
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

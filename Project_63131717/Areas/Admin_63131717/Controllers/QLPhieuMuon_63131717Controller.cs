using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using PagedList;
using Project_63131717.Models;
using Project_63131717.Security;

namespace Project_63131717.Areas.Admin_63131717.Controllers
{
    public class QLPhieuMuon_63131717Controller : CheckLogin_63131717Controller
    {
        private Project_63131717Entities db = new Project_63131717Entities();
        private const int pageSize = 10;

        // GET: Admin_63131717/QLPhieuMuon_63131717
        public ActionResult Index(int? page)
        {
            var pm = db.PhieuMuons.ToList();
            int pageNumber = (page ?? 1);
            return View(pm.ToPagedList(pageNumber, pageSize));

        }

        public ActionResult Edit(int id)
        {
            return View(db.PhieuMuons.Where(s => s.IDPM == id).FirstOrDefault());
        }


        [HttpPost]
        public ActionResult Edit([Bind(Include = "IDPM,IDDG,NgayMuon,NgayTra,TienPhat,GhiChu")] PhieuMuon p)
        {

            // TODO: Add update logic here
            if (ModelState.IsValid)
            {
                db.Entry(p).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();

        }


        public ActionResult Phat(int id)
        {
            return View(db.PhieuMuons.Where(s => s.IDPM == id).FirstOrDefault());
        }


        [HttpPost]
        public ActionResult Phat([Bind(Include = "IDPM,IDDG,NgayMuon,NgayTra,TienPhat,GhiChu")] PhieuMuon p)
        {

            if (ModelState.IsValid)
            {
                db.Entry(p).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();

        }


        public ActionResult Delete(int id)
        {
            return View(db.PhieuMuons.Where(s => s.IDPM == id).FirstOrDefault());
        }


        [HttpPost]
        public ActionResult Delete(int id, PhieuMuon p)
        {
            try
            {

                p = db.PhieuMuons.Where(s => s.IDPM == id).FirstOrDefault();
                if (p.TienPhat != 0)
                {
                    return Content("Độc giả chưa đóng tiền phạt thì không được xóa phiếu mượn!'");
                }
                db.PhieuMuons.Remove(p);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return Content("Bạn Không được xóa! Sách vẫn chưa được trả vui lòng kiểm tra lại.....");
            }
        }
    }
}

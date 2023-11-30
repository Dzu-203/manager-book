using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using PagedList;
using Project_63131717.Models;
using Project_63131717.Security;

namespace Project_63131717.Areas.Admin_63131717.Controllers
{
    public class QLCTPhieuMuon_63131717Controller : CheckLogin_63131717Controller
    {
        private Project_63131717Entities db = new Project_63131717Entities();
        private const int PageSize = 10;
        // GET: Admin_63131717/QLCTPhieuMuon_63131717
        public ActionResult Index(string tendg, int? page)
        {
            int pageNumber = (page ?? 1);
            var ct = db.CT_PM.ToList();
            return View(ct.ToPagedList(pageNumber, PageSize));

        }

        public ActionResult Edit(int id)
        {
            
            CT_PM ct = db.CT_PM.Find(id);

            if (ct == null)
            {
                return HttpNotFound();
            }
            ViewBag.TrangThai = new SelectList(db.TrangThais, "IDTrangThai", "TenTrangThai", ct.TrangThai);
            return View(ct);
        }

        // POST: ChiTiet/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,IDPM,IDSach,TenSach,IDDG,SoLuong,NgayTraThucTe,TrangThai")] CT_PM ct)
        {

            foreach (var sach in db.Saches.Where(s => s.IDSach == ct.IDSach))
            {
                var update_soluong = sach.SoLuong + ct.SoLuong;
                sach.SoLuong = update_soluong;
            }
            if (ModelState.IsValid)
            {
                db.Entry(ct).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TrangThai = new SelectList(db.TrangThais, "IDTrangThai", "TenTrangThai", ct.TrangThai);
            return View(ct);
        }


        public ActionResult Delete(int id)
        {
            return View(db.CT_PM.Where(s => s.ID == id).FirstOrDefault());
        }


        [HttpPost]
        public ActionResult Delete(int id, CT_PM ct)
        {
            try
            {

                // TODO: Add delete logic here
                ct = db.CT_PM.Where(s => s.ID == id).FirstOrDefault();
                if (ct.NgayTraThucTe == null)
                {
                    return Content("Lỗi! Chưa có ngày trả sách thực tế!");
                }
                if (ct.TrangThai != "0")
                {
                    return Content("Độc giả chưa trả sách nên không thế xóa phiếu mượn!");
                }

                db.CT_PM.Remove(ct);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        
       
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Project_63131717.Models;
using PagedList;
using Project_63131717.Security;

namespace Project_63131717.Areas.Admin_63131717
{
    public class QLSach_63131717Controller : CheckLogin_63131717Controller
    {
        private Project_63131717Entities db = new Project_63131717Entities();
        private const int PageSize = 10;

        // GET: Admin_63131717/QL_Sach_63131717
        public ActionResult Index(int? page, string searchDropdown, string searchInput)
        {
            var saches = db.Saches.Include(s => s.TheLoai).ToList();
            int pageNumber = (page ?? 1);

            if (!string.IsNullOrEmpty(searchInput))
            {
                switch (searchDropdown)
                {
                    case "tenSach":
                        saches = saches.Where(s => s.TenSach.Contains(searchInput)).ToList();
                        ViewBag.tenSach = searchInput;
                        break;

                    case "theLoai":
                        saches = saches.Where(s => s.TheLoai.TenTheLoai.Contains(searchInput)).ToList();
                        ViewBag.theLoai = searchInput;
                        break;

                    case "tacGia":
                        saches = saches.Where(s => s.TacGia.Contains(searchInput)).ToList();
                        ViewBag.tacGia = searchInput;
                        break;
                }
            }

            ViewBag.searchDropdown = searchDropdown;
            ViewBag.searchInput = searchInput;

            return View(saches.ToPagedList(pageNumber, PageSize));
        }




    // GET: Admin_63131717/QL_Sach_63131717/Details/5
    public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sach sach = db.Saches.Find(id);
            if (sach == null)
            {
                return HttpNotFound();
            }
            return View(sach);
        }

        // GET: Admin_63131717/QL_Sach_63131717/Create
        public ActionResult Create()
        {
            ViewBag.IDTheLoai = new SelectList(db.TheLoais, "IDTheLoai", "TenTheLoai");
            return View();
        }

        // POST: Admin_63131717/QLSach_63131717/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IDSach,TenSach,IDTheLoai,MoTa,TacGia,NgayXuatBan,SoLuong,HinhAnh")] Sach sach)
        {
            var existing = db.Saches.Find(sach.IDSach);
            if (existing != null)
            {
                return Content("ID đã tồn tại. Vui lòng chọn ID khác.");
            }
            // Kiểm tra xem đó có phải là yêu cầu POST hay không
            if (Request.HttpMethod == "POST")
            {
                var img = Request.Files["Avatar"];
                string postedFileName = System.IO.Path.GetFileName(img.FileName);
                // Lưu hình đại diện về Server
                var path = Server.MapPath("/Areas/Admin_63131717/Images/" + postedFileName);
                img.SaveAs(path);
                sach.HinhAnh = postedFileName;
                if (ModelState.IsValid)
                {
                    db.Saches.Add(sach);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            // Nếu đó không phải là một yêu cầu POST hoặc nếu có lỗi ModelState, hiển thị biểu mẫu
            ViewBag.IDTheLoai = new SelectList(db.TheLoais, "IDTheLoai", "TenTheLoai", sach.IDTheLoai);
            return View(sach);
        }




        // GET: Admin_63131717/QL_Sach_63131717/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sach sach = db.Saches.Find(id);
            if (sach == null)
            {
                return HttpNotFound();
            }
            ViewBag.IDTheLoai = new SelectList(db.TheLoais, "IDTheLoai", "TenTheLoai", sach.IDTheLoai);
            return View(sach);
        }

        // POST: Admin_63131717/QL_Sach_63131717/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IDSach,TenSach,IDTheLoai,MoTa,TacGia,NgayXuatBan,SoLuong,HinhAnh")] Sach sach)
        {
            if (Request.HttpMethod == "POST")
            {
                var img = Request.Files["Avatar"];

                if (img != null && img.ContentLength > 0)
                {
                    string postedFileName = System.IO.Path.GetFileName(img.FileName);
                    // Lưu hình đại diện về Server
                    var path = Server.MapPath("/Areas/Admin_63131717/Images/" + postedFileName);
                    img.SaveAs(path);
                    sach.HinhAnh = postedFileName;

                    if (ModelState.IsValid)
                    {
                        db.Entry(sach).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
            }

            // Nếu có lỗi ModelState hoặc không có tệp hình ảnh được chọn, hiển thị biểu mẫu chỉ đọc
            ViewBag.IDTheLoai = new SelectList(db.TheLoais, "IDTheLoai", "TenTheLoai", sach.IDTheLoai);
            return View(sach);
        }
        
        // GET: Admin_63131717/QL_Sach_63131717/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sach sach = db.Saches.Find(id);
            if (sach == null)
            {
                return HttpNotFound();
            }
            return View(sach);
        }

        // POST: Admin_63131717/QL_Sach_63131717/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Sach sach = db.Saches.Find(id);
            db.Saches.Remove(sach);
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

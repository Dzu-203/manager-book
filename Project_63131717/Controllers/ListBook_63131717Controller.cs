using PagedList;
using Project_63131717.Models;
using Project_63131717.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Project_63131717.Controllers
{
    public class ListBook_63131717Controller : CheckLogin_63131717Controller
    {
        Project_63131717Entities db = new Project_63131717Entities();
        private const int PageSize = 9;
        // GET: ListBook_63131717
        public ActionResult Index(string tenSach, int? page)
        {
            var sach = db.Saches.ToList();
            if (!string.IsNullOrEmpty(tenSach))
            {
                sach = sach.Where(s => s.TenSach.Contains(tenSach)).ToList();
                ViewBag.tenSach = tenSach;
            }
            int pageNumber = (page ?? 1);
            return View(sach.ToPagedList(pageNumber, PageSize));
        }
        public ActionResult SachTheoTheLoai(string tenTheLoai, int? page)
        {
            var sachTheoTheLoai = db.Saches.Where(s => s.TheLoai.TenTheLoai == tenTheLoai).ToList();
            ViewBag.TenTheLoai = tenTheLoai;
            int pageNumber = (page ?? 1);
            return View(sachTheoTheLoai.ToPagedList(pageNumber, PageSize));
        }
        public ActionResult HienThi(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var sach = db.Saches.Find(id);

            if (sach == null)
            {
                return HttpNotFound();
            }
            return View(sach);
        }
    }
}
using Project_63131717.Models;
using Project_63131717.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace Project_63131717.Controllers
{
    public class TraCuuPhieuMuon_63131717Controller : CheckLogin_63131717Controller
    {
        // GET: TraCuuPhieuMuon_63131717
        Project_63131717Entities db = new Project_63131717Entities();
        // GET: TraCuuPhieuMuon
        private const int pageSize = 10;
        public ActionResult Index(int? page)
        {
            int pageNumber = (page ?? 1);
            string username = Session["NameUser"] as string;
            var docGia = db.DocGias.FirstOrDefault(dg => dg.NameUser == username);
            var phieuMuon = db.CT_PM.Where(pm => pm.IDDG == docGia.IDDG).ToList();
            return View(phieuMuon.ToPagedList(pageNumber,pageSize));
        }
    }
}
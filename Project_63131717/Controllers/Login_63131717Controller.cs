using Project_63131717.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Project_63131717.Controllers
{
    public class Login_63131717Controller : Controller
    {
        Project_63131717Entities db = new Project_63131717Entities();
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult LoginAcc(string NameUser, string Password)
        {
            // Kiểm tra người dùng thông thường
            var user = db.DocGias.SingleOrDefault(u => u.NameUser == NameUser && u.Password == Password);
            var admin = db.DangNhaps.SingleOrDefault(a => a.Username == NameUser && a.Password == Password);

            if (user != null)
            {
                // Đây là người dùng thường, chuyển hướng đến trang người dùng
                Session["NameUser"] = user.NameUser;
                return RedirectToAction("Index", "ListBook_63131717");
            }
            else if (admin != null)
            {
                // Đây là quản trị viên, chuyển hướng đến trang quản trị
                Session["NameUser"] = admin.Username;
                return RedirectToAction("Index", "QLSach_63131717", new { area = "Admin_63131717" });
            }
            else
            {
                return View("Index");
            }
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(DocGia user)
        {
                if (ModelState.IsValid)
                {
                    // Kiểm tra xem NameUser và Password có trùng nhau không
                    var existingUser = db.DocGias
                        .Where(u => u.NameUser == user.NameUser)
                        .FirstOrDefault();

                    if (existingUser != null)
                    {

                        ViewBag.ErrorRegister = "Tài khoản đã tồn tại. Vui lòng nhập thông tin khác.";
                        return View();
                    }

                    // Nếu không trùng, tiếp tục thêm người dùng mới vào cơ sở dữ liệu
                    db.DocGias.Add(user);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            return View();
        }
        public ActionResult LogOut()
        {
            Session.Clear();
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home_63131717");
        }
    }
}

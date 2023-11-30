using System;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using Project_63131717.Models;
using Project_63131717.Security;

namespace Project_63131717.Controllers
{
    public class GioSach_63131717Controller : CheckLogin_63131717Controller
    {
        private Project_63131717Entities data = new Project_63131717Entities();

        public GioSach_63131717 GetSach()
        {
            GioSach_63131717 gio = Session["GioSach"] as GioSach_63131717;
            if (gio == null || Session["GioSach"] == null)
            {
                gio = new GioSach_63131717();
                Session["GioSach"] = gio;
            }
            return gio;
        }

        // GET: GioSach
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Addto(int id,int quantity)
        {
            var gio = data.Saches.SingleOrDefault(s => s.IDSach == id);
            if (gio != null)
            {
                GetSach().Add(gio,quantity);
            }
            return RedirectToAction("Show", "GioSach_63131717");
        }

        public ActionResult Show()
        {
            if (Session["GioSach"] == null)
            {
                return RedirectToAction("Index", "GioSach_63131717");
            }
            GioSach_63131717 gio = Session["GioSach"] as GioSach_63131717;
            return View(gio);
        }

        public ActionResult Update_quantity(FormCollection form)
        {
            GioSach_63131717 gio = Session["GioSach"] as GioSach_63131717;
            int id_sach = int.Parse(form["Idsach"]);
            int quantity = int.Parse(form["quantity"]);
            gio.Update(id_sach, quantity);
            return RedirectToAction("Show", "GioSach_63131717");
        }

        public ActionResult Remove(int id)
        {
            GioSach_63131717 gio = Session["GioSach"] as GioSach_63131717;
            gio.Remove(id);
            return RedirectToAction("Show", "GioSach_63131717");
        }

        public PartialViewResult BagBook()
        {
            int total = 0;
            GioSach_63131717 gio = Session["GioSach"] as GioSach_63131717;
            if (gio != null)
            {
                total = gio.Total();
            }

            ViewBag.infocart = total;
            return PartialView("BagBook");
        }

        public ActionResult Muon_Success()
        {
            return View();
        }

        [HttpPost]
        public ActionResult MuonSach(FormCollection form)
        {
            var idTrangThaiDangMuon = data.TrangThais
                                    .Where(tt => tt.TenTrangThai == "Đang Mượn")
                                    .Select(tt => tt.IDTrangThai)
                                    .FirstOrDefault();
            var idTrangThaiDaTra = data.TrangThais
                                    .Where(tt => tt.TenTrangThai == "Đã Trả")
                                    .Select(tt => tt.IDTrangThai)
                                    .FirstOrDefault();
            try
            {
                string username = form["NameUser"];
                string password = form["Password"];

                // Kiểm tra username và password với cơ sở dữ liệu
                var docGia = data.DocGias.FirstOrDefault(d => d.NameUser == username && d.Password == password);

                if (docGia == null)
                {
                    // Thông báo đăng nhập không thành công
                    TempData["ErrorMessage"] = "Thông tin đăng nhập không chính xác";
                    return RedirectToAction("Show", "GioSach_63131717");
                }

                // Tiếp tục xử lý khi đăng nhập thành công
                GioSach_63131717 gio = Session["GioSach"] as GioSach_63131717;
                PhieuMuon muon = new PhieuMuon();
                muon.IDDG = docGia.IDDG; // Sử dụng IDDG từ đối tượng DocGia
                muon.TienPhat = 0;
                muon.GhiChu = "";
                DateTime ngayMuon;
                if (gio.Item.Any(item => item._soluongSach == 0))
                {
                    TempData["ErrorMessage"] = "Vui lòng nhập số lượng sách muốn mượn.";
                    return RedirectToAction("Show", "GioSach_63131717");
                }
                if (DateTime.TryParseExact(form["NgayMuon"], "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out ngayMuon))
                {
                    muon.NgayMuon = ngayMuon;
                }
                else
                {
                    TempData["ErrorMessage"] = "Lỗi! Định dạng Ngày mượn không hợp lệ";
                    return RedirectToAction("Show", "GioSach_63131717");
                }
                DateTime ngayTra;
                if (DateTime.TryParse(form["NgayTra"], out ngayTra))
                {
                    muon.NgayTra = ngayTra;
                }
                else
                {
                    TempData["ErrorMessage"] = "Lỗi! Định dạng Ngày trả không hợp lệ";
                    return RedirectToAction("Show", "GioSach_63131717");
                }
                DateTime ngaymuon = Convert.ToDateTime(muon.NgayMuon);
                DateTime ngaytra = Convert.ToDateTime(muon.NgayTra);

                TimeSpan Time = ngaytra - ngaymuon;
                int TongSoNgay = Time.Days;

                if (TongSoNgay > 15)
                {
                    TempData["ErrorMessage"] = "Lỗi ! Thời gian mượn tối đa là 15 ngày";
                    return RedirectToAction("Show", "GioSach_63131717");
                }
                if (TongSoNgay <= 0)
                {
                    TempData["ErrorMessage"] = "Lỗi ! Vui lòng kiểm tra lại mốc thời gian mượn";
                    return RedirectToAction("Show", "GioSach_63131717");
                }

                int total = gio.Total();
                if (total > 3)
                {
                    TempData["ErrorMessage"] = "Tối đa được mượn 3 quyển sách một lần";
                    return RedirectToAction("Show", "GioSach_63131717");
                }

                data.PhieuMuons.Add(muon);

                foreach (var item in gio.Item)
                {
                    int tongsach = 0;
                    tongsach = tongsach + 1;

                    if (item._soluongSach == 0)
                    {
                        TempData["ErrorMessage"] = "Sách không đủ số lượng";
                        return RedirectToAction("Show", "GioSach_63131717");
                        
                    }
                    if (tongsach == 0)
                    {
                        TempData["ErrorMessage"] = "Vui lòng chọn lại số lượng sách!";
                        return RedirectToAction("Show", "GioSach_63131717");
                    }

                    CT_PM Detail = new CT_PM
                    {
                        IDPM = muon.IDPM,
                        IDSach = item.giosach.IDSach,
                        TenSach = item.giosach.TenSach,
                        IDDG = muon.IDDG,
                        SoLuong = item._soluongSach,
                        TrangThai = idTrangThaiDangMuon
                    };

                    data.CT_PM.Add(Detail);

                    foreach (var ct in data.CT_PM.Where(s => s.IDDG == muon.IDDG))
                    {
                        if (ct.TrangThai != idTrangThaiDaTra)
                        {
                            TempData["ErrorMessage"] = "Bạn chưa trả sách thì không được mượn thêm sách!";
                            return RedirectToAction("Show", "GioSach_63131717");
                        }
                    }

                    foreach (var pm in data.PhieuMuons.Where(s => s.IDDG == muon.IDDG))
                    {
                        if (pm.TienPhat != 0)
                        {
                            TempData["ErrorMessage"] = "Bạn chưa đóng tiền phạt thì không được mượn thêm sách!";
                            return RedirectToAction("Show", "GioSach_63131717");
                        }
                    }

                    foreach (var p in data.Saches.Where(s => s.IDSach == Detail.IDSach))
                    {
                        var update_soluong = p.SoLuong - item._soluongSach;
                        p.SoLuong = update_soluong;
                    }
                    foreach (var p in data.Saches.Where(s => s.IDSach == Detail.IDSach))
                    {
                        if (p.SoLuong < item._soluongSach)
                        {
                            TempData["ErrorMessage"] = "Không đủ sách theo yêu cầu của bạn!!";
                            return RedirectToAction("Show", "GioSach_63131717");
                        }
                    }
                }
                data.SaveChanges();
                gio.claer();
                return RedirectToAction("Muon_Success", "GioSach_63131717");
            }
            catch
            {
                TempData["ErrorMessage"] = "Vui lòng kiểm tra lại thông tin!!";
                return RedirectToAction("Show", "GioSach_63131717"); ;
            }
        }
        private bool IsValidTrangThai(string trangThai)
{
    // Bạn có thể thay đổi logic kiểm tra trạng thái theo yêu cầu của bạn.
    return trangThai == "Đã Trả" || trangThai == "Đang Mượn";
}
    }
}

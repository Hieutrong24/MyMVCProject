using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Web_LinhKienDienTu.DTO;
using Web_LinhKienDienTu.Lib;
using Web_LinhKienDienTu.Models;

namespace Web_LinhKienDienTu.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        string backendUrl = Environment.GetEnvironmentVariable("BACKEND_URL");

        public ActionResult KhachXem()
        {
            ViewBag.DanhMucList = LayDanhMuc();
            using (var db = new QL_LinhKienDienTuEntities())
            {
                var dsSanPham = db.SanPham.Select(sp => new SanPham_Model
                {
                    Id = sp.ID,
                    TenSP = sp.TENSP,
                    GiaGoc = sp.GIAGOC,
                    GiaHT = sp.GIAHT,
                    Img = sp.IMG,
                    Mota = sp.MOTA
                }).ToList();


                return View(dsSanPham);
            }
        }
        public ActionResult GioiThieu()
        {
            return View();
        }
        public ActionResult HoSo()
        {
            return View();
        }
        public ActionResult DangKy()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult DangKy(TaiKhoan model)
        {
            if (ModelState.IsValid)
            {
                using (var db = new QL_LinhKienDienTuEntities())
                {
                    try
                    {
                        db.TaiKhoan.Add(model);
                        db.SaveChanges();
                        var gioHang = new GioHang
                        {
                            EMAILTAIKHOAN = model.EMAIL,
                            NGAYTAO = DateTime.Now
                        };

                        db.GioHang.Add(gioHang);
                        db.SaveChanges();
                    }
                    catch (System.Data.Entity.Validation.DbEntityValidationException e)
                    {
                        foreach (var valitionErroes in e.EntityValidationErrors)
                        {
                            foreach (var valitionError in valitionErroes.ValidationErrors)
                            {
                                ModelState.AddModelError(valitionError.PropertyName, valitionError.ErrorMessage);
                            }
                        }
                        return View(model);
                    }

                }
                return RedirectToAction("DangKyThanhCong");
            }
            return View(model);
        }
        public ActionResult DangKyThanhCong()
        {
            return View();
        }
        public ActionResult DangNhap()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult DangNhap(TaiKhoan model)
        {
            if (ModelState.IsValid)
            {
                using (var db = new QL_LinhKienDienTuEntities())
                {
                    var taikhoan = db.TaiKhoan.FirstOrDefault(
                        x => x.EMAIL == model.EMAIL && x.PASSWORD == model.PASSWORD
                    );

                    if (taikhoan != null)
                    {
                        Session["ID"] = taikhoan.ID;
                        Session["Email"] = taikhoan.EMAIL;
                        Session["HoTen"] = taikhoan.HOTEN;
                        Session["Id_TaiKhoan_Admin"] = (taikhoan.ID_TAIKHOAN_ADMIN==null)?null:taikhoan.ID_TAIKHOAN_ADMIN;
                        TempData["Success"] = "Đăng nhập thành công!";
                        if (Session["Id_TaiKhoan_Admin"] != null && (int)Session["Id_TaiKhoan_Admin"] == 1)
                        {
                            return RedirectToAction("ThongKeDoanhThu", "Home");
                        }
                        else if (Session["Id_TaiKhoan_Admin"] == null)
                        {
                            return RedirectToAction("TrangChu", "Home");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Email hoặc mật khẩu không đúng.");
                    }
                }
            }

            return View(model);
        }
        public ActionResult DangXuat()
        {
            Session.Clear();
            return RedirectToAction("KhachXem", "Home");
        }
        public ActionResult TrangChu()
        {
            ViewBag.DanhMucList = LayDanhMuc();
            using (var db = new QL_LinhKienDienTuEntities())
            {
                var dsSanPham = db.SanPham.Select(sp => new SanPham_Model
                {
                    Id = sp.ID,
                    TenSP = sp.TENSP,
                    GiaHT = sp.GIAHT,
                    GiaGoc = sp.GIAGOC,
                    Img = sp.IMG,
                    Mota = sp.MOTA
                }).ToList();
                return View(dsSanPham);
            }
        }

        public ActionResult Intel_core()
        {
            using (var db = new QL_LinhKienDienTuEntities())
            {
                string tenSanPhamCanTim = "Intel Core";
                var dsSanPham = db.SanPham.Where(sp => sp.IDLOAISP == 11 && sp.TENSP.Contains(tenSanPhamCanTim)).Select(sp => new SanPham_Model
                {
                    Id = sp.ID,
                    TenSP = sp.TENSP,
                    GiaGoc = sp.GIAGOC,
                    GiaHT = sp.GIAHT,
                    Img = sp.IMG,
                    Mota = sp.MOTA
                }).ToList();
                return View(dsSanPham);
            }
        }
        public ActionResult Amd_ryzen()
        {
            ViewBag.DanhMucList = LayDanhMuc();
            using (var db = new QL_LinhKienDienTuEntities())
            {
                string tenSanPhamCanTim = "AMD Ryzen";
                var dsSanPham = db.SanPham.Where(sp => sp.IDLOAISP == 11 && sp.TENSP.Contains(tenSanPhamCanTim)).Select(sp => new SanPham_Model
                {
                    Id = sp.ID,
                    TenSP = sp.TENSP,
                    GiaGoc = sp.GIAGOC,
                    GiaHT = sp.GIAHT,
                    Img = sp.IMG,
                    Mota = sp.MOTA
                }).ToList();
                return View(dsSanPham);
            }
        }
        [HttpGet]
        public ActionResult ChiTietSanPham(int? id)
        {
            ViewBag.DanhMucList = LayDanhMuc();
            using (var db = new QL_LinhKienDienTuEntities())
            {
                var sp = db.SanPham.FirstOrDefault(s => s.ID == id);
                if (sp == null)
                {
                    return HttpNotFound();
                }
                return View (sp);
            }
        }
        public ActionResult ThongTinGioHang()
        {
            if (Session["Email"] == null)
            {
                return RedirectToAction("DangNhap", "Home");
            }
            ViewBag.HideDanhMuc = true;
            using (var db = new QL_LinhKienDienTuEntities())
            {
                string userEmail = (string)Session["Email"];
                var temp = db.GioHang.FirstOrDefault(sp => sp.EMAILTAIKHOAN == userEmail);
                var dsGioHang = (from ttgh in db.ThongTin_GioHang
                                 join sp in db.SanPham on ttgh.IDSANPHAM equals sp.ID
                                 join gh in db.GioHang on ttgh.IDGIOHANG equals gh.ID
                                 where gh.EMAILTAIKHOAN == userEmail
                                 select new ChiTietGioHang_Model
                                {
                                    IdSanPham = sp.ID,
                                    IdGioHang = gh.ID,
                                    TenSP = sp.TENSP,
                                    GiaGoc = sp.GIAGOC,
                                    GiaHT = sp.GIAHT,
                                    Img = sp.IMG,
                                    MoTa = sp.MOTA,
                                    SoLuong = (int)ttgh.SOLUONG
                                }).ToList();
                return View(dsGioHang);
            }
        }
        [HttpPost]
        public ActionResult ThongTinGioHang(ChiTietGioHang_Model item)
        {

            if (item == null)
            {
                return new HttpStatusCodeResult(400, "Không có dữ liệu sản phẩm để thêm vào giỏ hàng.");
            }
            ViewBag.HideDanhMuc = true;
            using (var db = new QL_LinhKienDienTuEntities())
            {
                var userEmail = (string)Session["Email"];
                if (userEmail == null)
                {

                    return new HttpStatusCodeResult(401, "Người dùng chưa đăng nhập.");
                }

                var gioHang = db.GioHang.FirstOrDefault(gh => gh.EMAILTAIKHOAN == userEmail);


                if (gioHang == null)
                {

                    gioHang = new GioHang
                    {
                        EMAILTAIKHOAN = userEmail,
                        NGAYTAO = DateTime.Now
                    };
                    db.GioHang.Add(gioHang);
                    db.SaveChanges();
                }


                var chiTietHienCo = db.ThongTin_GioHang.FirstOrDefault(ct =>
                    ct.IDGIOHANG == gioHang.ID && ct.IDSANPHAM == item.IdSanPham
                );

                if (chiTietHienCo != null)
                {

                    chiTietHienCo.SOLUONG += item.SoLuong;
                }
                else
                {

                    var chiTietMoi = new ThongTin_GioHang
                    {
                        
                        IDGIOHANG = gioHang.ID,
                        IDSANPHAM = item.IdSanPham,
                        SOLUONG = item.SoLuong
                    };
                    db.ThongTin_GioHang.Add(chiTietMoi);

                }

                try
                {
                    db.SaveChanges();
                }
                catch(System.Data.Entity.Validation.DbEntityValidationException e)
                {
                    foreach (var validationErrors in e.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            ModelState.AddModelError(validationError.PropertyName, validationError.ErrorMessage);
                        }
                    }
                    return new HttpStatusCodeResult(400, "Lỗi khi thêm sản phẩm vào giỏ hàng.");
                }
            }

            return new HttpStatusCodeResult(200, "Đã thêm sản phẩm vào giỏ hàng.");
        }
        [HttpPost]
        public JsonResult CapNhatSoLuong(int id, int soLuong)
        {
            using (var db = new QL_LinhKienDienTuEntities())
            {
                string email = Session["Email"] as string;

                var item = (from ttgh in db.ThongTin_GioHang
                            join gh in db.GioHang on ttgh.IDGIOHANG equals gh.ID
                            where gh.EMAILTAIKHOAN == email && ttgh.IDSANPHAM == id
                            select new
                            {
                                ThongTin = ttgh,
                                GioHang = gh
                                
                            }).FirstOrDefault();

                if (item != null)
                {
                    if (soLuong <= 0)
                    {
                        db.ThongTin_GioHang.Remove(item.ThongTin);
                    }
                    else
                    {
                        item.ThongTin.SOLUONG = soLuong;
                    }

                    db.SaveChanges();
                    return Json(new { success = true });
                }

                return Json(new { success = false, message = "Không tìm thấy sản phẩm trong giỏ." });
            }
        }

        [HttpGet]
        public JsonResult GetTinhVaXa()
        {
            using (var db = new QL_LinhKienDienTuEntities())
            {
                var xa_phuong = db.Xa_Phuong.Select(xp => new XaPhuong_Model
                {
                    Id = xp.ID,
                    TenXaPhuong = xp.TENXA_PHUONG,
                    Id_Tinh = (int)xp.ID_TINH
                }).ToList(); // Lấy hết ra bộ nhớ

                var tinh = db.Tinh.ToList().Select(t => new Tinh_Model
                {
                    Id = t.ID,
                    TenTinh = t.TENTINH,
                    XaPhuongs = xa_phuong.Where(xp => xp.Id_Tinh == t.ID).ToList()
                }).ToList(); // xử lý ngoài DB

                return Json(tinh, JsonRequestBehavior.AllowGet);
            }
        }
        //[HttpPost]
        //public ActionResult LichSuMuaHang(LichSuMuaHang_Model model)
        //{
        //    if (Session["Email"] == null)
        //        return new HttpStatusCodeResult(401);

        //    if (model.SanPhams == null || model.SanPhams.Count == 0)
        //        return new HttpStatusCodeResult(400, "Không có sản phẩm nào được chọn.");

        //    var emailBody = new StringBuilder();
        //    int idDauTien = 0;
        //    long tongTien = 0;

        //    emailBody.AppendLine("<p>Cảm ơn bạn đã đặt hàng. Chi tiết đơn hàng:</p>");
        //    emailBody.AppendLine("<table border='1' cellpadding='5' cellspacing='0' style='border-collapse: collapse; width: 100%;'>");
        //    emailBody.AppendLine("<thead><tr style='background-color: #f2f2f2;'><th>Tên sản phẩm</th><th>Số lượng</th><th>Ngày đặt</th><th>Mã đơn</th><th>Tổng tiền</th></tr></thead>");
        //    emailBody.AppendLine("<tbody>");

        //    using (var db = new QL_LinhKienDienTuEntities())
        //    {
        //        foreach (var sp in model.SanPhams)
        //        {
        //            var lichsu = new LishSuMuaHnag
        //            {
        //                NGAYMUA = DateTime.Now,
        //                DIACHI = model.DiaChi,
        //                SDT = model.SDT,
        //                TENNGUOINHAN = model.TenNguoiNhan,
        //                IDSANPHAM = sp.IdSanPham,
        //                ID_TTGIOHANG = sp.Id_TTGioHang,
        //                TONGTIEN = (tongTien += sp.Gia),
        //                DATHANHTOAN = false
        //            };

        //            db.LishSuMuaHnag.Add(lichsu);
        //            db.SaveChanges();

        //            if (idDauTien == 0)
        //                idDauTien = lichsu.ID;

        //            emailBody.AppendLine($"<tr><td>{sp.TenSanPham}</td><td>{sp.SoLuong}</td><td>{DateTime.Now:dd/MM/yyyy HH:mm}</td><td>{lichsu.ID}</td><td>{tongTien}</td></tr>");

        //        }
        //        return RedirectToAction("ChuyenHuongThanhToan", "Home", new { id = idDauTien });
        //    }

        //    emailBody.AppendLine("</tbody></table>");
        //    emailBody.AppendLine("<p style='font-weight: bold; color: red;'>Chúng tôi sẽ xử lý đơn hàng sớm nhất có thể.</p>");


        //    emailBody.AppendLine($"<p><a href='#' style='display:inline-block; padding:10px 20px; background-color:#17a2b8; color:white; text-decoration:none; border-radius:5px;'>Xác nhận đơn hàng</a></p>");

        //    try
        //    {
        //        GuiEmailXacNhan((string)Session["Email"], emailBody.ToString());
        //    }
        //    catch (Exception ex)
        //    {
        //        System.Diagnostics.Debug.WriteLine("Lỗi gửi email: " + ex.Message);
        //    }

        //}

        //[HttpPost]
        //public JsonResult LichSuMuaHang(LichSuMuaHang_Model model)
        //{
        //    if (Session["Email"] == null)
        //        return Json(new { success = false, message = "Chưa đăng nhập" });

        //    if (model.SanPhams == null || model.SanPhams.Count == 0)
        //        return Json(new { success = false, message = "Không có sản phẩm" });
        //    var emailBody = new StringBuilder();
        //    int idDauTien = 0;
        //    long tongTien = 0;
        //    emailBody.AppendLine("<p>Cảm ơn bạn đã đặt hàng. Chi tiết đơn hàng:</p>");
        //    emailBody.AppendLine("<table border='1' cellpadding='5' cellspacing='0' style='border-collapse: collapse; width: 100%;'>");
        //    emailBody.AppendLine("<thead><tr style='background-color: #f2f2f2;'><th>Tên sản phẩm</th><th>Số lượng</th><th>Ngày đặt</th><th>Mã đơn</th><th>Tổng tiền</th></tr></thead>");
        //    emailBody.AppendLine("<tbody>");

        //    using (var db = new QL_LinhKienDienTuEntities())
        //    {
        //        foreach (var sp in model.SanPhams)
        //        {
        //            var lichsu = new LishSuMuaHnag
        //            {
        //                NGAYMUA = DateTime.Now,
        //                DIACHI = model.DiaChi,
        //                SDT = model.SDT,
        //                TENNGUOINHAN = model.TenNguoiNhan,
        //                IDSANPHAM = sp.IdSanPham,
        //                ID_TTGIOHANG = sp.Id_TTGioHang,
        //                TONGTIEN = (tongTien += sp.Gia),
        //                DATHANHTOAN = false
        //            };

        //            db.LishSuMuaHnag.Add(lichsu);
        //            db.SaveChanges();

        //            if (idDauTien == 0)
        //                idDauTien = lichsu.ID;

        //            emailBody.AppendLine($"<tr><td>{sp.TenSanPham}</td><td>{sp.SoLuong}</td><td>{DateTime.Now:dd/MM/yyyy HH:mm}</td><td>{lichsu.ID}</td><td>{tongTien}</td></tr>");
        //        }
        //    }

        //    emailBody.AppendLine("</tbody></table>");
        //    emailBody.AppendLine("<p style='font-weight: bold; color: red;'>Chúng tôi sẽ xử lý đơn hàng sớm nhất có thể.</p>");


        //    emailBody.AppendLine($"<p><a href='#' style='display:inline-block; padding:10px 20px; background-color:#17a2b8; color:white; text-decoration:none; border-radius:5px;'>Xác nhận đơn hàng</a></p>");

        //    try
        //    {
        //        GuiEmailXacNhan((string)Session["Email"], emailBody.ToString());
        //    }
        //    catch (Exception ex)
        //    {
        //        System.Diagnostics.Debug.WriteLine("Lỗi gửi email: " + ex.Message);
        //    }
        //    string vnpUrl = TaoUrlThanhToanVNPAY(idDauTien, tongTien);

        //    return Json(new
        //    {
        //        success = true,
        //        redirectUrl = vnpUrl
        //    });
        //}
        [HttpGet]
        public JsonResult GetLichSuMuaHang()
        {
            string email = Session["Email"]?.ToString();
            if (string.IsNullOrEmpty(email))
            {
                 
                return Json(new { success = false, message = "Chưa đăng nhập" }, JsonRequestBehavior.AllowGet);
            }

            using (var db = new QL_LinhKienDienTuEntities())
            {
                var danhSach = (
                    from ls in db.LishSuMuaHnag
                    join ttgh in db.ThongTin_GioHang on ls.ID_TTGIOHANG equals ttgh.ID
                    join gh in db.GioHang on ttgh.IDGIOHANG equals gh.ID
                    where gh.EMAILTAIKHOAN == email
                    orderby ls.NGAYMUA descending
                    select new
                    {
                        MaDon = ls.ID,
                        NgayDat = ls.NGAYMUA,
                        TongTien = ls.TONGTIEN,
                        TrangThai = ls.DATHANHTOAN
                    }
                ).ToList();

                return Json(danhSach, JsonRequestBehavior.AllowGet);
            }
        }



        [HttpPost]
        public JsonResult LichSuMuaHang(LichSuMuaHang_Model model)
        {
            if (Session["Email"] == null)
                return Json(new { success = false, message = "Chưa đăng nhập" });

            if (model.SanPhams == null || model.SanPhams.Count == 0)
                return Json(new { success = false, message = "Không có sản phẩm" });

            var emailBody = new StringBuilder();
            int idDauTien = 0;
            long tongTien = 0;

            emailBody.AppendLine("<p>Cảm ơn bạn đã đặt hàng. Chi tiết đơn hàng:</p>");
            emailBody.AppendLine("<table border='1' cellpadding='5' cellspacing='0' style='border-collapse: collapse; width: 100%;'>");
            emailBody.AppendLine("<thead><tr style='background-color: #f2f2f2;'><th>Tên sản phẩm</th><th>Số lượng</th><th>Ngày đặt</th><th>Mã đơn</th><th>Giá</th></tr></thead>");
            emailBody.AppendLine("<tbody>");

            using (var db = new QL_LinhKienDienTuEntities())
            {
                foreach (var sp in model.SanPhams)
                {
                    var lichsu = new LishSuMuaHnag
                    {
                        NGAYMUA = DateTime.Now,
                        DIACHI = model.DiaChi,
                        SDT = model.SDT,
                        TENNGUOINHAN = model.TenNguoiNhan,
                        IDSANPHAM = sp.IdSanPham,
                        ID_TTGIOHANG = sp.Id_TTGioHang,
                        TONGTIEN = sp.Gia,
                        DATHANHTOAN = false
                    };

                    db.LishSuMuaHnag.Add(lichsu);
                    db.SaveChanges();

                    if (idDauTien == 0)
                        idDauTien = lichsu.ID;

                    tongTien += sp.Gia;

                    emailBody.AppendLine($"<tr><td>{sp.TenSanPham}</td><td>{sp.SoLuong}</td><td>{DateTime.Now:dd/MM/yyyy HH:mm}</td><td>{lichsu.ID}</td><td>{sp.Gia:N0} VND</td></tr>");
                }
            }

            emailBody.AppendLine("</tbody></table>");
            emailBody.AppendLine($"<p style='font-weight: bold; color: red;'>Tổng tiền: {tongTien:N0} VND</p>");
            emailBody.AppendLine("<p>Chúng tôi sẽ xử lý đơn hàng sớm nhất có thể.</p>");


            string linkXacNhan = "http://localhost:53277/Home/XacNhanDonHang?id=" + idDauTien;


            emailBody.AppendLine($"<p><a href='{linkXacNhan}' style='display:inline-block; padding:10px 20px; background-color:#28a745; color:white; text-decoration:none; border-radius:5px;'>Xác nhận và thanh toán qua VNPAY</a></p>");

          
            try
            {
                GuiEmailXacNhan((string)Session["Email"], emailBody.ToString());
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Lỗi gửi email: " + ex.Message);
            }

            //return Json(new
            //{
            //    success = true,
            //    redirectUrl = vnpUrl
            //});
            return Json(new { success = true, message = "Đơn hàng đã được ghi nhận. Vui lòng kiểm tra email để thanh toán." });

        }
        public ActionResult XacNhanDonHang(int id)
        {
            using (var db = new QL_LinhKienDienTuEntities())
            {
                var donHang = db.LishSuMuaHnag.FirstOrDefault(d => d.ID == id);
                if(donHang != null && donHang.DATHANHTOAN == false)
                {

                    donHang.DATHANHTOAN = true;  
                    db.SaveChanges();
                }

                 
                long tongTien = donHang.TONGTIEN ?? 0;
                string vnpUrl = TaoUrlThanhToanVNPAY(donHang.ID, tongTien);

             
                return Redirect(vnpUrl);
            }
        }


        private string TaoUrlThanhToanVNPAY(int Id ,long tongTien)
        {
            string vnp_Returnurl = ConfigurationManager.AppSettings["vnp_ReturnUrl"];
            string vnp_Url = ConfigurationManager.AppSettings["vnp_Url"];
            string vnp_TmnCode = ConfigurationManager.AppSettings["vnp_TmnCode"];
            string vnp_HashSecret = ConfigurationManager.AppSettings["vnp_HashSecret"];

            string ipAddress = Request.UserHostAddress;

             
            if (ipAddress == "::1")
            {
                ipAddress = "127.0.0.1";
            }

            VnPayLibrary vnpay = new VnPayLibrary();
            vnpay.AddRequestData("vnp_Version", "2.1.0");
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
            vnpay.AddRequestData("vnp_Amount", (tongTien * 100).ToString());
            vnpay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_IpAddr", ipAddress);
            vnpay.AddRequestData("vnp_Locale", "vn");
            vnpay.AddRequestData("vnp_OrderInfo", $"Thanh toan don hang #{Id}");
            vnpay.AddRequestData("vnp_OrderType", "other");
            vnpay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
            vnpay.AddRequestData("vnp_TxnRef",Id.ToString());

            

            string paymentUrl = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);
            System.Diagnostics.Debug.WriteLine("URL gửi lên VNPAY: " + paymentUrl);
            return paymentUrl;
        }

        public ActionResult ChuyenHuongThanhToan(int id)
        {
            
            using(var db = new QL_LinhKienDienTuEntities())
            {
                var donHang = db.LishSuMuaHnag.Find(id);
                if(donHang == null)
                {
                    return HttpNotFound();
                }
                long tongTien = donHang.TONGTIEN??0;
                var url = TaoUrlThanhToanVNPAY(id, tongTien); 
                return Redirect(url);
            }
        }

        public ActionResult ReturnVnpay()
        {
            VnPayLibrary vnpay = new VnPayLibrary();

            var response = Request.QueryString;
            foreach (string key in response.AllKeys)
            {
                if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_"))
                {
                    vnpay.AddResponseData(key, response[key]);
                }
            }


            string vnp_HashSecret = ConfigurationManager.AppSettings["vnp_HashSecret"];
            bool checkSignature = vnpay.ValidateSignature(vnp_HashSecret);

            ViewBag.Amount = vnpay.GetResponseData("vnp_Amount");
            ViewBag.OrderInfo = vnpay.GetResponseData("vnp_OrderInfo");
            ViewBag.PayDate = vnpay.GetResponseData("vnp_PayDate");
            ViewBag.BankCode = vnpay.GetResponseData("vnp_BankCode");

            StringBuilder debug = new StringBuilder();
            foreach (string key in Request.QueryString.AllKeys)
            {
                debug.AppendLine($"{key} = {Request.QueryString[key]}");
            }
            ViewBag.DebugLog = debug.ToString();
            if (checkSignature)
            {
                long orderId = long.Parse(vnpay.GetResponseData("vnp_TxnRef"));
                string responseCode = vnpay.GetResponseData("vnp_ResponseCode");

                using (var db = new QL_LinhKienDienTuEntities())
                {
                    var donHang = db.LishSuMuaHnag.Find(orderId);
                    if (donHang != null)
                    {
                        donHang.DATHANHTOAN = responseCode == "00";
                        db.SaveChanges();
                    }
                }

                ViewBag.Message = $"Mã đơn hàng: {orderId}, Kết quả thanh toán: {(responseCode == "00" ? "Thành công" : "Thất bại")}";
            }
            else
            {
                ViewBag.Message = " Xác minh chữ ký thất bại (Sai chữ ký)!";
                ViewBag.DebugLog = vnpay.DebugSignature(vnp_HashSecret);
                

            }

            return View();
        }




        private void GuiEmailXacNhan(string email, string noiDung)
        {
            string fromEmail = "tronghieungo20@gmail.com";
            string password = "dltndclocbsnhrhm";

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(fromEmail);
            mail.To.Add(email);
            mail.Subject = "Xác nhận đơn hàng";
            mail.IsBodyHtml = true;
            mail.Body = noiDung;

            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.Credentials = new NetworkCredential(fromEmail, password);
            smtp.EnableSsl = true;

            try
            {
                smtp.Send(mail);
            }
            catch (Exception e)
            {
                Console.WriteLine("Lỗi gửi mail: " + e.Message);
            }
        }



        [HttpGet]
        public ActionResult GopY()
        {
            ViewBag.DanhMucList = LayDanhMuc();
            return View();
        }
        [HttpPost]

        public ActionResult GopY(GopY_PhanHoi model, HttpPostedFileBase image, HttpPostedFileBase document)
        {
            if (ModelState.IsValid)
            {
                if (image != null && image.ContentLength > 0)
                {
                    var filename = Path.GetFileName(image.FileName);
                    var path = Path.Combine(Server.MapPath("~/Image_PhanHoi"), filename);
                    image.SaveAs(path);
                    model.Img = "~/Image_PhanHoi/" + filename;
                }

                if (document != null && document.ContentLength > 0)
                {
                    var docname = Path.GetFileName(document.FileName);
                    var docPath = Path.Combine(Server.MapPath("~/File_PhanHoi"), docname);
                    document.SaveAs(docPath);
                    model.DocumentPath = "~/File_PhanHoi/" + docname;
                }
                using (var db = new QL_LinhKienDienTuEntities())
                {
                    model.Id_TaiKhoan = (int)Session["ID"];

                    var gopY = new GopY
                    {
                        Name = model.Name,
                        SDT = model.SDT,
                        Email = model.Email,
                        Img = model.Img,
                        Address = model.Address,
                        Created_form = DateTime.Now,
                        Comment = model.Comment,
                        Id_TaiKhoan = model.Id_TaiKhoan,
                        DocumentPath = model.DocumentPath
                    };
                    db.GopY.Add(gopY);
                    db.SaveChanges();
                }
                ViewBag.Message = "Gửi góp ý thành công!"; ;
                return View();
            }
            return View(model);
        }
        public ActionResult ThongKeGopY(DateTime? NgayGui)
        {
            using (var db = new QL_LinhKienDienTuEntities())
            {
                var danhSach = db.GopY.Select(g => new Web_LinhKienDienTu.DTO.GopY_PhanHoi
                {
                    Id = g.Id,
                    Name = g.Name,
                    Email = g.Email,
                    SDT = g.SDT,
                    Address = g.Address,
                    Img = g.Img,
                    DocumentPath = g.DocumentPath,
                    Created_form = g.Created_form,
                    Comment = g.Comment
                }).OrderByDescending(x => x.Created_form).ToList();

                return View(danhSach);
            }
        }
        public void SendEmail(string toEmail, string subject, string body)
        {
            var fromAddress = new MailAddress("tronghieungo20@gmail.com", "HD-Electronic");
            var toAddress = new MailAddress(toEmail);
            string fromPassword = "dltndclocbsnhrhm";  

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };

            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
            {
                smtp.Send(message);
            }
        }

        [HttpPost]
        public ActionResult GuiPhanHoiKhachHang(string Email, string Name, string subject, string body)
        {
            try
            {
                SendEmail(Email, subject, body);  
                ViewBag.ThongBao = "✔️ Gửi phản hồi thành công đến khách hàng!";
            }
            catch (Exception ex)
            {
                ViewBag.Loi = "Gửi email thất bại: " + ex.Message;
            }

            
            using (var db = new QL_LinhKienDienTuEntities())
            {
                var list = db.GopY
                             .OrderByDescending(g => g.Created_form)
                             .Select(g => new GopY_PhanHoi
                             {
                                 Id = g.Id,
                                 Name = g.Name,
                                 Email = g.Email,
                                 SDT = g.SDT,
                                 Address = g.Address,
                                 Img = g.Img,
                                 Comment = g.Comment,
                                 Created_form = g.Created_form,
                                 DocumentPath = g.DocumentPath
                             }).ToList();

                return View("ThongKeGopY", list);
            }
        }




        public ActionResult QLSanPham()
        {
            var viewModel = new SanPhamTongHop_ViewModel();

            using (var db = new QL_LinhKienDienTuEntities())
            {
                viewModel.DanhSachSanPham = (from sp in db.SanPham
                                             join lsp in db.LoaiSP on sp.IDLOAISP equals lsp.ID
                                             select new SanPham_Model
                                             {
                                                 Id = sp.ID,
                                                 TenSP = sp.TENSP,
                                                 GiaNhap = sp.GIAGOC,
                                                 Img = sp.IMG,
                                                 Loai = lsp.TENLOAISP
                                             }).ToList();

                viewModel.DanhSachSPNhap = (from sp in db.Thongtin_SP_Nhap
                                            join ncc in db.NHACUNGCAP on sp.IDNHACUNGCAP equals ncc.ID
                                            where sp.IDTAIKHOAN_ADMIN == 1
                                            select new ThongtinSPNhap_Model
                                            {
                                                id = sp.ID,
                                                tenSP = sp.TENSP,
                                                GiaNhap = sp.GIANHAP,
                                                NgayNhap = sp.NGAYNHAP,
                                                IdNhaCungCap = (int)sp.IDNHACUNGCAP,
                                                IdTaiKhoan_Admin = (int)sp.IDTAIKHOAN_ADMIN,
                                                DiaChi = ncc.DIACHI,
                                                TenNhaCungCap = ncc.TENNHACUNGCAP,
                                                SoLuong = (int)sp.SOLUONG
                                            }).ToList();
            }

            return View(viewModel);
        }


        [HttpPost]
        public ActionResult QLSanPham(SanPham_Model model)
        {
           
                using (var db = new QL_LinhKienDienTuEntities())
                {
                     
                    var loaiSP = db.LoaiSP.FirstOrDefault(l => l.TENLOAISP.ToLower() == model.Loai.Trim().ToLower());

                    if (loaiSP == null)
                    {
                         
                        loaiSP = new LoaiSP
                        {
                            TENLOAISP = model.Loai.Trim()
                        };
                        db.LoaiSP.Add(loaiSP);
                        db.SaveChanges();  
                    }

                    
                    var sanPham = new SanPham
                    {
                        TENSP = model.TenSP,
                        GIAGOC = model.GiaGoc,
                        IMG = "~/Image/"+model.Img,
                        IDLOAISP = loaiSP.ID 
                    };

                    db.SanPham.Add(sanPham);
                    db.SaveChanges();
                }

                TempData["Message"] = "Thêm sản phẩm và loại thành công!";
                return RedirectToAction("QLSanPham");
        }
        [HttpPost]
        public ActionResult XoaSanPham(List<int> selectedIds, SanPham_Model model)
        {
            using (var db = new QL_LinhKienDienTuEntities())
            {
                if (selectedIds != null && selectedIds.Count > 0)
                {
                    foreach (int id in selectedIds)
                    {
                        var sanPham = db.SanPham.Find(id);
                        if (sanPham != null)
                        {
                            sanPham.TENSP = model.TenSP;
                            sanPham.GIAGOC = model.GiaGoc;
                        }
                    }

                    db.SaveChanges();
                    TempData["Message"] = "Xóa sản phẩm thành công!";
                }
                else
                {
                    TempData["Error"] = "Bạn chưa chọn sản phẩm nào để xóa.";
                }
            }

            return RedirectToAction("QLSanPham");
        }
        [HttpPost]
        public ActionResult SuaSanPham(SanPham_Model model)
        {
            using (var db = new QL_LinhKienDienTuEntities())
            {
                var sanPham = db.SanPham.Find(model.Id);
                if (sanPham != null)
                {
                    sanPham.TENSP = model.TenSP;
                    sanPham.GIAGOC = model.GiaGoc;
                    sanPham.IMG = "/Image/"+ model.Img;
                    db.SaveChanges();
                    TempData["Message"] = "Cập nhật sản phẩm thành công!";
                }
            }

            return RedirectToAction("QLSanPham");
        }


        [HttpGet]
        public ActionResult ThongKeDoanhThu()
        {
            using(var db = new QL_LinhKienDienTuEntities())
            {
                var doanhThu = (from ls in db.LishSuMuaHnag
                                
                                join gh in db.GioHang on ls.ID_TTGIOHANG equals gh.ID
                                join tk in db.TaiKhoan on gh.EMAILTAIKHOAN equals tk.EMAIL
                                group ls by tk.HOTEN into g 
                                select new ThongKeDoanhThu_Model
                                {
                                    TenKhachHang = g.Key,
                                    SoDonHang = g.Count()
                                }).ToList();
                return View(doanhThu);
            }
        }
        [HttpGet]
        public async Task<ActionResult> LayDanhSachTinh()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Token","227dc3c7-50fb-11f0-ac24-c6fe55da14a6");

            var response = await client.GetAsync("https://online-gateway.ghn.vn/shiip/public-api/master-data/province");
            var content = await response.Content.ReadAsStringAsync();

            return Content(content);
        }

        private List<DanhMuc_Model> LayDanhMuc()
        {
            using (var db = new QL_LinhKienDienTuEntities())
            {
                var danhMuc = (from dm in db.DanhMuc
                               select new DanhMuc_Model
                               {
                                   Id = dm.MaDanhMuc,
                                   Name = dm.TenDanhMuc,
                                   ChiTietDanhMucs = (from ct in db.ChiTietDanhMuc
                                                      where ct.MaDanhMuc == dm.MaDanhMuc
                                                      select new ChiTietDanhMuc_Model
                                                      {
                                                          Id = ct.MaChiTiet,
                                                          Name = ct.TenChiTiet
                                                      }).ToList()
                               }).ToList();
                return danhMuc;
            }
        }

        private QL_LinhKienDienTuEntities db = new QL_LinhKienDienTuEntities();




        [HttpPost]
        public JsonResult Create(string Name)
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                return Json(new { success = false, message = "Tên danh mục không được để trống!" });
            }

            var entity = new DanhMuc
            {
                TenDanhMuc = Name
            };

            db.DanhMuc.Add(entity);
            db.SaveChanges();

            return Json(new { success = true, message = "Thêm danh mục thành công!" });
        }




        [HttpGet]
        public ActionResult Delete(int id)
        {
            var entity = db.DanhMuc.Find(id);
            if (entity != null)
            {
                db.DanhMuc.Remove(entity);
                db.SaveChanges();
            }
            return RedirectToAction("DanhMuc");
        }



        // ========== CHI TIẾT DANH MỤC ==========

        public ActionResult ChiTietDanhMuc(int? danhMucId)
        {
            var list = db.ChiTietDanhMuc
                .Where(ct => !danhMucId.HasValue || ct.MaDanhMuc == danhMucId)
                .Select(ct => new ChiTietDanhMuc_Model
                {
                    Id = ct.MaChiTiet,
                    Name = ct.TenChiTiet,
                    IdDanhMuc = ct.MaDanhMuc
                }).ToList();
            ViewBag.DanhMucId = danhMucId;
            return View(list);
        }

        [HttpGet]
        public ActionResult CreateChiTiet()
        {
            ViewBag.DanhMucs = new SelectList(db.DanhMuc.ToList(), "MaDanhMuc", "TenDanhMuc");
            return View();
        }

        [HttpPost]
        public JsonResult CreateChiTiet(ChiTietDanhMuc_Model model)
        {
            ViewBag.DanhMucs = new SelectList(db.DanhMuc.ToList(), "MaDanhMuc", "TenDanhMuc");

            if (string.IsNullOrWhiteSpace(model.Name) || model.IdDanhMuc <= 0)
            {
                return Json(new { success = false, message = "Thiếu thông tin tên chi tiết hoặc danh mục!" });
            }

            var entity = new ChiTietDanhMuc
            {
                TenChiTiet = model.Name,
                MaDanhMuc = model.IdDanhMuc
            };

            db.ChiTietDanhMuc.Add(entity);
            db.SaveChanges();

            return Json(new { success = true, message = "Thêm chi tiết danh mục thành công!" });
        }


        public ActionResult DeleteChiTiet(int id)
        {
            var ct = db.ChiTietDanhMuc.FirstOrDefault(x => x.MaChiTiet == id);
            if (ct != null)
            {
                db.ChiTietDanhMuc.Remove(ct);
                db.SaveChanges();
            }
            return RedirectToAction("DanhMuc", new { danhMucId = ct?.MaDanhMuc });
        }

        public ActionResult DanhMuc()
        {
            ViewBag.DanhMucs = new SelectList(db.DanhMuc.ToList(), "MaDanhMuc", "TenDanhMuc");
            var danhMucs = db.DanhMuc.Select(d => new DanhMuc_Model
            {
                Id = d.MaDanhMuc,
                Name = d.TenDanhMuc
            }).ToList();

            var chiTietDanhMucs = (from ct in db.ChiTietDanhMuc
                                   join d in db.DanhMuc on ct.MaDanhMuc equals d.MaDanhMuc
                                   select new ChiTietDanhMuc_Model
                                   {
                                       Id = ct.MaChiTiet,
                                       Name = ct.TenChiTiet,
                                       IdDanhMuc = ct.MaDanhMuc,
                                       TenDanhMuc = d.TenDanhMuc
                                   }).ToList();

            var viewModel = new QuanLyDanhMucViewModel
            {
                DanhMucs = danhMucs,
                ChiTietDanhMucs = chiTietDanhMucs
            };

            return View(viewModel);
        }

        [HttpGet]
        public ActionResult QlDonHang()
        {
            using(var db = new QL_LinhKienDienTuEntities())
            {
                var dsDonHang = (from ls in db.LishSuMuaHnag
                    join gh in db.GioHang on ls.ID_TTGIOHANG equals gh.ID
                    
                    select new LichSuMuaHang_Model
                    {
                        Id = ls.ID,
                        TenNguoiNhan = ls.TENNGUOINHAN,
                        SDT = ls.SDT,
                        DiaChi = ls.DIACHI,
                        NgayDat = ls.NGAYMUA,
                        TongTien = (long)ls.TONGTIEN,
                        DaThanhToan = ls.DATHANHTOAN == true ? "Da thanh toan" : "Chua thanh toan",
                        Email = gh.EMAILTAIKHOAN
                    }).ToList();

                return View(dsDonHang);
            }
        }
        [HttpGet]
        public ActionResult ChiTietDonHang(int id)
        {
            using (var db = new QL_LinhKienDienTuEntities())
            {
               
                var donHang = db.LishSuMuaHnag.FirstOrDefault(x => x.ID == id);
                if (donHang == null)
                {
                    return HttpNotFound("Không tìm thấy đơn hàng.");
                }

                
                var model = new LichSuMuaHang_Model
                {
                    Id = donHang.ID,
                    TenNguoiNhan = donHang.TENNGUOINHAN,
                    SDT = donHang.SDT,
                    DiaChi = donHang.DIACHI,
                    NgayDat = donHang.NGAYMUA,
                    TongTien = (long)(donHang.TONGTIEN ?? 0),
                    DaThanhToan = donHang.DATHANHTOAN == true ? "Đã thanh toán" : "Chưa thanh toán"
                };

                return View(model);
            }
        }

        [HttpGet]
        public ActionResult XoaDonHang(int id)
        {
            using (var db = new QL_LinhKienDienTuEntities())
            {
                var donHang = db.LishSuMuaHnag.FirstOrDefault(x => x.ID == id);
                if (donHang == null)
                {
                    return HttpNotFound("Không tìm thấy đơn hàng để xóa.");
                }

                db.LishSuMuaHnag.Remove(donHang);
                db.SaveChanges();
            }

             
            return RedirectToAction("QlDonHang");
        }

        [HttpGet]
        public ActionResult QlNguoiDung()
        {
            using(var db = new QL_LinhKienDienTuEntities())
            {
                var dsNguoiDung = (from tk in db.TaiKhoan
                                   where tk.ID_TAIKHOAN_ADMIN == null
                                   select new QLNguoiDung_Model
                                   {
                    MaNguoiDung = tk.ID,
                    Email = tk.EMAIL,
                    MatKhau = tk.PASSWORD,
                    HoTen = tk.HOTEN,
                    SDT = tk.SDT
                }).ToList();
                return View(dsNguoiDung);
            }
        }

         

        [HttpGet]
        public ActionResult QuenMatKhau(string email)
        {
            using(var db = new QL_LinhKienDienTuEntities())
            {
                var matKhau = db.TaiKhoan.FirstOrDefault(tk => tk.EMAIL == email);
                if (matKhau == null)
                {
                    ViewBag.Error = "Không tìm thấy tài khoản với email đã nhập.";
                    return View();
                }

                
                string subject = "Khôi phục mật khẩu tài khoản";
                string body = $"Mật khẩu của bạn là: {matKhau.PASSWORD}";
                string toEmail = matKhau.EMAIL;

                try
                {
                    SendEmail(toEmail, subject, body); 
                    ViewBag.Success = "Mật khẩu đã được gửi về email của bạn.";
                }
                catch (Exception ex)
                {
                    ViewBag.Error = "Gửi email thất bại: " + ex.Message;
                }
                return View();
            }
        }


        //[HttpPost]
        //public async Task<ActionResult> TinhPhiShip()
        //{
        //    var client = new HttpClient();
        //    client.DefaultRequestHeaders.Add("Token", "227dc3c7-50fb-11f0-ac24-c6fe55da14a6");

        //    var body = new
        //    {
        //        service_type_id = 2,
        //        to_district_id = 1482,  
        //        to_ward_code = "21108",  
        //        height = 10,
        //        length = 20,
        //        weight = 1000,
        //        width = 15,
        //        insurance_value = 500000,
        //        cod_failed_amount = 0,

        //        from_district_id = 1542,
        //        from_ward_code = "100208"
        //    };
        //    var json = JsonConvert.SerializeObject(body);
        //    var content = new StringContent(json, Encoding.UTF8, "application/json");

        //    var response = await client.PostAsync("https://online-gateway.ghn.vn/shiip/public-api/v2/shipping-order/fee", content);
        //    var responseContent = await response.Content.ReadAsStringAsync();
        //    return Content(responseContent, "application/json");

        //}
        //[HttpGet]
        //public async Task<ActionResult> LayDanhSachCategory()
        //{
        //    using(var client = new HttpClient())
        //    {
        //        client.DefaultRequestHeaders.Add("Token", "227dc3c7-50fb-11f0-ac24-c6fe55da14a6");
        //        var response = await client.GetAsync("https://online-gateway.ghn.vn/shiip/public-api/v2/categories");
        //        var json = await response.Content.ReadAsStringAsync();

        //        return Content(json, "application/json");
        //    }
        //}
        //[HttpPost]
        //public async Task<ActionResult> TaoDonHangTuGioHang(DonHangGioHangModel model)
        //{
        //    using (var client = new HttpClient())
        //    {
        //        client.DefaultRequestHeaders.Add("Token", "227dc3c7-50fb-11f0-ac24-c6fe55da14a6");

        //        var tongTien = model.SanPham.Sum(sp => int.Parse(sp.GiaHT) * int.Parse(sp.SoLuong));
        //        var contentText = string.Join(" ", model.SanPham.Select(sp => sp.TenSP));  

        //        var data = new
        //        {
        //            payment_type_id = 2,
        //            note = "Giao nhanh",
        //            required_note = "KHONGCHOXEMHANG",
        //            to_name = model.TenNguoiNhan,
        //            to_phone = model.SDT,
        //            to_address = model.DiaChi,
        //            to_ward_code = model.WardCode,
        //            to_district_id = model.DistrictId,
        //            cod_amount = tongTien,
        //            content = contentText,  
        //            weight = 1000,
        //            length = 30,
        //            width = 20,
        //            height = 10,
        //            service_type_id = 2,
        //            service_id = 53320,
        //            shop_id = 5863304,
        //            from_name = "Shop Linh Kiện",
        //            from_phone = "0938123456",
        //            from_address = "Trần Văn Ơn, Phú Hòa",
        //            from_ward_code = "100208",
        //            from_district_id = 1542,

        //            items = model.SanPham.Select(sp => new
        //            {
        //                name = sp.TenSP,
        //                quantity = int.Parse(sp.SoLuong),
        //                price = int.Parse(sp.GiaHT),
        //                category = new { level1 = "Điện tử" }  
        //            })
        //        };

        //        var json = JsonConvert.SerializeObject(data);
        //        var content = new StringContent(json, Encoding.UTF8, "application/json");
        //        var response = await client.PostAsync("https://online-gateway.ghn.vn/shiip/public-api/v2/shipping-order/create", content);
        //        var result = await response.Content.ReadAsStringAsync();

        //        return Content(result, "application/json");
        //    }
        //}



        //[HttpGet]
        //public async Task<ActionResult> TheoDoiDon(string maVanDon)
        //{
        //    var client = new HttpClient();


        //    client.DefaultRequestHeaders.Add("Token", "227dc3c7-50fb-11f0-ac24-c6fe55da14a6");


        //    var data = new
        //    {
        //        order_code = maVanDon
        //    };


        //    var json = JsonConvert.SerializeObject(data);
        //    var content = new StringContent(json, Encoding.UTF8, "application/json");


        //    var response = await client.PostAsync("https://online-gateway.ghn.vn/shiip/public-api/v2/shipping-order/detail", content);
        //    var responseContent = await response.Content.ReadAsStringAsync();

        //    return Content(responseContent, "application/json");
        //}



    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_LinhKienDienTu.DTO
{
    public class QLNguoiDung_Model
    {
        public int MaNguoiDung { get; set; }
        public string TenDangNhap { get; set; }
        public string MatKhau { get; set; }
        public string HoTen { get; set; }
        public string Email { get; set; }
        public string SDT { get; set; }
        public DateTime NgaySinh { get; set; }
        public string DiaChi { get; set; }
        public bool GioiTinh { get; set; } // true: Nam, false: Nữ
        public DateTime NgayTao { get; set; }
        public bool TrangThai { get; set; } // true: Hoạt động, false: Không hoạt động
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_LinhKienDienTu.DTO
{
    public class ThongtinSPNhap_Model
    {
        public int id { get; set; }
        public string tenSP { get; set; }
        public double? GiaNhap { get; set; }
        public DateTime NgayNhap { get; set; }
        public int IdNhaCungCap { get; set; }
        public int IdTaiKhoan_Admin { get; set; }
        public string DiaChi { get; set; }
        public string TenNhaCungCap { get; set; }
        public string TenAdmin { get; set; }
        public int SoLuong { get; set; }

    }
}
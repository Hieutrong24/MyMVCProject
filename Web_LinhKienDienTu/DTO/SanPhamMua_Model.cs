using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_LinhKienDienTu.DTO
{
    public class SanPhamMua_Model
    {
        public int IdSanPham { get; set; }
        public string TenSanPham { get; set; }
        public int Id_TTGioHang { get; set; }
        public int SoLuong { get; set; }
        public long Gia { get; set; }
    }
}
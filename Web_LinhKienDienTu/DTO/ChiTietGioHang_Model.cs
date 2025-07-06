using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_LinhKienDienTu.DTO
{
    public class ChiTietGioHang_Model
    {
        public int IdSanPham { get; set; }
        public string TenSP { get; set; }
        public double? GiaGoc { get; set; }
        public double? GiaHT { get; set; }
        public string Img { get; set; }
        public int SoLuong { get; set; }
        public int IdGioHang { get; set; }
        public string MoTa { get; set; }
    }
}
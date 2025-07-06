using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_LinhKienDienTu.DTO
{
    public class SanPham_Model
    {
        public int Id { get; set; }
        public string TenSP { get; set; }
        public double? GiaGoc { get; set; }
        public double? GiaHT { get; set; }
        public double? GiaNhap { get; set; }
        public string Img { get; set; }
        public string Loai { get; set; }
        public string Mota { get; set; }
    }
}
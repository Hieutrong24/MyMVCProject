using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_LinhKienDienTu.DTO
{
    public class DonHangGioHangModel
    {
        public string TenNguoiNhan { get; set; }
        public string SDT { get; set; }
        public string DiaChi { get; set; }
        public int DistrictId { get; set; }
        public string WardCode { get; set; }
        public long? TongTien { get; set; }
        public List<SanPhamMua_Model> SanPham { get; set; }
    }
}
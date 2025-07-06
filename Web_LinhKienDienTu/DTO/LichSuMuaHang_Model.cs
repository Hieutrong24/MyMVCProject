using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_LinhKienDienTu.DTO
{
    public class LichSuMuaHang_Model
    {
        public int Id { get; set; }
        public string TenNguoiNhan { get; set; }
        public string SDT { get; set; }
        public string DiaChi { get; set; }
        public int DistrictId { get; set; }
        public string WardCode { get; set; }
        public DateTime NgayDat { get; set; }
        public long? TongTien { get; set; }
        public string DaThanhToan { get; set; }
        public string Email { get; set; }

        public List<SanPhamMua_Model> SanPhams { get; set; }
    }
}
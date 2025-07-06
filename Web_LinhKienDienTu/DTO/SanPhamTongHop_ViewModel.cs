using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_LinhKienDienTu.DTO
{
    public class SanPhamTongHop_ViewModel
    {
        public List<SanPham_Model> DanhSachSanPham { get; set; }
        public List<ThongtinSPNhap_Model> DanhSachSPNhap { get; set; }
    }
}
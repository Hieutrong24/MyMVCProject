using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_LinhKienDienTu.DTO
{
    public class QuanLyDanhMucViewModel
    {
        public List<DanhMuc_Model> DanhMucs { get; set; }
        public List<ChiTietDanhMuc_Model> ChiTietDanhMucs { get; set; }
    }
}
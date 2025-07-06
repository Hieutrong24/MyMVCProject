using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_LinhKienDienTu.DTO
{
    public class ChiTietDanhMuc_Model
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int IdDanhMuc { get; set; }
        public string TenDanhMuc { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_LinhKienDienTu.DTO
{
    public class DanhMuc_Model
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public List<ChiTietDanhMuc_Model> ChiTietDanhMucs{get; set;}
    }
}
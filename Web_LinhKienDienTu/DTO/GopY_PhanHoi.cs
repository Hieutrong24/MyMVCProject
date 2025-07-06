using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_LinhKienDienTu.DTO
{
    public class GopY_PhanHoi
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SDT { get; set; }
        public string Email { get; set; }
        public string Img { get; set; }
        public string DocumentPath { get; set; }
        public string Address { get; set; }
        public DateTime? Created_form { get; set; }
        public string Comment { get; set; }
        public int Id_TaiKhoan { get; set; }

    }
}
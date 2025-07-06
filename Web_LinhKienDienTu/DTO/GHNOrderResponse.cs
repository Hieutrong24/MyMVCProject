using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_LinhKienDienTu.DTO
{
    public class GHNOrderResponse
    {
        public int code { get; set; }
        public string message { get; set; }
        public object data { get; set; }
    }
}
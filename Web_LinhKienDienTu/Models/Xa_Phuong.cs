//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Web_LinhKienDienTu.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Xa_Phuong
    {
        public int ID { get; set; }
        public string TENXA_PHUONG { get; set; }
        public Nullable<int> ID_TINH { get; set; }
    
        public virtual Tinh Tinh { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

public class TaiKhoan_Model
{
    public int ID { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập email.")]
    [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
    public string EMAIL { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập mật khẩu.")]
    [DataType(DataType.Password)]
    [RegularExpression(@"^(?=.*[a-zA-Z])(?=.*\d)(?=.*[\W_]).{6,}$",
        ErrorMessage = "Mật khẩu phải có ít nhất 1 chữ cái, 1 số và 1 ký tự đặc biệt.")]
    public string PASSWORD { get; set; }

    public string HOTEN { get; set; }
    public int? ID_TAIKHOAN_ADMIN { get; set; }
    [Required(ErrorMessage = "Vui lòng nhập số điện thoại.")]
    [RegularExpression(@"^0\d{9}$", ErrorMessage = "Số điện thoại phải bắt đầu bằng 0 và có 10 chữ số.")]
    public string SDT { get; set; }

}

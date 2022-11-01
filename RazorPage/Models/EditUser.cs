using System.ComponentModel.DataAnnotations;

namespace RazorPage.Models
{
    public class EditUser
    {
        [DataType(DataType.Text)]
        [StringLength(15,MinimumLength =3)]
        [Required(ErrorMessage = "وارد کردن اسم الزامی است!")]
        public string Name { get; set; }
        [DataType(DataType.Text)]
        [StringLength(20,MinimumLength =4)]
        [Required(ErrorMessage = "وارد کردن فامیلی الزامی است!")]
        public string FamilyName { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [RegularExpression(@"[^0][^9]\d{1,9}",ErrorMessage ="شماره همراه باید بدون پیش شماره ۰۹ وارد شود")]
        public int? phonenumber { get; set; }
        public int? NoBorrowedbooks { get; set; }
        public int? NoLendedbooks { get; set; }
        [Required(ErrorMessage ="وارد کردن اسم شهر الزامی است!")]
        public string City { get; set; }
        [Required(ErrorMessage = "وارد کردن اسم خیابان الزامی است!")]
        public string Street { get; set; }
    }
}

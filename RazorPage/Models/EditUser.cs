using System.ComponentModel.DataAnnotations;

namespace RazorPage.Models
{
    public class EditUser
    {
        [DataType(DataType.Text)]
        [StringLength(15,MinimumLength =3)]
        public string Name { get; set; }
        [DataType(DataType.Text)]
        [StringLength(20,MinimumLength =4)]
        public string FamilyName { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [RegularExpression(@"[^0][^9]\d{1,9}",ErrorMessage ="شماره همراه باید بدون پیش شماره ۰۹ وارد شود")]
        public int? phonenumber { get; set; }
        public int? NoBorrowedbooks { get; set; }
        public int? NoLendedbooks { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace RazorPage.Models
{
    public class SignUp
    {
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public int Password { get; set; }
        public string Name { get; set; }
        public string FamilyName { get; set; }
        [RegularExpression(@"[^0][^9]\d{1,9}", ErrorMessage = "شماره همراه باید بدون پیش شماره ۰۹ وارد شود")]
        public int? PhoneNumber { get; set; }
        public Address address { get; set; }
    }
}

namespace RazorPage.Models
{
    public class SignUp
    {
        public string Email { get; set; }
        public int Password { get; set; }
        public string Name { get; set; }
        public string FamilyName { get; set; }
        public int? PhoneNumber { get; set; }
        public Address address { get; set; }
    }
}

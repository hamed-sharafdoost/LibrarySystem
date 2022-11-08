using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPage.Models;
using LibraryDatabase;
using Microsoft.Net.Http.Headers;

namespace RazorPage.Pages
{
    public class LoginModel : PageModel
    {
        public LoginUser Admin = new LoginUser() { Email = "nt92481@gmail.com", password = 13801380 };
        private DbContextLib _context;
        public LoginModel(DbContextLib context)
        {
            _context = context;
        }
        [BindProperty]
        public LoginUser LoginUser { get; set; }
        public void OnGet()
        {
            ViewData["Email"] = Request.Cookies["Email"]; //Utilizing cookies in order to make user expreince easier
            ViewData["Password"] = Request.Cookies["Password"];
            
        }
        public IActionResult OnPost()
        {
            CookieOptions cookieoption = new CookieOptions { Expires = DateTime.Now.AddDays(30), HttpOnly = true };

            var user = _context.Users.SingleOrDefault(user => user.Email == LoginUser.Email && user.Password == LoginUser.password);
            
            if (LoginUser.password == Admin.password && LoginUser.Email.Equals(Admin.Email)) // Validation of admin
            {
                TempData["Message"] = "Login was successfull";
                Response.Cookies.Append("Email", Admin.Email,cookieoption);
                Response.Cookies.Append("Password",Admin.password.ToString(),cookieoption);
                return RedirectToPage("Admin/admin");
            }
            else if(user is not null) //Validation of normal user
            {
                TempData["Message"] = "Login was successfull";
                Response.Cookies.Append("Email", user.Email,cookieoption);
                Response.Cookies.Append("Password",user.Password.ToString(),cookieoption);
                return RedirectToPage("User");
            }
            else // Incorrect email or password inserted
            {
                TempData["Message"] = "Try again";
                return Page();
            }
        }
    }
}

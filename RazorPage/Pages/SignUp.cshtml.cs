using LibraryDatabase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPage.Models;
using Mapster;

namespace RazorPage.Pages
{
    public class SignUpModel : PageModel
    {
        [BindProperty]
        public SignUp SignUp { get; set; }
        private DbContextLib _context;
        public SignUpModel(DbContextLib context)
        {
            _context = context;
        }
        public void OnGet()
        {
        }
        public IActionResult OnPost()
        {
            var user = SignUp.Adapt<User>();

            _context.Users.Add(user);
            if(_context.SaveChanges() > 0)
            {
                ViewData["Success"] = "You successfully SignedUp";
                return RedirectToPage("/Index");
            }
            else
            {
                ViewData["Unsuccess"] = "Try again!Something is wrong";
                return Page();
            }

        }
    }
}

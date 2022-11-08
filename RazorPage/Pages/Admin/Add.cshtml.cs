using LibraryDatabase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPage.Models;
using Mapster;

namespace RazorPage.Pages.Admin
{
    public class AddModel : PageModel
    {
        [BindProperty]
        public AddBook AddBook { get; set; }
        private DbContextLib _context;
        public AddModel(DbContextLib context)
        {
            _context = context;
        }
        public void OnGet()
        {
        }
        public IActionResult OnPost()
        {
            var Book = AddBook.Adapt<Books>();
            _context.Books.Add(Book);
            try
            {
                if (_context.SaveChanges() > 0)
                {
                    ViewData["Success"] = "New Book added successfully";
                    return RedirectToPage("admin");
                }
                else
                {
                    ViewData["Unsuccessfull"] = "Something is wrong.Refill the details";
                    return Page();
                }
            }
            catch (Exception ex)
            {
                ViewData["Unsuccessfull"] = "Something is wrong.Refill the details";
                return Page();
            }
        }
    }
    
}

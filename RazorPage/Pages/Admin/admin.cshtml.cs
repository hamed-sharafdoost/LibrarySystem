using LibraryDatabase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPage.Models;

namespace RazorPage.Pages.Admin
{
    public class adminModel : PageModel
    {
        [BindProperty]
        public EditBook book { get; set; }
        [BindProperty]
        public int BookId { get; set; }
        public IEnumerable<Books> books { get; set; }
        private DbContextLib _context;
        public adminModel(DbContextLib context)
        {
            _context = context;
        }
        public void OnGet()
        {
            books = _context.Set<Books>().AsEnumerable();
        }
        public IActionResult OnPostEdit() // Edit and update a book by sending BookId to the view
        {
            books = _context.Set<Books>().AsEnumerable();
            ViewData["Edit"] = BookId;
            return Page();
        }
        public IActionResult OnPostUpdate() // Confirm the edition and changing the database
        {
            books = _context.Set<Books>().AsEnumerable();
            Books select = _context.Books.SingleOrDefault(v => v.BooksId == BookId);
            if (book != null && ModelState.IsValid) 
            {
                select.Title = book.Title;
                select.Author = book.Author;
                select.Description = book.Description;
                select.Genre = book.Genre;
                select.Availabale = book.Availabale;
                select.LastBorrowdate = book.LastBorrowdate;
                _context.Books.Update(select);
                _context.SaveChanges();
                return RedirectToAction("Edit");
            }
            else
                return Page();
        }
    }
}

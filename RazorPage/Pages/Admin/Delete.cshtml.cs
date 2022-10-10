using LibraryDatabase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPage.Pages.Admin
{
    public class DeleteModel : PageModel
    {
        [BindProperty]
        public int BooksId { get; set; }
        public IEnumerable<Books> books { get; set; }
        private DbContextLib _context;
        public DeleteModel(DbContextLib context)
        {
            _context = context;
        }
        
        public void OnGet()
        {
            books = _context.Set<Books>().AsEnumerable();
        }
        public IActionResult OnPost()
        {
            books = _context.Set<Books>().AsEnumerable();
            var book = _context.Books.SingleOrDefault(v =>v.BooksId == BooksId);
            if (book != null)
            {
                _context.Books.Remove(book);
                _context.SaveChanges();
                return Page();
            }
            else
            {
                return RedirectToPage("admin");
            }
        }
    }
}

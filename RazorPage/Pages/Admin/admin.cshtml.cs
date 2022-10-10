using LibraryDatabase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPage.Pages.Admin
{
    public class adminModel : PageModel
    {
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
    }
}

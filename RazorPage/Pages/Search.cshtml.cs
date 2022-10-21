using LibraryDatabase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPage.Pages
{
    public class SearchModel : PageModel
    {

        [BindProperty]
        public List<int> checks { get; set; }
        public IQueryable<Books> books { get; set; }
        [BindProperty]
        public string searchparam { get; set; }
        private DbContextLib _context;
        public SearchModel(DbContextLib context)
        {
            _context = context;
        }
     
        public void OnGet()
        {
        }
        public IActionResult OnPost()
        {

            var find = _context.Books.Where(c => c.Title.Contains(searchparam) || c.Author.Contains(searchparam));
            
            if (find.Any())
            {
                books = find;
                return Page();
            }
            else
            {
                ViewData["SearchResult"] = "There is not a book with this information";
                return RedirectToPage("User");
            }
            
        }
        public IActionResult OnPostChoose()
        {
            var user = _context.Users.SingleOrDefault(b => b.Email.Equals(Request.Cookies["Email"]));
            if (user.NOborrwedbooks < 3 && user.NOborrwedbooks >= 0 || user.NOborrwedbooks == null)
            {
                if (checks.Count > 2)
                {
                    TempData["Limit2"] = "You are allowed to choose atlast 2 books";
                    return Page();
                }
                else if (checks.Count > 0)
                {
                    foreach (var book in checks)
                    {
                        var borrower = new Borrower { BooksId = book, BorrowExDate = DateTime.Now.AddDays(14) };
                        borrower.UserBorrowers = new List<UserBorrower>()
                    { new UserBorrower{Borrower = borrower,User = user } };
                        _context.Borrowers.Add(borrower);
                        _context.SaveChanges();
                    }
                    return RedirectToPage("Cart");
                }
                else
                {
                    return RedirectToPage("User");
                }
            }
            else
            {
                TempData["Limit2"] = "You already borrowed some books";
                return Page();
            }
        }
    }
}

using LibraryDatabase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace RazorPage.Pages
{
    public class UserModel : PageModel
    {
        public Books book;
        [BindProperty(SupportsGet =true)]
        public IEnumerable<Books> books { get; set; }
        [BindProperty]
        public List<int> Ids { get; set; }
        private DbContextLib _context;
        public User user;
        public UserModel(DbContextLib context)
        {
            _context = context;
        }
        public void OnGet()
        {
            books = _context.Set<Books>().AsEnumerable();
        }
        public IActionResult OnPost()
        {
            user = _context.Users.SingleOrDefault(x => x.Email == Request.Cookies["Email"].Trim());
            if (user.NOborrwedbooks < 3 && user.NOborrwedbooks >= 0 || user.NOborrwedbooks == null)
            {
                if (Ids.Count > 2)
                {
                    TempData["Limit"] = "You are allowed to choose atlast 2 books";
                    return RedirectToPage("User");
                }
                else if (Ids.Count > 0)
                {
                    foreach (int i in Ids)
                    {
                        book = _context.Books.SingleOrDefault(c => c.BooksId == i);
                        book.Availabale = false;
                        var borrower = new Borrower { BooksId = i, BorrowExDate = DateTime.Now.AddDays(14) };
                        borrower.UserBorrowers = new List<UserBorrower>() {
                        new UserBorrower
                        {
                            Borrower = borrower,User = user
                        } };
                        _context.Books.Update(book);
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
                TempData["Limit"] = "You already borrow some books";
                return RedirectToPage("Cart");
            }
        }
    }
}

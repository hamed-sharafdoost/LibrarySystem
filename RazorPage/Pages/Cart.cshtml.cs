using LibraryDatabase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace RazorPage.Pages
{
    public class CartModel : PageModel
    {
        public User user;
        public Books book;
        public int Count;
        public List<UserBorrower> userborrower { get; set; }
        [BindProperty]
        public int BooksId { get; set; }
        [BindProperty(SupportsGet = true)]
        public Borrower borrower1 { get; set; }
        public DbContextLib _context { get; set; }
        public CartModel(DbContextLib context)
        {
            _context = context;
        }
        public void OnGet()
        {
            var userid = _context.Users.SingleOrDefault(c => c.Email == Request.Cookies["Email"].ToString()).UserId;
            userborrower = _context.UserBorrowers.Where(c => c.UserId == userid).ToList();
            foreach (var list in userborrower)
            {
                var findbook = _context.Borrowers.Include(v => v.Books).FirstOrDefault(c => c.BorrowerId == list.BorrowerId);
                Count = _context.Books.Count(v => v.BooksId == findbook.Books.BooksId && findbook.Books.Availabale == true);
            }
            if (userborrower.Count > 2 || Count <= 0)
            {
                ViewData["disable"] = "disable";
            }
        }
        public IActionResult OnPost()
        {
            user = _context.Users.SingleOrDefault(b => b.Email == Request.Cookies["Email"]);
            var userid = _context.Users.SingleOrDefault(c => c.Email == Request.Cookies["Email"].ToString()).UserId;
            userborrower = _context.UserBorrowers.Where(c => c.UserId == userid).ToList();
            book = _context.Books.SingleOrDefault(b => b.BooksId == BooksId);
            if (string.IsNullOrEmpty(Request.Form["Sub"]))
            {
                var borrower = _context.Borrowers.SingleOrDefault(n => n.BooksId == BooksId);
                book.Availabale = true;
                if (borrower != null)
                {
                    _context.Borrowers.Remove(borrower);
                    _context.Books.Update(book);
                    _context.SaveChanges();
                    user.NOborrwedbooks = _context.UserBorrowers.Count(x => x.UserId == user.UserId);
                    _context.Users.Update(user);
                    _context.SaveChanges();
                    return RedirectToPage("Cart");
                }
                else
                {
                    return RedirectToPage("Cart");
                }
            }
            else
            {
                var finalsubmit = _context.UserBorrowers.Include(v => v.Borrower).Where(x => x.UserId == user.UserId).ToList();
                foreach(var select in finalsubmit)
                {
                    var selectbook = _context.Books.FirstOrDefault(b => b.BooksId == select.Borrower.BooksId);
                    selectbook.Availabale = false;
                    _context.Books.Update(selectbook);
                    _context.SaveChanges(true);
                }
                user.NOborrwedbooks = finalsubmit.Count();
                _context.Users.Update(user);
                _context.SaveChanges();
                ViewData["disable"] = "disable";
                return Page();
            }
            
        }
    }
}

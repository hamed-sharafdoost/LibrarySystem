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
        }
        public IActionResult OnPost()
        {
            user = _context.Users.SingleOrDefault(b => b.Email == Request.Cookies["Email"]);
            var userid = _context.Users.SingleOrDefault(c => c.Email == Request.Cookies["Email"].ToString()).UserId;
            userborrower = _context.UserBorrowers.Where(c => c.UserId == userid).ToList();
            if (string.IsNullOrEmpty(Request.Form["Sub"]))
            {
                var borrower = _context.Borrowers.SingleOrDefault(n => n.BooksId == BooksId);
                book = _context.Books.SingleOrDefault(b => b.BooksId == BooksId);
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
                    return RedirectToPage("Cart");
            }
            else
            {
                user.NOborrwedbooks = _context.UserBorrowers.Count(x => x.UserId == user.UserId);
                _context.Users.Update(user);
                _context.SaveChanges();
                ViewData["disable"] = "disable";
                RedirectToPage("Cart");
            }
            return Page();
        }
    }
}

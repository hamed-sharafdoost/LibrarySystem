using LibraryDatabase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace RazorPage.Pages
{
    public class CartModel : PageModel
    {
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
            //Retrieve books which selected by user to borrow them
            var userid = _context.Users.SingleOrDefault(c => c.Email == Request.Cookies["Email"].ToString()).UserId;
            userborrower = _context.UserBorrowers.Where(c => c.UserId == userid).ToList();
            foreach (var list in userborrower)
            {
                var findbook = _context.Borrowers.Include(v => v.Books).FirstOrDefault(c => c.BorrowerId == list.BorrowerId);
                Count = _context.Books.Count(v => v.BooksId == findbook.Books.BooksId && findbook.Books.Availabale == true);
            }
            //Check limitation of selected books that a user can borrow
            if (userborrower.Count > 2 || Count <= 0)
            {
                ViewData["disable"] = "disable";
            }
        }
        public IActionResult OnPost()
        {
            #region UpdateRegion
            User user = new User();
            Books book = new Books();
            int userid;
            user = _context.Users.SingleOrDefault(b => b.Email == Request.Cookies["Email"]);
            userid = user.UserId;
            userborrower = _context.UserBorrowers.Where(c => c.UserId == userid).ToList();
            //Removing books from Cart
            if (string.IsNullOrEmpty(Request.Form["Sub"]))
            {
                var borrower = _context.UserBorrowers.Include(b => b.Borrower).FirstOrDefault(v => v.Borrower.BooksId == BooksId);
                    _context.Borrowers.Remove(borrower.Borrower);
                    _context.UserBorrowers.Remove(borrower);
                    user.NOborrwedbooks = _context.UserBorrowers.Count(x => x.UserId == user.UserId);
                    _context.Users.Update(user);
                    _context.SaveChanges();
                    return RedirectToPage("Cart");
            }
            //Submit selected books
            else
            {
                var finalsubmit = _context.UserBorrowers.Include(v => v.Borrower).Where(x => x.UserId == user.UserId).ToList();
                foreach(var select in finalsubmit) //Updating information of books for submit
                {
                    var selectbook = _context.Books.FirstOrDefault(b => b.BooksId == select.Borrower.BooksId);
                    selectbook.Availabale = false;
                    selectbook.LastBorrowdate = select.Borrower.BorrowExDate;
                    _context.Books.Update(selectbook);
                    _context.SaveChanges();
                }
                user.NOborrwedbooks = finalsubmit.Count();
                _context.Users.Update(user);
                _context.SaveChanges();
                ViewData["disable"] = "disable";
                return Page();
            }
            #endregion

        }
    }
}

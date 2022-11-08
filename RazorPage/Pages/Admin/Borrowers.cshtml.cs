using LibraryDatabase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RazorPage.Models;

namespace RazorPage.Pages.Admin
{
    public class BorrowersModel : PageModel
    {
        [BindProperty]
        public int BooksId { get; set; }
        public List<ListOfborrowers> borrowers2 { get; set; }

        private readonly DbContextLib _context;
        public BorrowersModel(DbContextLib context)
        {
            _context = context;
        }
        public void OnGet()
        {
            List<ListOfborrowers> borrowers = new List<ListOfborrowers>(); //Make a list of binding model that includes users and their books
            var userborrowers = _context.Set<UserBorrower>().ToList();
            foreach (var user in userborrowers)
            {
                int Id = _context.Borrowers.FirstOrDefault(z => z.BorrowerId == user.BorrowerId).BooksId;
                ListOfborrowers entity = new ListOfborrowers { user = _context.Users.FirstOrDefault(b => b.UserId == user.UserId), book = _context.Books.FirstOrDefault(n => n.BooksId == Id) };
                borrowers.Add(entity);
            }
            borrowers2 = borrowers.ToList();
        }
        public IActionResult OnPostDelete()
        {

            var fetch = _context.UserBorrowers.Include(b => b.Borrower).FirstOrDefault(b => b.Borrower.BooksId == BooksId); //find a user and his/her information in Borrower table
            var user = _context.Users.FirstOrDefault(c => c.UserId == fetch.UserId); //find a user in Users table
            var book = _context.Books.FirstOrDefault(z => z.BooksId == fetch.Borrower.BooksId); //find user's books in Books table
            book.Availabale = true;
            book.LastBorrowdate = DateTime.Now;
            user.NOborrwedbooks--;
            _context.UserBorrowers.Remove(fetch);
            _context.Borrowers.Remove(fetch.Borrower);
            _context.Books.Update(book);
            _context.Users.Update(user);
            if (_context.SaveChanges() > 0)
                return RedirectToPage("Borrowers");
            else
            {
                ViewData["Message"] = "Something goes wrong!please try again";
                return Page();
            }
        }
    }
}

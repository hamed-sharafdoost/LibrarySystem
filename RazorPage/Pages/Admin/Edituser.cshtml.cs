using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LibraryDatabase;
using RazorPage.Models;
using Microsoft.EntityFrameworkCore;

namespace RazorPage.Pages.Admin
{
    public class EdituserModel : PageModel
    {
        public User? user;
        [BindProperty]
        public EditUser edit { get; set; } 
        [BindProperty]
        public int UserId { get; set; }
        [BindProperty]
        public IEnumerable<User> Users { get; set; }
        private DbContextLib _context;
        public EdituserModel(DbContextLib context)
        {
            _context = context;
        }
         public void OnGet()
        {
            Users = _context.Set<User>().AsEnumerable();
        }
        public IActionResult OnPostDelete()
        {
            Books book;
            user = _context.Users.SingleOrDefault(c => c.UserId == UserId); // find user in Users table
            if (user != null)
            {
                var userborrower = _context.UserBorrowers.Include(n => n.Borrower).Where(b => b.UserId == user.UserId).ToList();
                if (userborrower != null)
                {
                    _context.UserBorrowers.RemoveRange(userborrower); // Removing all rows related to this user in UserBorrowers table
                    foreach (var select in userborrower) //iterating and update books and removing rows in Borrowers table related to this user
                    {
                       book =_context.Books.FirstOrDefault(v => v.BooksId == select.Borrower.BooksId);
                       book.Availabale = true;
                        _context.Books.Update(book);
                        _context.Borrowers.Remove(select.Borrower);
                    }
                }
                _context.Users.Remove(user);
                _context.SaveChanges();
                return RedirectToAction("");
            }
            else
                return RedirectToAction("");
        }
        public IActionResult OnPostEdit()
        {
            Users = _context.Set<User>().AsEnumerable();
            ViewData["Edit"] = UserId;
            return Page();
        }
        public IActionResult OnPostUpdate()
        {
            Users = _context.Set<User>().AsEnumerable();
            if (edit != null && ModelState.IsValid)
            {
                User user = _context.Users.SingleOrDefault(c => c.UserId == UserId);
                user.Email = edit.Email;
                user.Name = edit.Name;
                user.FamilyName = edit.FamilyName;
                user.PhoneNumber = edit.phonenumber;
                user.NOborrwedbooks = edit.NoBorrowedbooks;
                user.NOLendedBooks = edit.NoLendedbooks;
                user.Password = user.Password;
                user.address.City = edit.City;
                user.address.Street = edit.Street;
                _context.Users.Update(user);
                _context.SaveChanges();
                return RedirectToAction("Edit");
            }
            else
                return Page();
        }
    }
}

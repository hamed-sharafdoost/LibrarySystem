using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LibraryDatabase;
using RazorPage.Models;

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
            user = _context.Users.SingleOrDefault(c => c.UserId == UserId);
            if (user != null)
            {
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

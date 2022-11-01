using System.ComponentModel.DataAnnotations;

namespace RazorPage.Models
{
    public class EditBook
    {
        [StringLength(50)]
        public string Title { get; set; }
        [StringLength(100)]
        public string? Description { get; set; }
        public string Genre { get; set; }
        public bool Availabale { get; set; }
        public string Author { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? LastBorrowdate { get; set; }
    }
}

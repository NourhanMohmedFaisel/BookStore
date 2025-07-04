using System.ComponentModel.DataAnnotations;

namespace BookStore.ViewModels
{
    public class ReviewViewModelWithId
    {
        public int Id { get; set; }

        [Required]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int Rating { get; set; }

        [Required]
        [StringLength(1000, ErrorMessage = "Comment cannot exceed 1000 characters.")]
        public string Comment { get; set; }

        public int BookId { get; set; }

        
        public string? BookTitle { get; set; }
        public string? UserName { get; set; }
        public DateTime ReviewDate { get; set; }
    }
}

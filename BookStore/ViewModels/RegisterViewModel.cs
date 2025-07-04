using System.ComponentModel.DataAnnotations;

namespace BookStore.ViewModels
{
    public class RegisterViewModel
    {
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{6,}$",
    ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, and one number.")]
        public string Password { get; set; }

        [Compare("Password")]
        [Display(Name = "Confirm Password")]

        [DataType(DataType.Password)]
       
        public string ConfirmPassword { get; set; }
        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        [Display(Name = "Phone Number")]
        public string phoneNumber { get; set; }
    }
}

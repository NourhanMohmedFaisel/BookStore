using System.ComponentModel.DataAnnotations;

namespace BookStore.ViewModels
{
    public class LoginUserViewModel
    {

        [Required(ErrorMessage = "*")]
        public string Name { get; set; }
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{6,}$",
    ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, and one number.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember Me!!")]
        public bool RememberMe { get; set; }
    }
}

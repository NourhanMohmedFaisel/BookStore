using System.ComponentModel.DataAnnotations;

namespace BookStore.ViewModels
{
    public class RoleViewModel
    {

        [Display(Name = "Role Name")]
        public string Name { get; set; }
    }
}

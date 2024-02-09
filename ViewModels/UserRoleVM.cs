using System.ComponentModel.DataAnnotations;

namespace DECKMASTER.ViewModels
{
    public class UserRoleVM
    {

        [Key]
        [Required]
        [Display(Name = "Role Name")]
        public string? RoleName { get; set; }

        [Required]
        [Display(Name = "Email")]
        public string? Email { get; set; }


    }
}

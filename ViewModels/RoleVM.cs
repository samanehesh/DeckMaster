using System.ComponentModel.DataAnnotations;

namespace DECKMASTER.ViewModels
{
    public class RoleVM
    {

        [Key]
        [Required]
        [Display(Name = "Role Name")]
        public string? RoleName { get; set; }


    }
}

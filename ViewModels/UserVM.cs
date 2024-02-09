using System.ComponentModel.DataAnnotations;

namespace DECKMASTER.ViewModels
{
    public class UserVM
    {
        [Key]
        [Required]
        [Display(Name = "Email")]
        public string? Email { get; set; }

    }
}

using System.ComponentModel.DataAnnotations;

namespace Truco.ViewModels
{
    public class RecuperarViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
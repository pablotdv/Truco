using System.ComponentModel.DataAnnotations;

namespace Truco.ViewModels
{
    public class RecuperarSenhaViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
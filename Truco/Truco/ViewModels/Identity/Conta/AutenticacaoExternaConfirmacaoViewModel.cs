using System.ComponentModel.DataAnnotations;

namespace Truco.ViewModels
{
    public class AutenticacaoExternaConfirmacaoViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
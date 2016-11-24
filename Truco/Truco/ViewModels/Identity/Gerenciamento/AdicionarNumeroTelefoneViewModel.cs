using System.ComponentModel.DataAnnotations;

namespace Truco.ViewModels
{
    public class AdicionarNumeroTelefoneViewModel
    {
        [Required]
        [Phone]
        [Display(Name = "Número do telefone")]
        public string Numero { get; set; }
    }
}
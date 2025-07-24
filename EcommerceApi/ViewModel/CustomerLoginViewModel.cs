using System.ComponentModel.DataAnnotations;

namespace EcommerceApi.ViewModel
{
    public class CustomerLoginViewModel
    {
        [Required(ErrorMessage = "Email é obrigatório.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Senha é obrigatória.")]
        public string Password { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace EcommerceApi.ViewModel
{
    public class UserLoginViewModel
    {
        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória.")]
        public string Password { get; set; }
    }
}

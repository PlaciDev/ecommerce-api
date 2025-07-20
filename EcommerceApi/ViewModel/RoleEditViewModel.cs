using System.ComponentModel.DataAnnotations;

namespace EcommerceApi.ViewModel
{
    public class RoleEditViewModel
    {
        [Required(ErrorMessage = "O nome do perfil é obrigatório.")]
        [StringLength(10, MinimumLength = 3, ErrorMessage = "O nome do perfil deve conter entre 3 e 10 caracteres.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "A descrição do perfil é obrigatória.")]
        [StringLength(50, MinimumLength = 20, ErrorMessage = "A descrição do perfil deve conter entre 20 e 50 caracteres.")]
        public string Description { get; set; }
    }
}

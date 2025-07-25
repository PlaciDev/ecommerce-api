using System.ComponentModel.DataAnnotations;

namespace EcommerceApi.ViewModel
{
    public class CategoryEditViewModel
    {
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(20, ErrorMessage = "O nome não pode exceder 20 caracteres.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "A descrição é obrigatória.")]
        [StringLength(100, ErrorMessage = "A descrição não pode exceder 100 caracteres.")]
        public string Description { get; set; }
    }
}

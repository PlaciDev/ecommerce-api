using System.ComponentModel.DataAnnotations;

namespace EcommerceApi.ViewModel
{
    public class ProductEditViewModel
    {
        [Required(ErrorMessage = "O nome do produto é obrigatório.")]
        [StringLength(50, ErrorMessage = "O nome do produto deve ter no máximo 50 caracteres.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "A descrição do produto é obrigatória.")]
        [StringLength(100, ErrorMessage = "A descrição do produto deve ter no máximo 100 caracteres.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "O preço do produto é obrigatório.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "O estoque do produto é obrigatório.")]
        public int Stock { get; set; }

        [Required(ErrorMessage = "A categoria do produto é obrigatória.")]
        public string CategoryName { get; set; }


    }
}

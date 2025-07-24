using System.ComponentModel.DataAnnotations;

namespace EcommerceApi.ViewModel
{
    public class AddressEditViewModel
    {
        [Required(ErrorMessage = "A rua é obrigatória")]
        public string Street { get; set; }

        [Required(ErrorMessage = "A cidade é obrigatória")]

        public string City { get; set; }

        [Required(ErrorMessage = "O estado é obrigatório")]

        public string State { get; set; }

        [Required(ErrorMessage = "O CEP é obrigatório")]

        public string ZipCode { get; set; }
 
    }
}

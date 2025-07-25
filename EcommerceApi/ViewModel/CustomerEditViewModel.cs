using System.ComponentModel.DataAnnotations;

namespace EcommerceApi.ViewModel
{
    public class CustomerEditViewModel
    {
        public UserEditViewModel User { get; set; }

        [Required(ErrorMessage = "O telefone é obrigatório.")]
        [Phone(ErrorMessage = "O número de telefone informado é inválido.")]
        public string PhoneNumber { get; set; }

        public AddressEditViewModel Address { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace Project.MvcClient.Models.DTO.User
{
    public class UserCreateDto
    {
        [Required(ErrorMessage = "* O nome é obrigatório!")]
        [MaxLength(100)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "* O sobrenome é obrigatório!")]
        [MaxLength(100)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "* A idade é obrigatória!")]
        [Range(18, 99, ErrorMessage = "* A idade deve estar entre 18 e 99!")]
        public int Age { get; set; }

        [Required(ErrorMessage = "* O email é obrigatório!")]
        [EmailAddress(ErrorMessage = "* O formato do email é inválido!")]
        [MaxLength(150)]
        public string Email { get; set; }
    }
}

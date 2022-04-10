using System.ComponentModel.DataAnnotations;

namespace UsersWeb.Models.View
{
    public class RegisterModel
    {
        [Required]
        [RegularExpression("[a-zA-Z]+", ErrorMessage = "Name must contain only latin letters")]
        public string? Name{ get; set; }

        [Required]
        [EmailAddress]
        public string? Mail { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Values are not equal")]
        public string? ConfirmPassword { get; set; }
    }
}

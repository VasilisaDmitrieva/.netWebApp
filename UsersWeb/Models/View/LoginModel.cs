using System.ComponentModel.DataAnnotations;

namespace UsersWeb.Models.View
{
    public class LoginModel
    {
        [Required]
        public string? Mail{ get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
    }
}

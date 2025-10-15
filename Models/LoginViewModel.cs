using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MyNewApp.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Username or Email is required.")]
        [MaxLength(50, ErrorMessage = "max 20 characters allowed")]
        [DisplayName("Username or Email")]
        public string UserName { get; set; }


        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "max 20 or min 5 characters allowed")]
        public string Password { get; set; }

    }
}

using System.ComponentModel.DataAnnotations;

namespace MyNewApp.Models
{
    public class RegistrationViewModel
    {


        public int Id { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        [MaxLength(50, ErrorMessage = "maz 50 characters allowed")]
        public string FirstName { get; set; }


        [Required(ErrorMessage = "Last name is required.")]
        [MaxLength(50, ErrorMessage = "maz 50 characters allowed")]
        public string LastName { get; set; }


        [Required(ErrorMessage = "Email is required.")]
        [RegularExpression(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Please Enter Valid Email.")]
        [MaxLength(50, ErrorMessage = "maz 50 characters allowed")]
        public string Email { get; set; }


        [Required(ErrorMessage = "Username is required.")]
        [MaxLength(20, ErrorMessage = "maz 20 characters allowed")]
        public string UserName { get; set; }


        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 5, ErrorMessage = "max 20 or min 5 characters allowed")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Please conferm your password.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }


    }
}
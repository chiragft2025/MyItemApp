using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MyNewApp.Models
{
    [Index(nameof(Email),IsUnique = true)]
    [Index(nameof(Username), IsUnique = true)]

    public class UserAccount
    {
        [Key]
        public int Id { get; set; }
        
        [Required(ErrorMessage = "First name is required")]
        [MaxLength(50,ErrorMessage = "Max 50 character is allowed" )]
        public string FirstName { get; set; }

        [MaxLength(50, ErrorMessage = "Max 50 character is allowed")]

        public string LastName { get; set; }

        [MaxLength(50, ErrorMessage = "Max 50 character is allowed")]
        [Required(ErrorMessage ="Email is Required")]
        
        public string Email { get; set; }

        [MaxLength(20, ErrorMessage = "Max 50 character is allowed")]
        [Required(ErrorMessage = "Username is Required")]
        public string Username { get; set; }

        [MaxLength(20, ErrorMessage = "Max 50 character is allowed")]
        [Required(ErrorMessage = "Password is Required")]
       
        public string Password { get; set; }
    }
}

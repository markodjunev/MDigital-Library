namespace MDigitalLibrary.Identity.Models
{
    using System.ComponentModel.DataAnnotations;

    public class RegisterInputModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [MinLength(6, ErrorMessage = "Password should be at least 6 symbols")]
        public string Password { get; set; }
    }
}

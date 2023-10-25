namespace MDigitalLibrary.Identity.Models
{
    using System.ComponentModel.DataAnnotations;

    public class ChangePasswordInputModel
    {
        [Required]
        public string CurrentPassword { get; set; }

        [Required]
        [MinLength(6, ErrorMessage = "Password should be at least 6 symbols")]
        public string NewPassword { get; set; }
    }
}

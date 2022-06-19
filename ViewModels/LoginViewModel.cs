using System.ComponentModel.DataAnnotations;

namespace RunGroopWebApp.ViewModels
{
    public class LoginViewModel

    {
        [Display(Name="Email Address")]
        [Required(ErrorMessage ="Email address is required")]
        public string  EmailAddress { get; set; }
        [Required(ErrorMessage = "Pasword address is required")]
        [DataType(DataType.Password)]   
        public string  Password { get; set; }
    }
}


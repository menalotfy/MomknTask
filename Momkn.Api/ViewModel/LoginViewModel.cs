using System.ComponentModel.DataAnnotations;

namespace Momkn.API.ViewModel
{

    public class LoginViewModel
    {

        [Required]
        //[EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string FCMToken { get; set; }

    }
}

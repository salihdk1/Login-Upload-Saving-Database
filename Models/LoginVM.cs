using System.ComponentModel.DataAnnotations;

namespace LDap.Models
{
    public class LoginVM
    {
        [Required(ErrorMessage = "Kullanıcı adı gereklidir.")]
        public required string username { get; set; }

        [Required(ErrorMessage = "Parola gereklidir.")]
        [DataType(DataType.Password)]
        public required string password { get; set; }

        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }
    }
}

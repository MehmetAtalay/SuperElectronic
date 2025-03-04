using System.ComponentModel.DataAnnotations;

namespace SuperElectronic.Models
{
    public class PasswordDTO
    {
        [Required(ErrorMessage = "Lutfen Bir Sifre Giriniz"), MaxLength(50)]
        public string Password { get; set; } = "";
        [Required(ErrorMessage = "Lutfen Bir Sifre Giriniz"), MaxLength(50)]
        public string NewPassword { get; set; } = "";

        [Required(ErrorMessage = "Pasaportu Onaylamaniz Gereklidir")]
        [Compare("Password", ErrorMessage = "Sifre ile Onayli Sifre uyusmuyor")]
        public string ConfirmPassword { get; set; } = "";
    }
}

using System.ComponentModel.DataAnnotations;

namespace SuperElectronic.Models
{

    public class RegisterDto
    {
        [Required(ErrorMessage = "Lutfen bir isim giriniz"), MaxLength(100)]
        public string FirstName { get; set; } = "";

        [Required(ErrorMessage = "Lutfen Bir Soyisim giriniz"), MaxLength(100)]
        public string LastName { get; set; } = "";

        [Required(ErrorMessage ="Lutfen gecerli bir email adresi giriniz"), EmailAddress, MaxLength(100)]
        public string Email { get; set; } = "";

        [Required(ErrorMessage = "Lutfen Bir Telefon Numarasi Giriniz"), MaxLength(20)]
        //[RegularExpression
        //((@"^(\+\d{1, 2}\s)?\(?\d{3}\)?[\s.-]\d{3}[\s.-]\d{4}$"),
        //ErrorMessage = "Gecerli Numara Giriniz Lutfen")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage ="Lutfen Bir Adres Giriniz"), MaxLength(200)]
        public string Address { get; set; } = "";

        [Required(ErrorMessage ="Lutfen Bir Sifre Giriniz"), MaxLength(50)]
        public string Password { get; set; } = "";

        [Required(ErrorMessage = "Pasaportu Onaylamaniz Gereklidir")]
        [Compare("Password", ErrorMessage = "Sifre ile Onayli Sifre uyusmuyor")]
        public string ConfirmPassword { get; set; } = "";
    }
}


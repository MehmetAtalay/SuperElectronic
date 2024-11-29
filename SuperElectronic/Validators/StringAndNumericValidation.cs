using Microsoft.Extensions.FileSystemGlobbing.Internal;
using Microsoft.Extensions.FileSystemGlobbing;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.RegularExpressions;


namespace SuperElectronic.Validators
{
    public sealed class StringAndNumericValidation :ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            //Regex ile name kismina sadece sayi yazilip yazilmadigina baktim ,  donen cevabi isNumbera atadim.
            bool isNumber = Regex.IsMatch((string)value!, @"^[-?\d_]*$");

            //Bir method ile ozel karakter icerip icermedigine baktim ozel karakter varsa islem iptal edildi
            //Ayni sekilde sadece sayi olup olmamasini kontrol ettim.
            bool ozelKarakterKontrolu = OzelKarakterIceriyormu((string)value!);
            if (isNumber)
            {
                return new ValidationResult("Sadece Sayi Giremessiniz" /*[validationContext.MemberName]*/);
            }
            if (!ozelKarakterKontrolu)
            {
                return new ValidationResult("Isimde ozel karakter kullanamazsiniz");
            }
            return ValidationResult.Success;
        }

        // Any methoduyla stringi enumarable obje gibi kontrol edip herhangi bir charin ozel karakter icerip icermedigine baktim.
        // Ozel karakter yoksa True varsa false doner.
        private bool OzelKarakterIceriyormu (string str)
        {
            return str.All(ch => char.IsLetterOrDigit(ch));
        }
    }
}


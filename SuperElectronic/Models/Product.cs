using Microsoft.EntityFrameworkCore;
using SuperElectronic.Validators;
using System.ComponentModel.DataAnnotations;

namespace SuperElectronic.Models
{
    public class Product
    {
        //ID
        public int Id { get; set; }

        //Urun Adi
        [Required(ErrorMessage = "Lutfen isim alanini bos birakmayiniz")]
        [MaxLength(100)]
        [StringAndNumericValidation]
        public string Name { get; set; } = "";
        //Urun Markasi
        [MaxLength(100)]
        public string Brand { get; set; } = "";
        // Urunun ait oldugu katogeri
        [MaxLength(100)]
        public string Category { get; set; } ="";
        //Urunun Fiyati
        [Precision(16,2)]
        //Bu regularExpression ile sayi veya decimal kabul ediyo .dan sonra 2 rakama kadar.
        [RegularExpression((@"^[1-9]\d*(\.\d+)?$"), ErrorMessage = "Girdiginiz bir sayi olmali")]
        public decimal Price { get; set; }
        //Urunun Aciklamasi
        //[MaxLength(1000)] Aciklamaya limit koymak istemedim ama yinede belli bir limit olsa
        //iyi olur.
        [Required(ErrorMessage = "Lutfen Urun bilgilerini Giriniz")]
        public string Description { get; set; } = "";
        //Urun Resmi Dosya Adi
        [MaxLength(100)]
        public string ImageFileName { get; set; } = "";
        //Urunun olusturuldugu tarih
        public DateTime CreatedAt { get; set; }

    }
}

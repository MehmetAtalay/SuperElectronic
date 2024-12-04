using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using FluentValidation;
using SuperElectronic.Validators;
using System.Runtime.CompilerServices;


namespace SuperElectronic.Models
{
    
   
    
    public class ProductDTO
    {

        //Name
        //Brand
        //Category
        //Price
        //Description
        //ImageFile

        Product product = new Product();
        //IDyi almadim cunku ID otomatik olusturulcak sql tarafinda kendisi unique bir id atiycak
        //[Required(ErrorMessage ="Bu alan bos birakilamaz")]
        [Required(ErrorMessage ="Lutfen isim alanini bos birakmayiniz")]
        [StringAndNumericValidation]
        public string Name { get; set; } = "";
        //Urun Markasi
        [Required(ErrorMessage ="Lutfen bir Marka Giriniz"),MaxLength(100),]
        public string Brand { get; set; } = "";
        // Urunun ait oldugu katogeri
        [Required, MaxLength(100)]
        public string Category { get; set; } = "";
        //Urunun Fiyati
        //[Precision(16, 2)]
        //Precisionu kaldirdim cunku bu veritabani icin gerekli
        [Required(ErrorMessage ="Fiyat giriniz Lutfen")]
        [Range(0,int.MaxValue,ErrorMessage ="Girdiginiz Fiyat Cok yuksek") ]
        //Bu regularExpression ile sayi veya decimal kabul ediyo .dan sonra 2 rakama kadar.
        [RegularExpression((@"^[1-9]\d*(\.\d+)?$"), ErrorMessage = "Girdiginiz bir sayi olmali")]
       
        public decimal Price { get; set; }
        //Urunun Aciklamasi
        //[MaxLength(1000)] Aciklamaya limit koymak istemedim ama yinede belli bir limit olsa
        //iyi olur.
        [Required(ErrorMessage ="Lutfen Urun bilgilerini Giriniz")]
        public string Description { get; set; } = "";
        //Urun Resmi Dosya Adi
        //Burda modeldekinin aksine image File adi deil imagin direk kendisi gerekicek.
        //O yuzden direk ImageFile Dedim
        //IFormFile interfacesini kullandim  cunku bir istekle gonderilen bir dosya olcak(HTTPREQUEST)
        //Bu optional cunku urunu olustururken bize lazim ama update ederken deistirilmeyebilir.
        [ImageValidation]
        public IFormFile? ImageFile { get; set; }
        //Urunun olusturuldugu tarih

        //Created At i almadim cunku created atide sistem kendisi atiycak.
    }
}

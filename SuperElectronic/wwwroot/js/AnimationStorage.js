

// EvenListener ekleyerek Sayfa yuklendiginde querySelectorla Idsi AnasayfaLogo Olani Sectik
//Displayed olarak sessionStorageye set ettik
//Sonra if ile get item ile AnasayfaLogo varmi diye kontrol ettik
// Eger varsa Animate etmekden sorumlu Classini Remove ettik
window.addEventListener('load', () => {
    document.querySelector('#AnaSayfaLogo') !== null
    {
        window.sessionStorage.setItem('AnaSayfaLogo', 'displayed');
    }
})
if (window.sessionStorage.getItem('AnaSayfaLogo')) {
    document.querySelector('#AnaSayfaLogo').classList.remove('animated')
}
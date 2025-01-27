using beestje_op_je_feestje.Models;
using System.ComponentModel.DataAnnotations;

namespace beestje_op_je_feestje.ViewModels
{
    public class ContactDetailsViewModel
    {
        [Required(ErrorMessage = "Voornaam is verplicht.")]
        public string First_Name { get; set; }
        public string? Middle_Name { get; set; }
        [Required(ErrorMessage = "Achternaam is verplicht.")]
        public string Last_Name { get; set; }
        [Required(ErrorMessage = "Straatnaam is verplicht.")]
        public string Street_Name { get; set; }
        [Required(ErrorMessage = "Huisnummer is verplicht.")]
        public int Street_Number { get; set; }
        [Required(ErrorMessage = "Woonplaats is verplicht.")]
        public string City { get; set; }
        [Required(ErrorMessage = "email-adres is verplicht.")]
        public string Email { get; set; }
        public DateTime SelectedDate { get; set; }
        public TypeEnum DiscountType { get; set; }
        public List<Animal> Animals { get; set; }
        public List<int> SelectedIdAnimals { get; set; }
        public string ImageUrl { get; set; }
    }
}
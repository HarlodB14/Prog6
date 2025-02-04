using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace beestje_op_je_feestje.ViewModels
{
    public class AccountViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Voornaam is verplicht.")]
        public string First_Name { get; set; }

        [Required(ErrorMessage = "Achternaam is verplicht.")]
        public string Last_Name { get; set; }

        [Required(ErrorMessage = "Straatnaam is verplicht.")]
        public string Street_Name { get; set; }

        [Required(ErrorMessage = "Huisnummer is verplicht.")]
        [Range(1, int.MaxValue, ErrorMessage = "Huisnummer moet een positief getal zijn.")]
        public int? Street_Number { get; set; }

        [Required(ErrorMessage = "Stad is verplicht.")]
        public string City { get; set; }

        [Required(ErrorMessage = "Kortingstype is verplicht.")]
        public string DiscountType { get; set; }

        public string? Email { get; set; }
        private string _password {  get; set; }
        public string? PhoneNumber { get; set; }

        public List<string> DiscountTypes { get; set; } = new List<string> { "Silver", "Gold", "Platinum", "Nothing" };
    }
}

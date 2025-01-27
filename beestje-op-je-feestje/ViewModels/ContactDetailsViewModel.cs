using beestje_op_je_feestje.Models;

namespace beestje_op_je_feestje.ViewModels
{
    public class ContactDetailsViewModel
    {
        public string First_Name { get; set; }
        public string Middle_Name { get; set; }
        public string Last_Name { get; set; }
        public string Street_Name { get; set; }
        public int Street_Number { get; set; }
        public string City { get; set; }
        public string Email { get; set; }
        public DateTime SelectedDate { get; set; }
        public TypeEnum DiscountType { get; set; }
        public List<Animal> Animals { get; set; }
        public List<int> SelectedIdAnimals { get; set; }
        public string ImageUrl { get; set; }
    }
    }
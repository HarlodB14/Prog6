using beestje_op_je_feestje.Models;
using System.ComponentModel.DataAnnotations;

namespace beestje_op_je_feestje.ViewModels
{
    public class BookingViewModel
    {
        public int Id { get; set; }
        [DataType(DataType.Date)]
        [FutureDate(ErrorMessage = "De geselecteerde datum moet in de toekomst liggen.")]
        [Required(ErrorMessage = "Selecteer een dag.")]
        public DateTime SelectedDate { get; set; }
        public List<Animal> Animals { get; set; } = new List<Animal>();

        public string DiscountType { get; set; } 

        public List<int> SelectedIdAnimals { get; set; } = new List<int>();
    }
}

using System.ComponentModel.DataAnnotations;

namespace beestje_op_je_feestje.ViewModels
{
    public class BookingViewModel
    {
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Selecteer een dag.")]
        //[FutureDate(ErrorMessage = "Selecteer minimaal een dag vanaf vandaag.")]
        public DateTime SelectedDate { get; set; }
    }
}

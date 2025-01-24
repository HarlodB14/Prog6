using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace beestje_op_je_feestje.Models
{
    public class Booking
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Account")]
        public int? UserId { get; set; }
        public DateTime SelectedDate { get; set; }
        public virtual ICollection<Animal> Animals { get; set; } 
        public int AmountOfAnimals { get; set; }



    }
}
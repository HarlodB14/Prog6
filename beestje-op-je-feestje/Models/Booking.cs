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

        [InverseProperty("Booking")]
        public virtual ICollection<Animal>? Animals { get; set; } = new List<Animal>();
        public int AmountOfAnimals { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace beestje_op_je_feestje.Models
{
    public class Animal
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Booking")]
        public int? BookingId { get; set; }
        public virtual Booking? Booking { get; set; } 

        [Required]
        public string Name { get; set; }
        [Required]
        public TypeEnum Type { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public string ImageUrl { get; set; }
        public bool IsBooked { get; set; }
        public DateTime? BookingDate { get; set; }
    }
}

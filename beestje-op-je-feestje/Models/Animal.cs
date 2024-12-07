using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace beestje_op_je_feestje.Models
{
    public class Animal
    {
        [Key]
        public int Id {  get; set; }
        [Required]
        public string name { get; set; }
        [Required]
        public Type type { get; set; }
        [Required]
        public decimal price { get; set; }
        [Required]
        public string imageUrl { get; set; }





    }
}

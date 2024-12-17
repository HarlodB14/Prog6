using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace beestje_op_je_feestje.Models
{
    public class Animal
    {
        [Key]
        public int Id {  get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public Type Type { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public string ImageUrl { get; set; }





    }
}

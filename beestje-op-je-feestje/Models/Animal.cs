using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace beestje_op_je_feestje.Models
{
    public class Animal
    {
        [Key]
        public int _id {  get; set; }
        [Required]
        public string _name { get; set; }
        [Required]
        public Type _type { get; set; }
        [Required]
        public decimal _price { get; set; }
        [Required]
        public string _imageUrl { get; set; }





    }
}

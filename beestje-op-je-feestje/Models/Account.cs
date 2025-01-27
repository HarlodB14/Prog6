using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace beestje_op_je_feestje.Models
{
    public class Account
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string First_Name { get; set; }
        [Required]
        public string? Middle_Name { get; set; }
        public string Last_Name { get; set; }
        [Required]
        public string Street_Name { get; set; }
        [Required]
        public int Street_Number { get; set; }
        [Required]
        public string City { get; set; }
        public string DiscountType { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
    }
}

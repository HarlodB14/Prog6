using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace beestje_op_je_feestje.Models
{
    public class Account : IdentityUser
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }
        public string DiscountType { get; set; }
        public string? Email { get; set; }
        [Required]
        public string? PhoneNumber { get; set; }
    }
}

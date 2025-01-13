using System.ComponentModel.DataAnnotations;

namespace beestje_op_je_feestje.ViewModels
{
    public class AccountViewModel
    {
        public int Id { get; set; }
        [Required]
        public string First_name { get; set; }
        [Required]
        public string Last_name { get; set; }
        [Required]
        public string Street_Name { get; set; }
        [Required]
        public int Street_Number { get; set; }
        [Required]
        public string City { get; set; }
        public string DiscountType { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }

    }
}

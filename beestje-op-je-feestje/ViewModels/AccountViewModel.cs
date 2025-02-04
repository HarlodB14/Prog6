using System.ComponentModel.DataAnnotations;

namespace beestje_op_je_feestje.ViewModels
{
    public class AccountViewModel
    {
        public int Id { get; set; }
        [Required]
        public string First_Name { get; set; }
        [Required]
        public string Last_Name { get; set; }
        [Required]
        public string Street_Name { get; set; }
        [Required]
        public int Street_Number { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string DiscountType { get; set; }
        [EmailAddress]
        public string? Email { get; set; }
        private string _password {  get; set; }
        [Phone]
        public string? PhoneNumber { get; set; }
        public List<string> DiscountTypes { get; set; } = new List<string> { "Silver", "Gold", "Platinum","Nothing" };
    }
}

using beestje_op_je_feestje.Models;
using Type = beestje_op_je_feestje.Models.Type;

namespace beestje_op_je_feestje.ViewModels
{
    public class AnimalViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Type Type { get; set; }

        public decimal Price { get; set; }

        public string ImageUrl { get; set; }
    }
}

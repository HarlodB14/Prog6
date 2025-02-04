using beestje_op_je_feestje.Models;
using TypeEnum = beestje_op_je_feestje.Models.TypeEnum;

namespace beestje_op_je_feestje.ViewModels
{
    public class AnimalViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public TypeEnum Type { get; set; }

        public decimal Price { get; set; }

        public string? ImageUrl { get; set; }
    }
}

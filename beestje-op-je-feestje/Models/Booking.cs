namespace beestje_op_je_feestje.Models
{
    public class Booking
    {
        private List<Animal> animals { get; set; }

        public int AmountOfAnimals { get; set; }

        public Booking()
        {
            animals = new List<Animal>();
        }
        public void AddAnimal(Animal animal)
        {
            animals.Add(animal);
        }

        public List<Animal> GetAllAnimals()
        {
            return animals;
        }

        public int GetAmountOfAnimals() => GetAllAnimals().Count;
    }
}
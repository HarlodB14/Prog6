namespace beestje_op_je_feestje.Models
{
    public class Booking
    {
        private List<Animal> Animals { get; set; }

        public DateTime SelectedDate { get; set; }

        public int AmountOfAnimals { get; set; }

        public Booking()
        {
            Animals = new List<Animal>();
        }
        public void AddAnimal(Animal animal)
        {
            Animals.Add(animal);
        }

        public List<Animal> GetAllAnimals()
        {
            return Animals;
        }

        public int GetAmountOfAnimals => Animals.Count;
    }
}
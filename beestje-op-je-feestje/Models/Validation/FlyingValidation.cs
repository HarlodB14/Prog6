namespace beestje_op_je_feestje.Models.Validation
{
    public class FlyingValidation : IGeneralValidation
    {
        private const int MaxFlyingAnimals = 2; 

        public bool Validate(List<Animal> animals, Booking booking)
        {
            int flyingAnimalsCount = animals.Count(animal => animal.Type == TypeEnum.flying);

            if (flyingAnimalsCount > MaxFlyingAnimals)
            {
                return false; 
            }

            return true;
        }
    }
}

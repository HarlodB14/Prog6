namespace beestje_op_je_feestje.Models.Validation
{
    public class RestrictedAnimalValidation : IGeneralValidation
    {
        public bool Validate(List<Animal> animals, Booking booking)
        {
            bool hasRestrictedAnimal = animals.Any(animal => animal.Name == "Leeuw" || animal.Name == "IJsbeer");

            bool hasBoerderijAnimal = animals.Any(animal => animal.Type == TypeEnum.farm);

            if (hasRestrictedAnimal && hasBoerderijAnimal)
            {
                return false;
            }

            return true;
        }
    }
}

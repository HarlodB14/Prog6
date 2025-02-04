
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace beestje_op_je_feestje.Models.Validation
{
    public class DateRestriction : IGeneralValidation
    {
        public bool Validate(List<Animal> animals, Booking booking)
        {
            bool hasPinguin = animals.Any(animal => animal.Name == "Pinguïn");
            bool hasDessertAnimal = animals.Any(animal => animal.Type == TypeEnum.dessert);
            bool hasSnowAnimal = animals.Any(animal => animal.Type == TypeEnum.snow);
            bool isWeekend = IsWeekend(booking);
            bool isWinter = IsWinterSeason(booking);
            bool isSummer = IsSummerSeason(booking);

            if (hasPinguin && isWeekend)
            {
                return true;
            }
            if (hasDessertAnimal && isWinter)
            {
                return true;
            }
            if (hasSnowAnimal && isSummer) {
                return true;
            }

            return false;

        }
        //check voor in de zomer
        private bool IsSummerSeason(Booking booking)
        {
            DateTime selectedDate = booking.SelectedDate;
            int specificMonth = selectedDate.Month;
            bool isSummer = (specificMonth >= 6 && specificMonth <= 8);
            return isSummer;
        }

        //check of boekingsdatum niet voor in het weekend staat
        public bool IsWeekend(Booking booking)
        {
            DateTime selectedDate = booking.SelectedDate;
            if ((selectedDate.DayOfWeek == DayOfWeek.Saturday) || (selectedDate.DayOfWeek == DayOfWeek.Sunday))
            {
                return true;
            }
            return false;
        }

        //check voor in de winter
        public bool IsWinterSeason(Booking booking)
        {
            DateTime selectedDate = booking.SelectedDate;

            int specificMonth = selectedDate.Month;

            bool isWinter = (specificMonth >= 10 && specificMonth <= 12) ||
                            (specificMonth >= 1 && specificMonth <= 2);

            return isWinter;
        }

    }
}

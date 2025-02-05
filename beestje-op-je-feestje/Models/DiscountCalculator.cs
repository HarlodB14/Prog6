namespace beestje_op_je_feestje.Models
{
    public class DiscountCalculator
    {
        private string _discountType;
        private double percentage = 0.0;

        public DiscountCalculator(string typeOfDiscount)
        {
            _discountType = typeOfDiscount;
        }

        public double BookingDayDiscount(DateTime bookingDate)
        {
            if (bookingDate.DayOfWeek == DayOfWeek.Monday || bookingDate.DayOfWeek == DayOfWeek.Tuesday)
            {
                return 15.0;
            }
            return 0.0;
        }

        public double AnimalNameDiscount(List<Animal> animals)
        {
            double animalNameDiscount = 0.0;

            foreach (Animal animal in animals)
            {
                if (animal != null && !string.IsNullOrEmpty(animal.Name))
                {
                    foreach (char letter in animal.Name.ToUpper())
                    {
                        if (letter >= 'A' && letter <= 'Z') 
                        {
                            int additionalDiscount = letter - 'A' + 2; 
                            animalNameDiscount += additionalDiscount;
                        }
                    }
                }
            }

            return animalNameDiscount;
        }

        public double DiscountCardDiscount(bool hasDiscountCard)
        {
            return hasDiscountCard ? 10.0 : 0.0;
        }

        public double ThreeOfAKind(List<Animal> animals)
        {
            int limit = animals.Count - 1;
            double threeOfAKindDiscount = 0.0;

            for (int i = 0; i < limit - 1; i++)
            {
                if (animals[i] != null && animals[i + 1] != null && animals[i + 2] != null)
                {
                    if (animals[i].Type.Equals(animals[i + 1].Type) && animals[i + 1].Type.Equals(animals[i + 2].Type))
                    {
                        threeOfAKindDiscount = 10.0;
                        break;
                    }
                }
            }
            return threeOfAKindDiscount;
        }

        public double DuckDiscount(List<Animal> animals)
        {
            double duckDiscount = 0.0;
            Random random = new Random();

            foreach (Animal animal in animals)
            {
                if (animal != null && animal.Name == "Eend")
                {
                    int chance = random.Next(1, 7);  
                    if (chance == 1)
                    {
                        duckDiscount = 50.0;
                        break;  
                    }
                }
            }

            return duckDiscount;
        }

        public double ApplyMaxDiscount(double totalDiscount)
        {
            return totalDiscount > 60.0 ? 60.0 : totalDiscount;
        }
        public double CalculateTotalDiscount(List<Animal> animals, DateTime bookingDate, bool hasDiscountCard)
        {
            double totalDiscount = 0.0;

            // Sample logic for handling discount types based on the string value
            if (_discountType == "ThreeOfKind")
            {
                totalDiscount += ThreeOfAKind(animals);
            }
            else if (_discountType == "Duck")
            {
                totalDiscount += DuckDiscount(animals);
            }
            else if (_discountType == "MondayDiscount" || _discountType == "TuesdayDiscount")
            {
                totalDiscount += BookingDayDiscount(bookingDate);
            }
            totalDiscount += AnimalNameDiscount(animals);
            totalDiscount += DiscountCardDiscount(hasDiscountCard);
            totalDiscount = ApplyMaxDiscount(totalDiscount);

            return totalDiscount;
        }
    }
}

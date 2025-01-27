using beestje_op_je_feestje.Models;
using Microsoft.EntityFrameworkCore;

namespace beestje_op_je_feestje.DAL
{
    public class AnimalRepo
    {
        private AnimalPartyContext _animalPartyContext;

        public AnimalRepo(AnimalPartyContext context)
        {
            _animalPartyContext = context;
        }

        public async Task InsertNewAnimalAsync(Animal animal)
        {
            _animalPartyContext.Add(animal);
            await SaveChangesAsync();
        }

        public List<Animal> GetAllAnimals()
        {
            var animals = _animalPartyContext.Animals.ToList();

            return animals;
        }

        public async Task<List<Animal>> GetAnimalsByIdsAsync(List<int> animalIds)
        {
            return await _animalPartyContext.Animals
                .Where(a => animalIds.Contains(a.Id))
                .ToListAsync();
        }

        public Animal GetAnimalById(int id)
        {
            var animal = _animalPartyContext.Animals.FirstOrDefault(a => a.Id == id);
            return animal;
        }

        public void DeleteAnimalById(int id)
        {
            var animal = GetAnimalById(id);
            _animalPartyContext.Animals.Remove(animal);
            _animalPartyContext.SaveChanges();

            if (!_animalPartyContext.Animals.Any())
            {
                _animalPartyContext.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('animals', RESEED, 0)");
            }
        }

        public async Task SaveChangesAsync()
        {
            await _animalPartyContext.SaveChangesAsync();
        }
    }
}

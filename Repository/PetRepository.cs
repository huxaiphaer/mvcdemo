using System.Collections.Generic;
using System.Linq;
using mvcdemo.Data;
using mvcdemo.Models;
using Int32 = System.Int32;

namespace mvcdemo.Repository
{
    public class PetRepository : IPetRepository
    {
        public ApplicationDbContext _context;

        public PetRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Create(Pet pet)
        {
            _context.Pets.Add(pet);
            _context.SaveChanges();
        }

        public void Edit(Pet pet)
        {
            _context.Pets.Update(pet);
            _context.SaveChanges();
        }

        public Pet GetSinglePet(int Id)
        {
            var pet = _context.Pets.FirstOrDefault(p => p.Id == Id);
            return pet;
        }

        public void Delete(Pet pet)
        {
            _context.Pets.Remove(pet);
            _context.SaveChanges();
        }

        public List<Pet> GetAllPets()
        {
            return _context.Pets.ToList();
        }

        public bool VerifyName(string name)
        {
            
            return _context.Pets.Any(p => p.Name.Equals(name));
        }

        public List<Pet> SearchPets(string search)
        {

            int age;
            bool success = Int32.TryParse(search, out age);

            if (!success)
                age = 0;
            
            
            return _context.Pets
                .Where(p =>
                p.Name.Contains(search)
            || p.Age == age ||
            p.Color.Contains(search)).ToList();
        }
    }
}
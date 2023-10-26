using Microsoft.EntityFrameworkCore;
using PracticaParcialApi.DAL;
using PracticaParcialApi.DAL.Entities;
using PracticaParcialApi.Domain.Interfaces;
using System.Diagnostics.Metrics;

namespace PracticaParcialApi.Domain.Services
{
    public class AnimalService : IAnimalService
    {

        private readonly DataBaseContext _context;

        public AnimalService(DataBaseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Animal>> GetAnimalsByOwnerIdAsync(Guid ownerId)
        {
            var animal = await _context.Animals.Where(a => a.OwnerId == ownerId).ToListAsync();
            return animal;
        }


        public async Task<Animal> CreateAnimalAsync(Animal animal, Guid ownerId)
        {
            try
            {
                animal.Id = Guid.NewGuid(); //Así se asigna automáticamente un ID a un nuevo registro
                animal.CreatedDate = DateTime.Now;
                animal.OwnerId = ownerId;
                animal.Owner = await _context.Owners.FirstOrDefaultAsync(o => o.Id == ownerId);

                _context.Animals.Add(animal); //Aquí estoy creando el objeto Country en el contexto de mi BD
                await _context.SaveChangesAsync(); //Aquí ya estoy yendo a la BD para hacer el INSERT en la tabla Countries
                return animal;
            }
            catch (DbUpdateException dbUpdateException)
            {
                //Esta exceptión me captura un mensaje cuando el país YA EXISTE (Duplicados)
                throw new Exception(dbUpdateException.InnerException?.Message ?? dbUpdateException.Message); //Coallesences Notation --> ??
            }
        }

        public async Task<Animal> GetAnimalByIdAsync(Guid id)
        {
            return await _context.Animals.FirstOrDefaultAsync(a => a.Id == id);
        }
        

        public async Task<Animal> EditAnimalAsync(Animal animal, Guid id)
        {
            try
            {
                animal.ModifiedDate = DateTime.Now;

                _context.Animals.Update(animal); //El método Update que es de EF CORE me sirve para Actualizar un objeto
                await _context.SaveChangesAsync();

                return animal;
            }
            catch (DbUpdateException dbUpdateException)
            {
                throw new Exception(dbUpdateException.InnerException?.Message ?? dbUpdateException.Message);
            }

        }

        public async Task<Animal> DeleteAnimalAsync(Guid id)
        {
            try
            {
                //Aquí, con el ID que traigo desde el controller, estoy recuperando el país que luego voy a eliminar.
                //Ese país que recupero lo guardo en la variable country
                var animal = await _context.Animals.FirstOrDefaultAsync(a => a.Id == id);
                if (animal == null) return null; //Si el país no existe, entonces me retorna un NULL

                _context.Animals.Remove(animal);
                await _context.SaveChangesAsync();

                return animal;
            }
            catch (DbUpdateException dbUpdateException)
            {
                throw new Exception(dbUpdateException.InnerException?.Message ?? dbUpdateException.Message);
            }
        }
        
    }
}

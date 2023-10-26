using Microsoft.EntityFrameworkCore;
using PracticaParcialApi.DAL;
using PracticaParcialApi.DAL.Entities;
using PracticaParcialApi.Domain.Interfaces;
using System.Diagnostics.Metrics;

namespace PracticaParcialApi.Domain.Services
{
    public class OwnerService : IOwnerService
    {

        private readonly DataBaseContext _context;

        public OwnerService(DataBaseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Owner>> GetOwnersAsync()
        {
            var owners = await _context.Owners.ToListAsync();   
            return owners;
        }

        public async Task<Owner> CreateOwnerAsync(Owner owner)
        {
            try
            {
                owner.Id = Guid.NewGuid(); //Así se asigna automáticamente un ID a un nuevo registro
                owner.CreatedDate = DateTime.Now;

                _context.Owners.Add(owner); //Aquí estoy creando el objeto Country en el contexto de mi BD
                await _context.SaveChangesAsync(); //Aquí ya estoy yendo a la BD para hacer el INSERT en la tabla Countries
                return owner;
            }
            catch (DbUpdateException dbUpdateException)
            {
                //Esta exceptión me captura un mensaje cuando el país YA EXISTE (Duplicados)
                throw new Exception(dbUpdateException.InnerException?.Message ?? dbUpdateException.Message); //Coallesences Notation --> ??
            }
        }

        public async Task<Owner> GetOwnerByIdAsync(Guid id)
        {
            return await _context.Owners.FirstOrDefaultAsync(o => o.Id == id);
        }


        public async Task<Owner> GetOwnerByNameAsync(string name, string lastName)
        {
            return await _context.Owners.FirstOrDefaultAsync(o => o.Name == name && o.LastName == lastName);
        }

        

        public async Task<Owner> EditOwnerAsync(Owner owner)
        {
            try
            {

                owner.ModifiedDate = DateTime.Now;

                _context.Owners.Update(owner); //El método Update que es de EF CORE me sirve para Actualizar un objeto
                await _context.SaveChangesAsync();

                return owner;
            }
            catch (DbUpdateException dbUpdateException)
            {
                throw new Exception(dbUpdateException.InnerException?.Message ?? dbUpdateException.Message);
            }
        }

        public async Task<Owner> DeleteOwnerAsync(Guid id)
        {
            try
            {
                //Aquí, con el ID que traigo desde el controller, estoy recuperando el país que luego voy a eliminar.
                //Ese país que recupero lo guardo en la variable country
                var owner = await _context.Owners.FirstOrDefaultAsync(o => o.Id == id);
                if (owner == null) return null; //Si el país no existe, entonces me retorna un NULL

                _context.Owners.Remove(owner);
                await _context.SaveChangesAsync();

                return owner;
            }
            catch (DbUpdateException dbUpdateException)
            {
                throw new Exception(dbUpdateException.InnerException?.Message ?? dbUpdateException.Message);
            }
        }

        
    }
}

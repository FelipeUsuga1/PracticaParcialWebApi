using Microsoft.EntityFrameworkCore;
using PracticaParcialApi.DAL;
using PracticaParcialApi.DAL.Entities;
using PracticaParcialApi.Domain.Interfaces;
using System.Diagnostics.Metrics;

namespace PracticaParcialApi.Domain.Services
{
    public class AppointmentService : IAppointmentService
    {

        private readonly DataBaseContext _context;

        public AppointmentService(DataBaseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsByAnimalIdAsync(Guid animalId)
        {
            var animal = await _context.Appointments.Where(a => a.AnimalId == animalId).ToListAsync();
            return animal;
        }       

        public async Task<Appointment> CreateAppointmentAsync(Appointment appointment, Guid animalId)
        {
            try
            {
                appointment.Id = Guid.NewGuid(); //Así se asigna automáticamente un ID a un nuevo registro
                appointment.CreatedDate = DateTime.Now;
                appointment.Fecha = DateTime.Now;
                appointment.AnimalId = animalId;
                appointment.Animal = await _context.Animals.FirstOrDefaultAsync(a => a.Id == animalId);

                _context.Appointments.Add(appointment); //Aquí estoy creando el objeto Country en el contexto de mi BD
                await _context.SaveChangesAsync(); //Aquí ya estoy yendo a la BD para hacer el INSERT en la tabla Countries
                return appointment;
            }
            catch (DbUpdateException dbUpdateException)
            {
                //Esta exceptión me captura un mensaje cuando el país YA EXISTE (Duplicados)
                throw new Exception(dbUpdateException.InnerException?.Message ?? dbUpdateException.Message); //Coallesences Notation --> ??
            }
        }

        public async Task<Appointment> GetAppointmentByIdAsync(Guid id)
        {
            return await _context.Appointments.FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Appointment> EditAppointmentAsync(Appointment appointment, Guid id)
        {
            try
            {
                appointment.ModifiedDate = DateTime.Now;

                _context.Appointments.Update(appointment); //El método Update que es de EF CORE me sirve para Actualizar un objeto
                await _context.SaveChangesAsync();

                return appointment;
            }
            catch (DbUpdateException dbUpdateException)
            {
                throw new Exception(dbUpdateException.InnerException?.Message ?? dbUpdateException.Message);
            }
        }

        public async Task<Appointment> DeleteAppointmentAsync(Guid id)
        {
            try
            {
                //Aquí, con el ID que traigo desde el controller, estoy recuperando el país que luego voy a eliminar.
                //Ese país que recupero lo guardo en la variable country
                var appointment = await _context.Appointments.FirstOrDefaultAsync(a => a.Id == id);
                if (appointment == null) return null; //Si el país no existe, entonces me retorna un NULL

                _context.Appointments.Remove(appointment);
                await _context.SaveChangesAsync();

                return appointment;
            }
            catch (DbUpdateException dbUpdateException)
            {
                throw new Exception(dbUpdateException.InnerException?.Message ?? dbUpdateException.Message);
            }
        }

        


    }
}

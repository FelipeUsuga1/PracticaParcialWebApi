using PracticaParcialApi.DAL.Entities;

namespace PracticaParcialApi.Domain.Interfaces
{
    public interface IAppointmentService
    {
        Task<IEnumerable<Appointment>> GetAppointmentsByAnimalIdAsync(Guid animalId);
        Task<Appointment> CreateAppointmentAsync(Appointment appointment, Guid animalId);
        Task<Appointment> GetAppointmentByIdAsync(Guid id);
        Task<Appointment> EditAppointmentAsync(Appointment appointment, Guid id);
        Task<Appointment> DeleteAppointmentAsync(Guid id);
    }
}

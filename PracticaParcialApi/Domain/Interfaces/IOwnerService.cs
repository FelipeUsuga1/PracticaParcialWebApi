using PracticaParcialApi.DAL.Entities;
using System.Diagnostics.Metrics;

namespace PracticaParcialApi.Domain.Interfaces
{
    public interface IOwnerService
    {
        Task<IEnumerable<Owner>> GetOwnersAsync();
        Task<Owner> CreateOwnerAsync(Owner owner);
        Task<Owner> GetOwnerByIdAsync(Guid id);
        Task<Owner> GetOwnerByNameAsync(string name, string lastName);
        Task<Owner> EditOwnerAsync(Owner owner);
        Task<Owner> DeleteOwnerAsync(Guid id);

    }
}
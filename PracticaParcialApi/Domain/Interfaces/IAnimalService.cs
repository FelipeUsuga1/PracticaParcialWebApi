using PracticaParcialApi.DAL.Entities;

namespace PracticaParcialApi.Domain.Interfaces
{
    public interface IAnimalService
    {
        Task<IEnumerable<Animal>> GetAnimalsByOwnerIdAsync(Guid ownerId);
        Task<Animal> CreateAnimalAsync(Animal animal, Guid ownerId);
        Task<Animal> GetAnimalByIdAsync(Guid id);
        Task<Animal> EditAnimalAsync(Animal animal, Guid id);
        Task<Animal> DeleteAnimalAsync(Guid id);
    }
}

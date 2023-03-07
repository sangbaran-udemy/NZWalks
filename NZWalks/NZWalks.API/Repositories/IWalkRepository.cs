using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public interface IWalkRepository
    {
        Task<IEnumerable<Walk>> GetAllAsync();
        Task<Walk> GetAsync(int walkId);
        Task<Walk> AddAsync(Walk walk);
        Task<Walk?> UpdateAsync(int walkId, Walk walk);
        Task<Walk?> DeleteAsync(int walkId);
    }
}

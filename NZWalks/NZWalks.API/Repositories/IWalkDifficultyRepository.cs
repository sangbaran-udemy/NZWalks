using NZWalks.API.Models.Domain;
using System.Collections;

namespace NZWalks.API.Repositories
{
    public interface IWalkDifficultyRepository
    {
        public Task<IEnumerable<Models.Domain.WalkDifficulty>> GetAllAsync();

        public Task<Models.Domain.WalkDifficulty?> GetByIdAsync(Guid id);

        public Task<Models.Domain.WalkDifficulty> AddAsync(WalkDifficulty walkDifficulty);

        public Task<Models.Domain.WalkDifficulty?> UpdateAsync(Guid id, WalkDifficulty walkDifficulty);

        public Task<Models.Domain.WalkDifficulty?> DeleteAsync(Guid id);
    }
}

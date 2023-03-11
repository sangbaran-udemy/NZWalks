using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class WalkDifficultyRepository : IWalkDifficultyRepository
    {
        private readonly NZWalksDBContext nZWalksDBContext;

        public WalkDifficultyRepository(NZWalksDBContext nZWalksDBContext) 
        {
            this.nZWalksDBContext = nZWalksDBContext;
        }

        public async Task<WalkDifficulty> AddAsync(WalkDifficulty walkDifficulty)
        {
            walkDifficulty.Id = Guid.NewGuid();
            await nZWalksDBContext.WalkDifficulty.AddAsync(walkDifficulty);
            await nZWalksDBContext.SaveChangesAsync();

            return walkDifficulty;
        }

        public async Task<IEnumerable<Models.Domain.WalkDifficulty>> GetAllAsync()
        {
            return await nZWalksDBContext
                .WalkDifficulty
                .ToListAsync();
        }

        public async Task<Models.Domain.WalkDifficulty?> GetByIdAsync(Guid id)
        {
            var walkDifficultyDomain = await nZWalksDBContext.WalkDifficulty.FindAsync(id);
            return walkDifficultyDomain;
        }

        public async Task<Models.Domain.WalkDifficulty?> UpdateAsync(Guid id, WalkDifficulty walkDifficulty)
        {
            var walkDifficultyDomain = await nZWalksDBContext.WalkDifficulty.FindAsync(id);

            if (walkDifficultyDomain != null)
            {
                walkDifficultyDomain.Code = walkDifficulty.Code;
                await nZWalksDBContext.SaveChangesAsync();
                return walkDifficultyDomain;
            }
            return null;
        }

        public async Task<Models.Domain.WalkDifficulty?> DeleteAsync(Guid guid)
        {
            var walkDifficultyDomain = await nZWalksDBContext.WalkDifficulty.FindAsync(guid);
            if (walkDifficultyDomain != null)
            {
                nZWalksDBContext.WalkDifficulty.Remove(walkDifficultyDomain);
                await nZWalksDBContext.SaveChangesAsync();
            }

            return walkDifficultyDomain;            
        }
    }
}

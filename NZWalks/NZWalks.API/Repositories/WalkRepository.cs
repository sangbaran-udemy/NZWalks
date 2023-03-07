using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class WalkRepository : IWalkRepository
    {
        private NZWalksDBContext nzWalksDBContext;
        public WalkRepository(NZWalksDBContext nZWalksDBContext) 
        {
            this.nzWalksDBContext = nZWalksDBContext;
        }

        public async Task<Walk> AddAsync(Walk walk)
        {
            await nzWalksDBContext.Walks.AddAsync(walk);
            await nzWalksDBContext.SaveChangesAsync();
            return walk;
        }

        public async Task<Walk?> DeleteAsync(int walkId)
        {
            var walk = await nzWalksDBContext.Walks.FindAsync(walkId);

            if (walk != null)
                nzWalksDBContext.Walks.Remove(walk);
            else
                walk = null;    

            await nzWalksDBContext.SaveChangesAsync();
            return walk;
        }

        public async Task<IEnumerable<Walk>> GetAllAsync()
        {
            return await 
                nzWalksDBContext
                .Walks
                .Include(x => x.Region)
                .Include(x => x.WalkDifficulty)
                .ToListAsync();
        }

        public async Task<Walk> GetAsync(int walkId)
        {
            var walk = nzWalksDBContext
                .Walks
                .Include(x => x.Region)
                .Include(x => x.WalkDifficulty)
                .FirstOrDefaultAsync(x => x.Id == walkId);

            return await walk;
        }

        public async Task<Walk?> UpdateAsync(int walkId, Walk walk)
        {
            var walkDomin = await nzWalksDBContext.Walks.FindAsync(walkId);

            if (walkDomin != null)
            {
                walkDomin.Name = walk.Name;
                walkDomin.Length = walk.Length;
                walkDomin.RegionId = walk.RegionId;
                walkDomin.WalkDifficultyId = walk.WalkDifficultyId;

                nzWalksDBContext.Walks.Update(walkDomin);
                await nzWalksDBContext.SaveChangesAsync();
                return walkDomin;
            }
            return null;
        }


    }
}

using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NZWalks.API.Data;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;
using System.Data;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalksController : Controller
    {
        private readonly IWalkRepository walkRepository;
        private readonly IMapper mapper;
        private readonly IRegionRepository regionRepository;
        private readonly IWalkDifficultyRepository walkDifficultyRepository;

        public WalksController(IWalkRepository walkRepository, IMapper mapper, IRegionRepository regionRepository,
            IWalkDifficultyRepository walkDifficultyRepository) 
        {
            this.walkRepository = walkRepository;
            this.mapper = mapper;
            this.regionRepository = regionRepository;
            this.walkDifficultyRepository = walkDifficultyRepository;
        }

        [HttpGet]
        [Authorize(Roles = "reader")]
        public async Task<IActionResult> GetAllWalksAsync()
        {
            //Fetch data from DB - Domain..
            var walksDomain = await walkRepository.GetAllAsync();

            //Convert Domain to DTO..
            var walksDTO = mapper.Map<List<Models.DTO.Walk>>(walksDomain);
            
            //Return the DTO..
            return Ok(walksDTO);
        }

        [HttpGet]
        [Route("{id:int}")]
        [ActionName("GetWalkAsync")]
        [Authorize(Roles = "reader")]
        public async Task<IActionResult> GetWalkAsync(int id)
        {
            //Get the Domain object
            var walkDomain = await walkRepository.GetAsync(id);

            //Convert Domain to DTO
            if (walkDomain == null)
                return NotFound();

            //Map the Domain to the DTO..
            var walkDTO = mapper.Map<Models.DTO.Walk>(walkDomain);

            //return DTO..
            return Ok(walkDTO);
        }

        [HttpPost]
        [Authorize(Roles = "writer")]
        public async Task<IActionResult> AddWalksAsync([FromBody] Models.DTO.AddWalkRequest addWalkRequest)
        {
            var isValidREquest = await ValidateAddWalksAsync(addWalkRequest);

            if(!isValidREquest)
                return BadRequest(ModelState);

            //Convert DTO to Domain object..
            var walkDomain = new Models.Domain.Walk
            {
                Length= addWalkRequest.Length,
                Name=addWalkRequest.Name,
                RegionId=addWalkRequest.RegionId,
                WalkDifficultyId=addWalkRequest.WalkDifficultyId
            };

            //Send Domain object to Repository..
            walkDomain = await walkRepository.AddAsync(walkDomain);

            //Convert Domain object back to DTO..
            var walkDTO = new Models.DTO.Walk
            {
                Id = walkDomain.Id,
                Name = walkDomain.Name,
                Length = walkDomain.Length,
                RegionId = walkDomain.RegionId,
                WalkDifficultyId = walkDomain.WalkDifficultyId
            };

            //Return DTO to client..
            return CreatedAtAction(nameof(GetWalkAsync), new { walkDTO.Id}, walkDTO);
        }

        [HttpPut]
        [Route("{id:int}")]
        [Authorize(Roles = "writer")]
        public async Task<IActionResult> UpdateAsync([FromRoute]int id, [FromBody]UpdateWalkRequest updateWalkRequest)
        {
            var isValidWalkRequest = await ValidateUpdateWalksAsync(updateWalkRequest);
            
            if (!isValidWalkRequest)
                return BadRequest(ModelState);

            //Convert DTO to Domain object..
            var walkDomain = new Models.Domain.Walk
            {
                Length = updateWalkRequest.Length,
                Name = updateWalkRequest.Name,
                RegionId = updateWalkRequest.RegionId,
                WalkDifficultyId = updateWalkRequest.WalkDifficultyId
            };

            //Save Domain to Repositiory..
            walkDomain = await walkRepository.UpdateAsync(id, walkDomain);

            //If null, return notFound
            if (walkDomain == null)
                return NotFound();

            //Convert back Domain to DTO and return response..
            var walkDTO = new Models.DTO.Walk
            {
                Id= walkDomain.Id,
                Name = walkDomain.Name,
                Length = walkDomain.Length,
                RegionId = walkDomain.RegionId,
                WalkDifficultyId= walkDomain.WalkDifficultyId
            };

            return Ok(walkDTO);
        }

        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(Roles = "writer")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var walkDomain = await walkRepository.DeleteAsync(id);

            /*if(walkDomain != null)
            {
                var walkDTO = new Models.DTO.Walk
                {
                    Id = walkDomain.Id,
                    Name = walkDomain.Name,
                    Length = walkDomain.Length,
                    RegionId = walkDomain.RegionId,
                    WalkDifficultyId = walkDomain.WalkDifficultyId
                };
                return Ok(walkDTO);
            }*/

            if(walkDomain != null)
            {
                var walkDTO = mapper.Map<Models.DTO.Walk>(walkDomain);
                return Ok(walkDTO);
            }                

            return NotFound();            
        }

        private async Task<bool> ValidateAddWalksAsync(Models.DTO.AddWalkRequest addWalkRequest)
        {
            if(addWalkRequest == null)
            {
                ModelState.AddModelError(nameof(AddWalkRequest), $"{nameof(AddWalkRequest)} cannot be null.");
                return false;
            }

            /*if (!string.IsNullOrWhiteSpace(addWalkRequest.Name))
                ModelState.AddModelError(nameof(addWalkRequest.Name), $"{nameof(addWalkRequest.Name)} is required.");

            if (addWalkRequest.Length <= 0)
                ModelState.AddModelError(nameof(addWalkRequest.Length), $"{nameof(addWalkRequest.Length)} cannot be <= 0.");*/

            var region = await regionRepository.GetAsync(addWalkRequest.RegionId);
            if (region == null)
                ModelState.AddModelError(nameof(addWalkRequest.RegionId), $"{nameof(addWalkRequest.RegionId)} is not valid.");

            var walkDifficulty = await walkDifficultyRepository.GetByIdAsync(addWalkRequest.WalkDifficultyId);
            if (walkDifficulty == null)
                ModelState.AddModelError(nameof(addWalkRequest.WalkDifficultyId), $"{nameof(addWalkRequest.WalkDifficultyId)} is not valid.");

            if (ModelState.ErrorCount > 0)
                return false;

            return true;
        }

        private async Task<bool> ValidateUpdateWalksAsync(Models.DTO.UpdateWalkRequest updateWalkRequest)
        {
            if (updateWalkRequest == null)
            {
                ModelState.AddModelError(nameof(AddWalkRequest), $"{nameof(AddWalkRequest)} cannot be null.");
                return false;
            }

            /*if (!string.IsNullOrWhiteSpace(updateWalkRequest.Name))
                ModelState.AddModelError(nameof(updateWalkRequest.Name), $"{nameof(updateWalkRequest.Name)} is required.");

            if (updateWalkRequest.Length <= 0)
                ModelState.AddModelError(nameof(updateWalkRequest.Length), $"{nameof(updateWalkRequest.Length)} cannot be <= 0.");*/

            var region = await regionRepository.GetAsync(updateWalkRequest.RegionId);
            if (region == null)
                ModelState.AddModelError(nameof(updateWalkRequest.RegionId), $"{nameof(updateWalkRequest.RegionId)} is not valid.");

            var walkDifficulty = await walkDifficultyRepository.GetByIdAsync(updateWalkRequest.WalkDifficultyId);
            if (walkDifficulty == null)
                ModelState.AddModelError(nameof(updateWalkRequest.WalkDifficultyId), $"{nameof(updateWalkRequest.WalkDifficultyId)} is not valid.");

            if (ModelState.ErrorCount > 0)
                return false;

            return true;
        } 
    }
}

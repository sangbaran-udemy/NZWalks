using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NZWalks.API.Data;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalksController : Controller
    {
        private readonly IWalkRepository walkRepository;
        private readonly IMapper mapper;

        public WalksController(IWalkRepository walkRepository, IMapper mapper) 
        {
            this.walkRepository = walkRepository;
            this.mapper = mapper;
        }

        [HttpGet]        
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
        public async Task<IActionResult> AddWalksAsync([FromBody] Models.DTO.AddWalkRequest addWalkRequest)
        {
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
            return CreatedAtAction(nameof(GetWalkAsync), new { Id = walkDTO.Id}, walkDTO);

        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> UpdateAsync([FromRoute]int id, [FromBody]UpdateWalkRequest updateWalkRequest)
        {
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
    }
}

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalkDifficultyController : Controller
    {
        private readonly IWalkDifficultyRepository walkDifficultyRepository;
        private readonly IMapper mapper;

        public WalkDifficultyController(IWalkDifficultyRepository walkDifficultyRepository, IMapper mapper)
        {
            this.walkDifficultyRepository = walkDifficultyRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync() 
        {
            //Get domain object from repository..
            var walkDifficultyDomainList = await walkDifficultyRepository.GetAllAsync();

            //Convert domain to DTO..
            var walkDifficultyDTOList = mapper.Map<List<Models.DTO.WalkDifficulty>>(walkDifficultyDomainList);

            //For null DTO, return NotFound()
            if(walkDifficultyDTOList == null)
                return NotFound();

            //Return Ok
            return Ok(walkDifficultyDTOList);
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var walkDifficultyDomain = await walkDifficultyRepository.GetByIdAsync(id);

            if (walkDifficultyDomain == null)
                return NotFound();

            var walkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficultyDomain);

            return Ok(walkDifficultyDTO);
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync(AddWalkDifficultyRequest addWalkDifficultyRequest)
        {
            var isValidRequest = ValidateAddAsync(addWalkDifficultyRequest);
            
            if (!isValidRequest)
                return BadRequest(ModelState);

            var walkDifficultyDomain = new Models.Domain.WalkDifficulty
            {
                Code = addWalkDifficultyRequest.Code
            };

            walkDifficultyDomain = await walkDifficultyRepository.AddAsync(walkDifficultyDomain);

            var walkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficultyDomain);

            return Ok(walkDifficultyDTO);            
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateAsync(Guid id, UpdateWalkDifficultyRequest updateWalkDifficultyRequest)
        {
            var isValidRequest = ValidateUpdateAsync(updateWalkDifficultyRequest);

            if(!isValidRequest)
                return BadRequest(ModelState);

            var walkDifficultyDomain = new Models.Domain.WalkDifficulty
            {
                Code = updateWalkDifficultyRequest.Code
            };

            walkDifficultyDomain = await walkDifficultyRepository.UpdateAsync(id, walkDifficultyDomain);

            if (walkDifficultyDomain == null)
                return NotFound();

            var walkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficultyDomain);

            return Ok(walkDifficultyDTO);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var walkDifficultyDomain = await walkDifficultyRepository.DeleteAsync(id);

            if(walkDifficultyDomain == null)
                return NotFound();

            var walkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficultyDomain);

            return Ok(walkDifficultyDTO);
        }

        private bool ValidateAddAsync(AddWalkDifficultyRequest addWalkDifficultyRequest)
        {
            if(addWalkDifficultyRequest == null)
                return false;

            if (string.IsNullOrWhiteSpace(addWalkDifficultyRequest.Code))
                ModelState.AddModelError(nameof(addWalkDifficultyRequest.Code),$"{nameof(addWalkDifficultyRequest.Code)} cannot be null or empty.");

            if (ModelState.ErrorCount > 0)
                return false;

            return true;
        }

        private bool ValidateUpdateAsync(UpdateWalkDifficultyRequest updateWalkDifficultyRequest)
        {
            if (updateWalkDifficultyRequest == null)
                return false;

            if (string.IsNullOrWhiteSpace(updateWalkDifficultyRequest.Code))
                ModelState.AddModelError(nameof(updateWalkDifficultyRequest.Code), $"{nameof(updateWalkDifficultyRequest.Code)} cannot be null or empty.");

            if (ModelState.ErrorCount > 0)
                return false;

            return true;
        }
    }
}

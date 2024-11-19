using AutoMapper;
using ItalyGeo.API.Mappings;
using ItalyGeo.API.Models.Domain;
using ItalyGeo.API.Models.DTO.Region;
using ItalyGeo.API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms.Mapping;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ItalyGeo.API.Controllers
{
    [Route("/[controller]/")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly IRegionRepository _regionRepository;
        private readonly IMapper _mapper;

        public RegionsController(IRegionRepository regionRepository, IMapper mapper)
        {
            this._regionRepository = regionRepository;
            this._mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] string? filterOn, [FromQuery] string? filterQuery, [FromQuery] bool orderByDescending = false)
        {
            var regionsDomain = await _regionRepository.GetAllAsync(filterOn, filterQuery, orderByDescending);
            var regionsDto = _mapper.Map<List<RegionDto>>(regionsDomain);

            // Return DTOs
            return Ok(regionsDto);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id)
        {
            var regionDomain = await _regionRepository.GetByIdAsync(id);
            if (regionDomain == null) return NotFound();
            var regionDto = _mapper.Map<RegionDto>(regionDomain);
            return Ok(regionDto);
        }

        [HttpGet]
        [Route("wikipath/{wikiPath}")]
        public async Task<IActionResult> GetByWikiPagePathAsync([FromRoute] string wikiPath)
        {
            var regionDomain = await _regionRepository.GetByWikiPagePathAsync(wikiPath);
            if (regionDomain == null) return NotFound();
            var regionDto = _mapper.Map<RegionDto>(regionDomain);
            return Ok(regionDto);
        }

        [HttpPost]
        [Authorize(policy: "AdminOnly")]
        public async Task<IActionResult> CreateAsync([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            if (ModelState.IsValid)
            {
                var regionDomain = _mapper.Map<Region>(addRegionRequestDto);
                var createdRegion = await _regionRepository.CreateAsync(regionDomain);
                return Ok(_mapper.Map<RegionDto>(regionDomain));
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPut]
        [Route("{id:guid}")]
        [Authorize(policy: "AdminOnly")]
        public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            if (ModelState.IsValid)
            {
                var regionDomain = _mapper.Map<Region>(updateRegionRequestDto);
                regionDomain = await _regionRepository.UpdateAsync(id, regionDomain);
                if (regionDomain == null) return NotFound();
                return Ok(_mapper.Map<RegionDto>(regionDomain));

            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpDelete]
        [Route("{id:guid}")]
        [Authorize(policy: "AdminOnly")]
        public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
        {
            if (ModelState.IsValid)
            {
                var deletedRegionDomain = await _regionRepository.DeleteAsync(id);
                if (deletedRegionDomain == null) return NotFound();
                return Ok(_mapper.Map<RegionDto>(deletedRegionDomain));
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}

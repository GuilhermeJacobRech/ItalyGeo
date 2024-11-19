using AutoMapper;
using ItalyGeo.API.Models.Domain;
using ItalyGeo.API.Models.DTO.Province;
using ItalyGeo.API.Models.DTO.Region;
using ItalyGeo.API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ItalyGeo.API.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class ProvincesController : ControllerBase
    {
        private readonly IProvinceRepository _provinceRepository;
        private readonly IMapper _mapper;

        public ProvincesController(IProvinceRepository provinceRepository, IMapper mapper)
        {
            this._provinceRepository = provinceRepository;
            this._mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] string? filterOn, [FromQuery] string? filterQuery, [FromQuery] bool orderByDescending = false)
        {
            var provincesDomain = await _provinceRepository.GetAllAsync(filterOn, filterQuery, orderByDescending);
            var provincesDto = _mapper.Map<List<ProvinceDto>>(provincesDomain);

            // Return DTOs
            return Ok(provincesDto);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id)
        {
            var provinceDomain = await _provinceRepository.GetByIdAsync(id);
            if (provinceDomain == null) return NotFound();
            var regionDto = _mapper.Map<ProvinceDto>(provinceDomain);
            return Ok(regionDto);
        }

        [HttpGet]
        [Route("wikipath/{wikiPath}")]
        public async Task<IActionResult> GetByWikiPagePathAsync([FromRoute] string wikiPath)
        {
            var provinceDomain = await _provinceRepository.GetByWikiPagePathAsync(wikiPath);
            if (provinceDomain == null) return NotFound();
            var provinceDto = _mapper.Map<ProvinceDto>(provinceDomain);
            return Ok(provinceDto);
        }

        [HttpPost]
        [Authorize(policy: "AdminOnly")]
        public async Task<IActionResult> CreateAsync([FromBody] AddProvinceRequestDto addProvinceRequestDto)
        {
            if (ModelState.IsValid)
            {
                var provinceDomain = _mapper.Map<Province>(addProvinceRequestDto);
                var createdProvince = await _provinceRepository.CreateAsync(provinceDomain);
                return Ok(_mapper.Map<ProvinceDto>(provinceDomain));
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPut]
        [Route("{id:guid}")]
        [Authorize(policy: "AdminOnly")]
        public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] UpdateProvinceRequestDto updateProvinceRequestDto)
        {
            if (ModelState.IsValid)
            {
                var provinceDomain = _mapper.Map<Province>(updateProvinceRequestDto);
                provinceDomain = await _provinceRepository.UpdateAsync(id, provinceDomain);
                if (provinceDomain == null) return NotFound();
                return Ok(_mapper.Map<ProvinceDto>(provinceDomain));

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
                var deletedProvinceDomain = await _provinceRepository.DeleteAsync(id);
                if (deletedProvinceDomain == null) return NotFound();
                return Ok(_mapper.Map<ProvinceDto>(deletedProvinceDomain));
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

    }
}

using AutoMapper;
using ItalyGeo.API.Models.Domain;
using ItalyGeo.API.Models.DTO.Comune;
using ItalyGeo.API.Models.DTO.Province;
using ItalyGeo.API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace ItalyGeo.API.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class ComunesController : ControllerBase
    {
        private readonly IComuneRepository _comuneRepository;
        private readonly IMapper _mapper;

        public ComunesController(IComuneRepository comuneRepository, IMapper mapper)
        {
            this._comuneRepository = comuneRepository;
            this._mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] string? filterOn, [FromQuery] string? filterQuery, 
            [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 1000, [FromQuery] bool orderByDescending = false)
        {
            var comunesDomain = await _comuneRepository.GetAllAsync(filterOn, filterQuery, pageNumber, pageSize, orderByDescending);
            var comunesDto = _mapper.Map<List<ComuneDto>>(comunesDomain);

            // Return DTOs
            return Ok(comunesDto);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id)
        {
            var comuneDomain = await _comuneRepository.GetByIdAsync(id);
            if (comuneDomain == null) return NotFound();
            var comuneDto = _mapper.Map<ComuneDto>(comuneDomain);

            // Return DTO
            return Ok(comuneDto);
        }

        [HttpGet]
        [Route("wikipath/{wikiPath}")]
        public async Task<IActionResult> GetByWikiPagePathAsync([FromRoute] string wikiPath)
        {
            var comuneDomain = await _comuneRepository.GetByWikiPagePathAsync(wikiPath);
            if (comuneDomain == null) return NotFound();
            var comuneDto = _mapper.Map<ComuneDto>(comuneDomain);
            return Ok(comuneDto);
        }

        [HttpPost]
        [Authorize(policy: "AdminOnly")]
        public async Task<IActionResult> CreateAsync([FromBody] AddComuneRequestDto addComuneRequestDto)
        {
            if (ModelState.IsValid)
            {
                var comuneDomain = _mapper.Map<Comune>(addComuneRequestDto);
                var createdComune = await _comuneRepository.CreateAsync(comuneDomain);
                return Ok(_mapper.Map<ComuneDto>(comuneDomain));
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPut]
        [Route("{id:guid}")]
        [Authorize(policy: "AdminOnly")]
        public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] UpdateComuneRequestDto updateComuneRequestDto)
        {
            if (ModelState.IsValid)
            {
                var comuneDomain = _mapper.Map<Comune>(updateComuneRequestDto);
                comuneDomain = await _comuneRepository.UpdateAsync(id, comuneDomain);
                if (comuneDomain == null) return NotFound();
                return Ok(_mapper.Map<ComuneDto>(comuneDomain));

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
                var deletedComuneDomain = await _comuneRepository.DeleteAsync(id);
                if (deletedComuneDomain == null) return NotFound();
                return Ok(_mapper.Map<ComuneDto>(deletedComuneDomain));
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}

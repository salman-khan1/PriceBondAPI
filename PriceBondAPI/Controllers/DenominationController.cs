using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PriceBondAPI.Models;
using PriceBondAPI.Models.DTOS.DenominationDto;
using PriceBondAPI.Models.DTOS.UserDto;
using PriceBondAPI.Repositories.DenominationRepository;

namespace PriceBondAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DenominationController : ControllerBase
    {
        private readonly PbdatabaseContext _context;
        private readonly IDenominationRepository _denominationRepository;

        public DenominationController(PbdatabaseContext context,IDenominationRepository denominationRepository)
        {
            _context = context;
            _denominationRepository = denominationRepository;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var denominations =await _denominationRepository.GetAllAsync();

            //DTO Mapping
            var denominationDto = new List<DenominationDto>();
            foreach (var denomination in denominations)
            {
                denominationDto.Add(new DenominationDto()
                {
                    Id = denomination.Id,
                    Value = denomination.Value,
                    Description = denomination.Description,
                    
                });
            }

            return Ok(denominationDto);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var denomination =await _denominationRepository.GetByIdAsync(id);
            if (denomination == null)
            {
                return BadRequest();
            }

            //Mapping Dto
            var denominationDto = new DenominationDto
            {
                Id = denomination.Id,
                Value = denomination.Value,
                Description = denomination.Description,
               
            };
            return Ok(denominationDto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddDenominationDto addDenomination)
        {
            var denomination = new Denomination
            {
                Value = addDenomination.Value,
                Description = addDenomination.Description,
            };
         denomination= await _denominationRepository.CreateAsync(denomination);

            var denominationDto = new DenominationDto
            {
                Id = denomination.Id,
                Value = denomination.Value,
                Description = denomination.Description,
             
            };

            return CreatedAtAction(nameof(Get), new { id =denominationDto.Id }, denominationDto);
        }


        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateDenominationDto updateDenomination)
        {
            var denomination = new Denomination
            {
                Value= updateDenomination.Value,
                Description = updateDenomination.Description,
            };
             denomination = await _denominationRepository.UpdateAsync(id, denomination);
            if (denomination == null) { return BadRequest(); }
            //Map Dto


            var denominationDto = new DenominationDto
            {
                Id = denomination.Id,
                Value = denomination.Value,
                Description = denomination.Description,
                         
            };
            return Ok(denominationDto);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var denomination =await _denominationRepository.DeleteAsync(id);

            if (denomination == null)
            {
                return BadRequest();
            }
            var denominationDto = new DenominationDto
            {
                Id= denomination.Id,
                Value= denomination.Value,
                Description = denomination.Description,
            };
            return Ok(denominationDto);
        }
    }
}

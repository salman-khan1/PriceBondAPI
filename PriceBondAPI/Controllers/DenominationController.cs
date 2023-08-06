using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PriceBondAPI.Models;
using PriceBondAPI.Models.DTOS.DenominationDto;
using PriceBondAPI.Models.DTOS.UserDto;

namespace PriceBondAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DenominationController : ControllerBase
    {
        private readonly PbdatabaseContext _context;

        public DenominationController(PbdatabaseContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var denominations =await _context.Denominations.ToListAsync();

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
            var denomination =await _context.Denominations.FirstOrDefaultAsync(x => x.Id == id);
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
          await _context.Denominations.AddAsync(denomination);
          await _context.SaveChangesAsync();

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
            var denomination =await _context.Denominations.FirstOrDefaultAsync(x => x.Id == id);
            if (denomination == null) { return BadRequest(); }
            //Map Dto

            denomination.Value = updateDenomination.Value;
            denomination.Description = updateDenomination.Description;
            await _context.SaveChangesAsync();

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
            var denomination =await _context.Denominations.FirstOrDefaultAsync(x => x.Id == id);
            if (denomination == null)
            {
                return BadRequest();
            }
            _context.Denominations.Remove(denomination);
           await _context.SaveChangesAsync();
            return Ok(denomination);
        }
    }
}

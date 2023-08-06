using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PriceBondAPI.Models;
using PriceBondAPI.Models.DTOS.BondDto;
using PriceBondAPI.Models.DTOS.DrawDto;

namespace PriceBondAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DrawController : ControllerBase
    {
        private readonly PbdatabaseContext _context;

        public DrawController (PbdatabaseContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var draws = await _context.Draws.ToListAsync();

            //DTO Mapping
            var drawDto = new List<DrawDto>();
            foreach (var draw in draws)
            {
                drawDto.Add(new DrawDto()
                {
                    Id = draw.Id,
                    DrawDate=draw.DrawDate,
                    DrawLocation=draw.DrawLocation,
                    Price=draw.Price,
                    DenominationId=draw.DenominationId,

                });
            }

            return Ok(drawDto);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var draw = await _context.Draws.FirstOrDefaultAsync(x => x.Id == id);
            if (draw == null)
            {
                return BadRequest();
            }

            //Mapping Dto
            var drawDto = new DrawDto
            {
                Id = draw.Id,
                DrawDate = draw.DrawDate,
                DrawLocation=draw.DrawLocation,
                Price=draw.Price,
                DenominationId= draw.DenominationId,

            };
            return Ok(drawDto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddDrawDto addDraw)
        {
            var draw = new Draw
            {
                DrawDate = addDraw.DrawDate,
                DrawLocation = addDraw.DrawLocation,
                Price=addDraw.Price,
                DenominationId=addDraw.DenominationId,
            };
            await _context.Draws.AddAsync(draw);
            await _context.SaveChangesAsync();

            var drawDto = new DrawDto
            {
                Id = draw.Id,
                DrawDate=draw.DrawDate,
                DrawLocation=draw.DrawLocation,
                Price=draw.Price,
                DenominationId=draw.DenominationId,
            };

            return CreatedAtAction(nameof(Get), new { id = drawDto.Id }, drawDto);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateDrawDto updateDraw)
        {
            var draw = await _context.Draws.FirstOrDefaultAsync(x => x.Id == id);
            if (draw == null) { return BadRequest(); }
            //Map Dto

            draw.DrawDate = updateDraw.DrawDate;
            draw.DrawLocation = updateDraw.DrawLocation;
            draw.Price = updateDraw.Price;
            draw.DenominationId = updateDraw.DenominationId;
            await _context.SaveChangesAsync();

            var drawDto = new DrawDto
            {
                Id = draw.Id,
                DrawDate=draw.DrawDate,
                DrawLocation=draw.DrawLocation,
                Price=draw.Price,
                DenominationId = draw.DenominationId,
            };
            return Ok(drawDto);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var draw = await _context.Draws.FirstOrDefaultAsync(x => x.Id == id);
            if (draw == null)
            {
                return BadRequest();
            }
            _context.Draws.Remove(draw);
            await _context.SaveChangesAsync();
            return Ok(draw);
        }
    }
}

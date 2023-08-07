using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PriceBondAPI.Models;
using PriceBondAPI.Models.DTOS.BondDto;
using PriceBondAPI.Models.DTOS.DrawDto;
using PriceBondAPI.Repositories.DrawRepository;

namespace PriceBondAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DrawController : ControllerBase
    {
        private readonly PbdatabaseContext _context;
        private readonly IDrawRepository _drawRepository;

        public DrawController (PbdatabaseContext context,IDrawRepository drawRepository)
        {
            _context = context;
            _drawRepository = drawRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var draws = await _drawRepository.GetAllAsync();

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
            var draw = await _drawRepository.GetByIdAsync(id);
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
            draw = await _drawRepository.CreateAsync(draw);


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
            var draw = new Draw
            {
                DrawLocation=updateDraw.DrawLocation,
                DrawDate=updateDraw.DrawDate,
                Price =updateDraw.Price,
                DenominationId=updateDraw.DenominationId,
            };

            draw= await _drawRepository.UpdateAsync(id,draw);

            if (draw == null) { return BadRequest(); }

            //Map domain to Dto
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
            var draw = await _drawRepository.DeleteAsync(id);
            if (draw == null)
            {
                return BadRequest();
            }
            var drawDto = new DrawDto 
            {
                Id = draw.Id,
                DrawDate=draw.DrawDate,
                DrawLocation=draw.DrawLocation,
                Price=draw.Price,
                DenominationId=draw.DenominationId,
            };
            return Ok(drawDto);
        }
    }
}

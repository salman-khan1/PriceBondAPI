using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PriceBondAPI.Models;
using PriceBondAPI.Models.DTOS.BondDto;
using PriceBondAPI.Models.DTOS.DenominationDto;

namespace PriceBondAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BondsController : ControllerBase
    {
        private readonly PbdatabaseContext _context;

        public BondsController(PbdatabaseContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var bonds =await _context.Bonds.ToListAsync();

            //DTO Mapping
            var bondDto = new List<BondDto>();
            foreach (var bond in bonds)
            {
                bondDto.Add(new BondDto()
                {
                    Id = bond.Id,
                    BondNumber = bond.BondNumber,
                    PurchaseDate = bond.PurchaseDate,

                });
            }

            return Ok(bondDto);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var bond = await _context.Bonds.FirstOrDefaultAsync(x => x.Id == id);
            if (bond == null)
            {
                return BadRequest();
            }

            //Mapping Dto
            var bondDto = new BondDto
            {
                Id = bond.Id,
                BondNumber = bond.BondNumber,
                PurchaseDate = bond.PurchaseDate,
                DenominationId = bond.DenominationId,
                UserId = bond.UserId

            };
            return Ok(bondDto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddBondDto addBond)
        {
            var bond = new Bond
            {
                BondNumber = addBond.BondNumber,
                PurchaseDate = addBond.PurchaseDate,
                DenominationId=addBond.DenominationId,
                UserId = addBond.UserId,
            };
            await _context.Bonds.AddAsync(bond);
            await _context.SaveChangesAsync();

            var bondDto = new BondDto
            {
                Id = bond.Id,
                BondNumber = bond.BondNumber,
                PurchaseDate = bond.PurchaseDate,
                DenominationId = bond.DenominationId,
                UserId=bond.UserId 

            };

            return CreatedAtAction(nameof(Get), new { id = bondDto.Id  }, bondDto);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateBondDto updateBond)
        {
            var bond = await _context.Bonds.FirstOrDefaultAsync(x => x.Id == id);
            if (bond == null) { return BadRequest(); }
            //Map Dto

            bond.BondNumber = updateBond.BondNumber;
            bond.PurchaseDate = updateBond.PurchaseDate;
            bond.DenominationId = updateBond.DenominationId;
            bond.UserId = updateBond.UserId;
            await _context.SaveChangesAsync();

            var bondDto = new BondDto
            {
                Id = bond.Id,
                BondNumber = bond.BondNumber,
                PurchaseDate = bond.PurchaseDate,
                DenominationId = bond.DenominationId,
                UserId = bond.UserId

            };
            return Ok(bondDto);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var bond = await _context.Bonds.FirstOrDefaultAsync(x => x.Id == id);
            if (bond == null)
            {
                return BadRequest();
            }
            _context.Bonds.Remove(bond);
            await _context.SaveChangesAsync();
            return Ok(bond);
        }
    }
}

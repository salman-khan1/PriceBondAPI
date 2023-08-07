using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PriceBondAPI.Models;
using PriceBondAPI.Models.DTOS.BondDto;
using PriceBondAPI.Models.DTOS.DenominationDto;
using PriceBondAPI.Repositories.BondRepository;

namespace PriceBondAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BondsController : ControllerBase
    {
        private readonly PbdatabaseContext _context;
        private readonly IBondRepository _bondRepository;

        public BondsController(PbdatabaseContext context,IBondRepository bondRepository)
        {
            _context = context;
            _bondRepository = bondRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var bonds =await _bondRepository.GetAllAsync();

            //DTO Mapping
            var bondDto = new List<BondDto>();
            foreach (var bond in bonds)
            {
                bondDto.Add(new BondDto()
                {
                    Id = bond.Id,
                    BondNumber = bond.BondNumber,
                    PurchaseDate = bond.PurchaseDate,
                    UserId=bond.UserId,
                    DenominationId=bond.DenominationId,

                });
            }

            return Ok(bondDto);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var bond = await _bondRepository.GetByIdAsync(id);
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
            await _bondRepository.CreateAsync(bond);

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
            var bond = new Bond
            {
                BondNumber=updateBond.BondNumber,
                PurchaseDate=updateBond.PurchaseDate,
                UserId=updateBond.UserId,
                DenominationId=updateBond.DenominationId,
            };

            bond = await _bondRepository.UpdateAsync(id, bond);

            if (bond == null) { return BadRequest(); }           

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
            var bond = await _bondRepository.DeleteAsync(id);
            if (bond == null)
            {
                return BadRequest();
            }
            var bondDto = new BondDto
            {
                Id=bond.Id,
                BondNumber=bond.BondNumber,
                PurchaseDate=bond.PurchaseDate,
                UserId=bond.UserId,
                DenominationId=bond.DenominationId               
            };
            return Ok(bondDto);
        }
    }
}

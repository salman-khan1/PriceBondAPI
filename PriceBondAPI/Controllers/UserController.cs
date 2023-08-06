using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PriceBondAPI.Models;
using PriceBondAPI.Models.DTOS.UserDto;

namespace PriceBondAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly PbdatabaseContext _context;

        public UserController(PbdatabaseContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users=await _context.Users.ToListAsync();

            //DTO Mapping
            var userDto=new List<UserDto>();
            foreach (var user in users)
            {
                userDto.Add(new UserDto() {
                    Id = user.Id,
                    Email = user.Email,
                    Name = user.Name,
                    RegistrationDate = user.RegistrationDate,
                });
            }
         
            return Ok(userDto);

        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var user =await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
            {
                return BadRequest();
            }

            //Mapping Dto
            var userDto = new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.Name,
                RegistrationDate = user.RegistrationDate,
                Bonds = user.Bonds,
            };
            return Ok(userDto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddUserDto addUserDto)
        {
            var user = new User
            {
                Name = addUserDto.Name,
                Email = addUserDto.Email,
                RegistrationDate = addUserDto.RegistrationDate,
            };
           await _context.Users.AddAsync(user);
           await _context.SaveChangesAsync();

            var userDto = new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.Name,
                RegistrationDate = user.RegistrationDate,
            };

            return CreatedAtAction(nameof(Get), new {id=userDto.Id} ,userDto);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateUserDto updateUser)
        {
            var user=await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            if(user == null) { return BadRequest(); }
            //Map Dto
            
            user.Name = updateUser.Name;
            user.Email= updateUser.Email;
            user.RegistrationDate = updateUser.RegistrationDate;
            await _context.SaveChangesAsync();

            var userDto = new UserDto
            {
                Id = user.Id,
                Name= user.Name,
                Email= user.Email,
                RegistrationDate= user.RegistrationDate,
            };
            return Ok(userDto);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var user=await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            if(user == null)
            {
                return BadRequest();
            }
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return Ok(user);
        }
    }
}
        

    


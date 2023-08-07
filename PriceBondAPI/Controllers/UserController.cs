using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PriceBondAPI.Models;
using PriceBondAPI.Models.DTOS.UserDto;
using PriceBondAPI.Repositories.UserRepository;

namespace PriceBondAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly PbdatabaseContext _context;
        private readonly IUserRepository _userRepository;

        public UserController(PbdatabaseContext context, IUserRepository userRepository)
        {
            _context = context;
           _userRepository = userRepository;
        }

        [HttpGet]
        [Authorize(Roles="Admin,User")]

        public async Task<IActionResult> GetAll()
        {
            var users=await _userRepository.GetAllAsync();

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
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Get(int id)
        {
            var user =await _userRepository.GetByIdAsync(id);
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
                //Bonds = user.Bonds,
            };
            return Ok(userDto);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Create([FromBody] AddUserDto addUserDto)
        {
            var user = new User
            {
                Name = addUserDto.Name,
                Email = addUserDto.Email,
                RegistrationDate = addUserDto.RegistrationDate,
            };
          user =await _userRepository.CreateAsync(user);


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
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateUserDto updateUser)
        {
            //Map dto to domain
            var user = new User
            {
                Name=updateUser.Name,
                Email=updateUser.Email,
                RegistrationDate = updateUser.RegistrationDate,
            };
           user= await _userRepository.UpdateAsync(id,user);

            if(user == null) { return BadRequest(); }

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
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var user=await _userRepository.DeleteAsync(id);
            if(user == null)
            {
                return BadRequest();
            }
            var userDto = new UserDto
            {
                Id=user.Id,
                Name=user.Name,
                Email= user.Email,
                RegistrationDate= user.RegistrationDate,
            };
            return Ok(userDto);
        }
    }
}
        

    


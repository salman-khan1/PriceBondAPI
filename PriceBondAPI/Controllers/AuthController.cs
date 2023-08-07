﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PriceBondAPI.Models.DTOS.AuthDto;
using PriceBondAPI.Repositories.TokkenRepository;

namespace PriceBondAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ITokenRepository _tokenRepository;

        public AuthController(UserManager<IdentityUser> userManager,ITokenRepository tokenRepository)
        {
            _userManager = userManager;
            _tokenRepository = tokenRepository;
        }

        //Post: /api/Auth/Register
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
        {
            var identityUser = new IdentityUser
            {
                UserName = registerRequestDto.Username,
                Email = registerRequestDto.Email,
            };
            var identityResult = await _userManager.CreateAsync(identityUser, registerRequestDto.Password);
            if (identityResult.Succeeded)
            {
                //Add Roles to use
                if (registerRequestDto.Roles != null && registerRequestDto.Roles.Any())
                {
                    identityResult = await _userManager.AddToRolesAsync(identityUser, registerRequestDto.Roles);
                    if (identityResult.Succeeded)
                    {
                        return Ok("User Was Register Successfully");
                    }
                }
            }
            return BadRequest("Something Went Wrong");
        }

        //Post: /api/Auth/Login
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {   
           var user = await _userManager.FindByEmailAsync(loginRequestDto.Email);

            if (user != null)
            {
               var checkPasswordResult = await _userManager.CheckPasswordAsync(user,loginRequestDto.Password);
                if (checkPasswordResult)
                {

                    //Get the roles for roles
                  var roles=  await _userManager.GetRolesAsync(user);
                    if(roles != null)
                    {
                       var jwtToken= _tokenRepository.CreateJWTToken(user, roles.ToList());
                        var response = new LoginResponseDto
                        {
                            JwtToken = jwtToken,
                        };
                       return Ok(response);

                    }
                }
            }

            return BadRequest("Username or Password is incorrect");

        }

    }
}

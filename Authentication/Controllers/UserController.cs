using Authentication.Models;
using Authentication.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Authentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserContext _context;
        private readonly ITokenService _service;

        public UserController(UserContext context, ITokenService service)
        {
            _context = context;
            _service = service;
        }

        // POST api/<UserController>
        [HttpPost]
        public async Task<ActionResult<UserDTO>> Register([FromBody] UserDTO userDto)
        {
            string msg = "";
            try
            {
                using var hmac = new HMACSHA512();
                User user = new User()
                {
                    PortfolioID = userDto.PortfolioID,
                    PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(userDto.Password)),
                    PasswordSalt = hmac.Key
                };
                _context.Users.Add(user);
                _context.SaveChanges();
                userDto.Password = "";
                userDto.JWTToken = _service.CreateToken(userDto);
                return userDto;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                msg = e.Message;
            }
            return BadRequest(msg);
        }

        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult<UserDTO>> Login([FromBody] UserDTO userDto)
        {
            var myUser = await _context.Users.SingleOrDefaultAsync(usr => usr.PortfolioID == userDto.PortfolioID);
            if (myUser == null)
            {
                return Unauthorized("Invalid Credentials");
            }
            using var hmac = new HMACSHA512(myUser.PasswordSalt);
            byte[] userPassword = hmac.ComputeHash(Encoding.UTF8.GetBytes(userDto.Password));
            for (int i = 0; i < userPassword.Length; i++)
            {
                if (userPassword[i] != myUser.PasswordHash[i])
                    return Unauthorized("Invalid Credentials");
            }
            userDto.Password = "";
            userDto.JWTToken = _service.CreateToken(userDto);
            return userDto;
        }
    }
}

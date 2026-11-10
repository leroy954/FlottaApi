using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FlottaApi.DTOs;
using FlottaApi.Models;
using Argon2Sharp;

namespace FlottaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {

        private FlottaContext _context;

        public LoginController(FlottaContext context)
        {
            _context = context;
        }


        [HttpPost]
        public IActionResult Login(LoginDTO loginDTO)
        {
            try
            {
                if (loginDTO == null)
                {
                    return BadRequest("Login  datas required!");
                }
                User loginUser = _context.Users.FirstOrDefault(u => u.LoginName == loginDTO.LoginName);
                if (loginUser == null)
                {
                    return BadRequest("Bad request!");
                }
                
                var (isOk, Parameters) = Argon2PhcFormat.VerifyPhcString(loginDTO.Password, loginUser.Hash);

                if (isOk)
                {
                    return Ok("token");
                }
                else
                {
                    return BadRequest("Bad password!");
                }

            }
            catch (Exception ex)
            {

                return StatusCode(500, "Something went wrong! " + ex.Message);
            }
        }
    }
}

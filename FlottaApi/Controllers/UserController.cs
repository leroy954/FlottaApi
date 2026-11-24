using FlottaApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace FlottaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly FlottaContext _context;

        public UserController(FlottaContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            try
            {
                var users = _context.Users.ToList();
                return Ok(users);
            }
            catch (Exception)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "Hiba történt a felhasználók lekérdezése közben."
                );
            }
        }

        [HttpPost("Create")]
        
        public async Task<IActionResult> Create(User user)
        {
            try
            {
                if (user == null)
                {
                    return BadRequest("A felhasználó adatai hiányoznak.");
                }
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
                return Ok("Felhasználó sikeresen létrehozva.");
            }
            catch (Exception ex)
            {
                return BadRequest("Hiba történt a felhasználó létrehozása közben.");
            }
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);
                if (user == null)
                {
                    return NotFound("A felhasználó nem található.");
                }
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return Ok("Felhasználó sikeresen törölve.");
            }
            catch (Exception ex)
            {
                return BadRequest("Hiba történt a felhasználó törlése közben.");
            }
        }
    }
}

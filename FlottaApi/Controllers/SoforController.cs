using FlottaApi.DTOs;
using FlottaApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlottaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SoforController : ControllerBase
    {
        private readonly FlottaContext _context;

        public SoforController(FlottaContext context)
        {
            _context = context;
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            try
            {
                var result = _context.Sofors.ToList();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest("Error! " + ex.Message);
            }
        }

        [HttpGet("GetById/{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                var result = _context.Sofors.FirstOrDefault(x => x.Id == id);
                if (result == null)
                {
                    return NotFound("Nincs ilyen sofőr!");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest("Error! " + ex.Message);
            }
        }

        [HttpPost("Create")]
        public IActionResult Create(Sofor sofor)
        {
            try
            {
                _context.Sofors.Add(sofor);
                _context.SaveChanges();
                return Ok("Sikeres mentés!");
            }
            catch (Exception ex)
            {
                return BadRequest("Error! " + ex.Message);
            }
        }

        [HttpPut("Update")]
        public IActionResult Update(Sofor sofor)
        {
            try
            {
                _context.Sofors.Update(sofor);
                _context.SaveChanges();
                return Ok("Sikeres módosítás!");
            }
            catch (Exception ex)
            {
                return BadRequest("Error! " + ex.Message);
            }
        }

        [HttpDelete("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var sofor = _context.Sofors.FirstOrDefault(x => x.Id == id);
                if (sofor == null)
                {
                    return NotFound("Nincs ilyen sofőr!");
                }
                _context.Sofors.Remove(sofor);
                _context.SaveChanges();
                return Ok("Sikeres törlés!");
            }
            catch (Exception ex)
            {
                return BadRequest("Error! " + ex.Message);
            }            
        }

        [HttpGet("GetAllData")]

        public IActionResult GetAllData()
        {
            try
            {
                var result = _context.Kikuldottjarmus.
                    Include(s => s.Kikuldetes).
                    Include(j => j.Gepjarmu).
                    Include(s => s.SoforNavigation).
                    Select(j=> new SoforRendszam() {Name = j.SoforNavigation.Name, Rendszam = j.Gepjarmu.Rendszam }).
                    GroupBy(s => s.Name).ToList();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest("Error! " + ex.Message);
            }
        }
    }
}
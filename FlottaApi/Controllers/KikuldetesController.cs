using FlottaApi.DTOs;
using FlottaApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlottaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KikuldetesController : ControllerBase
    {
        private readonly FlottaContext _context;

        public KikuldetesController(FlottaContext context)
        {
            _context = context;
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            try
            {
                var result = _context.Kikuldetes.ToList();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest("Hiba! " + ex.Message);
            }
        }

        [HttpGet("GetById/{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                var result = _context.Kikuldetes.FirstOrDefault(k => k.Id == id);
                if (result == null)
                {
                    return NotFound("Nincs ilyen kiküldetés!");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest("Hiba! " + ex.Message);
            }
        }

        [HttpPost("Create")]
        public IActionResult Create(Kikuldete kikuldetes)
        {
            try
            {
                _context.Kikuldetes.Add(kikuldetes);
                _context.SaveChanges();
                return Ok("Sikeres mentés!");
            }
            catch (Exception ex)
            {
                return BadRequest("Hiba! " + ex.Message);
            }
        }

        [HttpPut("Update/{id}")]
        public IActionResult Update(int id, Kikuldete kikuldetes)
        {
            try
            {
                var letezoKikuldetes = _context.Kikuldetes.FirstOrDefault(k => k.Id == id);
                if (letezoKikuldetes == null)
                {
                    return NotFound("Nincs ilyen kiküldetés!");
                }

                letezoKikuldetes.Celja = kikuldetes.Celja;
                letezoKikuldetes.Cim = kikuldetes.Cim;
                letezoKikuldetes.Kezdes = kikuldetes.Kezdes;
                letezoKikuldetes.Befejezes = kikuldetes.Befejezes;

                _context.Kikuldetes.Update(letezoKikuldetes);
                _context.SaveChanges();
                return Ok("Sikeres módosítás!");
            }
            catch (Exception ex)
            {
                return BadRequest("Hiba! " + ex.Message);
            }
        }

        [HttpDelete("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var kikuldetes = _context.Kikuldetes.
                    Include(k => k.Kikuldottjarmus).
                    FirstOrDefault(k => k.Id == id);
                if (kikuldetes == null)
                {
                    return NotFound("Nincs ilyen kiküldetés!");
                }
                _context.Kikuldetes.Remove(kikuldetes);
                _context.SaveChanges();
                return Ok("Sikeres törlés!");
            }
            catch (Exception ex)
            {
                return BadRequest("Hiba! " + ex.Message);
            }
        }

        [HttpGet("GetJarmuSofor/{id}")]
        public IActionResult GetJarmuSofor(int id)
        {
            try
            {
                if (!_context.Kikuldetes.Any(k => k.Id == id))
                {
                    return NotFound("Nincs ilyen kiküldetés!");
                }

                var result = _context.Kikuldottjarmus.
                    Include(j => j.Gepjarmu).
                    Include(s => s.SoforNavigation).
                    Where(k => k.KikuldetesId == id).
                    Select(j => new KikuldetesJarmuSofor()
                    {
                        GepjarmuId = j.GepjarmuId,
                        Rendszam = j.Gepjarmu.Rendszam,
                        SoforNev = j.SoforNavigation.Name
                    }).ToList();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest("Hiba! " + ex.Message);
            }
        }
    }
}

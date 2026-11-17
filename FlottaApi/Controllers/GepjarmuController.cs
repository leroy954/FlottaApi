using FlottaApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlottaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GepjarmuController : ControllerBase
    {

        FlottaContext _context;

        public GepjarmuController(FlottaContext context)
        {
            _context = context;
        }



        //[HttpGet("GetAll")]
        //public IActionResult GetAll()
        //{
        //    try
        //    {
        //        using(var context = new FlottaContext())
        //        {
        //            return Ok(context.Gepjarmus.ToList());
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        return BadRequest("Hiba! " + ex.Message );
        //    }
        //}

        [HttpGet("GetAll")]

        public IActionResult GetAll()
        {
            try
            {
                var result = _context.Gepjarmus.ToList();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest("Hiba! " + ex.Message);
            }
        }


        [HttpGet("GetById/{Id}")]
        public IActionResult GetById(string rendszam)
        {
            try
            {
                var result = _context.Gepjarmus.FirstOrDefault(g => g.Rendszam == rendszam);
                if (result == null)
                {
                    return NotFound("Nem található ilyen rendszám.");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Hiba! " + ex.Message);
            }
        }

        [HttpPost("UjGepjarmu")]

        public IActionResult PostGepjarmu(Gepjarmu gepjarmu)
        {
            try
            {
                _context.Gepjarmus.Add(gepjarmu);
                _context.SaveChanges();
                return Ok("Sikeres mentés!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Hiba! " + ex.Message);
            }
        }

        [HttpPut("UpdateGepjarmu/{rendszam}")]
        public IActionResult UpdateGepjarmu(string rendszam, Gepjarmu gepjarmu)
        {
            try
            {
                var existingRendszam = _context.Gepjarmus.FirstOrDefault(g => g.Rendszam == rendszam);
                if (existingRendszam == null)
                {
                    return NotFound("Nem található ilyen rendszám.");
                }

                existingRendszam.Rendszam = gepjarmu.Rendszam;

                _context.Gepjarmus.Update(existingRendszam);


                _context.SaveChanges();
                return Ok("Sikeres frissítés!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Hiba! " + ex.Message);
            }




        }

        [HttpDelete("DeleteGepjarmu/{rendszam}")]
        private IActionResult DeleteGepjarmu(string rendszam)
        {
            try
            {
                var existingRendszam = _context.Gepjarmus.FirstOrDefault(g => g.Rendszam == rendszam);
                if (existingRendszam == null)
                {
                    return NotFound("Nem található ilyen rendszám.");
                }
                _context.Gepjarmus.Remove(existingRendszam);
                _context.SaveChanges();
                return Ok("Sikeres törlés!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Hiba! " + ex.Message);
            }
        }
    }
}



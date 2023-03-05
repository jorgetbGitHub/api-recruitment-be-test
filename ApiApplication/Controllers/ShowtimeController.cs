using ApiApplication.Database.Entities;
using Microsoft.AspNetCore.Mvc;
using System;

namespace ApiApplication.Controllers
{
    [ApiController]
    [Route("showtimes")]
    public class ShowtimeController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get([FromQuery] Filter filter)
        {
            try
            {
                if (filter == null || filter.IsNullOrEmpty())
                    return Ok("No apply filter, return all");
                if (filter.Date.HasValue && !string.IsNullOrEmpty(filter.Title))
                    return Ok("Apply both, date and title, filters");
                if (filter.Date.HasValue)
                    return Ok("Only apply date filter");
                if (!string.IsNullOrEmpty(filter.Title))
                    return Ok("Only apply title filter");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        [HttpPost]
        public IActionResult Post(Showtime showtime)
        {
            try
            {
                return CreatedAtAction(nameof(Get), showtime.Id);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public IActionResult Put(Showtime showtime)
        {
            try
            {
                return Ok(showtime);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                return Ok(id);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch]
        public IActionResult Patch()
        {
            return StatusCode(500);
        }

        public class Filter
        {
            public DateTime? Date { get; set; }
            public string Title { get; set; }

            public bool IsNullOrEmpty()
            {
                return !Date.HasValue || string.IsNullOrEmpty(Title);
            }
        }
    }
}

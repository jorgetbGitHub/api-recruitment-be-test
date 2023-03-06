using ApiApplication.Database;
using ApiApplication.Database.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Linq;

namespace ApiApplication.Controllers
{
    [ApiController]
    [Route("showtimes")]
    public class ShowtimeController : ControllerBase
    {
        private readonly IShowtimesRepository _showtimeRepository;
        public ShowtimeController(IShowtimesRepository showtimesRepository)
        {
            _showtimeRepository = showtimesRepository;
        }

        [Authorize(Policy = "Read")]
        [HttpGet]
        public IActionResult Get([FromQuery] Filter filter)
        {
            var collection = Enumerable.Empty<ShowtimeEntity>();

            try
            {
                if (filter == null || filter.IsNullOrEmpty())
                    collection = _showtimeRepository.GetCollection();
                if (filter.Date.HasValue && !string.IsNullOrEmpty(filter.Title))
                    collection = _showtimeRepository.GetCollection(showtime =>
                        showtime.StartDate <= filter.Date && filter.Date <= showtime.EndDate
                        && (showtime.Movie?.Title?.Equals(filter.Title) ?? false));
                if (filter.Date.HasValue)
                    collection = _showtimeRepository.GetCollection(showtime =>
                        showtime.StartDate <= filter.Date && filter.Date <= showtime.EndDate);
                if (!string.IsNullOrEmpty(filter.Title))
                    collection = _showtimeRepository.GetCollection(showtime =>
                        showtime.Movie?.Title?.Equals(filter.Title) ?? false);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        [Authorize(Policy = "Write")]
        [HttpPost]
        public IActionResult Post(ShowtimeEntity showtime)
        {
            try
            {
                showtime = _showtimeRepository.Add(showtime);
                return CreatedAtAction(nameof(Get), showtime.Id);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Policy = "Write")]
        [HttpPut]
        public IActionResult Put(ShowtimeEntity showtime)
        {
            try
            {
                showtime = _showtimeRepository.Update(showtime);
                return Ok(showtime);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Policy = "Write")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                ShowtimeEntity showtime = _showtimeRepository.Delete(id);
                return Ok(showtime);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Policy = "Write")]
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

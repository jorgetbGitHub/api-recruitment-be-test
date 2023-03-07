using ApiApplication.Controllers.DTOs;
using ApiApplication.Database;
using ApiApplication.Database.Entities;
using ApiApplication.Middlewares;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ApiApplication.Controllers
{
    [ShowtimeExceptionFilter]
    [ApiController]
    [Route("showtimes")]
    public class ShowtimeController : ControllerBase
    {
        private readonly IShowtimesRepository _showtimeRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ShowtimeController> _logger;
        public ShowtimeController(IShowtimesRepository showtimesRepository, IMapper mapper, ILogger<ShowtimeController> logger)
        {
            _showtimeRepository = showtimesRepository;
            _mapper = mapper;
            _logger = logger;
        }

        [Authorize(Policy = "Read")]
        [HttpGet]
        public IActionResult Get([FromQuery] Filter filter)
        {
            var collection = Enumerable.Empty<ShowtimeEntity>();

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

            _logger.LogInformation($"Action = Get execution time lapse: {HttpContext.Features.Get<IHttpRequestTimeFeature>()?.RequestTime}");
            return Ok(collection);
        }

        [Authorize(Policy = "Write")]
        [HttpPost]
        public async Task<IActionResult> Post(ShowtimeEntityDto showtimeDto)
        {
            var showtime = _mapper.Map<ShowtimeEntity>(showtimeDto);
            showtime = await _showtimeRepository.Add(showtime);

            _logger.LogInformation($"Action = Post execution time lapse: {HttpContext.Features.Get<IHttpRequestTimeFeature>()?.RequestTime}");
            return CreatedAtAction(nameof(Get), showtime.Id);
        }

        [Authorize(Policy = "Write")]
        [HttpPut]
        public async Task<IActionResult> Put(ShowtimeEntityDto showtimeDto)
        {
            var showtime = _mapper.Map<ShowtimeEntity>(showtimeDto);
            showtime = await _showtimeRepository.Update(showtime);

            _logger.LogInformation($"Action = Put execution time lapse: {HttpContext.Features.Get<IHttpRequestTimeFeature>()?.RequestTime}");
            return Ok(showtime);
        }

        [Authorize(Policy = "Write")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            ShowtimeEntity showtime = _showtimeRepository.Delete(id);

            _logger.LogInformation($"Action = Delete execution time lapse: {HttpContext.Features.Get<IHttpRequestTimeFeature>()?.RequestTime}");
            return Ok(showtime);
        }

        [Authorize(Policy = "Write")]
        [HttpPatch]
        public IActionResult Patch()
        {
            _logger.LogInformation($"Action = Patch execution time lapse: {HttpContext.Features.Get<IHttpRequestTimeFeature>()?.RequestTime}");
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

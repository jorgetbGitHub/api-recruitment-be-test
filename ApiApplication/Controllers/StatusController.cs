using Microsoft.AspNetCore.Mvc;
using System;

namespace ApiApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StatusController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

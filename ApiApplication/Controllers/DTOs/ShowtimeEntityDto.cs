using System;
using System.Collections;
using System.Collections.Generic;

namespace ApiApplication.Controllers.DTOs
{
    public class ShowtimeEntityDto
    {
        public int Id { get; set; }
        public MovieEntityDto Movie { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public IEnumerable<string> Schedule { get; set; }
        public int AuditoriumId { get; set; }
    }
}

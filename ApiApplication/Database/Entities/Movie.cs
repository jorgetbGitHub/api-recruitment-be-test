using System;

namespace ApiApplication.Database.Entities
{
    public class Movie
    {
        public string Title { get; set; }
        public string IMDbId { get; set; }
        public string Starts { get; set; }
        public DateTime ReleaseDate { get; set; }
    }
}

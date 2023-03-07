using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiApplication.Database.Entities
{
    [ModelBinder()]
    public class MovieEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ImdbId { get; set; }
        public string Stars { get; set; }
        public DateTime ReleaseDate { get; set; }

        public int ShowtimeId { get; set; }

        public MovieEntity()
        {
            // NOOP
        }

        public MovieEntity(string imdbId)
        {
            ImdbId = imdbId;
        }
    }
}

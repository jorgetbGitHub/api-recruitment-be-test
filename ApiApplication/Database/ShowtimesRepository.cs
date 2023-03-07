using ApiApplication.Core;
using ApiApplication.Database.Entities;
using IMDbApiLib;
using IMDbApiLib.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiApplication.Database
{
    public class ShowtimesRepository : IShowtimesRepository
    {
        private readonly CinemaContext _context;
        private readonly AppSettings _settings;
        public ShowtimesRepository(CinemaContext context, IOptions<AppSettings> appSettings)
        {
            _context = context;
            _settings = appSettings.Value;
        }

        public async Task<ShowtimeEntity> Add(ShowtimeEntity showtimeEntity)
        {
            if (showtimeEntity.HasMovie())
            {
                var apiLib = new ApiLib(_settings.IMDbApiKey);
                TitleData titleData = await apiLib.TitleAsync(showtimeEntity.Movie.ImdbId);
                showtimeEntity.Movie = new MovieEntity()
                {
                    ImdbId = titleData.Id,
                    Title = titleData.Title,
                    Stars = titleData.Stars,
                    ReleaseDate = DateTime.Parse(titleData.ReleaseDate)
                };
            }
            else
            {
                throw new Exception($"ShowtimeEntity {showtimeEntity} requires Movie and Movie's ImdbId informed.");
            }

            _context.Showtimes.Add(showtimeEntity);
            _context.SaveChanges();
            return showtimeEntity;
        }

        public ShowtimeEntity Delete(int id)
        {
            var entity = _context.Showtimes.Find(id);
            if (entity != null)
            {
                _context.Showtimes.Remove(entity);
                _context.SaveChanges();
                return entity;
            }

            throw new Exception($"ShowtimeEntity with Id={id} was not found");
        }

        public ShowtimeEntity GetByMovie(Func<MovieEntity, bool> filter)
        {
            return _context.Showtimes.Include(t => t.Movie).FirstOrDefault(show => filter(show.Movie));
        }

        public IEnumerable<ShowtimeEntity> GetCollection()
        {
            return GetCollection(null);
        }

        public IEnumerable<ShowtimeEntity> GetCollection(Func<ShowtimeEntity, bool> filter)
        {
            if (filter == null)
                return _context.Showtimes.Include(t => t.Movie);
            else
                return _context.Showtimes.Include(t => t.Movie).Where(show => filter(show));
        }

        public async Task<ShowtimeEntity> Update(ShowtimeEntity showtimeEntity)
        {
            if (showtimeEntity.HasMovie())
            {
                var apiLib = new ApiLib(_settings.IMDbApiKey);
                TitleData titleData = await apiLib.TitleAsync(showtimeEntity.Movie.ImdbId);
                showtimeEntity.Movie = new MovieEntity()
                {
                    Id = showtimeEntity.Movie.Id,
                    ImdbId = titleData.Id,
                    Title = titleData.Title,
                    Stars = titleData.Stars,
                    ReleaseDate = DateTime.Parse(titleData.ReleaseDate)
                };
            }

            _context.Update(showtimeEntity);
            _context.SaveChanges();
            return showtimeEntity;
        }
    }
}

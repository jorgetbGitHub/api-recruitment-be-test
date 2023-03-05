using ApiApplication.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ApiApplication.Database
{
    public class ShowtimesRepository : IShowtimesRepository
    {
        private readonly CinemaContext _context;
        public ShowtimesRepository(CinemaContext context)
        {
            _context = context;
        }

        public ShowtimeEntity Add(ShowtimeEntity showtimeEntity)
        {
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
            return _context.Showtimes.FirstOrDefault(show => filter(show.Movie));
        }

        public IEnumerable<ShowtimeEntity> GetCollection()
        {
            return GetCollection(null);
        }

        public IEnumerable<ShowtimeEntity> GetCollection(Func<ShowtimeEntity, bool> filter)
        {
            return _context.Showtimes.Where(show => filter(show));
        }

        public ShowtimeEntity Update(ShowtimeEntity showtimeEntity)
        {
            _context.Update(showtimeEntity);
            _context.SaveChanges();
            return showtimeEntity;
        }
    }
}

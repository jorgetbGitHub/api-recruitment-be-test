﻿using ApiApplication.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiApplication.Database
{
    public interface IShowtimesRepository
    {
        IEnumerable<ShowtimeEntity> GetCollection();
        IEnumerable<ShowtimeEntity> GetCollection(Func<ShowtimeEntity, bool> filter);
        ShowtimeEntity GetByMovie(Func<MovieEntity, bool> filter);
        Task<ShowtimeEntity> Add(ShowtimeEntity showtimeEntity);
        Task<ShowtimeEntity> Update(ShowtimeEntity showtimeEntity);
        ShowtimeEntity Delete(int id);
    }
}

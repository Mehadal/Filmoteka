using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Filmoteka.Domain.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace Filmoteka.Domain.Interfaces
{
    /// <summary>
    /// Этот интерфейс нужен для абстрагирования бизнес-логики от инфраструктуры
    /// </summary>
    public interface IFilmRepository
    {
        Task<Film[]> GetFilm();

        Task AddFilm(Film film);

        Task AddFile(string description, byte[] file);
    }
}

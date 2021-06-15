using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FitnessRecords.Domain.Entities;

namespace FitnessRecords.Domain.Interfaces
{
    /// <summary>
    /// Служба для работы с интерфейсами - интерфейс нужен для dependency injection
    /// </summary>
    public interface IFilmService
    {
        /// <summary>
        /// Возвращает список упражнений
        /// </summary>
        /// <returns></returns>
        Task<Film[]> GetFilm();

        Task AddFilm(Film film);

        Task AddFile(string description, byte[] file);
    }
}

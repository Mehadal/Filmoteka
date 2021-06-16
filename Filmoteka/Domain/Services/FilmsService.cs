using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Filmoteka.Domain.Entities;
using Filmoteka.Domain.Interfaces;

namespace Filmoteka.Domain.Services
{
    public class FilmsService : IFilmService
    {
        private readonly IFilmRepository _filmRepository;

        public FilmsService(IFilmRepository repository)
        {
            _filmRepository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<Film[]> GetFilm()
        {
            return await _filmRepository.GetFilm();
        }

        public async Task AddFilm(Film film)
        {
            if (film == null)
            {
                Console.WriteLine("bad argument");
                throw new ArgumentNullException(nameof(film));
            }

            Console.WriteLine("repository");
            await _filmRepository.AddFilm(film);
        }

        public async Task AddFile(string description, byte[] file)
        {
            if (string.IsNullOrWhiteSpace(description))
            {
                Console.WriteLine("bad argument");
                throw new ArgumentNullException(nameof(description));
            }

            if (file==null )
                throw new ArgumentNullException(nameof(file));

            Console.WriteLine("repository");
            await _filmRepository.AddFile(description, file);
        }
    }
}

using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Filmoteka.Domain.Entities;
using Filmoteka.Domain.Interfaces;
using Filmoteka.Infrastructure.DTO;
using Microsoft.Extensions.Configuration;

namespace Filmoteka.Infrastructure.Repositories
{
    public class FilmRepository : IFilmRepository
    {
        private const string CONNECTION_STRING_NAME = "FilmsRecords";

        private readonly IConfiguration _configuration;

        public FilmRepository(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<Film[]> GetFilm()
        {
            List<FilmDTO> films = new List<FilmDTO>();                //Список элементов//

            using (var connection = new SqlConnection(_configuration.GetConnectionString(CONNECTION_STRING_NAME)))    //Подключение к бд//
            {
                await connection.OpenAsync();
                using (var cmd = new SqlCommand("SELECT * FROM dbo.Films", connection))
                {
                    var reader = await cmd.ExecuteReaderAsync();
                    while (reader.Read())                                                       //Добавление в список, пока читаются элементы//
                    {
                        films.Add(new FilmDTO()
                        {
                            id = int.Parse(reader["id"].ToString()),
                            name = reader["name"].ToString(),
                            description = reader["description"].ToString()
                        });
                    }
                }
            }

            return films.Select(e => e.ToEntity()).ToArray();             //Преобразование списка в массив//
        }

        public async Task AddFilm(Film film)
        {
            if (film == null)
            {
                Console.WriteLine("adding excercise via repo");
                throw new ArgumentNullException(nameof(film));
            }

            using (var connection = new SqlConnection(_configuration.GetConnectionString(CONNECTION_STRING_NAME)))
            {
                Console.WriteLine($"open con {connection.ConnectionString}");
                await connection.OpenAsync();
                Console.WriteLine("con opened");
                using (var cmd = new SqlCommand($"INSERT INTO dbo.Films (name, description) VALUES ('{film.Name}','{film.Description}')", connection))
                {
                    Console.WriteLine("exec command");
                    await cmd.ExecuteNonQueryAsync();
                    Console.WriteLine("exec command finished");
                }
            }
        }

        public async Task AddFile(string description, byte[] file)
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString(CONNECTION_STRING_NAME)))
            {
                await connection.OpenAsync();
                var b = Convert.ToBase64String(file);
                using (var cmd = new SqlCommand($"INSERT INTO dbo.Films (name, description, picture) VALUES ('{description}','{description}, {b}')", connection))
                {
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }
    }
}

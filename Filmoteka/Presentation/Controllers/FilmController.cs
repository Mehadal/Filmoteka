using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Filmoteka.Domain.Interfaces;
using Filmoteka.Presentation.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Serilog;
using Sentry;
namespace Filmoteka.Presentation.Controllers
{
    [ApiController]
    [Route("films")]
    public class FilmController : ControllerBase
    {
        private readonly IFilmService _filmService;
        private readonly ILogger<FilmController> _logger;

        public FilmController(IFilmService filmService, ILogger<FilmController> logger)
        {
            _filmService = filmService ?? throw new ArgumentNullException(nameof(filmService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var logger = new LoggerConfiguration()
                .WriteTo.Sentry("https://074d9f90cfc44b52a2d3c1d1d604842b@o572490.ingest.sentry.io/5721941")
                .WriteTo.Console()
                .Enrich.FromLogContext()
                .CreateLogger();
            try
            {
                logger.Information("Запрос доступных фильмов");

                //throw new Exception("Страшная ошибка");
                //Используем сервис как интерфейс, но вместо него в Startup.cs подставлена реализация
                return Ok((await _filmService.GetFilm())
                    .Select(film => new FilmModel(film)));
            }
            catch (Exception e)
            {
                logger.Error(e, "Произошла ошибка");
                return StatusCode(StatusCodes.Status500InternalServerError, "Произошла ошибка, обратитесь в службу поддержки!");
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] FilmModel model)
        {
            try
            {
                Console.WriteLine("Получение фильмов");

                if (model == null)
                {
                    Console.WriteLine("bad request");
                    return BadRequest("Введите данные");
                }

                await _filmService.AddFilm(model.ToEntity());
                return StatusCode(StatusCodes.Status201Created);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(StatusCodes.Status500InternalServerError, "Произошла ошибка, обратитесь в службу поддержки!");
            }
        }

        [HttpPut("file")]
        public async Task<IActionResult> PutFile()
        {
            try
            {
                var files = Request.Form.Files[0];
                var description = Request.Form["Description"];

                byte[] resultBytes = new byte[0];
                using (var st = files.OpenReadStream())
                {
                    st.Read(resultBytes);
                    await _filmService.AddFile(description, resultBytes);
                }



                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(StatusCodes.Status500InternalServerError, "Произошла ошибка, обратитесь в службу поддержки!");
            }
        }
    }
}

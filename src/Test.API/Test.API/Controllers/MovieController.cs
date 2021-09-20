using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Test.API.MVC.Models;
using Test.API.MVC.Services;

namespace Test.API.Controllers
{
    [Route("/api/Movie")]
    [Produces("application/json")]
    public class MovieController : Controller
    {
        private readonly IMovieService _movieService;

        public MovieController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        [HttpPost]
        public ActionResult Add(Movie movie)
        {
            var result = _movieService.Add(movie);
            if (result == 0)
                return BadRequest("Error.");

            return Ok(result);
        }

        [HttpDelete()]
        public ActionResult Remove(int id)
        {
            var result = _movieService.Remove(id);
            if (result == 0)
                return NotFound("Couldn't find the movie.");

            return Ok(result);
        }

        [HttpPut]
        public void Update(Movie movie)
        {
            _movieService.Update(movie);
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult<IEnumerable<Movie>>> GetAllAsync()
        {
            var result = await _movieService.List();
            if (result == null)
                return NotFound("Couldn't find any movie.");

            return Ok(result);
        }


        [HttpGet]
        [Route("GetById/")]
        public ActionResult<Movie> GetById(int id)
        {
            var result = _movieService.FindById(id);
            if (result == null)
                return NotFound("Couldn't find a movie with this id.");

            return Ok(result);
        }

        [HttpGet]
        [Route("GetWinnerInterval")]
        public ActionResult<IEnumerable<MovieWinner>> GetWinnerInterval()
        {
            var result = _movieService.WinnerInterval();
            if (result == null)
                return NotFound("Couldn't find any winner.");

            return Ok(result);
        }
    }
}
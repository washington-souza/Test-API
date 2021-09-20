using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Test.API;
using Test.API.Controllers;
using Test.API.MVC.Models;
using Test.API.MVC.Repositories;
using Test.API.MVC.Services;
using Test.API.Services;
using Xunit;

namespace MovieTests
{
    public class MovieTest
    {
        private readonly HttpClient _client;

        public MovieTest()
        {
            var server = new TestServer(new WebHostBuilder()
                        .UseEnvironment("Development")
                        .UseStartup<Startup>());

            _client = server.CreateClient();
        }

        [Fact]
        public void GetAll()
        {
            var movieServiceMock = new Mock<IMovieService>();

            movieServiceMock.Setup(p => p.List()).Returns(GetAllMovies());

            var movieController = new MovieController(movieServiceMock.Object);
            var movieResult = movieController.GetAllAsync();

            var movieResultList = Assert.IsAssignableFrom<Task<ActionResult<IEnumerable<Movie>>>>(movieResult);
            var actionValueList = Assert.IsType<ActionResult<IEnumerable<Movie>>>(movieResultList.Result);
            var actionValue = Assert.IsType<OkObjectResult>(actionValueList.Result);
            Assert.Equal(200, actionValue.StatusCode);
            Assert.Equal(6, ((IEnumerable<Movie>)actionValue.Value).ToList().Count());
        }

        [Fact]
        public void GetWinner()
        {
            var movieRepositoryMock = new Mock<IMovieRepository>();

            movieRepositoryMock.Setup(p => p.List()).Returns(GetAllMovies());

            var movieServiceMock = new MovieService(movieRepositoryMock.Object);

            var movieController = new MovieController(movieServiceMock);
            var movieResult = movieController.GetWinnerInterval();

            var movieResultList = Assert.IsAssignableFrom<ActionResult<IEnumerable<MovieWinner>>>(movieResult);
            var actionValue = Assert.IsType<OkObjectResult>(movieResultList.Result);
            Assert.Equal(200, actionValue.StatusCode);
            Assert.Equal(3, ((IEnumerable<MovieWinner>)actionValue.Value).ToList().Count());
        }

        [Fact]
        public void GetById()
        {
            var movieServiceMock = new Mock<IMovieService>();
            var movie = new Movie("2021", "Test", "Studio Test", "Producer Test", "yes");
            movie.Id = 1;

            movieServiceMock.Setup(p => p.FindById(1)).Returns(movie);

            var movieController = new MovieController(movieServiceMock.Object);
            var movieResult = movieController.GetById(1);

            var actionResult = Assert.IsAssignableFrom<ActionResult<Movie>>(movieResult);
            Assert.IsType<ActionResult<Movie>>(movieResult);
            var actionValue = Assert.IsType<OkObjectResult>(actionResult.Result);
            Assert.Equal(1, ((Movie)actionValue.Value).Id);
        }

        [Fact]
        public void DeleteById()
        {
            var movieServiceMock = new Mock<IMovieService>();
            movieServiceMock.Setup(p => p.Remove(1)).Returns(1);

            var movieController = new MovieController(movieServiceMock.Object);
            var movieResult = movieController.Remove(1);

            var actionResult = Assert.IsAssignableFrom<ActionResult>(movieResult);
            var actionValue = Assert.IsType<OkObjectResult>(actionResult);
            Assert.Equal(1, actionValue.Value);
        }

        [Fact]
        public void Insert()
        {
            var movieServiceMock = new Mock<IMovieService>();
            var movie = new Movie("2020", "Rambo", "Lion", "Prod", "yes");
            movie.Id = 1;

            movieServiceMock.Setup(p => p.Add(movie)).Returns(1);

            var movieController = new MovieController(movieServiceMock.Object);
            var movieResult = movieController.Add(movie);

            var actionResult = Assert.IsAssignableFrom<ActionResult>(movieResult);
            var actionValue = Assert.IsType<OkObjectResult>(actionResult);
            Assert.Equal(1, actionValue.Value);
        }

        private static Task<IEnumerable<Movie>> GetAllMovies()
        {
            var moviesList = new List<Movie>();
            moviesList.Add(new Movie()
            {
                Id = 1,
                Year = "2021",
                Title = "Bad movie",
                Producer = "Prod",
                Studio = "Stud",
                Winner = "no"
            });
            moviesList.Add(new Movie()
            {
                Id = 2,
                Year = "2003",
                Title = "Good movie",
                Producer = "Producer and Produtor",
                Studio = "Studio",
                Winner = "yes"
            });
            moviesList.Add(new Movie()
            {
                Id = 3,
                Year = "2009",
                Title = "Movie",
                Producer = "Produtor and Prod",
                Studio = "Estúdio",
                Winner = "yes"
            });
            moviesList.Add(new Movie()
            {
                Id = 4,
                Year = "2016",
                Title = "Also a bad movie",
                Producer = "Produc",
                Studio = "Stud",
                Winner = "no"
            });
            moviesList.Add(new Movie()
            {
                Id = 5,
                Year = "2019",
                Title = "Mo",
                Producer = "Producer and Produc",
                Studio = "Studio",
                Winner = "yes"
            });
            moviesList.Add(new Movie()
            {
                Id = 6,
                Year = "2011",
                Title = "Okaish movie",
                Producer = "Produtor and Produc",
                Studio = "Estúdio",
                Winner = "yes"
            });

            return Task<IEnumerable<Movie>>.Factory.StartNew(() => moviesList);
        }
    }
}
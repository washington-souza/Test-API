using System.Collections.Generic;
using System.Threading.Tasks;
using Test.API.MVC.Models;

namespace Test.API.MVC.Services
{
    public interface IMovieService
    {
        Task<IEnumerable<Movie>> List();

        int Add(Movie movie);

        Movie FindById(int id);

        IEnumerable<MovieWinner> WinnerInterval();

        int Remove(int id);

        void Update(Movie movie);
    }
}
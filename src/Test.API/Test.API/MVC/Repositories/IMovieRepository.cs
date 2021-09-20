using System.Collections.Generic;
using System.Threading.Tasks;
using Test.API.MVC.Models;

namespace Test.API.MVC.Repositories
{
    public interface IMovieRepository
    {
        Task<IEnumerable<Movie>> List();
        int Add(Movie movie);
        Movie FindById(int id);
        void Update(Movie movie);
        int Remove(int id);
    }
}

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Test.API.MVC.Models;
using Test.API.MVC.Repositories;
using Test.API.Persistences.Context;

namespace Test.API.Persistences.Repositories
{
    public class MovieRepository : BaseRepository, IMovieRepository
    {
        public MovieRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Movie>> List()
        {
            return await _context.Movies.ToListAsync();
        }

        public int Add(Movie movie)
        {
            _context.Movies.Add(movie);
            return _context.SaveChanges();
        }

        public Movie FindById(int id)
        {
            return _context.Movies.Find(id);
        }

        public int Remove(int id)
        {
            var movie = _context.Movies.Find(id);
            _context.Movies.Remove(movie);
            return _context.SaveChanges();
        }

        public void Update(Movie movie)
        {
            var movieToUpdate = _context.Movies.Find(movie.Id);
            _context.Entry(movieToUpdate).CurrentValues.SetValues(movie);
            _context.SaveChanges();
        }
    }
}
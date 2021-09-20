
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Test.API.MVC.Models;
using Test.API.MVC.Repositories;
using Test.API.MVC.Services;

namespace Test.API.Services
{
    public class MovieService : IMovieService
    {
        private IMovieRepository _movieRepository;

        public MovieService(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        public Task<IEnumerable<Movie>> List()
        {
            return _movieRepository.List();
        }

        public int Add(Movie movie)
        {
            return _movieRepository.Add(movie);
        }

        public Movie FindById(int id)
        {
            return _movieRepository.FindById(id);
        }

        public int Remove(int id)
        {
            return _movieRepository.Remove(id);
        }

        public void Update(Movie movie)
        {
            _movieRepository.Update(movie);
        }

        public IEnumerable<MovieWinner> WinnerInterval()
        {
            var moviesList = _movieRepository.List().Result;

            var newList = new List<Movie>();
            var resultMoviesByProducer = moviesList.Where(w => w.Winner == "yes").GroupBy(g => g.Producer);
            foreach (var movieByProducer in resultMoviesByProducer)
            {
                var firstSplitArray = movieByProducer.First().Producer.Split(",");
                foreach (var firstSplit in firstSplitArray)
                {
                    var secondSplitArray = firstSplit.Split("and");
                    foreach (var secondSplit in secondSplitArray)
                    {
                        var movie = movieByProducer.First();
                        newList.Add(new Movie(movie.Year, movie.Title, movie.Studio, secondSplit.Trim(), movie.Winner));
                    }
                }
            }

            var moviesByProducer = newList.GroupBy(g => g.Producer);

            var moviesWinners = FindMovieWinners(moviesByProducer);

            return moviesWinners;
        }

        private IEnumerable<MovieWinner> FindMovieWinners(IEnumerable<IGrouping<string, Movie>> moviesByProducer)
        {
            var lowerList = new List<MovieWinner>();
            var higherList = new List<MovieWinner>();

            foreach (var movieByProducer in moviesByProducer)
            {
                if (movieByProducer.Count() == 1)
                    continue;

                var movieOrderedByYear = movieByProducer.OrderBy(o => o.Year).Select(s => s.Year);
                var firstWin = int.Parse(movieOrderedByYear.First());
                var lastWin = int.Parse(movieOrderedByYear.Last());
                var years = lastWin - firstWin;

                lowerList = FindLowerInterval(movieByProducer.Key, years, firstWin, lastWin, lowerList);
                higherList = FindHigherInterval(movieByProducer.Key, years, firstWin, lastWin, higherList);
            }

            return lowerList.Concat(higherList);
        }

        private List<MovieWinner> FindLowerInterval(string key, int years, int firstWin, int lastWin, List<MovieWinner> lowerList)
        {
            if (lowerList.Count() == 0)
                lowerList.Add(new MovieWinner(key, years, firstWin, lastWin));
            else if (lowerList.Where(w => w.Interval >= years).Select(s => s.Interval).Count() > 0)
            {
                lowerList.RemoveAll(r => r.Interval > years);
                lowerList.Add(new MovieWinner(key, years, firstWin, lastWin));
            }
            return lowerList;
        }

        private List<MovieWinner> FindHigherInterval(string key, int years, int firstWin, int lastWin, List<MovieWinner> higherList)
        {
            if (higherList.Count() == 0)
                higherList.Add(new MovieWinner(key, years, firstWin, lastWin));
            else if (higherList.Where(w => w.Interval <= years).Select(s => s.Interval).Count() > 0)
            {
                higherList.RemoveAll(r => r.Interval < years);
                higherList.Add(new MovieWinner(key, years, firstWin, lastWin));
            }
            return higherList;
        }
    }
}
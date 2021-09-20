using CsvHelper.Configuration;

namespace Test.API.MVC.Models
{
    public sealed class MovieMap : ClassMap<Movie>
    {
        public MovieMap()
        {
            Map(m => m.Year).Name("year");
            Map(m => m.Title).Name("title");
            Map(m => m.Studio).Name("studios");
            Map(m => m.Producer).Name("producers");
            Map(m => m.Winner).Name("winner").Optional();
        }
    }
}

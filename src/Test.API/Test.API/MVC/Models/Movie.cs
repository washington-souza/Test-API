namespace Test.API.MVC.Models
{
    public class Movie
    {
        public int Id { get; set; }
        public string Year { get; set; }
        public string Title { get; set; }
        public string Studio { get; set; }
        public string Producer { get; set; }
        public string Winner { get; set; }

        public Movie(string year, string title, string studio, string producer, string winner)
        {
            Year = year;
            Title = title;
            Studio = studio;
            Producer = producer;
            Winner = winner;
        }

        public Movie()
        {
        }
    }
}

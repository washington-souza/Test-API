namespace Test.API.MVC.Models
{
    public class MovieWinner
    {
        public string Producer { get; set; }
        public int Interval { get; set; }
        public int PreviousWin { get; set; }
        public int FollowingWin { get; set; }

        public MovieWinner(string producer, int interval, int previousWin, int followingWin)
        {
            Producer = producer;
            Interval = interval;
            PreviousWin = previousWin;
            FollowingWin = followingWin;
        }
    }
}

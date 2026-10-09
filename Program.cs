namespace podcastWorld;

class Program
{
    static void Main(string[] args)
    {
        PodcastController podcastController = new PodcastController();
        podcastController.Run();
    }
}

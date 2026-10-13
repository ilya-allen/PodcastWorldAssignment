namespace podcastWorld;

/// <summary>
/// Top level program class.
/// 
/// Responsibility: Run the entire program.
/// </summary>

class Program
{
    static void Main(string[] args)
    {
        PodcastController podcastController = new PodcastController();
        podcastController.Run();
    }
}

namespace podcastWorld;


/// <summary>
/// A class that tracks the feedback of a specified episode by taking all playbacks for a specified episode.
/// 
/// Responsibility: Provide a class containing all reviews and thumbs up for an episode
/// </summary>
public class EpisodeFeedbackStats
{
    // Fields

    // Desired episode for statistics
    private Episode _episode;

    // Count of all thumbs up over the playbacks of the specific episode
    private int _thumbsUpCount;
    
    // List containing all written feedback.
    private List<Feedback> _reviews = new List<Feedback>();


    /// <summary>
    /// Gets the desired episode that will be read for all provided feedback. Ensuring the episode passed is indeed a real episode.
    /// </summary>
    public Episode Episode
    {
        get => _episode;
        set
        {
            if (value == null)
            {
                throw new ArgumentException("# Error - No episode provided.");
            }
            _episode = value;
        }
    }

    /// <summary>
    /// Gets the thumbs up count for the specified episode
    /// </summary>
    public int ThumbsUpCount
    {
        get => _thumbsUpCount;
        set
        {
            if (value < 0)
            {
                throw new ArgumentException("# Error - Thumbs up count cannot be negative.");
            }
            _thumbsUpCount = value;
        }
    }

    /// <summary>
    /// Gets all the written feedback inside of a list.
    /// </summary>
    public List<Feedback> Reviews
    {
        get => _reviews;
        set
        {
            if (value == null)
            {
                throw new ArgumentException("# Error - Reviews list cannot be null.");
            }
            _reviews = value;
        }
    }

    /// <summary>
    /// Episode feedback constructor
    /// </summary>
    /// <param name="episode">Valid episode</param>
    /// <param name="thumbsUpCount">Total amount of thumbs up</param>
    /// <param name="reviews">All written reviews</param>
    public EpisodeFeedbackStats(Episode episode, int thumbsUpCount = 0, List<Feedback> reviews = null)
    {
        Episode = episode;
        ThumbsUpCount = thumbsUpCount;
        Reviews = reviews ?? new List<Feedback>();
    }
}

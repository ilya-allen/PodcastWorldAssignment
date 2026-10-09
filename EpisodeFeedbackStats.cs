namespace podcastWorld;

public class EpisodeFeedbackStats
{
    private Episode _episode;
    private int _thumbsUpCount;
    private List<Feedback> _reviews = new List<Feedback>();

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

    public EpisodeFeedbackStats(Episode episode, int thumbsUpCount = 0, List<Feedback> reviews = null)
    {
        Episode = episode;
        ThumbsUpCount = thumbsUpCount;
        Reviews = reviews ?? new List<Feedback>();
    }
}

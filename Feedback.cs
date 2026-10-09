using System.Text.RegularExpressions; 
namespace podcastWorld;

public class Feedback
{
    private User _listener;
    private Episode _reviewedEpisode;
    private bool _isThumbsUp;
    private string _reviewedText;


    public User Listener
    {
        get => _listener;
        private set
        {
            if(value == null)
            {
                throw new ArgumentException("# Error - No user provided.");
            }
            _listener = value;
        }
    }

    public Episode ReviewedEpisode
    {
        get => _reviewedEpisode;
        private set
        {
            if(value == null)
            {
                throw new ArgumentException("# Error - No episode provided.");
            }
            _reviewedEpisode = value;
        }
    }

    public bool IsThumbsUp
    {
        get => _isThumbsUp;
        private set
        {
            _isThumbsUp = value;
        }
    }

    public string ReviewedText
    {
        get => _reviewedText;
        private set
        {
            if (string.IsNullOrEmpty(value) || !Regex.IsMatch(value, @"^(?=.*[a-zA-Z])[a-zA-Z0-9\s!""#$%&'()*+,\-./:;<=>?@\[\\\]^_`{|}~]+$"))
            {
                throw new ArgumentException("# Error - Supplied review description is invalid.");
            }
            _reviewedText = value;
        }
    }

    public Feedback(User listener, Episode reviewedEpisode, bool isThumbsUp, string reviewedText)
    {
        Listener = listener;
        ReviewedEpisode = reviewedEpisode;
        IsThumbsUp = isThumbsUp;
        ReviewedText = reviewedText;
    }


}

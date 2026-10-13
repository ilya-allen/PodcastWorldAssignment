using System.Text.RegularExpressions; 
namespace podcastWorld;


/// <summary>
/// A class representing an individual users both written feedback, alongside whether or not they gave the episode a thumbs up.
/// 
/// Resposibility: Provide a class of the users feedback on a specific episode
/// </summary>
public class Feedback
{
    // Fields

    // The listener providing the feedback
    private User _listener;

    // The desired episode being reviewed
    private Episode _reviewedEpisode;

    // Whether or not the listener liked the episode
    private bool _isThumbsUp;

    // Written feedback of the episode
    private string _reviewedText;


    /// <summary>
    /// Gets the listener of the episode.
    /// </summary>
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


    /// <summary>
    /// Gets the desired episode the user is reviewing
    /// </summary>
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

    /// <summary>
    /// Gets whether or not the user gave a thumbs up
    /// </summary>
    public bool IsThumbsUp
    {
        get => _isThumbsUp;
        private set
        {
            _isThumbsUp = value;
        }
    }

    /// <summary>
    /// Gets the written text feedback provided by the user, ensuring that it is a valid text feedback.
    /// </summary>
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

    /// <summary>
    /// Feedback constructor
    /// </summary>
    /// <param name="listener">A valid listener</param>
    /// <param name="reviewedEpisode">Desired Episode</param>
    /// <param name="isThumbsUp">Valid Boolean</param>
    /// <param name="reviewedText">Valid Text Description</param>

    public Feedback(User listener, Episode reviewedEpisode, bool isThumbsUp, string reviewedText)
    {
        Listener = listener;
        ReviewedEpisode = reviewedEpisode;
        IsThumbsUp = isThumbsUp;
        ReviewedText = reviewedText;
    }


}

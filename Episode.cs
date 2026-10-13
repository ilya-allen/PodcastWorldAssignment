namespace podcastWorld;
using System.Text.RegularExpressions;
using System.Globalization;


/// <summary>
/// A class used to represent individual episodes. Will fall under a podcasters overview
/// 
/// Responsibility: Represent an episode and store its data
/// </summary>

public class Episode
{

    // Fields

    // Name of the episode
    private string _episodeName;

    // Description of episode
    private string _episodeDescription;

    // Date of episode
    private string _episodeDate;

    // Length of episode in minutes
    private int _episodeLength;

    // Bool representing whether an episode is only for premium listeners
    private bool _episodePremiumOnly;



    // Properties
    

    /// <summary>
    /// Gets the name of the episode, checking the provided input value is valid, otherwise throwing an error
    /// </summary>
    public string EpisodeName
    {
        get => _episodeName;
        protected set
        {
            if (string.IsNullOrEmpty(value) || !Regex.IsMatch(value, @"^(?=.*[a-zA-Z])[a-zA-Z0-9 ':-]+$"))
            {
                throw new ArgumentException("# Error - Supplied episode's name is invalid.");
            }

            _episodeName = value;
        }
    }


    /// <summary>
    /// Gets the description of the episode, checking the provided text is valid, otherwise throwing an error
    /// </summary>
    public string EpisodeDescription
    {
        get => _episodeDescription;
        protected set
        {
            if (string.IsNullOrEmpty(value) || !Regex.IsMatch(value, @"^(?=.*[a-zA-Z])[a-zA-Z0-9\s!""#$%&'()*+,\-./:;<=>?@\[\\\]^_`{|}~]+$"))
            {
                throw new ArgumentException("# Error - Supplied podcast description is invalid.");
            }

            _episodeDescription = value;
        }
    }

    /// <summary>
    /// Gets the date of the episode, following real date time by using System.Globalization.
    /// </summary>
    public string EpisodeDate
    {
        get => _episodeDate;
        protected set
        {
            if (string.IsNullOrWhiteSpace(value) || 
                !DateTime.TryParseExact(value, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
            {
                throw new ArgumentException("# Error - Supplied date is invalid.");
            }

            _episodeDate = value;
        }
    }

    /// <summary>
    /// Gets the length of the episode in minutes, ensuring that the value is within the required range
    /// </summary>
    public int EpisodeLength
    {
        get => _episodeLength;
        protected set
        {
            if (value < PodcastConsts.MIN_EPISODE_LENGTH || value > PodcastConsts.MAX_EPISODE_LENGTH)
            {
                throw new ArgumentException("# Error - Supplied value is out of range.");
            }

            _episodeLength = value;
        }
    }

    /// <summary>
    /// Gets whether the episode requires for the user to be a premium listener to listen to or not.
    /// </summary>
    public bool EpisodePremiumOnly
    {
        get => _episodePremiumOnly;
        protected set
        {
            _episodePremiumOnly = value;
        }
    }

    /// <summary>
    /// Episode constructor
    /// </summary>
    /// <param name="name">Valid Name</param>
    /// <param name="description">Valid Text Description</param>
    /// <param name="date">Valid Real Date in accordance with system.globalization</param>
    /// <param name="length">Valid integer episode length in minutes</param>
    /// <param name="premiumOnly">Boolean for premium requirement</param>
    public Episode(string name, string description, string date, int length, bool premiumOnly)
    {
        EpisodeName = name;
        EpisodeDescription = description;
        EpisodeDate = date;
        EpisodeLength = length;
        EpisodePremiumOnly = premiumOnly;
    }

}

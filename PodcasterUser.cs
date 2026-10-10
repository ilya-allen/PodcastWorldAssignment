namespace podcastWorld;
using System.Text.RegularExpressions;
public class PodcasterUser : User
{

    // Fields

    // Stores the name of the podcast user's created podcast
    private string _podcastName;
    // Stores the text of the podcast users podcast description
    private string _podcastDescription;
    // Stores the genre type of the podcast, as taken from the podcast consts enum due to it being a number input
    private PodcastConsts.Genre _podcastGenre;
    // Stores all episodes that is created by the podcasters
    private List<Episode> _episodes = new List<Episode>();
    // Stores all feedback sent about the podcast
    private List<Feedback> _providedFeedback = new List<Feedback>();
    // Provides a read only option for the feedback list
    public IReadOnlyList<Feedback> ProvidedFeedback => _providedFeedback.AsReadOnly();
    // Simple counter for the podcast subscribers.
    public int PodcastSubscribers { get; private set; } = 0;
    

    // Properties
    // All properties contain specefic parameters in their setters,
    // ensuring that only correct values are stored, otherwise an error message is thrown.

    /// <summary>
    /// Gets the podcast name of the podcast users podcast, while checking it fits valid
    /// input for a podcast name.
    /// </summary>
    public string PodcastName
    {
        get => _podcastName;
        private set
        {
            // Make sure that the value isn't empty
            if(string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("# Error - Supplied podcast name is invalid.");
            }

            // Make sure that the podcast name contains at least one letter and 
            // only permits alphanumeric characters, spaces, apostrophes, hyphens, and colons.
            if (!Regex.IsMatch(value, @"^(?=.*[a-zA-Z])[a-zA-Z0-9 ':-]+$"))
            {
                throw new ArgumentException("# Error - Supplied podcast name is invalid.");
            }
                
            _podcastName = value;
        }
    }

    /// <summary>
    /// Gets the description of the podcast users podcast, sets it to ensure a valid
    /// description is provided.
    /// </summary>
    public string PodcastDescription
    {
        get => _podcastDescription;
        private set
        {
            // Ensures the description is not empty, or null
            if(string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("# Error - Supplied podcast description is invalid.");
            }

            // Uses regex to make sure the text consits of at least one letter and only letters, digits and standard english charachters 
            if (!Regex.IsMatch(value, @"^(?=.*[a-zA-Z])[a-zA-Z0-9\s!""#$%&'()*+,\-./:;<=>?@\[\\\]^_`{|}~]+$"))
            {
                throw new ArgumentException("# Error - Supplied podcast description is invalid.");
            }
                
            _podcastDescription = value;
        }
    }

    /// <summary>
    /// Gets and sets the genre of podcast with use of set enum of pre-defined allowed genres
    /// </summary>
    public PodcastConsts.Genre PodcastGenre
    {
        get => _podcastGenre;
        private set
        {
            // ensures the input value exists within the pre-defined genre enumeration
            if (!Enum.IsDefined(typeof(PodcastConsts.Genre), value))
            {
                throw new ArgumentException("# Error - Supplied menu input is out of range.");
            }

            _podcastGenre = value;
        }
    }

    /// <summary>
    /// Constructor for the podcaster user
    /// </summary>
    /// <param name="name">Name of the podcaster</param>
    /// <param name="age">Age of the podcaster</param>
    /// <param name="mobile">Mobile number of the podcaster</param>
    /// <param name="email">Email of the podcaster</param>
    /// <param name="password">Password of the podcaster</param>
    /// <param name="genre">Genre of the podcasters podcast</param>
    /// <param name="podcastName">Name of the podcasters podcast</param>
    /// <param name="podcastDescription">Description of the podcasters podcast</param>
    public PodcasterUser(string name, int age, string mobile, string email, string password, PodcastConsts.Genre genre, string podcastName, string podcastDescription) 
        : base(name, age, mobile, email, password)
    {
        PodcastGenre = genre;
        PodcastName = podcastName;
        PodcastDescription = podcastDescription;

    }


    /// <summary>
    /// Add a podcast to the podcasters episode list
    /// </summary>
    /// <param name="episode">A valid episode</param>
    public void AddEpisode(Episode episode)
    {
        _episodes.Add(episode);
    }

    // Code smell
    public List<Episode> GetEpisodeList()
    {
        return _episodes; // Code smell
    }

    /// <summary>
    /// Add a users feedback to this podcasters list
    /// </summary>
    /// <param name="feedback">Valid provided feedback for THIS podcast</param>
    public void AddReview(Feedback feedback)
    {
        _providedFeedback.Add(feedback);
    }

    /// <summary>
    /// Increase the subscriber count of the podcast
    /// </summary>
    public void IncrementSubscribers()
    {
        PodcastSubscribers++;
    }

    /// <summary>
    /// Decrease the subscriber count of the podcast
    /// </summary>
    public void DecrementSubscribers()
    {
        if (PodcastSubscribers > 0)
        {
            PodcastSubscribers--;
        }
    }

    /// <summary>
    /// Display information of the Podcast
    /// </summary>
    /// <returns>String containing properties of the podcast</returns>
    public string PodcastInfoString()
    {
        return $"Podcast Information.\n" +
           $"Name: {PodcastName}\n" +
           $"Podcaster: {Name}\n" +
           $"Podcast description: {PodcastDescription}\n";
    }

    /// <summary>
    /// Display a message of what kind of user.
    /// </summary>
    /// <returns>String of user type</returns>
    public override string UserType()
    {
        return "Podcaster";
    }

    /// <summary>
    /// Overridden string, providing the podcaster users registration message
    /// </summary>
    /// <returns>String upon registration</returns>
    public override string UserWelcomeString()
    {
        return $"Congratulations {Name}. You have registered as a podcaster.";
    }    

    /// <summary>
    /// Overridden string, providing the podcaster users login message
    /// </summary>
    /// <returns>String upon log in</returns>
    public override string UserWelcomeBackString()
    {
        return $"Welcome back {Name}.";
    }

    /// <summary>
    /// Overriden string, providing details of the podcaster user
    /// </summary>
    /// <returns>Formatted string, displaying all properties in one string</returns>

    public override string UserInfoString()
    {
        return $"Your details.\n" +
           $"Name: {Name}\n" +
           $"Age: {Age}\n" +
           $"Mobile phone number: {Mobile}\n" +
           $"Email: {Email}\n" +
           $"Podcast name: {PodcastName}\n" +
           $"Podcast genre: {PodcastGenre}\n" +
           $"Podcast description: {PodcastDescription}\n" +
           $"Number of podcast subscribers: {PodcastSubscribers}"; 
    }
}

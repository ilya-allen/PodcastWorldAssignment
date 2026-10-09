namespace podcastWorld;
using System.Text.RegularExpressions;
public class PodcasterUser : User
{
    private string _podcastName;
    private string _podcastDescription;
    private PodcastConsts.Genre _podcastGenre;
    private List<Episode> _episodes = new List<Episode>();
    private List<Feedback> _providedFeedback = new List<Feedback>();
    public IReadOnlyList<Feedback> ProvidedFeedback => _providedFeedback.AsReadOnly();
    public int PodcastSubscribers { get; private set; } = 0;
    

    public string PodcastName
    {
        get => _podcastName;
        private set
        {
            if(string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("# Error - Supplied podcast name is invalid.");
            }

            if (!Regex.IsMatch(value, @"^(?=.*[a-zA-Z])[a-zA-Z0-9 ':-]+$"))
            {
                throw new ArgumentException("# Error - Supplied podcast name is invalid.");
            }
                
            _podcastName = value;
        }
    }

    public string PodcastDescription
    {
        get => _podcastDescription;
        private set
        {
            if(string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("# Error - Supplied podcast description is invalid.");
            }

            if (!Regex.IsMatch(value, @"^(?=.*[a-zA-Z])[a-zA-Z0-9\s!""#$%&'()*+,\-./:;<=>?@\[\\\]^_`{|}~]+$"))
            {
                throw new ArgumentException("# Error - Supplied podcast description is invalid.");
            }
                
            _podcastDescription = value;
        }
    }
    public PodcastConsts.Genre PodcastGenre
    {
        get => _podcastGenre;
        private set
        {
            if (!Enum.IsDefined(typeof(PodcastConsts.Genre), value))
            {
                throw new ArgumentException("# Error - Supplied menu input is out of range.");
            }

            _podcastGenre = value;
        }
    }

    public PodcasterUser(string name, int age, string mobile, string email, string password, PodcastConsts.Genre genre, string podcastName, string podcastDescription) 
        : base(name, age, mobile, email, password)
    {
        PodcastGenre = genre;
        PodcastName = podcastName;
        PodcastDescription = podcastDescription;

    }

    public void AddEpisode(Episode episode)
    {
        _episodes.Add(episode);
    }
    public List<Episode> GetEpisodeList()
    {
        return _episodes;
    }

    public void AddReview(Feedback feedback)
    {
        _providedFeedback.Add(feedback);
    }

    public void IncrementSubscribers()
    {
        PodcastSubscribers++;
    }

    public void DecrementSubscribers()
    {
        if (PodcastSubscribers > 0)
        {
            PodcastSubscribers--;
        }
    }

    public string PodcastInfoString()
    {
        return $"Podcast Information.\n" +
           $"Name: {PodcastName}\n" +
           $"Podcaster: {Name}\n" +
           $"Podcast description: {PodcastDescription}\n";
    }

    public override string UserType()
    {
        return "Podcaster";
    }

    public override string UserWelcomeString()
    {
        return $"Congratulations {Name}. You have registered as a podcaster.";
    }

    public override string UserWelcomeBackString()
    {
        return $"Welcome back {Name}.";
    }


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

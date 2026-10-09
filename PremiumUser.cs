namespace podcastWorld;
using System.Text.RegularExpressions;
using System.Globalization;

public class PremiumUser : User
{

    private string _date = string.Empty;
    private int _payID;
    private List<PodcasterUser> _subscribedPodcasts = new List<PodcasterUser>();
    public IReadOnlyList<PodcasterUser> SubscribedPodcasts => _subscribedPodcasts.AsReadOnly();

    public string Date
    {
        get => _date;
        private set
        {
            // Validates strict dd/MM/yyyy formatting AND real calendar days/leap years
            if (string.IsNullOrWhiteSpace(value) || 
                !DateTime.TryParseExact(value, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
            {
                throw new ArgumentException("# Error - Supplied date is invalid.");
            }

            _date = value;
        }
    }

    public int PayID
    {
        get => _payID;
        private set
        {

            if (value < PodcastConsts.MIN_PAYID_VALUE || value > PodcastConsts.MAX_PAYID_VALUE)
            {
                throw new ArgumentException("# Error - Supplied pay ID is invalid.");
            }
 
            _payID = value;
        }
    }


    public PremiumUser(string name, int age, string mobile, string email, string password, string date, int payID) 
        : base(name, age, mobile, email, password)
    {
        Date = date;       
        PayID = payID; 
    }

    public void Subscribe(PodcasterUser podcaster)
    {
        _subscribedPodcasts.Add(podcaster);
        podcaster.IncrementSubscribers();
    }

    public void Unsubscribe(PodcasterUser podcaster)
    {
        _subscribedPodcasts.Remove(podcaster);
        podcaster.DecrementSubscribers();
    }

    public override string UserWelcomeString()
    {
        return $"Congratulations {Name}. You have registered as a premium listener.";
    }

    public override string UserWelcomeBackString()
    {
        return $"Welcome back {Name}.";
    }

    public override string UserType()
    {
        return "Premium Listener";
    }

    public override string UserInfoString()
    {
        return $"Your details.\n" +
           $"Name: {Name}\n" +
           $"Age: {Age}\n" +
           $"Mobile phone number: {Mobile}\n" +
           $"Email: {Email}\n" +
           $"Episodes started: {EpisodesStarted}\n" +
           $"Episodes completed: {EpisodesCompleted}\n" +
           $"Total minutes listened to: {TotalMinutesListened}\n" +
           $"Registration Date: {Date}\n" +
           $"Pay ID: {PayID}"; 
    }
}

namespace podcastWorld;
using System.Text.RegularExpressions;
using System.Globalization;

public class PremiumUser : User
{
    // Fields

    
    // Stores payment date of a premium user
    private string _date = string.Empty;

    
    // Stores the payment ID of a premium user
    private int _payID;
    
    // Stores all subscribed podcasts of a premium user
    private List<PodcasterUser> _subscribedPodcasts = new List<PodcasterUser>();
    
    // Public accessor, allowing for the list of subscribed podcasts to be read (ONLY READ)
    public IReadOnlyList<PodcasterUser> SubscribedPodcasts => _subscribedPodcasts.AsReadOnly();


    // Properties
    // All properties contain specefic parameters in their setters,
    // ensuring that only correct values are stored, otherwise an error message is thrown.

    /// <summary>
    /// Gets the payment date from user, sets with specific parameters of real date and calendar times
    /// </summary>
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

    /// <summary>
    /// Gets the PAYID from user, set such that it is between a certain range
    /// </summary>
    public int PayID
    {
        get => _payID;
        private set
        {
            // Pay id must between the set minimum and maximum values
            if (value < PodcastConsts.MIN_PAYID_VALUE || value > PodcastConsts.MAX_PAYID_VALUE)
            {
                throw new ArgumentException("# Error - Supplied pay ID is invalid.");
            }
 
            _payID = value;
        }
    }

    /// <summary>
    /// Constructor for premium user, initialising the new premium user class
    /// while forcing base parameters
    /// </summary>
    /// <param name="name"></param>
    /// <param name="age"></param>
    /// <param name="mobile"></param>
    /// <param name="email"></param>
    /// <param name="password"></param>
    /// <param name="date"></param>
    /// <param name="payID"></param>
    public PremiumUser(string name, int age, string mobile, string email, string password, string date, int payID) 
        : base(name, age, mobile, email, password)
    {
        Date = date;       
        PayID = payID; 
    }

    /// <summary>
    /// Add a podcast to the premium users list of subscribed podcasts
    /// </summary>
    /// <param name="podcaster">A valid podcaster user</param>
    public void Subscribe(PodcasterUser podcaster)
    {
        _subscribedPodcasts.Add(podcaster);
        podcaster.IncrementSubscribers();
    }

    /// <summary>
    /// Remove a podcast from the premium users list of subscribed podcasts
    /// </summary>
    /// <param name="podcaster">A valid podcaster user</param>
    public void Unsubscribe(PodcasterUser podcaster)
    {
        _subscribedPodcasts.Remove(podcaster);
        podcaster.DecrementSubscribers();
    }

    /// <summary>
    /// Overridden string, providing the premium users registration message
    /// </summary>
    /// <returns>String upon registration</returns>
    public override string UserWelcomeString()
    {
        return $"Congratulations {Name}. You have registered as a premium listener.";
    }

    
    /// <summary>
    /// Overridden string, providing the premium users login message
    /// </summary>
    /// <returns>String upon log in</returns>
    public override string UserWelcomeBackString()
    {
        return $"Welcome back {Name}.";
    }

    /// <summary>
    /// Overriden string, providing the user's type in text
    /// </summary>
    /// <returns>String message of user type</returns>
    public override string UserType()
    {
        return "Premium Listener";
    }

    /// <summary>
    /// Overriden string, providing details of the premium user
    /// </summary>
    /// <returns>Formatted string, displaying all properties in one string</returns>
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

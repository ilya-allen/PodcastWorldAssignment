namespace podcastWorld;

/// <summary>
/// A child class of User, serving as the most basic tier of user for the PodcastWorld app.
/// This class has the least amount of extra features.
/// 
/// Responsibility: Represent a free user on the PodcastWorld App
/// </summary>

public class RegularUser : User
{

    /// <summary>
    /// Dynamic stat, as regular listeners have to listen to ads every 30 minutes.
    /// </summary>
    public int AdsListened => TotalMinutesListened / 30;

    /// <summary>
    /// Constructor for the child class (regular user) of User.
    /// Base enforces the parameter from the parent class.
    /// </summary>
    /// <param name="name">Name of user</param>
    /// <param name="age">Age of user</param>
    /// <param name="mobile">Mobile number of user</param>
    /// <param name="email">Email of user</param>
    /// <param name="password">Password of user</param>
    public RegularUser(string name, int age, string mobile, string email, string password) 
        : base(name, age, mobile, email, password)
    {
        
    }

    /// <summary>
    /// Overriden string, providing the user's type in text
    /// </summary>
    /// <returns>String message of user type</returns>
    public override string UserType()
    {
        return "Regular Listener";
    }

    /// <summary>
    /// Overridden string, providing the regular users registration message
    /// </summary>
    /// <returns>String upon registration</returns>
    public override string UserWelcomeString()
    {
        return $"Congratulations {Name}. You have registered as a regular listener.";
    }

    /// <summary>
    /// Overridden string, providing the regular users login message
    /// </summary>
    /// <returns>String upon log in</returns>
    public override string UserWelcomeBackString()
    {
        return $"Welcome back {Name}.";
    }


    /// <summary>
    /// Overriden string, providing details of the regular user
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
           $"Advertisements listened to: {AdsListened}"; 
    }
}

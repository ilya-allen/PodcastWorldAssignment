namespace podcastWorld;

public class RegularUser : User
{

    public int AdsListened => TotalMinutesListened / 30;
    public RegularUser(string name, int age, string mobile, string email, string password) 
        : base(name, age, mobile, email, password)
    {
        
    }

    public override string UserType()
    {
        return "Regular Listener";
    }

    public override string UserWelcomeString()
    {
        return $"Congratulations {Name}. You have registered as a regular listener.";
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
           $"Episodes started: {EpisodesStarted}\n" +
           $"Episodes completed: {EpisodesCompleted}\n" +
           $"Total minutes listened to: {TotalMinutesListened}\n" +
           $"Advertisements listened to: {AdsListened}"; 
    }
}

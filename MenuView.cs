namespace podcastWorld;

public class MenuView
{

    public void DisplayHeader() 
    {
        Console.WriteLine("=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+==");
        Console.WriteLine("         Welcome to PodCastWorld!           ");
        Console.WriteLine("      Podcasting all over the world.        ");   
        Console.WriteLine("=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+==");
    }

    public void DisplayExitMessage()
    {
        Console.WriteLine("See you on the next rotation.");
    }

    public int DisplayMainMenu()
    {

        Console.WriteLine("Main Menu.");
        Console.WriteLine("Please make a choice from the menu below.");
        Console.WriteLine("1. Log in as a registered user.");
        Console.WriteLine("2. Register as a new user.");
        Console.WriteLine("3. Exit.");
        Console.WriteLine("Please enter a choice between 1 and 3:");
        return Convert.ToInt32(Console.ReadLine());
    }

    public int RegistrationMenu()
    {
        Console.WriteLine("Registration Menu.");
        Console.WriteLine("Please enter your user type.");
        Console.WriteLine("1. A regular listener.");
        Console.WriteLine("2. A premium listener.");
        Console.WriteLine("3. A podcaster.");
        Console.WriteLine("Please enter a choice between 1 and 3:");
        return Convert.ToInt32(Console.ReadLine());
        
    }

    public List<Func<object>> RegisterNewUser() 
    {
        // Collect the inputs directly into a single list
        List<Func<object>> userData = new List<Func<object>>
        {
            () => getUserName(),     
            () => getUserAge(),      
            () => getUserMobile(), 
            () => getUserEmail(),  
            () => registerNewPassword() 
        };

        return userData;
    }

    public List<Func<object>> RegisterNewPremiumUser()
    {
        List<Func<object>> userData = new List<Func<object>>
        {
            () => getUserDate(),
            () => getUserPayID()
        };

        return userData;
    }

    public List<Func<object>> RegisterNewPodcasterUser()
    {
        List<Func<object>> userData = new List<Func<object>>
        {
            () => getPodcastGenre(),
            () => getPodcastName(),
            () => getPodcastDescription()
        };

        return userData;
    }

    public List<Func<object>> GetLogIn()
    {
        Console.WriteLine("Log in Menu.");
        List<Func<object>> userData = new List<Func<object>> {
            getUserEmail,
            getUserPassword
        };
        return userData;
    }

    public List<Func<object>> GetEpisode()
    {
        List<Func<object>> episodeData = new List<Func<object>> {
            () => getEpisodeName(),     
            () => getPodcastDescription(),      
            () => getEpisodeDate(), 
            () => getEpisodeLength(),  
            () => getEpisodePremium()
        };
        return episodeData;
    }

    public List<Func<object>> GetPlayback()
    {
        List<Func<object>> playbackData = new List<Func<object>> {
            () => getPlaybackDate(),     
            () => getMinutesListened()
        };
        return playbackData;
    }

    public List<Func<object>> GetReview()
    {
        List<Func<object>> reviewData = new List<Func<object>> {
            getIsThumbsUp,
            getReviewText
        };
        return reviewData;
    }
    public int RegularListenerMenu()
    {
        Console.WriteLine();
        Console.WriteLine("Regular Listener Menu.");
        Console.WriteLine("Please make a choice from the menu below.");
        Console.WriteLine("1. See my details.");
        Console.WriteLine("2. Change my password.");
        Console.WriteLine("3. View all podcasts.");
        Console.WriteLine("4. Listen to a podcast episode.");
        Console.WriteLine("5. Provide feedback on a podcast episode.");
        Console.WriteLine("6. Log out.");
        Console.WriteLine("Please enter a choice between 1 and 6: ");

        if (int.TryParse(Console.ReadLine(), out int choice))
        {
            return choice;
        }
        return -1;
    }

    public int PremiumListenerMenu()
    {
        Console.WriteLine();
        Console.WriteLine("Premium Listener Menu.");
        Console.WriteLine("Please make a choice from the menu below.");
        Console.WriteLine("1. See my details.");
        Console.WriteLine("2. Change my password.");
        Console.WriteLine("3. View all podcasts.");
        Console.WriteLine("4. Listen to a podcast episode.");
        Console.WriteLine("5. Listen to a subscribed podcast episode.");
        Console.WriteLine("6. Subscribe to a podcast.");
        Console.WriteLine("7. Unsubscribe from a podcast.");
        Console.WriteLine("8. Provide feedback on a podcast episode.");
        Console.WriteLine("9. Log out.");
        Console.WriteLine("Please enter a choice between 1 and 9: ");

        if (int.TryParse(Console.ReadLine(), out int choice))
        {
            return choice;
        }
        return -1;
    }
    public int PodcasterMenu()
    {
        Console.WriteLine();
        Console.WriteLine("Podcaster Menu.");
        Console.WriteLine("Please make a choice from the menu below.");
        Console.WriteLine("1. See my details.");
        Console.WriteLine("2. Change my password.");
        Console.WriteLine("3. Create an episode.");
        Console.WriteLine("4. View my episodes.");
        Console.WriteLine("5. View statistics about my episodes.");
        Console.WriteLine("6. View feedback about my episodes.");
        Console.WriteLine("7. Log out.");
        Console.WriteLine("Please enter a choice between 1 and 7:");

        if (int.TryParse(Console.ReadLine(), out int choice))
        {
            return choice;
        }
        return -1;
    }

    public void DisplayPodcasterEpisodes(List<Episode> episodes)
    {
        Console.WriteLine("Your episodes.");
        Console.WriteLine("Num  Name                          Recorded Date  Length (min)   Premium Only");

        int count = 1;
        foreach(Episode episode in episodes)
        {
            Console.WriteLine($"{count}.   {episode.EpisodeName}                   {episode.EpisodeDate}     {episode.EpisodeLength}             {episode.EpisodePremiumOnly}");
            count++;
        }
    }

    public void DisplayPodcasterStatistics(Dictionary<Episode, int[]> episodeStatistics)
    {
        Console.WriteLine("Statistics about your episodes.");
        Console.WriteLine("Num  Name                          Listeners Started   Listeners Completed");

        int count = 1;
        foreach(var (episode, stats) in episodeStatistics)
        {
            Console.WriteLine($"{count}.   {episode.EpisodeName}                   {stats[0]}     {stats[1]}");
            count++;
        }
    }

    public void DisplayEpisodes(List<Episode> episodes)
    {
        Console.WriteLine("List of episodes.");
        Console.WriteLine("Num  Name                          Recorded Date  Length (min)   Premium Only   Description");

        int count = 1;
        foreach(Episode episode in episodes)
        {
            Console.WriteLine($"{count}.   {episode.EpisodeName}                   {episode.EpisodeDate}     {episode.EpisodeLength}             {episode.EpisodePremiumOnly}          {episode.EpisodeDescription}");
            count++;
        }
    }

    public int DisplayEpisodes(List<Episode> episodes, string promptMessage)
    {
        Console.WriteLine("List of episodes.");
        Console.WriteLine("Num  Name                          Recorded Date  Length (min)   Premium Only   Description");

        int count = 1;
        foreach(Episode episode in episodes)
        {
            Console.WriteLine($"{count}.   {episode.EpisodeName}                   {episode.EpisodeDate}     {episode.EpisodeLength}             {episode.EpisodePremiumOnly}          {episode.EpisodeDescription}");
            count++;
        }
        Console.WriteLine(promptMessage);

        if (int.TryParse(Console.ReadLine(), out int choice))
        {
            return choice;
        }
        return -1;
    }

    public int DisplayPodcasts(List<PodcasterUser> podcasters, string promptMessage) 
    {
        List<PodcasterUser> orderedList =  podcasters.OrderBy(user => user.PodcastName).ToList();
        Console.WriteLine("Num  Name                     Podcaster           Subscribers");

        int count = 1;
        foreach(PodcasterUser podcaster in orderedList)
        {
            Console.WriteLine($"{count}.   {podcaster.PodcastName}     {podcaster.Name}          {podcaster.PodcastSubscribers}");
            count++;
        }
        Console.WriteLine(promptMessage);

        if (int.TryParse(Console.ReadLine(), out int choice))
        {
            return choice;
        }
        return -1;
    } 
    public void DisplayPodcasts(List<PodcasterUser> podcasters) 
    {
        List<PodcasterUser> orderedList =  podcasters.OrderBy(user => user.PodcastName).ToList();
        Console.WriteLine("Num  Name                     Podcaster           Subscribers");

        int count = 1;
        foreach(PodcasterUser podcaster in orderedList)
        {
            Console.WriteLine($"{count}.   {podcaster.PodcastName}     {podcaster.Name}          {podcaster.PodcastSubscribers}");
            count++;
        }
    } 

    public int DisplayFeedback(List<EpisodeFeedbackStats> providedFeedback)
    {
        Console.WriteLine("Feedback about your episodes.");
        Console.WriteLine("Num  Name                          Thumbs Up");

        int count = 1;
        foreach(EpisodeFeedbackStats feedback in providedFeedback)
        {
            Console.WriteLine($"{count}.   {feedback.Episode.EpisodeName}                   {feedback.ThumbsUpCount}");
            count++;
        }
        Console.WriteLine("Please enter the episode's menu number to view the text feedback or enter 0 to cancel:");

        if (int.TryParse(Console.ReadLine(), out int choice))
        {
            return choice;
        }
        return -1;
    }

    public void DisplayFeedbackText(List<Feedback> providedReviews)
    {
        int count = 1;
        foreach(Feedback review in providedReviews)
        {
            Console.WriteLine($"Listener {count}: {review.ReviewedText}");
            count++;
        }
    }


    public string ChangeCurrentPassword()
    {
        Console.WriteLine("Please enter your current password:");
        string password = Console.ReadLine();
        return password;
    }

    public string ChangeNewPassword()
    {
        Console.WriteLine("Please enter your new password:");
        string password = Console.ReadLine();
        return password;
    }



    private string getUserName()
    {
        Console.WriteLine("Please enter your name:");
        string name = Console.ReadLine();
        return name;
    }
    private int getUserAge()
    {
        Console.WriteLine("Please enter your age between 18 and 99:");
        if (int.TryParse(Console.ReadLine(), out int age))
        {
            return age;
        }
        return -99999; // CODE SMELL CODE SMELL CODE SMELL CODE SMELL CODE SMELL CODE SMELL CODE SMELL 
    }
    private string getUserMobile()
    {
        Console.WriteLine("Please enter your mobile number:");
        string mobile = Console.ReadLine();
        return mobile;
    }
    private string getUserEmail()
    {
        Console.WriteLine("Please enter your email:");
        string email = Console.ReadLine();
        return email;
    }
    private string registerNewPassword()
    {
        Console.WriteLine("Please enter your password:");
        Console.WriteLine("Your password must:");
        Console.WriteLine("- be at least 8 characters long");
        Console.WriteLine("- contain a number");
        Console.WriteLine("- contain a lowercase letter");
        Console.WriteLine("- contain an uppercase letter");
        string password = Console.ReadLine();
        return password;
    }

    private string getUserPassword()
    {
        Console.WriteLine("Please enter your password:");
        string password = Console.ReadLine();
        return password;
    }

    private string getUserDate()
    {
        Console.WriteLine("Please enter the registration date in dd/MM/yyyy format:");
        string date = Console.ReadLine();
        return date;
    }

    private int getUserPayID()
    {
        Console.WriteLine("Please enter your pay ID between 100000 and 999999:");
        if (int.TryParse(Console.ReadLine(), out int payID))
        {
            return payID;
        }
        return -1;
    }

    private int getPodcastGenre()
    {
        Console.WriteLine("Please enter your podcast genre:");
        Console.WriteLine("1. News.");
        Console.WriteLine("2. Politics.");
        Console.WriteLine("3. Sports.");
        Console.WriteLine("4. Entertainment.");
        Console.WriteLine("5. Education.");
        Console.WriteLine("6. Hobbies.");
        Console.WriteLine("Please enter a choice between 1 and 6:");
        if (int.TryParse(Console.ReadLine(), out int genreChoice))
        {
            return genreChoice;
        }
        return -1;
    }

    public int ReadGenreChoice()
    {
        Console.WriteLine("Please enter a choice between 1 and 6:");
        if (int.TryParse(Console.ReadLine(), out int genreChoice))
        {
            return genreChoice;
        }
        return -1;
    }

    private string getPodcastName()
    {
        Console.WriteLine("Please enter your podcast name:");
        return Console.ReadLine();
    }

    private string getPodcastDescription()
    {
        Console.WriteLine("Please enter your podcast description:");
        return Console.ReadLine();
    }


    //EPISODES EPISODES EPISODES EPISODES EPISODES EPISODES 
    //EPISODES EPISODES EPISODES EPISODES EPISODES EPISODES 
    //EPISODES EPISODES EPISODES EPISODES EPISODES EPISODES 
    //EPISODES EPISODES EPISODES EPISODES EPISODES EPISODES 
    
    private string getEpisodeName()
    {
        Console.WriteLine("Please enter your episode's name:");
        return Console.ReadLine();
    }

    private string getEpisodeDate()
    {
        Console.WriteLine("Please enter your episode's recorded date in dd/MM/yyyy format:");
        return Console.ReadLine();
    }

    private int getEpisodeLength()
    {
        Console.WriteLine("Please enter your episode's length in minutes between 1 and 300:");
        if (int.TryParse(Console.ReadLine(), out int episodeLength))
        {
            return episodeLength;
        }
        return -1;
    }

    private string getEpisodePremium()
    {
        Console.WriteLine("Please enter if episode is reserved for premium listeners only (true/false):");
        return Console.ReadLine() ?? "";
    }

    private string getPlaybackDate()
    {
        Console.WriteLine("Please enter the date in dd/MM/yyyy format that you listened to the episode:");
        return Console.ReadLine();
    }

    private int getMinutesListened()
    {
        Console.WriteLine("Please enter how many minutes you listened to the episode:");
        if (int.TryParse(Console.ReadLine(), out int minutesListened))
        {
            return minutesListened;
        }
        return -1;
    }

    private string getIsThumbsUp()
    {
        Console.WriteLine("Do you give the episode a thumbs-up (true) or a thumbs-down (false) rating:");
        return Console.ReadLine() ?? "";
    } 

    private string getReviewText()
    {
        Console.WriteLine("Please enter some text feedback about the episode:");
        return Console.ReadLine() ?? "";
    }

    public int GetInteger(string promptMessage) // CODE SMELL???
    {
        Console.WriteLine(promptMessage);

        if (int.TryParse(Console.ReadLine(), out int choice))
        {
            return choice;
        }
        return -1;
    }

    public void DisplayError(string str)
    {
        Console.WriteLine("#####");
        Console.WriteLine(str);
        Console.WriteLine("#####");
    }

    public void DisplayError(ArgumentException e)
    {
        Console.WriteLine("#####");
        Console.WriteLine(e.Message);
         Console.WriteLine("# Please try again.");
        Console.WriteLine("#####");
    }

    
    public void DisplayString(string str)
    {
        Console.WriteLine(str);
    }
    
}

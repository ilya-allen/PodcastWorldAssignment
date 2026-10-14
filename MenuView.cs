namespace podcastWorld;

/// <summary>
/// Presents all displays and user input prompts to the screen. All messages and
/// information displayed to the user must go through the Menu View class.
/// 
/// Responsibility: User Interactions
/// </summary>
public class MenuView
{

    /// <summary>
    /// Displays the main heading on startup
    /// </summary>
    public void DisplayHeader() 
    {
        Console.WriteLine("=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+==");
        Console.WriteLine("         Welcome to PodCastWorld!           ");
        Console.WriteLine("      Podcasting all over the world.        ");   
        Console.WriteLine("=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+=+==");
    }

    /// <summary>
    /// Displays exit message on logout
    /// </summary>
    public void DisplayExitMessage()
    {
        Console.WriteLine("See you on the next rotation.");
    }

    /// <summary>
    /// Presents the user with the three possible options on startup.
    /// </summary>
    /// <returns></returns>
    public int DisplayMainMenu()
    {

        Console.WriteLine("Main Menu.");
        Console.WriteLine("Please make a choice from the menu below.");
        Console.WriteLine("1. Log in as a registered user.");
        Console.WriteLine("2. Register as a new user.");
        Console.WriteLine("3. Exit.");
        return GetInteger("Please enter a choice between 1 and 3:", 3);
    }

    public int RegistrationMenu()
    {
        Console.WriteLine("Registration Menu.");
        Console.WriteLine("Please enter your user type.");
        Console.WriteLine("1. A regular listener.");
        Console.WriteLine("2. A premium listener.");
        Console.WriteLine("3. A podcaster.");
        return GetInteger("Please enter a choice between 1 and 3:", 3);
        
    }


    // List<Func<object>>'s
    // Explanation:
    //  By using a List Func Object, the service is able to take each step
    /// of the provided list of functions one by one, so that if any errors is found at a certain step,
    /// the service can stop, and wait until a correct input
    /// is provided before moving onto the next step. This means that all the private individual steps are stored
    /// in one list, without needing to call them all at once


    /// <summary>
    /// A collection of all steps needed to register a new user.
    /// </summary>
    /// <returns>A list containing each method of registration</returns>
    public List<Func<object>> RegisterNewUser() 
    {
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

    /// <summary>
    /// A collection of additional steps needed for premium user registration.
    /// This is added alongside the general registration
    /// </summary>
    /// <returns>List of methods for premium user</returns>
    public List<Func<object>> RegisterNewPremiumUser()
    {
        List<Func<object>> userData = new List<Func<object>>
        {
            () => getUserDate(),
            () => getUserPayID()
        };

        return userData;
    }

    /// <summary>
    /// A collection of additional methods needed for podcaster user registration.
    /// </summary>
    /// <returns>Additional methods needed for podcaster registration</returns>
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


    /// <summary>
    /// A collection of methods needed for user log in. General for all users
    /// </summary>
    /// <returns>List containing email and password inputs</returns>
    public List<Func<object>> GetLogIn()
    {
        Console.WriteLine("Log in Menu.");
        List<Func<object>> userData = new List<Func<object>> {
            getUserEmail,
            getUserPassword
        };
        return userData;
    }

    /// <summary>
    /// A collection of the methods needed for episode creation
    /// </summary>
    /// <returns>List containing user prompts</returns>
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

    /// <summary>
    /// A collection of all methods needed for creating a playback session
    /// </summary>
    /// <returns>User prompts for listening to a new episode</returns>
    public List<Func<object>> GetPlayback()
    {
        List<Func<object>> playbackData = new List<Func<object>> {
            () => getPlaybackDate(),     
            () => getMinutesListened()
        };
        return playbackData;
    }

    /// <summary>
    /// A collection of all methods needed to create a review
    /// </summary>
    /// <returns>A list of necessary prompts for a new review</returns>
    public List<Func<object>> GetReview()
    {
        List<Func<object>> reviewData = new List<Func<object>> {
            getIsThumbsUp,
            getReviewText
        };
        return reviewData;
    }

    /// <summary>
    /// Displays choices for a regular listener to make once logged in.
    /// </summary>
    /// <returns>Integer choice from user, otherwise -1 if invalid input</returns>
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

        return GetInteger("Please enter a choice between 1 and 6: ", 6);
    }

    /// <summary>
    /// Displays choices for a premium listener upon log in.
    /// </summary>
    /// <returns>An integer, representing the choice made by the user</returns>
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

        return GetInteger("Please enter a choice between 1 and 9: ", 9);
    }

    /// <summary>
    /// Displays choices for a podcaster upon log in.
    /// </summary>
    /// <returns>An integer representing the choice</returns>
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
        
        return GetInteger("Please enter a choice between 1 and 7: ", 7);
    }


    /// <summary>
    /// This method takes a collection input and provides a formatted table from the inputs
    /// </summary>
    /// <param name="episodes">A list of valid episodes to be displayed</param>
    public void DisplayPodcasterEpisodes(IReadOnlyList<Episode> episodes)
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

    /// <summary>
    /// This method takes a collection input of podcaster statistics and provides a formatted table from the inputs
    /// </summary>
    /// <param name="episodeStatistics">A dictionary of both episodes and integers</param>
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

    /// <summary>
    /// Displays a formatted table of a collection of episodes.
    /// </summary>
    /// <param name="episodes">A valid list of episodes</param>
    public void DisplayEpisodes(IReadOnlyList<Episode> episodes)
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
        
        return GetInteger(promptMessage, episodes.Count);
    }

    /// <summary>
    /// Displays a list of Podcasts, but also takes an input from the user
    /// such that a chosen podcast can be further represented
    /// </summary>
    /// <param name="podcasters">List of valid podcasters</param>
    /// <param name="promptMessage">Provided prompt message</param>
    /// <returns>Integer representing the choice of podcast</returns>
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
        return GetInteger(promptMessage, orderedList.Count);
    } 

    /// <summary>
    /// Displays a formatted table of a collection of podcasts.
    /// </summary>
    /// <param name="podcasters">Valid list of podcaseters</param>
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

    /// <summary>
    /// Displays a collection of feedback on a podcast, while also providing the oppurtunity
    /// to see each feedback in more detail through user choice
    /// </summary>
    /// <param name="providedFeedback">Valid list of feedback</param>
    /// <returns></returns>
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

        return GetInteger("Please enter the episode's menu number to view the text feedback or enter 0 to cancel:", providedFeedback.Count);
    }

    /// <summary>
    /// Displays all provided text feedback of an episode
    /// </summary>
    /// <param name="providedReviews"></param>
    public void DisplayFeedbackText(List<Feedback> providedReviews)
    {
        int count = 1;
        foreach(Feedback review in providedReviews)
        {
            Console.WriteLine($"Listener {count}: {review.ReviewedText}");
            count++;
        }
    }


    // Public password helper methods.

    public string ChangeCurrentPassword()
    {
        Console.WriteLine("Please enter your current password:");
        string password = Console.ReadLine() ?? "";
        return password;
    }

    public string ChangeNewPassword()
    {
        Console.WriteLine("Please enter your new password:");
        string password = Console.ReadLine() ?? "";
        return password;
    }



    // Private methods, calling for each user input required for the specific registration step
    private string getUserName()
    {
        Console.WriteLine("Please enter your name:");
        string name = Console.ReadLine() ?? "";
        return name;
    }
    private int getUserAge()
    {
        return GetInteger("Please enter your age between 18 and 99:");
    }
    private string getUserMobile()
    {
        Console.WriteLine("Please enter your mobile number:");
        string mobile = Console.ReadLine() ?? "";
        return mobile;
    }
    private string getUserEmail()
    {
        Console.WriteLine("Please enter your email:");
        string email = Console.ReadLine() ?? "";
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
        string password = Console.ReadLine() ?? "";
        return password;
    }

    private string getUserPassword()
    {
        Console.WriteLine("Please enter your password:");
        string password = Console.ReadLine() ?? "";
        return password;
    }


    // Premium registration input methods
    private string getUserDate()
    {
        Console.WriteLine("Please enter the registration date in dd/MM/yyyy format:");
        string date = Console.ReadLine() ?? "";
        return date;
    }

    private int getUserPayID()
    {
        return GetInteger("Please enter your pay ID between 100000 and 999999:");
    }


    // Podcaster Registration Methods

    private int getPodcastGenre()
    {
        Console.WriteLine("Please enter your podcast genre:");
        Console.WriteLine("1. News.");
        Console.WriteLine("2. Politics.");
        Console.WriteLine("3. Sports.");
        Console.WriteLine("4. Entertainment.");
        Console.WriteLine("5. Education.");
        Console.WriteLine("6. Hobbies.");
        return GetInteger("Please enter a choice between 1 and 6:", 6);
    }

    public int ReadGenreChoice()
    {
        return GetInteger("Please enter a choice between 1 and 6:", 6);
    }

    private string getPodcastName()
    {
        Console.WriteLine("Please enter your podcast name:");
        return Console.ReadLine() ?? "";
    }

    private string getPodcastDescription()
    {
        Console.WriteLine("Please enter your podcast description:");
        return Console.ReadLine() ?? "";
    }



    // Episode creation input methods
    
    private string getEpisodeName()
    {
        Console.WriteLine("Please enter your episode's name:");
        return Console.ReadLine() ?? "";
    }

    private string getEpisodeDate()
    {
        Console.WriteLine("Please enter your episode's recorded date in dd/MM/yyyy format:");
        return Console.ReadLine() ?? "";
    }

    private int getEpisodeLength()
    {
        return GetInteger("Please enter your episode's length in minutes between 1 and 300:");
    }

    private string getEpisodePremium()
    {
        Console.WriteLine("Please enter if episode is reserved for premium listeners only (true/false):");
        return Console.ReadLine() ?? "";
    }


    // Playback session input methods
    private string getPlaybackDate()
    {
        Console.WriteLine("Please enter the date in dd/MM/yyyy format that you listened to the episode:");
        return Console.ReadLine() ?? "";
    }

    private int getMinutesListened()
    {
        return GetInteger("Please enter how many minutes you listened to the episode:");
    }


    // Review input methods
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



    /// <summary>
    /// Public input helper method, used to ensure an integer is inputted to the system
    /// If not, then an error number is passed.
    /// </summary>
    /// <param name="promptMessage">Prompt message for the prompt</param>
    /// <param name="maxNum">Maximum allowed value, otherwise pass error</param>
    /// <returns>An integer within the allowed range, otherwise an error value</returns>

    public int GetInteger(string promptMessage, int maxNum)
    {
        Console.WriteLine(promptMessage);
        if (int.TryParse(Console.ReadLine(), out int choice))
        {
            if(choice <= maxNum) return choice;
        }
        return -101;
    }

    /// <summary>
    /// Public input method, used to ensure an integer is inputted to the system
    /// </summary>
    /// <param name="promptMessage">Prompt message for the prompt</param>
    /// <returns>An integer, or an error value</returns>
    public int GetInteger(string promptMessage)
    {
        Console.WriteLine(promptMessage);

        if (int.TryParse(Console.ReadLine(), out int choice))
        {
            return choice;
        }
        return -101;
    }

    /// <summary>
    /// Displays an error message with the provided reason as a string.
    /// It is not expected that the user must try again, rather just lets the user know a problem has occured
    /// </summary>
    /// <param name="str">String representing reason for error</param>
    public void DisplayError(string str)
    {
        Console.WriteLine("#####");
        Console.WriteLine(str);
        Console.WriteLine("#####");
    }

    /// <summary>
    /// Displays an error message with the provided reason as a string.
    /// This comes from an invalid input, and prompts the user to try again
    /// </summary>
    /// <param name="e">Thrown argument error, usually by the domain class</param>
    public void DisplayError(ArgumentException e)
    {
        Console.WriteLine("#####");
        Console.WriteLine(e.Message);
         Console.WriteLine("# Please try again.");
        Console.WriteLine("#####");
    }

    /// <summary>
    /// Public helper method, allowing for classes outside of the MenuView to still display strings to the UI.
    /// </summary>
    /// <param name="str">Message</param>
    public void DisplayString(string str)
    {
        Console.WriteLine(str);
    }
    
}

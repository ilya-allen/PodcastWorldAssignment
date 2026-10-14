using System.Net.WebSockets;
using System.Globalization;
using System.Linq;

namespace podcastWorld;

/// <summary>
/// A class that contains methods which validate if inputs used to create
/// episodes, playback sessions and feedback are valid. This class also contains
/// methods which handle features used with podcasts like subscribing or viewing the existing feedback.
/// </summary>
public class PodcastService
{
    // Fields

    // Store the app's current user repository
    private UserRepository userRepository;
    // // Store the view class
    private MenuView view;

    /// <summary>
    /// Podcast Service controller taking class inputs
    /// </summary>
    /// <param name="userRepo">Current user repository</param>
    /// <param name="menuView">View class</param>
    public PodcastService(UserRepository userRepo, MenuView menuView)
    {
        userRepository = userRepo;
        view = menuView;
    }

    /// <summary>
    /// This child class of Episode is used to validate inputs provided by the user
    /// as the new methods inside test each input with the parent classes domain rules.
    /// If an input is invalid, it will throw the same error the Episode class does.
    /// </summary>
    private class ValidationEpisode : Episode
    {
        public ValidationEpisode() : base("Hello World", "The first and most important lesson about coding.", "01/04/2026", 90, false) {}

        // Each method below takes the input and tests it against the domain rules of Episode class.
        // Throws an error when invalid
        public void TestName(string input) => EpisodeName = input;
        public void TestDescription(string input) => EpisodeDescription = input;
        public void TestDate(string input) => EpisodeDate = input;
        public void TestLength(int input) => EpisodeLength = input;
        public void TestPremium(bool input) => EpisodePremiumOnly = input;
        
    }

    /// <summary>
    /// Displays all podcasters and their corresponding episodes
    /// </summary>
    /// <param name="user">The user requesting to view all podcasts</param>
    public void ViewAllPodcasts(User user)
    {
        if(!ensurePodcastExists()) return; // Null tester, checks if there is any podcasters before proceeding.
        view.DisplayString($"Displaying all podcasts for {user.Name}.");

        PodcasterUser? userInput = getPodcaster();  
        if(userInput == null) return; // Null checker 
        view.DisplayString(userInput.PodcastInfoString());
        if(!ensureEpisodeExists(userInput)) return; // Null checker
        view.DisplayEpisodes(userInput.Episodes);
            
    }

    /// <summary>
    /// Subscribe to a specific podcast, then add to the premium users subscribed podcast list
    /// </summary>
    /// <param name="premiumUser">The premium user wishing to subscribe</param>
    public void SubscribeToPodcast(PremiumUser premiumUser)
    {
        if(!ensurePodcastExists()) return; // Null tester
        view.DisplayString("Subscribed podcasts.");

        // Display the users subscribed podcasts (if any)
        if(premiumUser.SubscribedPodcasts.Count > 0)
        {
            view.DisplayPodcasts(premiumUser.SubscribedPodcasts.ToList());
        } else
        {
            view.DisplayString("You are not subscribed to any podcasts.");
        }

        Console.WriteLine("");
        view.DisplayString("Available podcasts.");

        // Exclude podcasts the user has already listened to.
        List<PodcasterUser> availablePodcasts = userRepository.GetPodcasts().Except(premiumUser.SubscribedPodcasts).ToList();
        if(availablePodcasts.Count > 0)
        {
            int userInput = view.DisplayPodcasts(availablePodcasts, "Please enter the podcast's menu number to subscribe or enter 0 to cancel:");

            if (userInput == 0) return;

            if (userInput > 0 && userInput <= availablePodcasts.Count)
            {
                premiumUser.Subscribe(availablePodcasts[userInput - 1]);
                view.DisplayString($"Congratulations, you have been subscribed to {availablePodcasts[userInput - 1].PodcastName}.");
            }
        } else
        {
            view.DisplayString("You are subscribed to all available podcasts.");
        }

    }

    /// <summary>
    /// Unsubscribe from a specific podcast, also remove the podcast from the premium users subscribed list
    /// </summary>
    /// <param name="premiumUser">The premium user managing unsubscriptions</param>
    public void UnsubscribeToPodcast(PremiumUser premiumUser)
    {
        if(!ensurePodcastExists()) return;
        view.DisplayString("Subscribed podcasts.");

        if(premiumUser.SubscribedPodcasts.Count > 0)
        {
            // Present subscribed podcasts in alphabet order
            List<PodcasterUser> subscribedPodcasts = premiumUser.SubscribedPodcasts.OrderBy(user => user.PodcastName).ToList();
            int userInput = view.DisplayPodcasts(subscribedPodcasts, "Please enter the podcast's menu number to unsubscribe or enter 0 to cancel:");
            if (userInput == 0) return;

            if (userInput > 0 && userInput <= subscribedPodcasts.Count)
            {
                premiumUser.Unsubscribe(subscribedPodcasts[userInput - 1]);
                view.DisplayString($"You have been unsubscribed to {subscribedPodcasts[userInput - 1].PodcastName}.");
            }
            
        } else
        {
            view.DisplayString("You are not subscribed to any podcasts.");
        }
        
    }

    /// <summary>
    /// Prompts the user for episode details, validates them, then adds the new episode to the podcasters episodes list.
    /// </summary>
    /// <param name="podcasterUser">The podcaster creating the episode</param>
    public void CreateEpisode(PodcasterUser podcasterUser)
    {
        List<Func<object>> episodeInput = view.GetEpisode(); // Prompts
        List<string> validatedInputs = validateEpisode(episodeInput); 

        // Extract all valid inputs from the list, these inputs are stored individually
        // and then a new class is created with these values for initialisation.
        string finalName = validatedInputs[0];
        string finalDescription = validatedInputs[1];
        string finalDate = validatedInputs[2];
        int finalLength = int.Parse(validatedInputs[3]);
        bool finalPremiumOnly = bool.Parse(validatedInputs[4]);


        Episode newEpisode = new Episode(finalName, finalDescription, finalDate, finalLength, finalPremiumOnly);
        podcasterUser.AddEpisode(newEpisode);
    }

    /// <summary>
    /// Handles the creation and/or updating of an episode playback session.
    /// Also handles for when the listener is a premium listener and wishes for a premium only podcast.
    /// </summary>
    /// <param name="user">The listener</param>
    /// <param name="premiumPodcasts">If true, the user is premium</param>
    public void CreatePlayback(User user, bool premiumPodcasts = false)
    {
        if(premiumPodcasts)
        {
            if(!ensurePodcastExists((PremiumUser)user)) return;
            view.DisplayString($"Displaying subscribed podcasts for {user.Name} to listen to.");
        } else
        {
            if(!ensurePodcastExists()) return;
            view.DisplayString($"Displaying podcasts for {user.Name} to listen to.");   
        }


        PodcasterUser? userInput;
        if(premiumPodcasts) 
        {
            userInput = getPodcaster((PremiumUser)user);
        } else
        {
            userInput = getPodcaster();
        }
        if (userInput == null) return;
        
        if(!ensureEpisodeExists(userInput)) return;
        view.DisplayString(userInput.PodcastInfoString());
        

        Episode? episode = getPodcastEpisode(userInput);
        if (episode == null) return;
        if(user.UserType() == "Regular Listener" && episode.EpisodePremiumOnly == true) 
        {
            view.DisplayError("# Error - This episode can only be view by premium listeners.");
            return;
        }

        Playback? checkExistingPlayback = user.GetPlayback(episode);
        int previousMinutes = checkExistingPlayback?.MinutesListened ?? 0;

        // check and send messages if neeeded
        if (checkExistingPlayback != null && previousMinutes < episode.EpisodeLength)
        {
            MinutesMessage(user, userInput, episode, previousMinutes, 0, checkExistingPlayback.LastListenedDate, isInitialMessage: true);
        }
        else if (previousMinutes >= episode.EpisodeLength)
        {
            view.DisplayError("# Error - You have already finished this episode and cannot listen to it again.");
            return;
        }

        List<Func<object>> playbackInput = view.GetPlayback();
        List<string> validatedInputs = validatePlayback(playbackInput, episode, previousMinutes, checkExistingPlayback);

        string finalDate = validatedInputs[0];
        int finalMinutes = int.Parse(validatedInputs[1]);

        if (checkExistingPlayback == null)
        {
            Playback newPlayback = new Playback(episode, finalDate, finalMinutes);
            user.AddPlayback(newPlayback);
        }
        else
        {
            checkExistingPlayback.UpdatePlayback(finalDate, finalMinutes);
        }

        MinutesMessage(user, userInput, episode, previousMinutes, finalMinutes, finalDate);

    }

    /// <summary>
    /// Display the episodes from the podcasters perspective
    /// </summary>
    /// <param name="podcasterUser">Podcaster</param>
    public void ViewPodcastEpisodes(PodcasterUser podcasterUser)
    {
        if(!ensureEpisodeExists(podcasterUser, true)) return;
        view.DisplayPodcasterEpisodes(podcasterUser.Episodes);
    }

    /// <summary>
    /// Display all the podcasters episodes statistics.
    /// </summary>
    /// <param name="podcasterUser">Podcaster viewing their statistics</param>
    public void ViewEpisodeStatistics(PodcasterUser podcasterUser)
    {
        if(!ensureEpisodeExists(podcasterUser, true)) return;
        List<Episode> podcasterEpisodes = podcasterUser.Episodes.ToList();

        // This is the placeholder for each episode and their respective statistics before being passed into the view display function
        Dictionary<Episode, int[]> episodeStatistics = new Dictionary<Episode, int[]>();
        foreach (Episode episode in podcasterEpisodes)
        {
            // Count listeners who have started
            IEnumerable<User> listenersStarted = userRepository.GetListeners().Where(u =>
            {
                Playback? getPlayback = u.GetPlayback(episode);
                return getPlayback != null && getPlayback.MinutesListened > 0;
            });

            // Count listeners who have completed the episode
            IEnumerable<User> listenersCompleted = userRepository.GetListeners().Where(u =>
            {
                Playback? getPlayback = u.GetPlayback(episode);
                return getPlayback != null && getPlayback.MinutesListened >= getPlayback.PlaybackEpisode.EpisodeLength;
            });


            episodeStatistics.Add(episode, [listenersStarted.Count(), listenersCompleted.Count()]);
        }

        view.DisplayPodcasterStatistics(episodeStatistics);
    }
 
    /// <summary>
    /// Prompts user to select an episode and submit a feedback rating
    /// </summary>
    /// <param name="user"></param>
    public void CreateFeedback(User user)
    {
        if(!ensurePodcastExists()) return;
        view.DisplayString($"Displaying potential podcasts for {user.Name} to provide feedback on.");
       
        PodcasterUser? podcasterUser = getPodcaster();
        if(podcasterUser == null) return;
        if(!ensureEpisodeExists(podcasterUser)) return;
    
        List<Episode> episodeList = podcasterUser.Episodes.ToList();
        Episode? userInputEpisode = getPodcastEpisode(episodeList.ToList());
        if(userInputEpisode == null) return;
        
        // Prompt user for inputs then validate these inputs
        List<Func<object>> reviewInput = view.GetReview();
        List<string> validatedInputs = validateFeedback(reviewInput, user, userInputEpisode);

        // Store extracted validated inputs before initialising a feedback class with them
        bool finalIsThumbsUp = bool.Parse(validatedInputs[0]);
        string finalText = validatedInputs[1];
        

        Feedback newFeedback = new Feedback(user, userInputEpisode, finalIsThumbsUp, finalText);
        podcasterUser.AddReview(newFeedback);

    }

    /// <summary>
    /// Display all feedback that was provided by listeners, then have the ability to see the written text feedback aswell.
    /// </summary>
    /// <param name="podcasterUser">Podcaster viewing the feedback</param>
    public void ViewFeedback(PodcasterUser podcasterUser)
    {
        if(!ensureEpisodeExists(podcasterUser, true)) return;
        List<Feedback> podcasterFeedback = podcasterUser.ProvidedFeedback.ToList();
        List<Episode> podcasterEpisodes = podcasterUser.Episodes.ToList();

        // Placeholder list for all episodes and their possible feedback
        List<EpisodeFeedbackStats> statsList = new List<EpisodeFeedbackStats>();
        
        foreach(Episode episode in podcasterEpisodes)
        {
            List<Feedback> episodeFeedback = podcasterFeedback.Where(f => f.ReviewedEpisode == episode).ToList();

            int thumbsUpCount = episodeFeedback.Count(f => f.IsThumbsUp);

            EpisodeFeedbackStats episodeStats = new EpisodeFeedbackStats(episode, thumbsUpCount, episodeFeedback);
            statsList.Add(episodeStats);
        }


        bool isValidInput = false;

        while(!isValidInput)
        {   
            // Display all episodes w/ feedback
            int userInput = view.DisplayFeedback(statsList);

            if(userInput == 0) return; // Cancel clause
            if(userInput == -1 || userInput > statsList.Count)
            {
                view.DisplayError("# Error - Provided value is out of range.");
            } else
            {
                view.DisplayFeedbackText(statsList[userInput - 1].Reviews); // Display specific statistics
                isValidInput = true;
            }

            
        }
        

        
    }

    // Provides a list of all validated inputs for an episode. Used during episode creation
    private List<string> validateEpisode(List<Func<object>> episodeDetails)
    {
        // Each input required
        const int NAME = 0, DESCRIPTION = 1, DATE = 2, LENGTH = 3, PREMIUM_ONLY = 4;

        List<string> validInputs = new List<string>();
        int index = 0;
        foreach(object detail in episodeDetails)
        {
            bool isValidInput = false;

            while(!isValidInput)
            {
                try
                {
                    var inputMethod = (Func<object>)detail;
                    object methodResult = inputMethod();

                    // A new instance of the tester class, featuring methods for each input field, testing aginst Episode's domain rules
                    ValidationEpisode validationEpisode = new ValidationEpisode();

                    switch (index)
                    {
                        case NAME: // Episode Name
                            string nameInput = methodResult.ToString() ?? "";
                            validationEpisode.TestName(nameInput);
                            validInputs.Add(nameInput);
                            break;

                        case DESCRIPTION: // Episode Description
                            string descInput = methodResult.ToString() ?? "";
                            validationEpisode.TestDescription(descInput);
                            validInputs.Add(descInput);
                            break;

                        case DATE: // Episode Date (dd/MM/yyyy)
                            string dateInput = methodResult.ToString() ?? "";
                            validationEpisode.TestDate(dateInput);
                            validInputs.Add(dateInput);
                            break;

                        case LENGTH: // Episode Length (1-300)
                            if (!int.TryParse(methodResult.ToString(), out int lengthInput))
                            {
                                throw new ArgumentException("# Error - Supplied episode length is invalid.");
                            }
                            validationEpisode.TestLength(lengthInput);
                            validInputs.Add(lengthInput.ToString());
                            break;

                        case PREMIUM_ONLY: // Episode Premium Only (bool)
                            if (!bool.TryParse(methodResult.ToString(), out bool premiumInput))
                            {
                                throw new ArgumentException("# Error - Supplied value must be a boolean.");
                            }
                            validationEpisode.TestPremium(premiumInput);
                            validInputs.Add(premiumInput.ToString());
                            break;
                    }

                    isValidInput = true; 
                }
                catch (ArgumentException e)
                {
                    view.DisplayError(e);
                }
            }
            index++;
        }
        return validInputs;
    }

    // Provides a list of validated inputs for playback session. 
    private List<string> validatePlayback(List<Func<object>> playbackDetails, Episode targetEpisode, int previousMinutes, Playback? existingPlayback = null)    
    {
        const int DATE = 0, MINUTES_LISTENED = 1;
        List<string> validInputs = new List<string>();
        int index = 0;
        foreach(object detail in playbackDetails)
        {
            bool isValidInput = false;
            while(!isValidInput)
            {
                try
                {
                    var inputMethod = (Func<object>)detail;
                    object methodResult = inputMethod();

                    switch (index)
                    {
                        case DATE: // Playback Date
                            string dateInput = methodResult.ToString() ?? "";

                            // Parse the new date input
                            if (!DateTime.TryParseExact(dateInput, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime newDate))
                            {
                                throw new ArgumentException("# Error - Supplied date is invalid.");
                            }

                            // Determine the reference date to compare against (existing playback date or episode release date)
                            string referenceDateStr = existingPlayback != null ? existingPlayback.LastListenedDate : targetEpisode.EpisodeDate;

                            if (DateTime.TryParseExact(referenceDateStr, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime referenceDate))
                            {
                                if (newDate < referenceDate)
                                {
                                    throw new ArgumentException($"# Error - Supplied date must be after {referenceDate:d/M/yyyy}.");
                                }
                            }

                            validInputs.Add(dateInput);
                            break;

                        case MINUTES_LISTENED: // Minutes Listened
                            if (!int.TryParse(methodResult.ToString(), out int minutesListened))
                            {
                                throw new ArgumentException("# Error - Supplied value is out of range.");
                            }

                            int remainingMinutes = targetEpisode.EpisodeLength - previousMinutes;

                            if (minutesListened <= 0 || minutesListened > remainingMinutes)
                            {
                                throw new ArgumentException("# Error - Supplied value is out of range.");
                            }

                            // Due to only input needing testing under domain rules. Rather than creating a whole child class, a simple test instance is enough to just test the minutes.
                            Playback testMinutes = new Playback(targetEpisode, targetEpisode.EpisodeDate, minutesListened);
                            validInputs.Add(minutesListened.ToString());
                            break;
                    }

                    isValidInput = true; 
                }
                catch (ArgumentException e)
                {
                    view.DisplayError(e); 
                }
            }
            index++;
        }
        return validInputs;
    }

    // Returns a list of validated feedback inputs.
    private List<string> validateFeedback(List<Func<object>> playbackDetails, User user, Episode targetEpisode)
    {
        const int RATING = 0, FEEDBACK = 1; 

        List<string> validInputs = new List<string>();
        int index = 0;
        foreach(object detail in playbackDetails)
        {
            bool isValidInput = false;

            
            while(!isValidInput)
            {
                try
                {
                    var inputMethod = (Func<object>)detail;
                    object methodResult = inputMethod();


                    switch (index)
                    {
                        case RATING: // Review Rating
                            if (!bool.TryParse(methodResult.ToString(), out bool reviewInput))
                            {
                                throw new ArgumentException("# Error - Supplied value must be a boolean.");
                            }
                            validInputs.Add(reviewInput.ToString());
                            break;

                        case FEEDBACK: // Review Feedback
                            string reviewText = methodResult.ToString() ?? "";
                            Feedback testText = new Feedback(user, targetEpisode, false, reviewText);
                            validInputs.Add(reviewText);
                            break;

                    }

                    isValidInput = true; 
                }
                catch (ArgumentException e)
                {
                    view.DisplayError(e);
                }
            }
            index++;
        }
        return validInputs;

    }


    // Simply check if any podcasts exist. 
    private bool ensurePodcastExists(PremiumUser premiumUser = null)
    {
        if(!userRepository.HasPodcast())
        {
            view.DisplayError("# Error - There are no podcasts.");
            return false;
        }

        if (premiumUser != null)
        {
            if(premiumUser.SubscribedPodcasts.Count < 1)
            {
                view.DisplayError("# Error - You are not subscribed to any podcasts.");
                return false;
            }
        }

        return true;
    }

    // Taking a podcaster user, check if any episodes exist
    private bool ensureEpisodeExists(PodcasterUser podcasterUser, bool fromPodcaster = false)
    {
        if(podcasterUser.Episodes.Count < 1 && fromPodcaster == false)
        {
            view.DisplayError("# Error - The podcaster has not recorded any episodes.");
            return false;
        }

        if(podcasterUser.Episodes.Count < 1 && fromPodcaster == true)
        {
            view.DisplayError("# Error - You have not recorded any episodes.");
            return false;
        }
        return true;
    }


    // Return a chosen podcaster from a list of podcasters available
    private PodcasterUser? getPodcaster(PremiumUser premiumUser = null)
    {
        List<PodcasterUser> podcasters;
        if(premiumUser != null)
        {
            podcasters = premiumUser.SubscribedPodcasts.ToList();
        } else
        {
           podcasters = userRepository.GetPodcasts(); 
        }
        bool isValidInput = false;

        view.DisplayPodcasts(podcasters);
        while (!isValidInput)
        {
            try
            {
                int userInput = view.GetInteger("Please enter the podcast's menu number to view more information or enter 0 to cancel:");  // CODE SMELL 
                if (userInput == 0)
                {
                    return null;
                }
                if (userInput < 1 || userInput > podcasters.Count)
                {
                    throw new ArgumentException("# Error - Supplied value is out of range.");
                }
                isValidInput = true;
                return podcasters[userInput - 1];
            }
            catch (ArgumentException e)
            {
                view.DisplayError(e);
            }
        }

        return null;
    }

    // Return a chosen episode from a provided podcaster.
    private Episode? getPodcastEpisode(PodcasterUser podcasterUser)
    {
        List<Episode> episodes = podcasterUser.Episodes.ToList();

        view.DisplayEpisodes(episodes);
        bool isValidInput = false;
        while (!isValidInput)
        {
            try
            {
                int userInput = view.GetInteger("Please enter the episode's menu number to listen to the episode or enter 0 to cancel:"); // Code smell 
                if (userInput == 0)
                {
                    return null;
                }
                if (userInput < 1 || userInput > episodes.Count)
                {
                    throw new ArgumentException("# Error - Supplied value is out of range.");
                }
                isValidInput = true;
                return episodes[userInput - 1];
            }
            catch (ArgumentException e)
            {
                view.DisplayError(e);
            }
        }

        return null;
    }

    // Prompt the user to select an episode from a provided episode list
    private Episode? getPodcastEpisode(List<Episode> providedEpisodes)
    {
        List<Episode> episodes = providedEpisodes;

        view.DisplayEpisodes(episodes);
        bool isValidInput = false;
        while (!isValidInput)
        {
            try
            {
                int userInput = view.GetInteger("Please enter the episode's menu number to provide feedback or 0 to cancel:"); // Code smell 
                if (userInput == 0)
                {
                    return null;
                }
                if (userInput < 1 || userInput > episodes.Count)
                {
                    throw new ArgumentException("# Error - Supplied value is out of range.");
                }
                isValidInput = true;
                return episodes[userInput - 1];
            }
            catch (ArgumentException e)
            {
                view.DisplayError(e);
            }
        }

        return null;
    }

    // Format and display playback progress to the user.
    private void MinutesMessage(User user, PodcasterUser podcast, Episode episode, int previousMinutes, int finalMinutes, string date, bool isInitialMessage = false) {
        if (isInitialMessage)
        {
            int remainingMinutes = episode.EpisodeLength - previousMinutes;
            view.DisplayString($"You have listened to podcast {podcast.PodcastName}, episode {episode.EpisodeName}, for {previousMinutes} minutes. The last time you listened was on {date}. You have {remainingMinutes} minutes remaining.");
        } else if (previousMinutes + finalMinutes >= episode.EpisodeLength)
        {
            view.DisplayString($"Congratulations {user.Name}. You have finished the episode {episode.EpisodeName}.");
        } else
        {
            int remainingMinutes = episode.EpisodeLength - (previousMinutes + finalMinutes);
            view.DisplayString($"In total you have listened to podcast {podcast.PodcastName}, episode {episode.EpisodeName}, for {previousMinutes + finalMinutes} minutes. The last time you listened was on {date}. You have {remainingMinutes} minutes remaining.");
        }
    }
}

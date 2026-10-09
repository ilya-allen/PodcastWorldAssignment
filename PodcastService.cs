using System.Net.WebSockets;
using System.Globalization;
using System.Linq;

namespace podcastWorld;

public class PodcastService
{
    private readonly UserRepository userRepository;
    private readonly MenuView view;

    public PodcastService(UserRepository userRepo, MenuView menuView)
    {
        userRepository = userRepo;
        view = menuView;
    }

    private class ValidationEpisode : Episode
    {
        public ValidationEpisode() : base("Hello World", "The first and most important lesson about coding.", "01/04/2026", 90, false) {}

        public void TestName(string input) => EpisodeName = input;
        public void TestDescription(string input) => EpisodeDescription = input;
        public void TestDate(string input) => EpisodeDate = input;
        public void TestLength(int input) => EpisodeLength = input;
        public void TestPremium(bool input) => EpisodePremiumOnly = input;
        
    }

    public void ViewAllPodcasts(User user)
    {
        if(!ensurePodcastExists()) return;
        view.DisplayString($"Displaying all podcasts for {user.Name}.");

        PodcasterUser? userInput = getPodcaster();  
        if(userInput == null) return;
        view.DisplayString(userInput.PodcastInfoString());
        if(!ensureEpisodeExists(userInput)) return;
        view.DisplayEpisodes(userInput.GetEpisodeList());
            
    }

    public void SubscribeToPodcast(PremiumUser premiumUser)
    {
        if(!ensurePodcastExists()) return;
        view.DisplayString("Subscribed podcasts.");
        if(premiumUser.SubscribedPodcasts.Count > 0)
        {
            view.DisplayPodcasts(premiumUser.SubscribedPodcasts.ToList());
        } else
        {
            view.DisplayString("You are not subscribed to any podcasts.");
        }

        Console.WriteLine("");
        view.DisplayString("Available podcasts.");
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

    public void UnsubscribeToPodcast(PremiumUser premiumUser)
    {
        if(!ensurePodcastExists()) return;
        view.DisplayString("Subscribed podcasts.");

        if(premiumUser.SubscribedPodcasts.Count > 0)
        {
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
    public void CreateEpisode(PodcasterUser podcasterUser)
    {
        List<Func<object>> episodeInput = view.GetEpisode();

        List<string> validatedInputs = validateEpisode(episodeInput);

        string finalName = validatedInputs[0];
        string finalDescription = validatedInputs[1];
        string finalDate = validatedInputs[2];
        int finalLength = int.Parse(validatedInputs[3]);
        bool finalPremiumOnly = bool.Parse(validatedInputs[4]);


        Episode newEpisode = new Episode(finalName, finalDescription, finalDate, finalLength, finalPremiumOnly);
        podcasterUser.AddEpisode(newEpisode);
    }

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

        // check and send messages ifneeeded
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

    public void ViewPodcastEpisodes(PodcasterUser podcasterUser)
    {
        if(!ensureEpisodeExists(podcasterUser, true)) return;
        view.DisplayPodcasterEpisodes(podcasterUser.GetEpisodeList());
    }

    public void ViewEpisodeStatistics(PodcasterUser podcasterUser)
    {
        if(!ensureEpisodeExists(podcasterUser, true)) return;
        List<Episode> podcasterEpisodes = podcasterUser.GetEpisodeList();

        Dictionary<Episode, int[]> episodeStatistics = new Dictionary<Episode, int[]>();
        foreach (Episode episode in podcasterEpisodes)
        {
            IEnumerable<User> listenersStarted = userRepository.GetListeners().Where(u =>
            {
                Playback? getPlayback = u.GetPlayback(episode);
                return getPlayback != null && getPlayback.MinutesListened > 0;
            });

            IEnumerable<User> listenersCompleted = userRepository.GetListeners().Where(u =>
            {
                Playback? getPlayback = u.GetPlayback(episode);
                return getPlayback != null && getPlayback.MinutesListened >= getPlayback.PlaybackEpisode.EpisodeLength;
            });

            episodeStatistics.Add(episode, [listenersStarted.Count(), listenersCompleted.Count()]);
        }

        view.DisplayPodcasterStatistics(episodeStatistics);
    }
 
    public void CreateFeedback(User user)
    {
        if(!ensurePodcastExists()) return;
        view.DisplayString($"Displaying potential podcasts for {user.Name} to provide feedback on.");
        PodcasterUser? targetPodcaster = getPodcaster();
        if(targetPodcaster == null) return;
        if(!ensureEpisodeExists(targetPodcaster)) return;
        

        List<Episode> episodeList = targetPodcaster.GetEpisodeList();

        Episode? userInputEpisode = getPodcastEpisode(episodeList.ToList());
        if(userInputEpisode == null) return;
        
        List<Func<object>> reviewInput = view.GetReview();
        List<string> validatedInputs = validateFeedback(reviewInput, user, userInputEpisode);

        bool finalIsThumbsUp = bool.Parse(validatedInputs[0]);
        string finalText = validatedInputs[1];
        


        Feedback newFeedback = new Feedback(user, userInputEpisode, finalIsThumbsUp, finalText);
        targetPodcaster.AddReview(newFeedback);

    }

    public void ViewFeedback(PodcasterUser podcasterUser)
    {
        if(!ensureEpisodeExists(podcasterUser, true)) return;
        List<Feedback> podcasterFeedback = podcasterUser.ProvidedFeedback.ToList();
        List<Episode> podcasterEpisodes = podcasterUser.GetEpisodeList();

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
            int userInput = view.DisplayFeedback(statsList);

            if(userInput == 0) return;
            if(userInput == -1 || userInput > statsList.Count)
            {
                view.DisplayError("# Error - Provided value is out of range.");
            } else
            {
                view.DisplayFeedbackText(statsList[userInput - 1].Reviews);
                isValidInput = true;
            }

            
        }
        

        
    }

    private List<string> validateEpisode(List<Func<object>> episodeDetails)
    {
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

                    ValidationEpisode validationEpisode = new ValidationEpisode();

                    switch (index)
                    {
                        case 0: // Episode Name
                            string nameInput = methodResult.ToString() ?? "";
                            validationEpisode.TestName(nameInput);
                            validInputs.Add(nameInput);
                            break;

                        case 1: // Episode Description
                            string descInput = methodResult.ToString() ?? "";
                            validationEpisode.TestDescription(descInput);
                            validInputs.Add(descInput);
                            break;

                        case 2: // Episode Date (dd/MM/yyyy)
                            string dateInput = methodResult.ToString() ?? "";
                            validationEpisode.TestDate(dateInput);
                            validInputs.Add(dateInput);
                            break;

                        case 3: // Episode Length (1-300)
                            if (!int.TryParse(methodResult.ToString(), out int lengthInput))
                            {
                                throw new ArgumentException("# Error - Supplied episode length is invalid.");
                            }
                            validationEpisode.TestLength(lengthInput);
                            validInputs.Add(lengthInput.ToString());
                            break;

                        case 4: // Episode Premium Only (bool)
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

private List<string> validatePlayback(List<Func<object>> playbackDetails, Episode targetEpisode, int previousMinutes, Playback? existingPlayback = null)    
    {
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
                        case 0: // Playback Date
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

                        case 1: // Minutes Listened
                            if (!int.TryParse(methodResult.ToString(), out int minutesListened))
                            {
                                throw new ArgumentException("# Error - Supplied value is out of range.");
                            }

                            int remainingMinutes = targetEpisode.EpisodeLength - previousMinutes;

                            if (minutesListened <= 0 || minutesListened > remainingMinutes)
                            {
                                throw new ArgumentException("# Error - Supplied value is out of range.");
                            }

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

    private List<string> validateFeedback(List<Func<object>> playbackDetails, User user, Episode targetEpisode)
    {
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
                        case 0: // Review Rating
                            if (!bool.TryParse(methodResult.ToString(), out bool reviewInput))
                            {
                                throw new ArgumentException("# Error - Supplied value must be a boolean.");
                            }
                            validInputs.Add(reviewInput.ToString());
                            break;

                        case 1: // Review Feedback
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

    private bool ensureEpisodeExists(PodcasterUser podcasterUser, bool fromPodcaster = false)
    {
        if(podcasterUser.GetEpisodeList().Count < 1 && fromPodcaster == false)
        {
            view.DisplayError("# Error - The podcaster has not recorded any episodes.");
            return false;
        }

        if(podcasterUser.GetEpisodeList().Count < 1 && fromPodcaster == true)
        {
            view.DisplayError("# Error - You have not recorded any episodes.");
            return false;
        }
        return true;
    }


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

    private Episode? getPodcastEpisode(PodcasterUser podcasterUser)
    {
        List<Episode> episodes = podcasterUser.GetEpisodeList();

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

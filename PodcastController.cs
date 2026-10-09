namespace podcastWorld;

public class PodcastController
{
    private User? user;
    private MenuView view;
    private UserRepository userRepository;
    private UserService userService;
    private PodcastService podcastService;
    private User? currentUser;


    public PodcastController()
    {
        user = null;
        view = new MenuView();
        userRepository = new UserRepository();
        userService = new UserService(userRepository, view);
        podcastService = new PodcastService(userRepository, view);
        currentUser = null;

    }

    public void Run()
    {
        view.DisplayHeader();
        bool continueLoop = true;

        while(continueLoop)
        {
            continueLoop = RunMainMenu();
        }
        
    }

    public bool RunMainMenu()
    {
        int userInput = view.DisplayMainMenu();
        switch(userInput)
        {
            case 1: // ALSO CREATE AN INTS FILE
                LogInUser();
                break;
            case 2: // ALSO CREATE AN INTS FILE
                RegisterUser(); 
                break;
            case 3:
                view.DisplayExitMessage();
                return false;
        }
        return true;
    }

    public void LogInUser()
    {
        if(userRepository.UserCount() > 0)
        {
            User loginAttempt = userService.AuthenticateUser(view.GetLogIn());
            currentUser = loginAttempt;
            view.DisplayString(currentUser.UserWelcomeBackString());

            switch(currentUser.UserType())
            {
                case "Regular Listener":
                    RegularLogInMenu();
                    break;
                case "Premium Listener":
                    PremiumLogInMenu();    
                    break;
                case "Podcaster":
                    PodcasterLogInMenu();
                    break;
            }
        } else
        {
            view.DisplayError("# Error - There are no people registered.");
        }
        
        
    }


    public void RegisterUser()
    {
        const int REGULAR_USER = 1, PREMIUM_USER = 2, PODCASTER = 3;
        int userInput = view.RegistrationMenu();
        switch(userInput)
        {
            case REGULAR_USER:
                createNewRegularUser();
                break;
            case PREMIUM_USER:
                createNewPremiumUser();
                break;
            case PODCASTER:
                createNewPodcasterUser();
                break;
        } 
    }

    private void RegularLogInMenu()
    {
        int userInput;
        while(currentUser is not null)
        {
            userInput = view.RegularListenerMenu();
            switch (userInput)
            {
                case 1:
                    view.DisplayString(currentUser.UserInfoString());
                    break;
                case 2:
                    userService.ValidatePassword(currentUser);
                    view.DisplayString("Password has been updated.");
                    break;
                case 3:
                    podcastService.ViewAllPodcasts(currentUser);
                    break;
                case 4:
                    podcastService.CreatePlayback(currentUser);
                    break;
                case 5:
                    podcastService.CreateFeedback(currentUser);
                    break;
                case 6:
                    currentUser = null;
                    break;
                default:
                    break;
                
            }
        }

    }

    private void PremiumLogInMenu()
    {
        int userInput;
        while(currentUser is not null)
        {
            userInput = view.PremiumListenerMenu();
            switch (userInput)
            {
                case 1:
                    view.DisplayString(currentUser.UserInfoString());
                    break;
                case 2:
                    userService.ValidatePassword(currentUser);
                    view.DisplayString("Password has been updated.");
                    break;
                case 3:
                    podcastService.ViewAllPodcasts(currentUser);
                    break;
                case 4:
                    podcastService.CreatePlayback(currentUser);
                    break;
                case 5:
                    podcastService.CreatePlayback(currentUser, true);
                    break;
                case 6:
                    podcastService.SubscribeToPodcast((PremiumUser)currentUser);
                    break;
                case 7:
                    podcastService.UnsubscribeToPodcast((PremiumUser)currentUser);
                    break;
                case 8:
                    podcastService.CreateFeedback(currentUser);
                    break;
                case 9:
                    currentUser = null;
                    break;
                default:
                    break;
                
            }
        }
    }
    private void PodcasterLogInMenu()
    {
        int userInput;
        while(currentUser is not null)
        {
            userInput = view.PodcasterMenu();
            switch (userInput)
            {
                case 1:
                    view.DisplayString(currentUser.UserInfoString());
                    break;
                case 2:
                    userService.ValidatePassword(currentUser);
                    view.DisplayString("Password has been updated.");
                    break;
                case 3:
                    podcastService.CreateEpisode((PodcasterUser)currentUser);
                    break;
                case 4:
                    podcastService.ViewPodcastEpisodes((PodcasterUser)currentUser);
                    break;
                case 5:
                    podcastService.ViewEpisodeStatistics((PodcasterUser)currentUser);
                    break;
                case 6:
                    podcastService.ViewFeedback((PodcasterUser)currentUser);
                    break;
                case 7:
                    currentUser = null;
                    break;
                default:
                    break;
                
            }
        }
    }
 
    private void createNewRegularUser()
    {
        view.DisplayString("Registering as a regular listener.");
        List<Func<object>> userMethods = view.RegisterNewUser();
        List<string> validatedInputs = userService.ValidateNewUser(userMethods);

        string finalName = validatedInputs[0];
        int finalAge = int.Parse(validatedInputs[1]); // Safe to parse because the service already verified it's a number
        string finalEmail = validatedInputs[3];
        string finalMobile = validatedInputs[2]; // Safe to parse
        string finalPassword = validatedInputs[4];

        // 5. Create your actual concrete User object (e.g., a 'Listener' or 'Creator' subclass)
        // Since 'User' is abstract, substitute 'ListenerUser' below with your actual concrete class name:
        User newUser = new RegularUser(finalName, finalAge, finalMobile, finalEmail, finalPassword);
        view.DisplayString(newUser.UserWelcomeString());
        userRepository.AddUser(newUser); 
    }

    private void createNewPremiumUser()
    {
        view.DisplayString("Registering as a premium listener.");
        List<Func<object>> userMethods = view.RegisterNewUser();
        List<Func<object>> premiumUserMethods = view.RegisterNewPremiumUser();
        List<string> validatedInputs = userService.ValidateNewUser(userMethods);
        List<string> premiumValidatedInputs = userService.ValidatePremiumUser(premiumUserMethods);

        string finalName = validatedInputs[0];
        int finalAge = int.Parse(validatedInputs[1]); // Safe to parse because the service already verified it's a number
        string finalEmail = validatedInputs[3];
        string finalMobile = validatedInputs[2]; // Safe to parse
        string finalPassword = validatedInputs[4];
        string finalDate = premiumValidatedInputs[0];
        int finalPayID = int.Parse(premiumValidatedInputs[1]);

        User newUser = new PremiumUser(finalName, finalAge, finalMobile, finalEmail, finalPassword, finalDate, finalPayID);
        view.DisplayString(newUser.UserWelcomeString());
        userRepository.AddUser(newUser); 
    }

    private void createNewPodcasterUser()
    {
        view.DisplayString("Registering as a podcaster.");
        List<Func<object>> userMethods = view.RegisterNewUser();
        List<Func<object>> podcasterUserMethods = view.RegisterNewPodcasterUser();
        List<string> validatedInputs = userService.ValidateNewUser(userMethods);
        List<string> podcasterValidatedInputs = userService.ValidatePodcasterUser(podcasterUserMethods);

        string finalName = validatedInputs[0];
        int finalAge = int.Parse(validatedInputs[1]); // Safe to parse because the service already verified it's a number
        string finalEmail = validatedInputs[3];
        string finalMobile = validatedInputs[2]; // Safe to parse
        string finalPassword = validatedInputs[4];

        PodcastConsts.Genre finalPodcastGenre = (PodcastConsts.Genre)int.Parse(podcasterValidatedInputs[0]);
        string finalPodcastName = podcasterValidatedInputs[1];
        string finalPodcastDescription = podcasterValidatedInputs[2];


        User newUser = new PodcasterUser(finalName, finalAge, finalMobile, finalEmail, finalPassword, finalPodcastGenre, finalPodcastName, finalPodcastDescription);
        view.DisplayString(newUser.UserWelcomeString());
        userRepository.AddUser(newUser); 
    }


    


}

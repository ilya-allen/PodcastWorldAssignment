namespace podcastWorld;


/// <summary>
/// A class that controls the rest of the program. This connects the view and the backend
/// data stored in the domain classes. This also acts as a bridge for all service classes.
/// 
/// Responsibility: Controll the program via connecting UI with backend
/// </summary>
public class PodcastController
{
    private MenuView view;
    private UserRepository userRepository;
    private UserService userService;
    private PodcastService podcastService;
    private User? currentUser;


    /// <summary>
    /// Constructor for the controller
    /// </summary>
    public PodcastController()
    {
        view = new MenuView();
        userRepository = new UserRepository();
        userService = new UserService(userRepository, view);
        podcastService = new PodcastService(userRepository, view);
        currentUser = null;

    }

    /// <summary>
    /// The initial top of the stack loop.
    /// This method keeps the entire program running. When the user exits
    /// this loop will end, and therefore the entire program.
    /// </summary>
    public void Run()
    {
        view.DisplayHeader();
        bool continueLoop = true;

        while(continueLoop)
        {
            continueLoop = RunMainMenu();
        }
        
    }
    
    /// <summary>
    /// This method is the first task. Asks the user whether they wish to register
    /// or to log in through their respective methods.
    /// </summary>
    /// <returns></returns>
    public bool RunMainMenu()
    {
        const int LOG_IN = 1, REGISTER = 2, EXIT = 3;

        int userInput = view.DisplayMainMenu(); // will take an input from one of the above
        switch(userInput)
        {
            case LOG_IN: 
                LogInUser();
                break;
            case REGISTER: 
                RegisterUser(); 
                break;
            case EXIT:
                view.DisplayExitMessage();
                return false;
        }
        return true;
    }

    /// <summary>
    /// If the user wishes to log in, this method is called.
    /// Proccesses the type of user and calls the following types method.
    /// </summary>
    public void LogInUser()
    {
        if(userRepository.UserCount() > 0) // If no users are registered, prompt error
        {
            // Proccess Login and welcome the user
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

    /// <summary>
    /// Processes the registration of a new user, taking their preffered user type
    /// and all necessary details to create the user.
    /// </summary>

    public void RegisterUser()
    {
        // All possible choices for the user to take, each one leading to their
        // respective log in methods.
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

    /// <summary>
    /// Once a regular listener has logged in, this proccesses all possible tasks
    /// that the regular listener can choose to do.
    /// </summary>
    private void RegularLogInMenu()
    {
        // Aside from the common opening options for all users, these additional inputs are specific
        // to the Regular listener.
        const int FEEDBACK = 5, EXIT = 6;
        int userInput;

        while(currentUser is not null) // Loop as long as the user is logged in
        {
            userInput = view.RegularListenerMenu();
            switch (userInput)
            {
                case PodcastConsts.VIEW_DETAILS:
                    view.DisplayString(currentUser.UserInfoString());
                    break;
                case PodcastConsts.CHANGE_PASSWORD:
                    userService.ValidatePassword(currentUser);
                    view.DisplayString("Password has been updated.");
                    break;
                case PodcastConsts.VIEW_PODCASTS:
                    podcastService.ViewAllPodcasts(currentUser);
                    break;
                case PodcastConsts.LISTEN:
                    podcastService.CreatePlayback(currentUser);
                    break;
                case FEEDBACK:
                    podcastService.CreateFeedback(currentUser);
                    break;
                case EXIT:
                    currentUser = null;
                    break;
                default:
                    break;
                
            }
        }

    }

    /// <summary>
    /// Once a premium user has logged in, this method proccesses all options for the 
    /// premium user to take while logged in.
    /// </summary>
    private void PremiumLogInMenu()
    {
        // Additional premium user specific methods.
        const int PREMIUM_LISTEN = 5, SUBSCRIBE = 6, UNSUBSCRIBE = 7, FEEDBACK = 8, EXIT = 9; 
        int userInput;

        while(currentUser is not null) // loop while logged in
        {
            userInput = view.PremiumListenerMenu();
            switch (userInput)
            {
                case PodcastConsts.VIEW_DETAILS:
                    view.DisplayString(currentUser.UserInfoString());
                    break;
                case PodcastConsts.CHANGE_PASSWORD:
                    userService.ValidatePassword(currentUser);
                    view.DisplayString("Password has been updated.");
                    break;
                case PodcastConsts.VIEW_PODCASTS:
                    podcastService.ViewAllPodcasts(currentUser);
                    break;
                case PodcastConsts.LISTEN:
                    podcastService.CreatePlayback(currentUser);
                    break;
                case PREMIUM_LISTEN:
                    podcastService.CreatePlayback(currentUser, true);
                    break;
                case SUBSCRIBE:
                    podcastService.SubscribeToPodcast((PremiumUser)currentUser);
                    break;
                case UNSUBSCRIBE:
                    podcastService.UnsubscribeToPodcast((PremiumUser)currentUser);
                    break;
                case FEEDBACK:
                    podcastService.CreateFeedback(currentUser);
                    break;
                case EXIT:
                    currentUser = null;
                    break;
                default:
                    break;
                
            }
        }
    }

    /// <summary>
    /// Proccesses all possible tasks the podcaster can undertake while logged in.
    /// </summary>

    private void PodcasterLogInMenu()
    {
        // create all choices, unique to podcasters
        const int CREATE_EPISODE = 3, VIEW_EPISODES = 4, VIEW_STATS = 5, FEEDBACK = 6, EXIT = 7;
        int userInput;
        while(currentUser is not null) // continue looping until user has logged out
        {
            userInput = view.PodcasterMenu();
            switch (userInput)
            {
                case PodcastConsts.VIEW_DETAILS:
                    view.DisplayString(currentUser.UserInfoString());
                    break;
                case PodcastConsts.CHANGE_PASSWORD:
                    userService.ValidatePassword(currentUser);
                    view.DisplayString("Password has been updated.");
                    break;
                case CREATE_EPISODE:
                    podcastService.CreateEpisode((PodcasterUser)currentUser);
                    break;
                case VIEW_EPISODES:
                    podcastService.ViewPodcastEpisodes((PodcasterUser)currentUser);
                    break;
                case VIEW_STATS:
                    podcastService.ViewEpisodeStatistics((PodcasterUser)currentUser);
                    break;
                case FEEDBACK:
                    podcastService.ViewFeedback((PodcasterUser)currentUser);
                    break;
                case EXIT:
                    currentUser = null;
                    break;
                default:
                    break;
                
            }
        }
    }
 
    /// <summary>
    /// This method handles the connection between the user service and controller of creating a new regular listener.
    /// </summary>
    private void createNewRegularUser()
    {
        // This section takes all user inputs through displaying prompts
        view.DisplayString("Registering as a regular listener.");
        List<Func<object>> userMethods = view.RegisterNewUser();

        // Test if the users inputs match the domain class requirements. If so, return a list of valid inputs
        List<string> validatedInputs = userService.ValidateNewUser(userMethods);

        // Store all validated inputs in their own seperate variables so they can be passed into the new instance of the class
        string finalName = validatedInputs[0];
        int finalAge = int.Parse(validatedInputs[1]); 
        string finalEmail = validatedInputs[3];
        string finalMobile = validatedInputs[2]; 
        string finalPassword = validatedInputs[4];

        // Create a new instance of the user with the stored validated inputs.
        User newUser = new RegularUser(finalName, finalAge, finalMobile, finalEmail, finalPassword);

        // Display the successful creation of the user
        view.DisplayString(newUser.UserWelcomeString());
        userRepository.AddUser(newUser); 
    }

    /// <summary>
    /// This method handles the connection between the user service and the controller class for creating a new premium user
    /// </summary>
    private void createNewPremiumUser()
    {
        // This section takes all user inputs through displaying prompts
        view.DisplayString("Registering as a premium listener.");
        List<Func<object>> userMethods = view.RegisterNewUser();
        List<Func<object>> premiumUserMethods = view.RegisterNewPremiumUser();

        // Test if the users inputs match the domain class requirements. If so, return a list of valid inputs
        // Additional inputs are tested as the premium class features two new inputs
        List<string> validatedInputs = userService.ValidateNewUser(userMethods);
        List<string> premiumValidatedInputs = userService.ValidatePremiumUser(premiumUserMethods);

        // Store all validated inputs in their own seperate variables so they can be passed into the new instance of the class
        string finalName = validatedInputs[0];
        int finalAge = int.Parse(validatedInputs[1]); // Safe to parse because the service already verified it's a number
        string finalEmail = validatedInputs[3];
        string finalMobile = validatedInputs[2]; // Safe to parse
        string finalPassword = validatedInputs[4];
        string finalDate = premiumValidatedInputs[0];
        int finalPayID = int.Parse(premiumValidatedInputs[1]);

        // Create a new instance of the user with the stored validated inputs.
        User newUser = new PremiumUser(finalName, finalAge, finalMobile, finalEmail, finalPassword, finalDate, finalPayID);

        // Display the successful creation of the user
        view.DisplayString(newUser.UserWelcomeString());
        userRepository.AddUser(newUser); 
    }

    /// <summary>
    /// This method handles the connection between the user service and the controller for creating a new podcaster
    /// </summary>
    private void createNewPodcasterUser()
    {
        // This section takes all user inputs through displaying prompts
        view.DisplayString("Registering as a podcaster.");
        List<Func<object>> userMethods = view.RegisterNewUser();
        List<Func<object>> podcasterUserMethods = view.RegisterNewPodcasterUser();

        // Validate the provided inputs alongside the new inputs of podcast name
        List<string> validatedInputs = userService.ValidateNewUser(userMethods);
        List<string> podcasterValidatedInputs = userService.ValidatePodcasterUser(podcasterUserMethods);

        // User details
        string finalName = validatedInputs[0];
        int finalAge = int.Parse(validatedInputs[1]); // Safe to parse because the service already verified it's a number
        string finalEmail = validatedInputs[3];
        string finalMobile = validatedInputs[2]; // Safe to parse
        string finalPassword = validatedInputs[4];

        // Podcast specific information
        PodcastConsts.Genre finalPodcastGenre = (PodcastConsts.Genre)int.Parse(podcasterValidatedInputs[0]);
        string finalPodcastName = podcasterValidatedInputs[1];
        string finalPodcastDescription = podcasterValidatedInputs[2];

        // Create a new instance of the user
        User newUser = new PodcasterUser(finalName, finalAge, finalMobile, finalEmail, finalPassword, finalPodcastGenre, finalPodcastName, finalPodcastDescription);

        // Display the successful creation of the user
        view.DisplayString(newUser.UserWelcomeString());
        userRepository.AddUser(newUser); 
    }


    


}

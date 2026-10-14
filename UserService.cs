using System.ComponentModel.Design;

namespace podcastWorld;


/// <summary>
/// Manages user acount operations, including registration for all users and authentication.
/// 
/// Responsibility: Handle methods involving user involvement
/// </summary>
public class UserService
{

    // Fields

    // Store the current view being used
    private MenuView view;

    // Store current user repository
    private UserRepository userRepository;

    /// <summary>
    /// Initialises a new instance of the user service class
    /// </summary>
    /// <param name="userRepo">The existing user repository</param>
    /// <param name="menuView">The view class responsible for UI</param>
    public UserService(UserRepository userRepo, MenuView menuView)
    {
        userRepository = userRepo;
        view = menuView;
        
    }

    /// <summary>
    /// This child class of User is used to validate inputs provided by the user
    /// as the new methods inside test each input with the parent classes domain rules.
    /// If an input is invalid, it will throw the same error the User abstract class does.
    /// </summary>
    private class ValidationUser : User
    {
        public ValidationUser() : base("John Doe", 22, "0412345678", "johndoe@email.com", "safePassword12w3") {}

        public void TestName(string input) => Name = input;
        public void TestAge(int input) => Age = input;
        public void TestEmail(string input) => Email = input;
        public void TestMobile(string input) => Mobile = input;
        public void TestPassword(string input) => Password = input;
        
        // Because User is an abstract class, these methods are forced. As they aren't being used, just returning an empty string will stop any errors appearing
        public override string UserWelcomeString()
        {
            return string.Empty;
        }

        public override string UserInfoString()
        {
            return string.Empty;
        }

        public override string UserWelcomeBackString()
        {
            return string.Empty;
        }

        public override string UserType()
        {
            return string.Empty;
        }
    }


    /// <summary>
    /// Iterates through input functions, and on each step, test the input by using the ValidationUser class tester methods.
    /// Each input is tested under domain rules and if invalid, an error is thrown. Otherwise, the input is saved to the list.
    /// </summary>
    /// <param name="userDetails">A provided list of functions to gain each input necessary</param>
    /// <returns>All validated inputs used to register a new method</returns>
    public List<string> ValidateNewUser(List<Func<object>> userDetails)
    {
        const int NAME = 0, AGE = 1, MOBILE = 2, EMAIL = 3, PASSWORD = 4;

        // Placeholder for valid inputs
        List<string> validInputs = new List<string>();
        int index = 0;
        foreach(object detail in userDetails)
        {
            bool isValidInput = false;

            
            while(!isValidInput)
            {
                try
                {
                    var inputMethod = (Func<object>)detail;
                    object methodResult = inputMethod();

                    ValidationUser testUser = new ValidationUser();

                    switch (index)
                    {
                        case NAME:
                            string nameInput = methodResult.ToString();
                            testUser.TestName(nameInput); // Tester method, done using domain rules under ValidationUser
                            validInputs.Add(nameInput);
                            isValidInput = true;
                            break;

                        case AGE: 
                            int ageInput = (int)methodResult;
                            testUser.TestAge(ageInput);
                            validInputs.Add(ageInput.ToString());
                            isValidInput = true;
                            break;

                        case MOBILE: 
                            string mobileInput = methodResult.ToString();
                            testUser.TestMobile(mobileInput);
                            validInputs.Add(mobileInput.ToString());
                            isValidInput = true;
                            break;

                        case EMAIL: 
                            string emailInput = methodResult.ToString();
                            if(userRepository.HasUser(emailInput))
                                throw new ArgumentException("# Error - Email already registered.");
                            testUser.TestEmail(emailInput);
                            validInputs.Add(emailInput);
                            isValidInput = true;
                            break;

                        case PASSWORD:
                            string passwordInput = methodResult.ToString();
                            testUser.TestPassword(passwordInput);
                            validInputs.Add(passwordInput);
                            isValidInput = true;
                            break;
                    }
                    
                } catch (ArgumentException e)
                {
                    view.DisplayError(e);
                }
            }
            index++;
            
        }
        return validInputs;
    }

    /// <summary>
    /// Iterates through provided details, catered to the registration of a premium user.
    /// </summary>
    /// <param name="userDetails">A list of view input methods, prompting the user for premium registration inputs.</param>
    /// <returns>A list of valid inputs for premium user registration</returns>
    public List<string> ValidatePremiumUser(List<Func<object>> userDetails)
    {
        const int DATE = 0, PAY_ID = 1;

        List<string> validInputs = new List<string>();
        int index = 0;
        foreach(object detail in userDetails)
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
                        case DATE:
                            string dateInput = methodResult.ToString();
                            // Instantiate a dummy PremiumUser to test setter validation rules. With only two inputs needing testing. A new child class would be redundant
                            PremiumUser testPremiumDate = new PremiumUser("Test", 20, "0412345678", "test@email.com", "Pass1234", dateInput, 107542);
                            validInputs.Add(dateInput);
                            isValidInput = true;
                            break;

                        case PAY_ID: 
                            if (!int.TryParse(methodResult.ToString(), out int payIdInput))
                                throw new ArgumentException("# Error - Supplied pay ID is invalid.");
                            // Instantiate a dummy PremiumUser to test PayID setter rules. With only two inputs needing testing. A new child class would be redundant
                            PremiumUser testPremiumPay = new PremiumUser("Test", 20, "0412345678", "test@email.com", "Pass1234", "01/01/2026", payIdInput);
                            validInputs.Add(payIdInput.ToString());
                            isValidInput = true;
                            break;
                        

                    }
                    
                } catch (ArgumentException e)
                {
                    view.DisplayError(e);
                }
            }
            index++;
            
        }
        return validInputs;
    }

    /// <summary>
    /// Iterates through provided details, catered to the registration of a podcaster user.
    /// </summary>
    /// <param name="userDetails">A list of view input methods, prompting the user for podcaster registration inputs.</param>
    /// <returns>A list of valid inputs for premium user registration</returns>
    public List<string> ValidatePodcasterUser(List<Func<object>> userDetails)
    {
        const int GENRE = 0, PODCAST_NAME = 1, DESCRIPTION = 2;

        List<string> validInputs = new List<string>();
        int index = 0;
        foreach(object detail in userDetails)
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
                        case GENRE: // Genre Choice
                            while (!isValidInput)
                            {
                                try
                                {
                                    string rawInput = methodResult.ToString();
                                    if (!int.TryParse(rawInput, out int genreChoice))
                                    {
                                        throw new ArgumentException("# Error - Supplied value must be an integral number.");
                                    }

                                    // Convert to genre enum and test against the domain setter rules
                                    PodcastConsts.Genre selectedGenre = (PodcastConsts.Genre)genreChoice;
                                    PodcasterUser testGenre = new PodcasterUser("Test", 20, "0412345678", "test@email.com", "Pass1234", selectedGenre, "Hello World", "A podcast about coding"); 

                                    validInputs.Add(genreChoice.ToString());
                                    isValidInput = true;
                                }
                                catch (ArgumentException e)
                                {
                                    view.DisplayError(e);
                                    methodResult = view.ReadGenreChoice(); // Re-prompt for the genre choice
                                }
                            }
                            break;

                        case PODCAST_NAME: // Podcast name validation
                            string nameInput = methodResult.ToString();

                            PodcasterUser testName = new PodcasterUser("Test", 20, "0412345678", "test@email.com", "Pass1234", PodcastConsts.Genre.News, nameInput, "A podcast about coding"); 

                            // 3. Store as string to keep List<string> consistent
                            validInputs.Add(nameInput);
                            isValidInput = true;
                            break;
                        case DESCRIPTION: // Description validation
                            string descriptionInput = methodResult.ToString();

                            PodcasterUser testDescription = new PodcasterUser("Test", 20, "0412345678", "test@email.com", "Pass1234", PodcastConsts.Genre.News, "Hello World", descriptionInput); 

                            // 3. Store as string to keep List<string> consistent
                            validInputs.Add(descriptionInput);
                            isValidInput = true;
                            break;
                        

                    }
                    
                } catch (ArgumentException e)
                {
                    view.DisplayError(e);
                    
                }
            }
            index++;
            
        }
        return validInputs;
    }



    /// <summary>
    /// Authenticates a user with a list of user inputs
    /// </summary>
    /// <param name="userDetails">A list of view methods, ready to be iterated over</param>
    /// <returns>A user, ready to be passed into the current user field</returns>
    public User? AuthenticateUser(List<Func<object>> userDetails)
    {
        string email = string.Empty;
        bool isValidInput = false;
        User? currentUser = null;

        // Validate that the user exists
        while(!isValidInput)
        {
            string userInput = userDetails[0]().ToString();
            if(userRepository.HasUser(userInput)) {
                email = userInput;
                currentUser = userRepository.GetUser(email);
                isValidInput = true;
            } else
            {
                view.DisplayError("# Error - Email is not registered.");
            }
        }
        
        // Check that the password matches
        isValidInput = false;
        while(!isValidInput)
        {
            try
            {
                string userInput = userDetails[1]().ToString();

                ValidationUser testUser = new ValidationUser();
                testUser.TestPassword(userInput);
                if(currentUser.ValidatePassword(userInput)) {
                    return currentUser;
                } else
                {
                    view.DisplayError("# Error - Entered password does not match existing password.");
                }   
            } catch (ArgumentException e)
            {
                view.DisplayError(e);
            }
            
        }
        return null;
    }

    /// <summary>
    /// Allows a user to change their current password for a new one.
    /// </summary>
    /// <param name="currentUser">The user modifiying their password</param>
    public void ValidatePassword(User? currentUser) 
    {
        bool isValidInput = false;

        // Verify current password is existing
        while(!isValidInput)
        {
            try
            {
                currentUser.ValidateChangePassword(view.ChangeCurrentPassword());
                isValidInput = true;
            } catch (ArgumentException e)
            {
                view.DisplayError(e);
            }
        }

        isValidInput = false;

        // Collect new password, and ensure that it is valid through domain rules
        while(!isValidInput)
        {
            try
            {
                string userInput = view.ChangeNewPassword();
                currentUser.NewPassword(userInput);
                currentUser.UpdatePassword(userInput);
                isValidInput = true;
            } catch (ArgumentException e)
            {
                view.DisplayError(e);
            }
        }

    }
}

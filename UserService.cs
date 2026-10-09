using System.ComponentModel.Design;

namespace podcastWorld;

public class UserService
{
    private MenuView view;
    private UserRepository userRepository;


    public UserService(UserRepository userRepo, MenuView menuView)
    {
        view = menuView;
        userRepository = userRepo;

    }

    private class ValidationUser : User
    {
        public ValidationUser() : base("John Doe", 22, "0412345678", "johndoe@email.com", "safePassword12w3") {}

        public void TestName(string input) => Name = input;
        public void TestAge(int input) => Age = input;
        public void TestEmail(string input) => Email = input;
        public void TestMobile(string input) => Mobile = input;
        public void TestPassword(string input) => Password = input;
        
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

    /*
        USER VALIDATION USER VALIDATION USER VALIDATION USER VALIDATION USER VALIDATION USER VALIDATION
        USER VALIDATION USER VALIDATION USER VALIDATION USER VALIDATION USER VALIDATION USER VALIDATION
        USER VALIDATION USER VALIDATION USER VALIDATION USER VALIDATION USER VALIDATION USER VALIDATION
        USER VALIDATION USER VALIDATION USER VALIDATION USER VALIDATION USER VALIDATION USER VALIDATION
        USER VALIDATION USER VALIDATION USER VALIDATION USER VALIDATION USER VALIDATION USER VALIDATION
    */


    public List<string> ValidateNewUser(List<Func<object>> userDetails)
    {
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
                        case 0:
                            string nameInput = methodResult.ToString();
                            testUser.TestName(nameInput);
                            validInputs.Add(nameInput);
                            isValidInput = true;
                            break;

                        case 1: 
                            int ageInput = (int)methodResult;
                            if (ageInput == -99999) // CODE SMELL CODE SMELL CODE SMELL CODE SMELL CODE SMELL CODE SMELL CODE SMELL 
                                throw new ArgumentException("# Error - Supplied value must be an integral number.");
                            testUser.TestAge(ageInput);
                            validInputs.Add(ageInput.ToString());
                            isValidInput = true;
                            break;

                        case 2: 
                            string mobileInput = methodResult.ToString();
                            testUser.TestMobile(mobileInput);
                            validInputs.Add(mobileInput.ToString());
                            isValidInput = true;
                            break;

                        case 3: 
                            string emailInput = methodResult.ToString();
                            if(userRepository.HasUser(emailInput))
                                throw new ArgumentException("# Error - Email already registered.");
                            testUser.TestEmail(emailInput);
                            validInputs.Add(emailInput);
                            isValidInput = true;
                            break;

                        case 4:
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

    public List<string> ValidatePremiumUser(List<Func<object>> userDetails)
    {
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
                        case 0:
                            string dateInput = methodResult.ToString();
                            // Instantiate a dummy PremiumUser to test setter validation rules
                            PremiumUser testPremiumDate = new PremiumUser("Test", 20, "0412345678", "test@email.com", "Pass1234", dateInput, 107542);
                            validInputs.Add(dateInput);
                            isValidInput = true;
                            break;

                        case 1: 
                            if (!int.TryParse(methodResult.ToString(), out int payIdInput))
                                throw new ArgumentException("# Error - Supplied pay ID is invalid.");
                            // Instantiate a dummy PremiumUser to test PayID setter rules
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

    public List<string> ValidatePodcasterUser(List<Func<object>> userDetails)
    {
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
                        case 0: // Genre Choice
                            while (!isValidInput)
                            {
                                try
                                {
                                    string rawInput = methodResult.ToString();

                                    // 1. Check integer format
                                    if (!int.TryParse(rawInput, out int genreChoice))
                                    {
                                        throw new ArgumentException("# Error - Supplied value must be an integral number.");
                                    }

                                    // 2. Test enum domain setter (1 to 6)
                                    PodcastConsts.Genre selectedGenre = (PodcastConsts.Genre)genreChoice;
                                    PodcasterUser testGenre = new PodcasterUser("Test", 20, "0412345678", "test@email.com", "Pass1234", selectedGenre, "Hello World", "A podcast about coding"); 

                                    validInputs.Add(genreChoice.ToString());
                                    isValidInput = true;
                                }
                                catch (ArgumentException e)
                                {
                                    view.DisplayError(e);
                                    methodResult = view.ReadGenreChoice();
                                }
                            }
                            break;

                        case 1: 
                            string nameInput = methodResult.ToString();

                            PodcasterUser testName = new PodcasterUser("Test", 20, "0412345678", "test@email.com", "Pass1234", PodcastConsts.Genre.News, nameInput, "A podcast about coding"); 

                            // 3. Store as string to keep List<string> consistent
                            validInputs.Add(nameInput);
                            isValidInput = true;
                            break;
                        case 2:
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



    public User? AuthenticateUser(List<Func<object>> userDetails)
    {
        string email = string.Empty;
        bool isValidInput = false;
        User? currentUser = null;
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
    public void ValidatePassword(User? currentUser) 
    {
        bool isValidInput = false;
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

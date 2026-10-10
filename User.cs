using System.Security.Cryptography;
using System.Text.RegularExpressions;
namespace podcastWorld;

/// <summary>
/// An abstract class, serving as a base for any listener/podcaster. Their details are checked and stored through here.
/// 
/// Responsibility: To represent a user on the PodcastWorld App
/// </summary>
public abstract class User
{
    // A set of private fields, used to store the information provided through
    // the public get setters. The information inside these fields are correct provided
    // the property setters do their job.

    
    // The name of the user
    private string _name = string.Empty;
    
    // The age of the user
    private int _age;
    
    // The email of the user
    private string _email = string.Empty;
    
    // The mobile number of the user.
    private string _mobile;
    
    // The password of the user.
    private string _password = string.Empty;

    
    // A list, storing all playback sessions of the user. Whether complete or incomplete
    private readonly List<Playback> _playbacks = new List<Playback>();



    // Properties, used to check inputs from the constructor and set fields with correct inputs,
    // otherwise rejecting invalid inputs. Protected set is used so that the children classes of
    // RegularListener, PremiumListener and PodcasterUser can set properties aswell but other
    // external classes can't


    /// <summary>
    /// User Name Property
    /// </summary>
    public string Name 
    {
        get => _name;
        protected set
        {
            if(string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("# Error - Supplied name is invalid.");
            }

            if (!Regex.IsMatch(value, @"^[a-zA-Z '.-]+$"))
            {
                throw new ArgumentException("# Error - Supplied name is invalid.");
            }

            if (!value.Any(char.IsLetter))
            {
                throw new ArgumentException("Name must contain at least one letter.");
            }
                
            _name = value;
        }
    }

    /// <summary>
    /// User Age Property
    /// </summary>
    public int Age 
    {
        get => _age;
        protected set
        {
            if(value < PodcastConsts.MIN_AGE || value > PodcastConsts.MAX_AGE)
            {
                throw new ArgumentException("# Error - Supplied age is invalid.");
            }
            _age = value;
        }
    }
    /// <summary>
    /// User Email Property
    /// </summary>
    public string Email 
    {
        get => _email;
        protected set
        {
            if (!Regex.IsMatch(value, @"^[^@]+@[^@]+$"))
            {
                throw new ArgumentException("# Error - Supplied email is invalid.");
            }

            _email = value;
        }    
    }
    /// <summary>
    /// User Mobil Property
    /// </summary>
    public string Mobile 
    {
        get => _mobile;
        protected set
        {

            if (string.IsNullOrWhiteSpace(value) || !Regex.IsMatch(value, @"^0\d{9}$"))
            {
                // Fixed the typo message here
                throw new ArgumentException("# Error - Supplied mobile number is invalid."); 
            }
            _mobile = value;
        }    
    }
    /// <summary>
    /// User Password Property
    /// </summary>
    public string Password 
    {
        get => _password;
        protected set
        {
            if(value.Length < PodcastConsts.MIN_PASSWORD_LENGTH)
            {
                throw new ArgumentException("# Error - Supplied password is invalid.");
            }

            if (!Regex.IsMatch(value, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)"))
            {
                throw new ArgumentException("# Error - Supplied password is invalid.");
            }
            _password = value;
        }   
    }


    // Dynamic public properties
    public IReadOnlyList<Playback> Playbacks => _playbacks.AsReadOnly();

    /// <summary>
    /// Dynamic Stat of how many episodes the user has started
    /// </summary>
    public int EpisodesStarted => _playbacks.Count;
    
    /// <summary>
    /// Dynamic stat of how many episodes were completed
    /// </summary>
    public int EpisodesCompleted => 
        _playbacks.Count(p => p.MinutesListened >= p.PlaybackEpisode.EpisodeLength);

    /// <summary>
    /// Dynamic stat of how minutes listened in total
    /// </summary>
    public int TotalMinutesListened => 
        _playbacks.Sum(p => p.MinutesListened);

    /// <summary>
    /// A constructor for the user
    /// </summary>
    /// <param name="name">The name of user</param>
    /// <param name="age">The age of user</param>
    /// <param name="mobile">The mobile number of user</param>
    /// <param name="email">The email of user</param>
    /// <param name="password">The password of user</param>
    public User(string name, int age, string mobile, string email, string password) {
        Name = name;
        Age = age;
        Email = email;
        Mobile = mobile;
        Password = password;
    }


    // Abstract strings, which force each class to provide their own
    // tailored output, specific to their needs. EG. a premium user
    // will need to not only display basics, but PAYID etc.

    /// <summary>
    /// Registration Welcome String
    /// </summary>
    /// <returns>Will return a tailored welcome string, depending on the class</returns>
    public abstract string UserWelcomeString();
    /// <summary>
    /// Login Welcome String
    /// </summary>
    /// <returns>Will return a tailored welcome back string, depending on the class</returns>
    public abstract string UserWelcomeBackString();
    /// <summary>
    /// User information string
    /// </summary>
    /// <returns>Will return all of the user-provided details in a formatted detail string.</returns>
    public abstract string UserInfoString();
    /// <summary>
    /// Type of user string
    /// </summary>
    /// <returns>A string describing the user type</returns>
    public abstract string UserType();

    /// <summary>
    /// Retrieves the playback session through provided episode search input
    /// </summary>
    /// <param name="episode">Episode it is searching for</param>
    /// <returns>A playback session involving the episode involved</returns>
    public Playback? GetPlayback(Episode episode)
    {
        return _playbacks.FirstOrDefault(p => p.PlaybackEpisode.EpisodeName == episode.EpisodeName);
    }


    /// <summary>
    /// Add a playback session to the stored list
    /// </summary>
    /// <param name="playback">Provided Playback Session</param>
    public void AddPlayback(Playback playback)
    {
        _playbacks.Add(playback);
    }

    public List<Episode> GetCompletedEpisodes()
    {
        return _playbacks.Where(p => p.MinutesListened >= p.PlaybackEpisode.EpisodeLength).Select(p => p.PlaybackEpisode).ToList();
    }

    /// <summary>
    /// Check if input password matches the stored password
    /// </summary>
    /// <param name="inputPassword">User-Input Password</param>
    /// <returns>A boolean whether or not condition is met</returns>
    public bool ValidatePassword(string inputPassword)
    {
        return inputPassword == Password;
    }

    /// <summary>
    /// To start changing the password, the user must confirm they are the authorised user
    /// </summary>
    /// <param name="inputPassword">Input Password</param>
    /// <exception cref="ArgumentException">If invalid the exception is thrown, ready to be caught and displayed</exception>
    public void ValidateChangePassword(string inputPassword)
    {
        if(inputPassword != Password)
        {
            throw new ArgumentException("# Error - Entered password does not match existing password.");
        }
    }

    /// <summary>
    /// A finalising method called to ensure that the new password provided isn't the same
    /// </summary>
    /// <param name="inputPassword">Input Password</param>
    /// <exception cref="ArgumentException">If same, will throw an exception, telling the user their password is the same.</exception>
    public void NewPassword(string inputPassword)
    {
        if(inputPassword == Password)
        {
            throw new ArgumentException("# Error - New password is the same as the existing password.");
        }
    }

    /// <summary>
    /// Once finalised, the password will be updated
    /// </summary>
    /// <param name="inputPassword">Password to replace old one</param>
    public void UpdatePassword(string inputPassword)
    {
        Password = inputPassword;
    }

}

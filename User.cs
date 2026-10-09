using System.Security.Cryptography;
using System.Text.RegularExpressions;
namespace podcastWorld;

/// <summary>
/// An abstract class, serving as a base for any listener/podcaster. Their details are created and stored through here.
/// </summary>
public abstract class User
{

    // Fields
    private string _name = string.Empty;
    private int _age;
    private string _email = string.Empty;
    private string _mobile;
    private string _password = string.Empty;


    private readonly List<Playback> _playbacks = new List<Playback>();

    // Properties

    public IReadOnlyList<Playback> Playbacks => _playbacks.AsReadOnly();

    public int EpisodesStarted => _playbacks.Count;

    public int EpisodesCompleted => 
        _playbacks.Count(p => p.MinutesListened >= p.PlaybackEpisode.EpisodeLength);

    public int TotalMinutesListened => 
        _playbacks.Sum(p => p.MinutesListened);
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


    public User(string name, int age, string mobile, string email, string password) {
        Name = name;
        Age = age;
        Email = email;
        Mobile = mobile;
        Password = password;
    }

    public abstract string UserWelcomeString();
    public abstract string UserWelcomeBackString();
    public abstract string UserInfoString();
    public abstract string UserType();

    public Playback? GetPlayback(Episode episode)
    {
        return _playbacks.FirstOrDefault(p => p.PlaybackEpisode.EpisodeName == episode.EpisodeName);
    }

    public void AddPlayback(Playback playback)
    {
        _playbacks.Add(playback);
    }

    public List<Episode> GetCompletedEpisodes()
    {
        return _playbacks.Where(p => p.MinutesListened >= p.PlaybackEpisode.EpisodeLength).Select(p => p.PlaybackEpisode).ToList();
    }
    public bool ValidatePassword(string inputPassword)
    {
        return inputPassword == Password;
    }

    public void ValidateChangePassword(string inputPassword)
    {
        if(inputPassword != Password)
        {
            throw new ArgumentException("# Error - Entered password does not match existing password.");
        }
    }

    public void NewPassword(string inputPassword)
    {
        if(inputPassword == Password)
        {
            throw new ArgumentException("# Error - New password is the same as the existing password.");
        }
    }

    public void UpdatePassword(string inputPassword)
    {
        Password = inputPassword;
    }

}

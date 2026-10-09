namespace podcastWorld;
using System.Text.RegularExpressions;
using System.Globalization;

public class Episode
{
    private string _episodeName;
    private string _episodeDescription;
    private string _episodeDate;
    private int _episodeLength;
    private bool _episodePremiumOnly;

    public string EpisodeName
    {
        get => _episodeName;
        protected set
        {
            if (string.IsNullOrEmpty(value) || !Regex.IsMatch(value, @"^(?=.*[a-zA-Z])[a-zA-Z0-9 ':-]+$"))
            {
                throw new ArgumentException("# Error - Supplied episode's name is invalid.");
            }

            _episodeName = value;
        }
    }

    public string EpisodeDescription
    {
        get => _episodeDescription;
        protected set
        {
            if (string.IsNullOrEmpty(value) || !Regex.IsMatch(value, @"^(?=.*[a-zA-Z])[a-zA-Z0-9\s!""#$%&'()*+,\-./:;<=>?@\[\\\]^_`{|}~]+$"))
            {
                throw new ArgumentException("# Error - Supplied podcast description is invalid.");
            }

            _episodeDescription = value;
        }
    }

    public string EpisodeDate
    {
        get => _episodeDate;
        protected set
        {
            if (string.IsNullOrWhiteSpace(value) || 
                !DateTime.TryParseExact(value, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
            {
                throw new ArgumentException("# Error - Supplied date is invalid.");
            }

            _episodeDate = value;
        }
    }

    public int EpisodeLength
    {
        get => _episodeLength;
        protected set
        {
            if (value < 1 || value > 300)
            {
                throw new ArgumentException("# Error - Supplied value is out of range.");
            }

            _episodeLength = value;
        }
    }

    public bool EpisodePremiumOnly
    {
        get => _episodePremiumOnly;
        protected set
        {
            _episodePremiumOnly = value;
        }
    }

    public Episode(string name, string description, string date, int length, bool premiumOnly)
    {
        EpisodeName = name;
        EpisodeDescription = description;
        EpisodeDate = date;
        EpisodeLength = length;
        EpisodePremiumOnly = premiumOnly;
    }

}

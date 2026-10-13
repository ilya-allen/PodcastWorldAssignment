namespace podcastWorld;
using System.Globalization;


/// <summary>
/// A class, representing a playback session for when a user wishes to listen to an episode. This playback session allows
/// for a user to listen to an episode over multiple sessions, while saving their progress inside this class.
/// 
/// Responsibility: Provide a playback session of a specific episode for a user to return to over time.
/// </summary>
public class Playback
{

    // Fields

    // Provided episode for playback
    private Episode _playbackEpisode = null;

    // How many minutes the user listened to in that specific session
    private int _minutesListened;

    // Last time the user listened to the episode
    private string _lastListenedDate = string.Empty;

    // Properties

    /// <summary>
    /// Gets the episode the user is listening to.
    /// </summary>
    public Episode PlaybackEpisode
    {
        get => _playbackEpisode;
        private set
        {
            if (value == null)
            {
                throw new ArgumentException("# Error - Episode cannot be null.");
            }
            _playbackEpisode = value;
        }
    }

    /// <summary>
    /// Gets the users input of when they last listened to the episode. This utilizes system.globalization for real date values
    /// </summary>
    public string LastListenedDate
    {
        get => _lastListenedDate;
        private set
        {
            if (string.IsNullOrWhiteSpace(value) || 
                !DateTime.TryParseExact(value, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedListenDate))
            {
                throw new ArgumentException("# Error - Supplied date is invalid.");
            }

            if (PlaybackEpisode != null && !string.IsNullOrWhiteSpace(PlaybackEpisode.EpisodeDate))
            {
                if (DateTime.TryParseExact(PlaybackEpisode.EpisodeDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime recordedDate))
                {
                    if (parsedListenDate < recordedDate)
                    {
                        throw new ArgumentException($"# Error - Supplied date must be after {recordedDate:d/M/yyyy}.");
                    }
                }
            }

            _lastListenedDate = value;
        }
    }

    /// <summary>
    /// Gets the amount of minutes that the user listened to the episode for.
    /// </summary>
    public int MinutesListened
    {
        get => _minutesListened;
        private set
        {
            if (value <= 0 || (PlaybackEpisode != null && value > PlaybackEpisode.EpisodeLength))
            {
                throw new ArgumentException("# Error - Supplied value is out of range.");
            }
            _minutesListened = value;
        }
    }

    /// <summary>
    /// Updates an already existing uncomplete playback session with a new amount of minutes listened to aswell as the last listened date.
    /// </summary>
    /// <param name="newDate">New valid date</param>
    /// <param name="additionalMinutes">Minutes</param>
    public void UpdatePlayback(string newDate, int additionalMinutes)
    {
        LastListenedDate = newDate;          
        MinutesListened += additionalMinutes; 
    }


    /// <summary>
    /// Playback constructor
    /// </summary>
    /// <param name="playbackEpisode">Provided valid episode</param>
    /// <param name="lastListenedDate">Initial last listened to date</param>
    /// <param name="minutesListened">Initial amount of minutes listened to.</param>
    public Playback(Episode playbackEpisode, string lastListenedDate, int minutesListened)
    {
        PlaybackEpisode = playbackEpisode;
        LastListenedDate = lastListenedDate;
        MinutesListened = minutesListened;
    }
}
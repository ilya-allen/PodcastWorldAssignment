namespace podcastWorld;
using System.Globalization;

public class Playback
{
    private Episode _playbackEpisode = null;
    private int _minutesListened;
    private string _lastListenedDate = string.Empty;

    public Playback(Episode playbackEpisode, string lastListenedDate, int minutesListened)
    {
        PlaybackEpisode = playbackEpisode;
        LastListenedDate = lastListenedDate;
        MinutesListened = minutesListened;
    }

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

    public void UpdatePlayback(string newDate, int additionalMinutes)
    {
        LastListenedDate = newDate;          
        MinutesListened += additionalMinutes; 
    }
}
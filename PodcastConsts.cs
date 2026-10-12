namespace podcastWorld;

public static class PodcastConsts
{
    public const int MAX_AGE = 99;
    public const int MIN_AGE = 18;
    public const int MOBILE_LENGTH = 10;
    public const int MIN_PASSWORD_LENGTH = 8;
    public const int MIN_PAYID_VALUE = 100000;
    public const int MAX_PAYID_VALUE = 999999;

    // Login Consts
    public const int VIEW_DETAILS = 1;
    public const int CHANGE_PASSWORD = 2;
    public const int VIEW_PODCASTS = 3;
    public const int LISTEN = 4;
    

    public enum Genre
    {
        News = 1,
        Politics = 2,
        Sports = 3,
        Entertainment = 4,
        Education = 5,
        Hobbies = 6
    }


}

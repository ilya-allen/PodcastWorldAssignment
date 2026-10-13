using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Linq;

namespace podcastWorld;

/// <summary>
/// A repository of the Users. It stores relevant data and behaviours  
/// related to the collection of employees.
/// 
/// Responsibility: Store and Grab users.
/// </summary>
public class UserRepository
{
    /// <summary>
    /// A collection used to store all users within the podcastworld app
    /// </summary>
    private Dictionary<string, User> users;

    /// <summary>
    /// Constructor for UserRepository class
    /// </summary>
    public UserRepository()
    {
        users = new Dictionary<string, User>();
    }

    /// <summary>
    /// Retrieves all podcasters
    /// </summary>
    /// <returns>An alphabetically ordered list, featuring all podcasts</returns>
    public List<PodcasterUser> GetPodcasts()
    {
        List<PodcasterUser> unOrderedList =  users.Values.OfType<PodcasterUser>().ToList();
        return unOrderedList.OrderBy(user => user.PodcastName).ToList();
    }

    /// <summary>
    /// Retrieves all listeners (non-podcasters)
    /// </summary>
    /// <returns>An alphaberically ordered list, featuring all listeners</returns>
    public List<User> GetListeners()
    {
        List<User> unOrderedList = users.Values.Where(u => u is not PodcasterUser).ToList();
        return unOrderedList.OrderBy(user => user.Name).ToList();
    }


    /// <summary>
    /// Add a new user to the list
    /// </summary>
    /// <param name="user">Valid user</param>
    public void AddUser(User user)
    {
        users.Add(user.Email, user);
    }

    /// <summary>
    /// Search the user repository for a provided user
    /// </summary>
    /// <param name="email">Email of the user that they wish to find</param>
    /// <returns>Boolean value, if the user database contains user</returns>
    public bool HasUser(string email)
    {
        return users.ContainsKey(email);
    }

    /// <summary>
    /// Checks if ANY podcasters exist at all.
    /// </summary>
    /// <returns>Boolean value, depending if a podcaster exists or not</returns>
    public bool HasPodcast()
    {
        return users.Values.OfType<PodcasterUser>().Any();
    }

    /// <summary>
    /// Provides the amount of registered users
    /// </summary>
    /// <returns></returns>
    public int UserCount()
    {
        return users.Count();
    }

    /// <summary>
    /// Provides a user once the email is provided, usually done after ensuring that the user exists.
    /// </summary>
    /// <param name="email">A valid email of a registered user</param>
    /// <returns>A valid user</returns>
    public User GetUser(string email)
    {
        return users[email];
    }


    

}
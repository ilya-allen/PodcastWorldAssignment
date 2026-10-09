using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Linq;

namespace podcastWorld;

/// <summary>
/// A repository of the Users. It stores relevant data and behaviours  
/// related to the collection of employees.
/// </summary>
public class UserRepository
{
    private Dictionary<string, User> users;
    public UserRepository()
    {
        users = new Dictionary<string, User>();
    }


    public List<PodcasterUser> GetPodcasts()
    {
        List<PodcasterUser> unOrderedList =  users.Values.OfType<PodcasterUser>().ToList();
        return unOrderedList.OrderBy(user => user.PodcastName).ToList();
    }

    public List<User> GetListeners()
    {
        List<User> unOrderedList = users.Values.Where(u => u is not PodcasterUser).ToList();
        return unOrderedList.OrderBy(user => user.Name).ToList();
    }
    // Adds new users
    public void AddUser(User user)
    {
        users.Add(user.Email, user);
    }

    public bool HasUser(string email)
    {
        return users.ContainsKey(email);
    }

    public bool HasPodcast()
    {
        return users.Values.OfType<PodcasterUser>().Any();
    }

    public int UserCount()
    {
        return users.Count();
    }

    public User GetUser(string email)
    {
        return users[email];
    }


    

}
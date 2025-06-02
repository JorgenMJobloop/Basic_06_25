using System.Text;
using System.Text.Json;

public class User : IUser
{
    public string GetUsername(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            Console.WriteLine("No username provided!");
        }
        return username;
    }

    public string UpdateUsername(string username)
    {
        string? updateUsername = Console.ReadLine();
        username = updateUsername!;
        return username;
    }
}
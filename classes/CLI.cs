using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

public class CLI : ICLI
{
    private User? users;

    public void AppendFileContent(string filePath, string content)
    {
        if (!File.Exists(filePath))
        {
            File.Create(filePath);
        }
        File.AppendAllText(filePath, content);
    }

    public string GreetUser(string username)
    {
        return $"Hello {username}";
    }

    public void ReadFileContent(string filePath)
    {
        string content = File.ReadAllText(filePath);
        Console.WriteLine(content);
    }


    private string GetPassword()
    {
        return "asdjaskfjskgjsdkf";
    }

    private void AddToJSONFile()
    {
        Dictionary<string, string[]> userTable = new Dictionary<string, string[]>
    {
        { "username", new[] { "user1", "user2" } },
        { "password", new[] { "pass1", "pass2" } }
    };

        string jsonContent = JsonSerializer.Serialize(userTable, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText("test_file.json", jsonContent);
        Console.WriteLine("JSON file written successfully:");
        Console.WriteLine(jsonContent);
    }

    private void ReadFromJSONFile(string filePath)
    {
        try
        {
            string jsonContent = File.ReadAllText(filePath);
            Dictionary<string, string[]> content = JsonSerializer.Deserialize<Dictionary<string, string[]>>(jsonContent)!;

            foreach (var kvp in content)
            {
                Console.WriteLine($"{kvp.Key}: [{string.Join(", ", kvp.Value)}]");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading JSON: {ex.Message}");
        }
    }

    private void DebugMode()
    {
        Console.WriteLine("Running debug mode");
        Console.WriteLine($"Testing JSON methods...");
        ReadFromJSONFile("test_file.json");
    }

    private string GetSecrets(string filePath, bool shouldGetEnv)
    {
        string? env = File.ReadAllText(filePath);
        string? optional = Environment.GetEnvironmentVariable(filePath);
        if (!shouldGetEnv) return optional!.ToString();
        return env;
    }

    private bool ValidateLogin(string filePath, string username, string password)
    {
        try
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine("The user data file was not found!");
                return false;
            }

            string jsonContent = File.ReadAllText(filePath);
            var userData = JsonSerializer.Deserialize<Dictionary<string, string[]>>(jsonContent);

            if (userData == null || !userData.ContainsKey("username") || !userData.ContainsKey("password"))
            {
                Console.WriteLine("Invalid file structure!");
                return false;
            }

            var usernames = userData["username"];
            var passwords = userData["password"];

            for (int i = 0; i < usernames.Length; i++)
            {
                if (i < passwords.Length && usernames[i] == username && passwords[i] == password)
                {
                    return true;
                }
            }
            return false;
        }
        catch (Exception exception)
        {
            Console.WriteLine($"There was an error when attempting to log in: {exception.Message}");
            return false;    
        }
    }


    private void UpdateUserFile(string filePath, string username, string password)
    {
        Dictionary<string, List<string>> userData;

        if (File.Exists(filePath))
        {
            string jsonContent = File.ReadAllText(filePath);

            try
            {
                var existingData = JsonSerializer.Deserialize<Dictionary<string, string[]>>(jsonContent);
                userData = existingData!.ToDictionary(
                    keyValuePair => keyValuePair.Key,
                    keyValuePair => keyValuePair.Value.ToList()
                ) ?? new Dictionary<string, List<string>>()
                {
                    {"username", new List<string>()},
                    {"passowrd", new List<string>()}
                };
            }
            catch
            {
                Console.WriteLine("Recieved invalid JSON file format! Reinitalizing.");
                userData = new Dictionary<string, List<string>>()
                {
                    {"username", new List<string>()},
                    {"password", new List<string>()}
                };
            }
        }
        else
        {
            userData = new Dictionary<string, List<string>>()
            {
                {"username", new List<string>()},
                {"password", new List<string>()}
            };
        }

        userData["username"].Add(username);
        userData["password"].Add(password);

        var updateData = userData.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.ToArray());
        string updateJSONFileData = JsonSerializer.Serialize(updateData, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(filePath, updateJSONFileData);

        Console.WriteLine("A new user was successfully added!");
    }

    public void RunCLI()
    {
        Console.WriteLine("Welcome to the User CLI\n");
        Console.WriteLine("Usage: login <username> <password>\nadduser <username> <password>\nlogout\nread <file>\nappend <file> <content>\ngreet <username>\ndebug [debug mode]");

        string username = "test";

        string? arguments = Console.ReadLine();

        switch (arguments!.ToLower())
        {
            case "login":
                Console.WriteLine("Enter username: ");
                string user = Console.ReadLine()!;
                Console.WriteLine("Enter password: ");
                string passwd = Console.ReadLine()!;
                // Check the validity of the incomming user data
                bool success = ValidateLogin("test_file.json", user, passwd);
                if (success)
                {
                    Console.WriteLine($"Login Successfull!\nWelcome {user}");
                }
                else
                {
                    Console.WriteLine("Invalid username or password!");    
                }
                break;
            case "adduser":
                Console.WriteLine("Enter a new username: ");
                string _username = Console.ReadLine()!;
                Console.WriteLine("Enter a new password: ");
                string _password = Console.ReadLine()!;
                UpdateUserFile("test_file.json", _username, _password);
                break;
            case "read":
                string? file = Console.ReadLine();
                ReadFileContent(file!);
                break;
            case "append":
                string filePath = Console.ReadLine()!;
                Console.WriteLine("Write changes:\n");
                string content = Console.ReadLine()!;
                AppendFileContent(filePath, content);
                break;
            case "greet":
                Console.WriteLine(GreetUser(username));
                break;
            case "debug":
                DebugMode();
                break;
            default:
                return;
        }
    }
}
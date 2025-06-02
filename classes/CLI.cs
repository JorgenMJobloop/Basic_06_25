using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

public class CLI : ICLI
{
    private User? users;

    public void AppendFileContent(string filePath, string content)
    {
        throw new NotImplementedException();
    }

    public string GreetUser(string username)
    {
        return $"Hello {username}";
    }

    public void ReadFileContent(string filePath)
    {
        throw new NotImplementedException();
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
        AddToJSONFile();
        ReadFromJSONFile("test_file.json");
    }

    private string GetSecrets(string filePath, bool shouldGetEnv)
    {
        string? env = File.ReadAllText(filePath);
        string? optional = Environment.GetEnvironmentVariable(filePath);
        if (!shouldGetEnv) return optional!.ToString();
        return env;
    }

    public void RunCLI()
    {
        Console.WriteLine("Welcome to the User CLI\n");
        Console.WriteLine("Usage: login username, password\nlogout\nread <file>\nappend <file>\ngreet <username>\ndebug [debug mode]");

        string username = "test";

        string? arguments = Console.ReadLine();

        switch (arguments!.ToLower())
        {
            case "login":
                Console.WriteLine("");
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
                GreetUser(username);
                break;
            case "debug":
                DebugMode();
                Console.WriteLine(GetSecrets(".env", false));
                break;
            default:
                return;
        }
    }
}
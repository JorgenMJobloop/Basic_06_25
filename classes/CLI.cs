using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using Spectre.Console;

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
        var hour = DateTime.Now.Hour;
        string greeting;

        if (hour >= 5 && hour < 12)
        {
            greeting = "Good morning";
        }
        else if (hour >= 12 && hour < 18)
        {
            greeting = "Good afternoon";
        }
        else if (hour >= 18 && hour < 23)
        {
            greeting = "Good evening";
        }
        else
        {
            greeting = "Good night, exiting program...";
            Environment.Exit(0);
        }
        return $"{greeting}, {username}!";
    }

    public void ReadFileContent(string filePath)
    {
        var table = new Table();
        table.AddColumn("Text");
        table.AddColumn(new TableColumn("Content").Centered());
        string content = File.ReadAllText(filePath);

        table.AddRow("text content", $"[white]{content}[/]");

        AnsiConsole.Write(table);
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
                AnsiConsole.MarkupLine("[red]The user data file was not found![/]");
                return false;
            }

            string jsonContent = File.ReadAllText(filePath);
            var userData = JsonSerializer.Deserialize<Dictionary<string, string[]>>(jsonContent);

            if (userData == null || !userData.ContainsKey("username") || !userData.ContainsKey("password"))
            {
                AnsiConsole.MarkupLine("[red]Invalid file structure![/]");
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
            AnsiConsole.MarkupLine($"[red]There was an error when attempting to log in: {exception.Message}[/]");
            return false;    
        }
    }

    private void DisplayUserData(Dictionary<string, string[]> userData)
    {
        var table = new Table();
        table.AddColumn("Username");
        table.AddColumn("Password");

        var usernames = userData["username"];
        var passwords = userData["password"];

        for (int i = 0; i < usernames.Length; i++)
        {
            string pw = i < passwords.Length ? passwords[i] : "<missing>";
            table.AddRow(usernames[i], pw);
        }

        AnsiConsole.Write(table);
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
                AnsiConsole.MarkupLine("[red]Recieved invalid JSON file format! Reinitalizing.[/]");
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

    private void ShowNewWelcomeMessage()
    {
            AnsiConsole.Write(new Panel("Welcome to the [green]User CLI[/]")
                .Header("")
                .Border(BoxBorder.Rounded)
                .Padding(1, 1)
            );
    }

    public void RunCLI()
    {

        while (true)
        {
            ShowNewWelcomeMessage();

            var command = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("Select [green] command[/]:")
                .AddChoices("login", "adduser", "read", "append", "debug", "exit")
            );

            switch (command)
            {
                case "login":
                    var loginUser = AnsiConsole.Ask<string>("[green]Enter username:[/]");
                    var loginPassword = AnsiConsole.Prompt(new TextPrompt<string>("[green] enter password:[/]")
                        .PromptStyle("red")
                        .Secret());
                    if (ValidateLogin("test_file.json", loginUser, loginPassword))
                    {
                        AnsiConsole.MarkupLine($"[bold green]Login successful. Welcome, [underline]{loginUser}[/]![/]");
                    }
                    else
                    {
                        AnsiConsole.MarkupLine("[bold red]Invalid username or passowrd.[/]");
                    }
                    break;
                case "adduser":
                    AnsiConsole.MarkupLine("[green]Enter a new username:[/]");
                    string _username = Console.ReadLine()!;
                    AnsiConsole.MarkupLine("[green]Enter a new password:[/]");
                    string _password = Console.ReadLine()!;
                    UpdateUserFile("test_file.json", _username, _password);
                    break;
                case "read":
                    AnsiConsole.MarkupLine("[green]Enter name of the file you wish to read: (supported formats: <.txt, .md, .json, .*>)[/]");
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
                    var _userName = AnsiConsole.Ask<string>("Enter your [green]username[/]:");
                    var message = GreetUser(_userName);
                    AnsiConsole.MarkupLine($"[bold yellow]{message}[/]");
                    break;
                case "debug":
                    DebugMode();
                    break;
                case "exit":
                    AnsiConsole.MarkupLine("[red]Exiting program...[/]");
                    Environment.Exit(0);
                    break;
                default:
                    return;
            }

            AnsiConsole.WriteLine(); // newline
        }
    }
}
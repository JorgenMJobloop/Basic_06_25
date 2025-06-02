public interface ICLI
{
    /// <summary>
    /// Print a greeting to the user of the program
    /// </summary>
    /// <param name="username">The username of any given user</param>
    /// <returns>A greeting</returns>
    string GreetUser(string username);
    /// <summary>
    /// Read the content of any given file
    /// </summary>
    /// <param name="filePath">File to read the content of</param>
    void ReadFileContent(string filePath);

    /// <summary>
    /// Append text content to a given file.
    /// </summary>
    /// <param name="filePath">File to append to</param>
    /// <param name="content">The content to append</param>
    void AppendFileContent(string filePath, string content);
    /// <summary>
    /// Main loop
    /// </summary>
    void RunCLI();
}
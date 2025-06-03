using BCrypt;
public static class HashPasswords
{
    public static string Hashpassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }
}
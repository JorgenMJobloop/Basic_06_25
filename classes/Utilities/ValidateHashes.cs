public class ValidateHashes
{
    public string StringToHash(string input)
    {
        return input;
    }

    public string HashInput(string input)
    {
        return HashPasswords.Hashpassword(input);
    }

    public bool ValidateIfEqual()
    {
        string test = StringToHash("Hello");

        string directlyHashed = HashInput("Hello");

        string validateIfEqual = HashInput(test);

        if (validateIfEqual == directlyHashed)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
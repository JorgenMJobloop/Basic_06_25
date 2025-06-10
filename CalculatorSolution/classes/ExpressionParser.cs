public class ExpressionParser
{
    private string? Input;
    private int Position;

    public double Parse(string input)
    {
        Input = input.Replace(" ", "");
        Position = 0;
        return ParseExpression();
    }

    private double ParseExpression()
    {
        double x = ParseTerm();
        while (Position < Input!.Length)
        {
            if (Input[Position] == '+')
            {
                Position++;
                x += ParseTerm();
            }
            else if (Input[Position] == '-')
            {
                Position++;
                x -= ParseTerm();
            }
            else break;
        }
        return x;
    }

    private double ParseTerm()
    {
        double x = ParseFactor();
        while (Position < Input!.Length)
        {
            if (Input[Position] == '*')
            {
                Position++;
                x *= ParseFactor();
            }
            else if (Input[Position] == '/')
            {
                Position++;
                x /= ParseFactor();
            }
            else break;
        }
        return x;
    }


    private double ParseFactor()
    {
        if (Input![Position] == '(')
        {
            Position++;
            double result = ParseExpression();
            Position++;
            return result;
        }

        int start = Position;
        while (Position < Input!.Length && (char.IsDigit(Input[Position]) || Input[Position] == '.'))
        {
            Position++;
        }

        string number = Input.Substring(start, Position - start);

        return double.Parse(number, System.Globalization.CultureInfo.InvariantCulture);
    }
}
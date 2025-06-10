public class CLI
{
    Calculator calculator = new Calculator();
    ExpressionParser parser = new ExpressionParser();


    public void RunCLI()
    {
        Console.WriteLine("Welcome to the C# Calculator CLI");
        Console.WriteLine("Usage <add>, <divide>, <multiply>, <subtract>, <power>, <pythagoras>");
        while (true)
        {
            string input = Console.ReadLine()!;

            Console.WriteLine("Number a:");
            string numA = Console.ReadLine()!;
            Console.WriteLine("Number b:");
            string numB = Console.ReadLine()!;

            double.TryParse(numA, out double a);
            double.TryParse(numB, out double b);

            calculator.A = a;
            calculator.B = b;

            switch (input.ToLower())
            {
                case "add":
                    Console.WriteLine(calculator.Add(a, b));
                    break;
                case "multiply":
                    Console.WriteLine(calculator.Multiply(a, b));
                    break;
                case "subtract":
                    Console.WriteLine(calculator.Subtract(a, b));
                    break;
                case "power":
                    Console.WriteLine(calculator.Power(a, b));
                    break;
                case "pythagoras":
                    Console.WriteLine(calculator.Pythagoras());
                    break;
                default:
                    return;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Globalization;

public class Interpreter
{

    private double Evaluate(string expression)
    {
        var tokens = Tokenize(expression);
        var rpn = ToRPN(tokens);
        return EvaluateRPN(rpn);
    }

    private List<string> Tokenize(string expression)
    {
        var tokens = new List<string>();
        int index = 0;

        while (index < expression.Length)
        {
            char c = expression[index];

            if (char.IsWhiteSpace(c))
            {
                index++;
                continue;
            }

            // check for tokens
            if ("+-*/()".Contains(c))
            {
                tokens.Add(c.ToString());
            }
            else if (char.IsDigit(c) || c == '.')
            {
                var number = "";
                while (index < expression.Length && (char.IsDigit(expression[index]) || expression[index] == '.'))
                {
                    number += expression[index];
                    index++;
                }
                tokens.Add(number);
            }
            else
            {
                throw new Exception($"Invalid character: {c}");
            }
        }
        return tokens;
    }

    private List<string> ToRPN(List<string> tokens)
    {
        var output = new List<string>();
        var ops = new Stack<string>();
        var precedence = new Dictionary<string, int>()
        {
            {"+", 1}, {"-", 1}, {"*", 2}, {"/", 2}
        };

        foreach (var token in tokens)
        {
            if (double.TryParse(token, NumberStyles.Any, CultureInfo.InvariantCulture, out _))
            {
                output.Add(token);
            }
            else if ("+-*/".Contains(token))
            {
                while (ops.Count > 0 && ops.Peek() != "(" && precedence[ops.Peek()] >= precedence[token])
                {
                    output.Add(ops.Pop());
                }
                ops.Push(token);
            }
            else if (token == "(")
            {
                ops.Push(token);
            }
            else if (token == ")")
            {
                while (ops.Count > 0 && ops.Peek() != "(")
                {
                    output.Add(ops.Pop());
                }
                if (ops.Count == 0 || ops.Pop() != "(")
                {
                    throw new Exception("Invalid parentheses!");
                }
            }
        }

        while (ops.Count > 0)
        {
            var op = ops.Pop();
            if (op == "(" || op == ")")
            {
                throw new Exception("Invalid parentheses!");
            }
            output.Add(op);
        }

        return output;
    }

    private double EvaluateRPN(List<string> rpn)
    {
        var stack = new Stack<double>();

        foreach (var token in rpn)
        {
            if (double.TryParse(token, NumberStyles.Any, CultureInfo.InvariantCulture, out double value))
            {
                stack.Push(value);
            }
            else
            {
                if (stack.Count < 2)
                {
                    throw new Exception("Invalid expression!");
                }

                double b = stack.Pop();
                double a = stack.Pop();

                double result = token switch
                {
                    "+" => a + b,
                    "-" => a - b,
                    "*" => a * b,
                    "/" => a / b,
                    _ => throw new Exception($"Unknown operator: {token}")
                };

                stack.Push(result);
            }
        }

        if (stack.Count != 1)
        {
            throw new Exception("Invalid expression!");
        }

        return stack.Pop();
    }


    public void RunREPL()
    {
        Console.WriteLine($"Basic Interpreter (currently supports: +,-,*,/, parentheses). Press 'exit' to quit.");

        while (true)
        {
            Console.Write("> ");

            var input = Console.ReadLine()!;

            if (string.IsNullOrWhiteSpace(input))
            {
                break;
            }

            try
            {
                double result = Evaluate(input);
                Console.WriteLine($"> {result}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"An error occured: {e.Message}");
            }
        }
    }

}
namespace Recap;

class Program
{
    static void Main(string[] args)
    {
        PrintEmployees printEmployees = new PrintEmployees();

        Interpreter interpreter = new Interpreter();

        CustomStack<string> stack = new CustomStack<string>();

        CustomStack<double> numberStack = new CustomStack<double>();

        Birds birds = new Birds();

        stack.Push("Hello World");

        stack.Print();

        numberStack.Push(1);
        numberStack.Push(2);
        numberStack.Push(3);

        numberStack.Print();

        printEmployees.Print();

        birds.Species = "Chicken";
        birds.Cry();

        //interpreter.RunREPL();
    }
}

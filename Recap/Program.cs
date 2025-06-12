namespace Recap;

class Program
{
    static void Main(string[] args)
    {
        CustomStack<string> stack = new CustomStack<string>();

        CustomStack<double> numberStack = new CustomStack<double>();


        stack.Push("Hello World");
        stack.Pop();

        stack.Print();


        numberStack.Push(1);
        numberStack.Push(2);
        numberStack.Push(3);

        numberStack.Print();

        List<Employee> employees = new List<Employee>()
        {
            new Developer { Name = "Peter"},
            new Manager { Name = "Bill Lumbergh"},
        };

        foreach (var employee in employees)
        {
            employee.Work();
        }

    }
}

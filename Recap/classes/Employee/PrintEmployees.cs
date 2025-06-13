public class PrintEmployees
{
    public void Print()
    {
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
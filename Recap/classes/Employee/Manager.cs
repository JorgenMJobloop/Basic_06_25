public class Manager : Employee
{
    public override void Work()
    {
        Console.WriteLine($"{Name} wants you to come in on, hmm, saturday, and fix some issues!");
    }
}
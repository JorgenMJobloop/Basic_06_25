public class CustomStack<T> : ICustomStack<T>
{
    private List<T> Elements = new List<T>();
    public void Push(T item)
    {
        Elements.Add(item);
    }

    public T Pop()
    {
        var item = Elements[^1];
        Elements.RemoveAt(Elements.Count - 1);
        return item;
    }

    public void Print()
    {
        foreach (var items in Elements)
        {
            Console.WriteLine($"Elements currently in stack: {items}");
        }
    }
}
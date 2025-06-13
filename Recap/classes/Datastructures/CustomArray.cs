/// <summary>
/// Naive implementation of the Array datastructure
/// </summary>
/// <typeparam name="T">Generic type paramater</typeparam>
public class CustomArray<T>
{
    private readonly T[]? _array;
    private int SIZE;

    public void Push(T item)
    {
        if (SIZE >= 255)
        {
            throw new IOException("Byte size cannot be greater than 8 bits");
        }
        _array![SIZE] = item;
    }

    public T Pop()
    {
        var index = _array![^1];
        return index;
    }
}
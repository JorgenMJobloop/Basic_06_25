public interface ICustomStack<T>
{
    void Push(T item);

    T Pop();
}
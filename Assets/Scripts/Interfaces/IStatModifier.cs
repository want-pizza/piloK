public interface IStatModifier<T>
{
    public T Modify(T value);
    public int Priority { get; }
}

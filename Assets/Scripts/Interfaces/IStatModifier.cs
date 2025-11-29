public interface IStatModifier<T>
{
    public T Modify(T baseValue);
    public int Priority { get; }
}

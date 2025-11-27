using System;
using System.Collections.Generic;

public class Stat<T>
{
    public T BaseValue { get; set; }

    private List<IStatModifier<T>> modifiers = new();

    public event Action<T> OnValueChanged;

    private T lastValue;

    public Stat(T value)
    {
        BaseValue = value;
    }

    public static implicit operator T(Stat<T> obj)
    {
        return obj.Value;
    }

    public T Value
    {
        get
        {
            T value = BaseValue;

            foreach (var mod in modifiers)
            {
                value = mod.Modify(value);
            }

            if (!EqualityComparer<T>.Default.Equals(lastValue, value))
            {
                lastValue = value;
                OnValueChanged?.Invoke(value);
            }

            return value;
        }
    }

    public void AddModifier(IStatModifier<T> modifier)
    {
        modifiers.Add(modifier);
        _ = Value;
    }

    public void RemoveModifier(IStatModifier<T> modifier)
    {
        modifiers.Remove(modifier);
        _ = Value;
    }

    public void ClearModifiers()
    {
        modifiers.Clear();
        _ = Value;
    }
}

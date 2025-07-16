using System;
using UnityEngine;

public class Field<T> :  IField
{
    public T Value
    {
        get => value;
        set => UpdateValue(value);
    }

    public event Action<T> OnValueChanged;
    public event Action OnValueChangedNoArgs;

    protected T value;

    public Field(T value = default)
    {
        this.value = value;
    }

    public static implicit operator T(Field<T> obj)
    {
        return obj.Value;
    }

    protected void UpdateValue(T value)
    {
        if (Equals(this.value, value))
            return;

        this.value = value;
        OnValueChanged?.Invoke(value);
        OnValueChangedNoArgs?.Invoke();
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplyModifier<T> : IStatModifier<T>
{
    public T Value;

    private int priority = 1;
    public int Priority => priority;

    public MultiplyModifier(T value)
    {
        Value = value;
    }

    public T Modify(T value)
    {
        if (typeof(T) == typeof(int))
        {
            int result = (int)(object)value * (int)(object)Value;
            return (T)(object)result;
        }
        else if (typeof(T) == typeof(float))
        {
            float result = (float)(object)value * (float)(object)Value;
            return (T)(object)result;
        }
        else
        {
            Debug.Log($"Unregistrated type of value - {typeof(T)}");
            return (T)(object)value;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddModifier<T> : IStatModifier<T>
{
    public T Value;

    public AddModifier(T value)
    {
        Value = value;
    }

    public T Modify(T baseValue)
    {
        if (typeof(T) == typeof(int))
        {
            int result = (int)(object)baseValue + (int)(object)Value;
            return (T)(object)result;
        }
        else if (typeof(T) == typeof(float))
        {
            float result = (float)(object)baseValue + (float)(object)Value;
            return (T)(object)result;
        }
        else
        {
            Debug.Log($"Unregistrated type of value - {typeof(T)}");
            return (T)(object)baseValue;
        }
    }

}


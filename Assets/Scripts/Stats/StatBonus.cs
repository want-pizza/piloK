[System.Serializable]
public struct StatBonus
{
    public StatType statType;
    public ModifierType modifier;
    public float amount;
    //public IStatModifier<float> modifier; --- need Odin
    
}

[System.Serializable]
public enum ModifierType
{
    add,
    multiply,
    addAfterMultiply
}

[System.Serializable]
public struct StatPreview
{
    public StatType Type;
    public float OldValue;
    public float NewValue;
    public int Delta;

    public StatPreview(StatType type, float oldValue, float newValue)
    {
        Type = type;
        OldValue = oldValue;
        NewValue = newValue;
        Delta = (int)(newValue - oldValue);
    }
}
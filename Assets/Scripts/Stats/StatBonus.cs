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
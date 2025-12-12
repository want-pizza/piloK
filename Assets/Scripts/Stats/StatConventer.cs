public static class ModifierFactory
{
    public static IStatModifier<float> GetModifier(ModifierType modifierType, float amount)
    {
        switch (modifierType)
        {
            case ModifierType.add:
                return new AddModifier<float>(amount);

            //case ModifierType.multiply:
            //    return new MultiplyModifier(amount);

            //case ModifierType.addAfterMultiply:
            //    return new AddAfterMultiplyModifier(amount);

            default:
                throw new System.NotImplementedException(
                    $"Unknown modifier type: {modifierType}"
                );
        }
    }
}

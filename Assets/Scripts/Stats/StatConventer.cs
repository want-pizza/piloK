using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StatConventer
{
    public static IStatModifier<float> GetStatModiier(ModifierType modifierType, float amount)
    {
        if(modifierType == ModifierType.add)
            return new AddModifier<float>(amount);

        if (modifierType == ModifierType.multiply)
            return new AddModifier<float>(amount);

        if (modifierType == ModifierType.addAfterMultiply)
            return new AddModifier<float>(amount);

        return null;
    }
}

using System.Collections.Generic;

public static class StatPreviewUtility
{
    public static float CalculatePreviewValue(Stat<float> original, StatBonus bonus)
    {
        // тимчасовий стат
        var temp = new Stat<float>(original.BaseValue);

        // копіюємо існуючі модифікатори
        foreach (var mod in original.Modifires)
            temp.AddModifier(mod);

        // додаємо модифікатор від предмета
        var previewMod = ModifierFactory.GetModifier(bonus.modifier, bonus.amount);
        temp.AddModifier(previewMod);

        // значення після модифікації
        return temp.Value;
    }

}

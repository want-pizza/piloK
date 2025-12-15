using System.Collections.Generic;

public static class StatPreviewUtility
{
    public static float CalculatePreviewValue(
         Stat<float> original,
         List<StatBonus> bonuses
     )
    {
        // 1) тимчасовий стат з базовим значенням
        var temp = new Stat<float>(original.BaseValue);

        // 2) копіюємо ВСІ існуючі модифікатори
        foreach (var mod in original.Modifires)
        {
            temp.AddModifier(mod);
        }

        // 3) додаємо ВСІ бонуси предмета
        for (int i = 0; i < bonuses.Count; i++)
        {
            var bonus = bonuses[i];
            var previewMod = ModifierFactory.GetModifier(
                bonus.modifier,
                bonus.amount
            );

            temp.AddModifier(previewMod);
        }

        // 4) повертаємо фінальне значення
        return temp.Value;
    }
}

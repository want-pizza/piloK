using System.Collections.Generic;
using UnityEngine;

public class ItemRandomizer
{
    private ItemPoolRuntime pool;
    private Dictionary<ItemRareness, int> rarityWeights;

    public ItemRandomizer(ItemPoolRuntime pool, Dictionary<ItemRareness, int> rarityWeights)
    {
        this.pool = pool;
        this.rarityWeights = rarityWeights;
    }

    public BaseItemObject GetRandomItem()
    {
        var available = new List<BaseItemObject>(pool.GetAvailable());
        if (available.Count == 0)
            return null;

        // Вибираємо рідкість
        ItemRareness rarity = RollRarity();

        // Фільтруємо доступні айтеми по рідкості
        var filtered = available.FindAll(i => i.Rareness == rarity);
        if (filtered.Count == 0)
            filtered = available; // якщо немає потрібної рідкості, беремо будь-який

        // Вибір випадкового айтема
        var selected = filtered[Random.Range(0, filtered.Count)];
        pool.Consume(selected);

        return Object.Instantiate(selected); // повертаємо копію
    }

    public List<BaseItemObject> GetRandomItems(int quantity)
    {
        var result = new List<BaseItemObject>();

        // Робимо локальну копію доступного пулу
        var available = new List<BaseItemObject>(pool.GetAvailable());

        if (available.Count == 0 || quantity <= 0)
            return result; // нічого повертати

        quantity = Mathf.Min(quantity, available.Count); // обмежуємо кількість

        while (result.Count < quantity)
        {
            // Вибираємо рідкість
            ItemRareness rarity = RollRarity();

            // Фільтруємо доступні айтеми по рідкості
            var filtered = available.FindAll(i => i.Rareness == rarity);
            if (filtered.Count == 0)
                filtered = available; // якщо нема потрібної рідкості

            // Вибір випадкового айтема
            var selected = filtered[Random.Range(0, filtered.Count)];

            // Додаємо в результат і віднімаємо з локальної копії
            result.Add(Object.Instantiate(selected));
            available.Remove(selected);   // щоб не повторювався
            pool.Consume(selected);        // зменшуємо кількість в пулі
        }

        return result;
    }


    private ItemRareness RollRarity()
    {
        int total = 0;
        foreach (var w in rarityWeights.Values)
            total += w;

        int roll = Random.Range(0, total);
        int acc = 0;

        foreach (var pair in rarityWeights)
        {
            acc += pair.Value;
            if (roll < acc)
                return pair.Key;
        }

        return ItemRareness.Usual;
    }
}

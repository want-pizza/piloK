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

        ItemRareness rarity = RollRarity();

        var filtered = available.FindAll(i => i.Rareness == rarity);
        if (filtered.Count == 0)
            filtered = available;

        var selected = filtered[Random.Range(0, filtered.Count)];
        pool.Consume(selected);

        return Object.Instantiate(selected);
    }

    public List<BaseItemObject> GetRandomItems(int quantity)
    {
        var result = new List<BaseItemObject>();

        var available = new List<BaseItemObject>(pool.GetAvailable());

        if (available.Count == 0 || quantity <= 0)
            return result;

        quantity = Mathf.Min(quantity, available.Count);

        while (result.Count < quantity)
        {
            ItemRareness rarity = RollRarity();

            var filtered = available.FindAll(i => i.Rareness == rarity);
            if (filtered.Count == 0)
                filtered = available;

            var selected = filtered[Random.Range(0, filtered.Count)];

            result.Add(Object.Instantiate(selected));
            available.Remove(selected);
            pool.Consume(selected);
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

using System.Collections.Generic;

public class ItemPoolRuntime
{
    private Dictionary<BaseItemObject, int> pool = new Dictionary<BaseItemObject, int>();

    public ItemPoolRuntime(IEnumerable<ItemPoolEntry> entries)
    {
        foreach (var entry in entries)
        {
            pool[entry.item] = entry.count;
        }
    }

    public bool IsAvailable(BaseItemObject item)
    {
        return pool.ContainsKey(item) && pool[item] != 0;
    }

    public void Consume(BaseItemObject item)
    {
        if (pool[item] > 0)
            pool[item]--;
    }

    public IEnumerable<BaseItemObject> GetAvailable()
    {
        foreach (var kvp in pool)
            if (kvp.Value != 0)
                yield return kvp.Key;
    }
}
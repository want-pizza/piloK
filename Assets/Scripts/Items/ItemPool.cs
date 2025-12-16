using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Item Pool")]
public class ItemPool : ScriptableObject
{
    public List<ItemPoolEntry> entries = new List<ItemPoolEntry>();

    [Header("Default counts by rareness")]
    public int usualCount = -1;
    public int rareCount = 5;
    public int epicCount = 3;
    public int legendaryCount = 1;

    [Header("Root for FillFolder method")]
    public string root = "Assets/ScriptableObjects/Items";

    private void OnValidate()
    {
        foreach (var entry in entries)
        {
            if (entry.item == null)
                continue;

            entry.rareness = entry.item.Rareness;

            if (entry.count != 0)
                continue;

            entry.count = GetDefaultCount(entry.rareness);
        }
    }

    private int GetDefaultCount(ItemRareness rareness)
    {
        return rareness switch
        {
            ItemRareness.Usual => usualCount,
            ItemRareness.Rare => rareCount,
            ItemRareness.Epic => epicCount,
            ItemRareness.Legendary => legendaryCount,
            _ => -1
        };
    }

    [ContextMenu("Recalculate Counts")]
    private void RecalculateCounts()
    {
        foreach (var entry in entries)
        {
            if (entry.item == null)
                continue;

            entry.count = GetDefaultCount(entry.item.Rareness);
        }
    }

    [ContextMenu("Fill Pool From Folder")]
    private void FillFromFolder()
    {
#if UNITY_EDITOR
        entries.Clear();

        string[] guids = AssetDatabase.FindAssets(
            "t:BaseItemObject",
            new[] { root }
        );

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);

            string directory = System.IO.Path
                .GetDirectoryName(path)
                .Replace("\\", "/");

            if (directory != root)
                continue;

            var item = AssetDatabase.LoadAssetAtPath<BaseItemObject>(path);

            if (item == null)
                continue;

            entries.Add(new ItemPoolEntry
            {
                item = item,
                count = 0 // OnValidate сам поставить
            });
        }

        EditorUtility.SetDirty(this);
#endif
    }
}

[System.Serializable]
public class ItemPoolEntry
{
    public BaseItemObject item;

    [Tooltip("-1 = infinite")]
    public int count;

    [HideInInspector]
    public ItemRareness rareness;
}

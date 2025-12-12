using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLevel : MonoBehaviour, ICharacterLevel
{
    [SerializeField] private LevelUpItemChoiceUI ui;
    [SerializeField] private List<ItemStatData> allItems;
    [SerializeField] private ItemStatData[] items;

    private Field<int> currentLevel = new Field<int>(1);
    private Field<int> currentXP = new Field<int>(0);

    public Field<int> CurrentLevel => currentLevel;
    public Field<int> CurrentXP => currentXP;

    //event Action<int> OnLevelUp;
    private void OnLevelUp()
    {
        items = GetRandomItems(3);
        ui.Open(items);
        PauseController.Instance.SetPause(true);
    }

    public void GainXP(int value)
    {
        currentXP.Value += value;
        while (currentXP >= GetXPToNextLevel())
        {
            currentXP.Value -= GetXPToNextLevel();
            currentLevel.Value++;
            OnLevelUp();
        }
    }

    public int GetXPToNextLevel()
    {
        return currentLevel * 100;
    }

    private ItemStatData[] GetRandomItems(int count)
    {
        // робимо локальний список
        List<ItemStatData> pool = new List<ItemStatData>(allItems);

        // гарантуємо що count не більший за розмір пулу
        count = Mathf.Min(count, pool.Count);

        ItemStatData[] result = new ItemStatData[count];

        for (int i = 0; i < count; i++)
        {
            int index = UnityEngine.Random.Range(0, pool.Count);
            result[i] = Instantiate(pool[index]); // робимо копію!
            pool.RemoveAt(index);
        }

        return result;
    }

}
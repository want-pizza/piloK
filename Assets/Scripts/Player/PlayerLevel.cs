using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLevel : MonoBehaviour, ICharacterLevel
{
    [SerializeField] private LevelUpItemChoiceUI ui;
    [SerializeField] private RuntimeItemData test;
    [SerializeField] private List<BaseItemObject> allItems;
    [SerializeField] private BaseItemObject[] items;


    private Field<int> currentLevel = new Field<int>(1);
    private Field<int> currentXP = new Field<int>(0);

    public Field<int> CurrentLevel => currentLevel;
    public Field<int> CurrentXP => currentXP;

    //event Action<int> OnLevelUp;
    private void OnLevelUp()
    {
        items = GetRandomItems(3);
        items[0] = test;
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

    private BaseItemObject[] GetRandomItems(int count)
    {
        // робимо локальний список
        List<BaseItemObject> pool = new List<BaseItemObject>(allItems);

        // гарантуємо що count не більший за розмір пулу
        count = Mathf.Min(count, pool.Count);

        BaseItemObject[] result = new BaseItemObject[count];

        for (int i = 0; i < count; i++)
        {
            int index = UnityEngine.Random.Range(0, pool.Count);
            result[i] = Instantiate(pool[index]); // робимо копію!
            pool.RemoveAt(index);
        }

        return result;
    }

}
using System.Collections.Generic;
using UnityEngine;

public class PlayerLevel : MonoBehaviour, ICharacterLevel
{
    [SerializeField] private LevelUpItemChoiceUI ui;
    [SerializeField] private BaseItemObject test;
    [SerializeField] private ItemPool itemPool;

    private ItemPoolRuntime runtimePool;
    private ItemRandomizer randomizer;

    private Field<int> currentLevel = new Field<int>(1);
    private Field<int> currentXP = new Field<int>(0);

    public Field<int> CurrentLevel => currentLevel;
    public Field<int> CurrentXP => currentXP;

    private void Awake()
    {
        runtimePool = new ItemPoolRuntime(itemPool.entries);

        var weights = new Dictionary<ItemRareness, int>
        {
            { ItemRareness.Usual, 60 },
            { ItemRareness.Rare, 25 },
            { ItemRareness.Epic, 10 },
            { ItemRareness.Legendary, 5 }
        };

        randomizer = new ItemRandomizer(runtimePool, weights);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            GainXP(100);
        }
    }

    private void OnLevelUp()
    {
        var items = randomizer.GetRandomItems(3);


        // for tests
        //items[0] = test;

        ui.Open(items);
        PauseController.Instance.SetPause(true);
        Time.timeScale = 0;
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
}

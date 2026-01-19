using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class PlayerLevel : MonoBehaviour, ICharacterLevel
{
    [SerializeField] private LevelUpItemChoiceUI ui;
    [SerializeField] private BaseItemObject test;
    [SerializeField] private ItemPool itemPool;

    private ItemPoolRuntime runtimePool;
    private ItemRandomizer randomizer;

    private Field<int> currentLevel = new Field<int>(0);
    private Field<int> currentXP = new Field<int>(0);

    public Field<int> CurrentLevel => currentLevel;
    public Field<int> CurrentXP => currentXP;

    [Header("Reroll Settings")]
    [SerializeField] int rerollCost = 5;
    [SerializeField] float rerollCostMultiplier = 1.25f;


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
        RunStatsCollector.Instance.SetLevel(currentLevel);

        var items = randomizer.GetRandomItems(3);

        // for tests
        //items[0] = test;

        ui.Open(items);
        PauseController.Instance.SetPause(true);
        Time.timeScale = 0;
    }

    public bool Reroll()
    {
        if (CoinController.Instance.Coins <= rerollCost)
            return false;

        CoinController.Instance.AddCoins(-rerollCost);
        rerollCost = Convert.ToInt32(rerollCost * rerollCostMultiplier);

        var items = randomizer.GetRandomItems(3);
        ui.Open(items);
        return true;
    }

    public void GainXP(int value)
    {
        currentXP.Value += value;

        while (currentXP >= GetXPToNextLevel(currentLevel))
        {
            currentXP.Value -= GetXPToNextLevel(currentLevel);
            currentLevel.Value++;
            OnLevelUp();
        }
    }

    public int GetXPToNextLevel(int level)
    {
        if(level == 0)
            return 40;

        return Mathf.Min(
                GetXPToNextLevel(--level) + 50,
                300
            );
    }
}

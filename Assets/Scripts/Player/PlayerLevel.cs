using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class PlayerLevel : MonoBehaviour, ICharacterLevel
{
    [SerializeField] private LevelUpItemChoiceUI levelUpItemChoiceUI;
    [SerializeField] private PlayerLevelUI levelUI;
    [SerializeField] private BaseItemObject test;
    [SerializeField] private ItemPool itemPool;

    private ItemPoolRuntime runtimePool;
    private ItemRandomizer randomizer;

    private Field<int> currentLevel = new Field<int>(0);
    private Field<int> currentXP = new Field<int>(0);

    private Field<bool> isDead;

    public Field<int> CurrentLevel => currentLevel;
    public Field<int> CurrentXP => currentXP;

    [Header("Reroll Settings")]
    [SerializeField] int rerollPrice = 5;
    [SerializeField] float rerollCostMultiplier = 1.25f;


    private void Awake()
    {
        runtimePool = new ItemPoolRuntime(itemPool.entries);
        isDead = GetComponent<PlayerLifeCircle>().FieldIsDead;

        var weights = new Dictionary<ItemRareness, int>
        {
            { ItemRareness.Usual, 60 },
            { ItemRareness.Rare, 25 },
            { ItemRareness.Epic, 10 },
            { ItemRareness.Legendary, 5 }
        };

        randomizer = new ItemRandomizer(runtimePool, weights);
    }
    private void OnLevelUp()
    {   
        RunStatsCollector.Instance.SetLevel(currentLevel);

        var items = randomizer.GetRandomItems(3);

        // for tests
        //items[0] = test;

        levelUI.SetLevel(currentLevel);
        levelUpItemChoiceUI.Open(items);
        PauseController.Instance.SetPause(true);
        Time.timeScale = 0;
    }

    public bool Reroll()
    {
        if (CoinController.Instance.Coins <= rerollPrice)
            return false;

        CoinController.Instance.AddCoins(-rerollPrice);
        rerollPrice = Convert.ToInt32(rerollPrice * rerollCostMultiplier);

        levelUpItemChoiceUI.UpdateRerollPrise(rerollPrice);

        var items = randomizer.GetRandomItems(3);
        levelUpItemChoiceUI.Open(items);
        return true;
    }

    public void GainXP(int value)
    {
        if (isDead)
            return;

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

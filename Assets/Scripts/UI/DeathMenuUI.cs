using QuantumTek.SimpleMenu;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DeathMenuUI : MonoBehaviour
{
    [SerializeField] private Transform statsContainer;
    [SerializeField] private RunStatUI runStatPrefab;
    [SerializeField] private RectTransform contentRT;

    public void Open(RunStats stats)
    {
        Build(stats);

        Canvas.ForceUpdateCanvases();
        contentRT.gameObject.SetActive(true);
        StartCoroutine(OpenRoutine());

    }

    private IEnumerator OpenRoutine()
    {
        yield return null;
        RebuildUI();
    }

    public void RebuildUI()
    {
        LayoutRebuilder.MarkLayoutForRebuild(contentRT);
        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(contentRT);
    }

    private void Build(RunStats stats)
    {
        Clear();

        Add("Max HP", stats.maxHP);
        Add("Level", stats.level);
        Add("Wave", stats.wave);
        Add("Survived Time", FormatTime(stats.survivedTime));
        Add("Enemies Killed", stats.enemiesKilled);
        Add("Damage Dealt", stats.damageDealt);
        Add("Max One Shot", stats.oneShotMaxDamageDealt);
        Add("Coins Gained", stats.coinsGained);
    }

    private void Add(string label, int value)
    {
        var row = Instantiate(runStatPrefab, statsContainer);
        row.Setup(label, value.ToString());
    }

    private void Add(string label, float value)
    {
        var row = Instantiate(runStatPrefab, statsContainer);
        row.Setup(label, value.ToString("0.##"));
    }

    private void Add(string label, string value)
    {
        var row = Instantiate(runStatPrefab, statsContainer);
        row.Setup(label, value);
    }

    private void Clear()
    {
        foreach (Transform child in statsContainer)
            Destroy(child.gameObject);
    }

    private string FormatTime(float seconds)
    {
        int min = Mathf.FloorToInt(seconds / 60f);
        int sec = Mathf.FloorToInt(seconds % 60f);
        return $"{min:00}:{sec:00}";
    }
}

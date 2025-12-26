using UnityEngine;

public class DeathMenuController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private DeathMenuUI deathMenuUI;

    private bool isShown = false;

    private void OnEnable()
    {
        RunManager.Instance.RunEnded += ShowDeathMenu;
    }
    private void OnDisable()
    {
        RunManager.Instance.RunEnded -= ShowDeathMenu;
    }

    public void ShowDeathMenu()
    {
        if (isShown)
            return;

        isShown = true;

        RunStats stats = RunStatsCollector.Instance.GetSnapshot();
        deathMenuUI.Open(stats);
    }
}

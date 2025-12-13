using UnityEngine;
using TMPro;

public class WaveTimerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private TextMeshProUGUI waveNumber;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private Animator animator;

    private const string IsEnding = "endIsNear";
    private const string SpeedParam = "speed";

    private float dangerThreshold = 3f;

    public void Subscribe(WaveController controller)
    {
        controller.OnTimerChanged += UpdateWaveTimer;
        controller.OnWaveStarted += SetWaveNumber;
        controller.OnInterWave += UpdateInterWaveTimer;
    }
    public void Unsubscribe(WaveController controller)
    {
        controller.OnTimerChanged -= UpdateWaveTimer;
        controller.OnWaveStarted -= SetWaveNumber;
        controller.OnInterWave -= UpdateInterWaveTimer;
    }

    private void UpdateWaveTimer(int waveIndex, float timeLeft)
    {
        timerText.text = timeLeft.ToString("#.##");

        bool danger = timeLeft <= dangerThreshold;
        animator.SetBool(IsEnding, danger);

        float normalized = Mathf.Clamp01(1f - timeLeft / 10f);
        animator.SetFloat(SpeedParam, 1f + normalized * 2f);
    }

    private void UpdateInterWaveTimer(float timeLeft)
    {
        waveText.text = "Next wave in";
        timerText.text = Mathf.Ceil(timeLeft).ToString();

        animator.SetBool(IsEnding, false);
        animator.SetFloat(SpeedParam, 1f);
    }

    private void SetWaveNumber(int waveIndex)
    {
        waveNumber.text = $"{waveIndex + 1}";
    }
}

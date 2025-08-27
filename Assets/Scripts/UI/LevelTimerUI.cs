using TMPro;
using UnityEngine;

public class LevelTimerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    private int minutes;
    private int seconds;
    private int milliseconds;

    private void OnEnable()
    {
        LevelTimerManager.Instance.isHideField.OnValueChanged += ShowTimer;
        ShowTimer(LevelTimerManager.Instance.isHideField);
    }
    private void OnDisable()
    {
        LevelTimerManager.Instance.isHideField.OnValueChanged -= ShowTimer;
    }
    private void Update()
    {
        float time = LevelTimerManager.Instance.GetTimer();

        minutes = Mathf.FloorToInt(time / 60f);

        seconds = Mathf.FloorToInt(time % 60f);

        milliseconds = Mathf.FloorToInt((time * 100f) % 100f);

        timerText.text = $"{minutes:00}:{seconds:00}.{milliseconds:00}";

    }

    public void ShowTimer(bool value)
    {
        Debug.Log($"LevelTimerUI ShowTimer({!value})");
        timerText.gameObject.SetActive(!value);
    }
}

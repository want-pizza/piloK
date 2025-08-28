using TMPro;
using UnityEngine;

public class LevelTimerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;

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
        timerText.text = ConvertToStringWithMileseconds(LevelTimerManager.Instance.GetTimer); 
    }

    public static string ConvertToStringWithMileseconds(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);

        int seconds = Mathf.FloorToInt(time % 60f);

        int milliseconds = Mathf.FloorToInt((time * 100f) % 100f);

        return $"{minutes:00}:{seconds:00}.{milliseconds:00}";
    }

    public static string ConvertToString(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);

        int seconds = Mathf.FloorToInt(time % 60f);

        return $"{minutes:00}:{seconds:00}";
    }

    public void ShowTimer(bool value)
    {
        Debug.Log($"LevelTimerUI ShowTimer({!value})");
        timerText.gameObject.SetActive(!value);
    }
}

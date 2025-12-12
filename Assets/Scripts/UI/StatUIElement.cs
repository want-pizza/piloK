using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatUIElement : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI valueText;
    public TextMeshProUGUI previewText;

    public void SetName(string name)
    {
        nameText.text = name;
    }

    public void SetValue(float value)
    {
        valueText.text = value.ToString("0.##");
    }

    public void ShowPreview(float delta, float newValue)
    {
        // +1.5 → зелений
        // -0.3 → червоний
        // 0 → синій

        if (delta > 0)
            previewText.color = Color.green;
        else if (delta < 0)
            previewText.color = Color.red;
        else
            previewText.color = Color.blue;

        previewText.text = $"→ {newValue:0.##}";
    }

    public void ClearPreview()
    {
        Debug.Log("HidePreview");
        previewText.text = "";
    }
}

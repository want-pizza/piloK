using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatUIElement : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI valueText;

    public void SetName(string name)
    {
        nameText.text = name;
    }

    public void SetValue(float value)
    {
        valueText.text = value.ToString("0.##");
    }
}

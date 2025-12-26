using TMPro;
using UnityEngine;

public class RunStatUI : MonoBehaviour
{
    [SerializeField] private TMP_Text label;
    [SerializeField] private TMP_Text value;

    public void Setup(string labelText, string valueText)
    {
        label.text = labelText;
        value.text = valueText;
    }
}

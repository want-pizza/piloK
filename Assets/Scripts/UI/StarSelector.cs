using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class StarEntry
{
    public string name;
    public UnityEngine.GameObject starObject;
}

public class StarSelector : MonoBehaviour
{
    [SerializeField] private List<StarEntry> starEntries = new List<StarEntry>();

    private Dictionary<string, UnityEngine.GameObject> stars;

    private void Awake()
    {
        stars = new Dictionary<string, UnityEngine.GameObject>();
        foreach (var entry in starEntries)
        {
            if (!stars.ContainsKey(entry.name))
                stars.Add(entry.name, entry.starObject);
        }
    }

    public void SetStar(string name, bool isActive, string label, Color textColor)
    {
        if (!stars.ContainsKey(name))
        {
            Debug.LogWarning($"Star with id {name} not found!");
            return;
        }

        var star = stars[name];

        star.GetComponent<Animator>().SetBool("isActive", isActive);

        TextMeshProUGUI textMeshPro = star.GetComponentInChildren<TextMeshProUGUI>();
        if (textMeshPro != null)
        {
            textMeshPro.text = label;
            textMeshPro.color = textColor;
        }
    }
}

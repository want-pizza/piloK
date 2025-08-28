using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class StarEntry
{
    public int id;
    public GameObject starObject;
}

public class StarSelector : MonoBehaviour
{
    [SerializeField] private List<StarEntry> starEntries = new List<StarEntry>();

    private Dictionary<int, GameObject> stars;

    private void Awake()
    {
        stars = new Dictionary<int, GameObject>();
        foreach (var entry in starEntries)
        {
            if (!stars.ContainsKey(entry.id))
                stars.Add(entry.id, entry.starObject);
        }
    }

    public void SetStar(int id, bool isActive, string label, Color textColor)
    {
        if (!stars.ContainsKey(id))
        {
            Debug.LogWarning($"Star with id {id} not found!");
            return;
        }

        var star = stars[id];

        star.GetComponent<Animator>().SetBool("isActive", isActive);

        TextMeshProUGUI textMeshPro = star.GetComponentInChildren<TextMeshProUGUI>();
        if (textMeshPro != null)
        {
            textMeshPro.text = label;
            textMeshPro.color = textColor;
        }
    }
}

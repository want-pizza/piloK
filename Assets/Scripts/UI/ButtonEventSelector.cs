using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

[System.Serializable]
public class MenuButtonEntry
{
    public string name;
    public Button button;
}

public class ButtonEventSelector : MonoBehaviour
{
    [SerializeField] private List<MenuButtonEntry> buttonEntries = new List<MenuButtonEntry>();

    private Dictionary<string, Button> buttons;

    private void Awake()
    {
        buttons = new Dictionary<string, Button>();
        foreach (var entry in buttonEntries)
        {
            if (!string.IsNullOrEmpty(entry.name) && entry.button != null)
            {
                if (!buttons.ContainsKey(entry.name))
                    buttons.Add(entry.name, entry.button);
            }
        }
    }
    public void Subscribe(string name, Action<string> callback, string value)
    {
        if (buttons.TryGetValue(name, out var button))
        {
            button.onClick.AddListener(() => callback?.Invoke(value));
        }
        else
        {
            Debug.LogWarning($"Button with id '{name}' not found!");
        }
    }

    public void Subscribe(string name, Action callback)
    {
        if (buttons.TryGetValue(name, out var button))
        {
            button.onClick.AddListener(() => callback?.Invoke());
        }
        else
        {
            Debug.LogWarning($"Button with id '{name}' not found!");
        }
    }
}
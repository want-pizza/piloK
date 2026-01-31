using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerLevelUI : MonoBehaviour
{
    [SerializeField] private TMP_Text levelText;

    public void SetLevel(int value)
    {
        levelText.text = "LVL " + value.ToString();
    }
}

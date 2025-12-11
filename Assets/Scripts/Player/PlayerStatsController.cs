using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsController : MonoBehaviour
{
    [SerializeField] private PlayerStats model;
    [SerializeField] private PlayerStatsUI view;

    public void ShowStats()
    {
        var stats = model.GetAllStats();
        view.DisplayStats(stats);
    }
}

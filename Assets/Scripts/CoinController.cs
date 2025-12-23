using System;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    public static CoinController Instance;

    public int Coins { get; private set; }

    public event Action<int> OnCoinsDelta;

    private void Awake()
    {
        Instance = this;
    }

    public void AddCoins(int amount)
    {
        Coins += amount;
        OnCoinsDelta?.Invoke(amount);
    }
}

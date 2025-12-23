using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CoinHolderUI : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TMP_Text coinsText;
    [SerializeField] private TMP_Text previewText;

    [SerializeField] private float animationDuration = 1.2f;

    private int displayedCoins;
    private Coroutine animRoutine;

    private void OnEnable()
    {
        CoinController.Instance.OnCoinsDelta += PlayCoinAnimation;
        displayedCoins = CoinController.Instance.Coins;
        coinsText.text = displayedCoins.ToString();
        previewText.text = "";
    }

    private void OnDisable()
    {
        CoinController.Instance.OnCoinsDelta -= PlayCoinAnimation;
    }

    private void PlayCoinAnimation(int delta)
    {
        if (animRoutine != null)
            StopCoroutine(animRoutine);

        animRoutine = StartCoroutine(AnimateCoins(delta));
    }

    private IEnumerator AnimateCoins(int delta)
    {
        int startCoins = displayedCoins;
        int targetCoins = startCoins + delta;

        float time = 0f;

        while (time < animationDuration)
        {
            float t = time / animationDuration;

            int current = Mathf.RoundToInt(
                Mathf.Lerp(startCoins, targetCoins, t)
            );

            int remaining = targetCoins - current;

            displayedCoins = current;
            coinsText.text = displayedCoins.ToString();

            previewText.text = remaining > 0 ? $"+{remaining}" : "";

            time += Time.deltaTime;
            yield return null;
        }

        // фінал
        displayedCoins = targetCoins;
        coinsText.text = displayedCoins.ToString();
        previewText.text = string.Empty;
    }
}

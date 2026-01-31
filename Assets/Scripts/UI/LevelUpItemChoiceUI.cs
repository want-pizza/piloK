using QuantumTek.SimpleMenu;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class LevelUpItemChoiceUI : MonoBehaviour
{
    [SerializeField] private PlayerInventoryPresenter inventoryPresenter;
    [SerializeField] private PlayerStatsController statsController;
    [SerializeField] private PlayerLevel playerLevel;
    [SerializeField] private ItemCardUI itemCardPrefab;
    [SerializeField] private TMP_Text rerollValueTMP;
    [SerializeField] private Transform itemsContainer;

    [Header("Audio Settings")]
    [SerializeField] private AudioClip levelUp;

    private bool isOpened = false;

    public void Open(List<BaseItemObject> items)
    {
        if(!isOpened)
            AudioManager.Instance.PlayCantPausedSFX(levelUp);

        isOpened = true;
        inventoryPresenter.RefreshUI();
        GetComponent<SM_Window>().Toggle(true);

        foreach (Transform child in itemsContainer)
            Destroy(child.gameObject);

        StartCoroutine(SpawnItemsRoutine(items));
    }

    private IEnumerator SpawnItemsRoutine(List<BaseItemObject> items)
    {
        List<ItemCardUI> spawned = new();

        foreach (var item in items)
        {
            var card = Instantiate(itemCardPrefab, itemsContainer);
            card.gameObject.SetActive(false); 
            card.SetupItemStatData(statsController, item, SelectItem);
            spawned.Add(card);
        }

        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(
            itemsContainer as RectTransform
        );

        foreach (var card in spawned)
        {
            yield return new WaitForSecondsRealtime(card.AppearDuration - 0.05f);
            card.gameObject.SetActive(true);
            yield return new WaitForSecondsRealtime(0.15f);
        }
    }

    public void UpdateRerollPrise(int value)
    {
        rerollValueTMP.text = value.ToString();
    }

    private void SelectItem(BaseItemObject item)
    {
        Debug.Log("inventoryPresenter.TryPickupItem");
        inventoryPresenter.TryPickupItem(item, 1);
        PauseController.Instance.SetPause(false);
        Close();
    }

    public void TryReroll()
    {
        if (!playerLevel.Reroll())
        {
            ErrorHandler.Instance.PlayErrorNoArgs();
        }
    }

    public void Close()
    { 
        GetComponent<SM_Window>().Toggle(false); 
        isOpened = false;
    }
}

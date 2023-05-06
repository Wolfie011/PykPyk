using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemHolder : MonoBehaviour
{
    private ShopItem Item;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI amountText;
    [SerializeField] private Image iconImage;
    [SerializeField] private Image currencyImage;
    [SerializeField] private TextMeshProUGUI priceText;

    private LevelSystem levelSystem;

    public void Initialize(ShopItem item)
    {
        Item = item;
        iconImage.sprite = Item.Icon;
        titleText.text = Item.Name;
        descriptionText.text = Item.Description;
        currencyImage.sprite = ShopManager.currencySprites[Item.Currency];
        priceText.text = Item.Price.ToString();
        /*levelSystem = GetComponent<LevelSystem>();
        int currentLevel = levelSystem.GetLevel();
        if (Item.Level >= currentLevel)
        {
            UnlockItem();
        }*/
    }
    public void UnlockItem()
    {
        iconImage.gameObject.AddComponent<ShopItemDrag>();
        iconImage.transform.GetChild(0).gameObject.SetActive(true);
    }
}

using System.Collections.Generic;
using NUnit.Framework.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image itemImage;
    [SerializeField] private TMP_Text itemNameText;
    [SerializeField] private TMP_Text itemAmountText;
    [SerializeField] private TMP_Text itemPriceText;
    [SerializeField] private Button buyButton;

    private GameObject _itemPrefab;
    private int _itemId;

    public void SetupItem(GameObject prefab, int id, string name, Sprite icon, int price, int stock, System.Action onBuyClicked)
    {
        _itemPrefab = prefab;
        _itemId = id;

        if (itemImage != null)
            itemImage.sprite = icon;

        if (itemNameText != null)
            itemNameText.text = name;

        if (itemAmountText != null)
            itemAmountText.text = $"In Stock: {stock}";

        if (itemPriceText != null)
            itemPriceText.text = $"{price} coins";

        if (buyButton != null)
        {
            buyButton.onClick.RemoveAllListeners();
            buyButton.onClick.AddListener(() => onBuyClicked?.Invoke());
        }
    }

    public void UpdateStock(int newStock)
    {
        if (itemAmountText != null)
            itemAmountText.text = $"In Stock: {newStock}";
    }

    public void SetButtonText(string text)
    {
        TMP_Text buttonText = buyButton.GetComponentInChildren<TMP_Text>();
        if (buttonText != null)
        {
            buttonText.text = text;
        }
    }
}

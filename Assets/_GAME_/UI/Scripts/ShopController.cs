using System.Collections.Generic;
using UnityEngine;

public class ShopController : MonoBehaviour
{
    public static ShopController Instance { get; private set; }

    [Header("References")]
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private GameObject shopItemEntryPrefab;
    [SerializeField] private Transform contentParent;
    [SerializeField] private InventoryController inventoryController;
    [SerializeField] private PlayerCoinManager playerCoinManager;

    [Header("Shop Settings")]
    [SerializeField, Range(0f, 1f)] private float sellRate = 0.5f;

    [Header("Shop Items")]
    [SerializeField] private List<ShopItemData> shopItems = new List<ShopItemData>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        shopPanel?.SetActive(false);
    }

    private void Start() => PopulateShop();

    private void PopulateShop()
    {
        foreach (Transform child in contentParent)
            Destroy(child.gameObject);

        foreach (ShopItemData data in shopItems)
        {
            if (data.itemPrefab == null) continue;

            GameObject entry = Instantiate(shopItemEntryPrefab, contentParent);
            ShopItemUI entryUI = entry.GetComponent<ShopItemUI>();

            if (entryUI != null)
            {
                Item item = data.itemPrefab.GetComponent<Item>();
                if (item == null) continue;

                Sprite icon = item.GetComponent<UnityEngine.UI.Image>()?.sprite;
                entryUI.SetupItem(
                    data.itemPrefab,
                    item.id,
                    item.Name,
                    icon,
                    data.price,
                    data.stock,
                    () => OnItemButtonClicked(data, entryUI)
                );

                entryUI.SetButtonText(data.isSellMode ? "Sell" : "Buy");
            }
        }
    }

    private void OnItemButtonClicked(ShopItemData data, ShopItemUI entryUI)
    {
        if (data.isSellMode)
            HandleSellItem(data, entryUI);
        else
            HandleBuyItem(data, entryUI);
    }

    private void HandleBuyItem(ShopItemData data, ShopItemUI entryUI)
    {
        if (data.stock <= 0)
        {
            Debug.Log("Hết hàng!");
            return;
        }

        if (!playerCoinManager.SpendCoins(data.price))
        {
            Debug.Log("Không đủ tiền!");
            return;
        }

        bool success = inventoryController.AddItem(data.itemPrefab);
        if (success)
        {
            data.stock--;
            entryUI.UpdateStock(data.stock);
        }
        else
        {
            playerCoinManager.AddCoins(data.price);
            Debug.Log("Inventory đầy!");
        }
    }

    private void HandleSellItem(ShopItemData data, ShopItemUI entryUI)
    {
        Item item = data.itemPrefab.GetComponent<Item>();
        if (item == null) return;

        int sellPrice = Mathf.RoundToInt(data.price * sellRate);

        var items = inventoryController._getItemCounts();
        if (!items.ContainsKey(item.id) || items[item.id] <= 0)
        {
            Debug.Log($"Không có {item.Name} trong inventory để bán!");
            return;
        }

        inventoryController.RemoveItemsFromInventory(item.id, 1);
        playerCoinManager.AddCoins(sellPrice);
        data.stock++;
        entryUI.UpdateStock(data.stock);

        Debug.Log($"Đã bán {item.Name} với giá {sellPrice} coins.");
    }

    public void OpenShop()
    {
        PauseController.SetPause(true);
        shopPanel?.SetActive(true);
    }
    public void CloseShop()
    {
        PauseController.SetPause(false);
        shopPanel?.SetActive(false);
    }
}
[System.Serializable]
public class ShopItemData
{
    public GameObject itemPrefab;
    public int price = 100;
    public int stock = 5;
    public bool isSellMode = false;
}

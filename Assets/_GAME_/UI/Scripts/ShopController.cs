using System.Collections.Generic;
using UnityEngine;

public class ShopController : MonoBehaviour
{
    public static ShopController Instance { get; private set; }
    private bool _isOpen = false;

    [Header("References")]
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private GameObject shopItemEntryPrefab; // prefab UI (ShopItemEntry)
    [SerializeField] private Transform contentParent;         // Content của ScrollView
    [SerializeField] private InventoryController inventoryController;
    [SerializeField] private PlayerCoinManager playerCoinManager;

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

        if (shopPanel != null)
            shopPanel.SetActive(false); // ẩn khi bắt đầu
    }
    private void Start()
    {
        PopulateShop();
    }

    private void PopulateShop()
    {
        // Xóa entry cũ (nếu có)
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }

        // Tạo entry mới cho từng item trong danh sách
        foreach (ShopItemData data in shopItems)
        {
            if (data.itemPrefab == null) continue;

            GameObject entry = Instantiate(shopItemEntryPrefab, contentParent);
            ShopItemUI entryUI = entry.GetComponent<ShopItemUI>();

            if (entryUI != null)
            {
                Item item = data.itemPrefab.GetComponent<Item>();
                if (item != null)
                {
                    Sprite icon = item.GetComponent<UnityEngine.UI.Image>()?.sprite;

                    entryUI.SetupItem(
                        data.itemPrefab,
                        item.id,
                        item.Name,
                        icon,
                        data.price,
                        data.stock,
                        () => OnBuyItem(data, entryUI)
                    );
                }
            }
        }
    }

    private void OnBuyItem(ShopItemData data, ShopItemUI entryUI)
    {
        if (data.stock <= 0)
        {
            Debug.Log("Hết hàng!");
            return;
        }

        // 🔹 Kiểm tra đủ tiền không
        if (!playerCoinManager.SpendCoins(data.price))
        {
            Debug.Log("Không đủ tiền để mua vật phẩm này!");
            return;
        }

        bool success = inventoryController.AddItem(data.itemPrefab);
        if (success)
        {
            data.stock--;
            entryUI.UpdateStock(data.stock);
            Debug.Log($"Đã mua {data.itemPrefab.name}, còn lại {data.stock}");
        }
        else
        {
            Debug.Log("Inventory đầy, không thể mua!");
            // hoàn lại tiền nếu mua thất bại
            playerCoinManager.AddCoins(data.price);
        }
    }

    public void OpenShop()
    {
        if (shopPanel == null) return;

        _isOpen = true;
        shopPanel.SetActive(true);
        Debug.Log("[ShopController] Shop opened!");
    }

    public void CloseShop()
    {
        if (shopPanel == null) return;

        _isOpen = false;
        shopPanel.SetActive(false);
        Debug.Log("[ShopController] Shop closed!");
    }

    public bool IsOpen() => _isOpen;
}

[System.Serializable]
public class ShopItemData
{
    public GameObject itemPrefab; // Prefab của Item
    public int price = 100;       // Giá bán
    public int stock = 5;         // Số lượng còn lại
}

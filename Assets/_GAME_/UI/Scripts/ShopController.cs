using UnityEngine;

public class ShopController : MonoBehaviour
{
    [Header("Shop Setup")]
    [SerializeField] private GameObject shopItemUIPrefab; // Prefab UI entry (ShopItemEntry)
    [SerializeField] private Transform contentParent;     // Content của ScrollView

    [Header("Items for Sale")]
    [SerializeField] private Item[] itemsForSale; // Prefab item trong shop
    [SerializeField] private int[] itemPrices;    // Giá tương ứng

    [Header("Player Data")]
    public int playerCoins = 100;

    private InventoryController _inventory;

    private void Start()
    {
        _inventory = InventoryController._instance;

        if (_inventory == null)
        {
            Debug.LogError("Không tìm thấy InventoryController trong scene!");
            return;
        }

        //PopulateShop();
    }

    //private void PopulateShop()
    //{
    //    for (int i = 0; i < itemsForSale.Length; i++)
    //    {
    //        GameObject entry = Instantiate(shopItemUIPrefab, contentParent);
    //        ShopItemUI ui = entry.GetComponent<ShopItemUI>();

    //        int price = itemPrices.Length > i ? itemPrices[i] : 10;
    //        ui.Setup(itemsForSale[i], price, this);
    //    }
    //}

    public void BuyItem(Item itemPrefab, int price)
    {
        if (playerCoins < price)
        {
            Debug.Log("Không đủ tiền!");
            return;
        }

        bool added = _inventory.AddItem(itemPrefab.gameObject);
        if (added)
        {
            playerCoins -= price;
            Debug.Log($"Mua {itemPrefab.Name} thành công với giá {price} coins!");
        }
        else
        {
            Debug.Log("Không thể thêm item — kho đã đầy!");
        }
    }
}

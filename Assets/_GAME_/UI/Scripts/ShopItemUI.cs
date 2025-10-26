using System.Collections.Generic;
using NUnit.Framework.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemUI : MonoBehaviour
{
    [Header("References")]
    public Transform _shopListContent;        // nơi chứa danh sách item
    public GameObject _shopEntryPrefab;       // prefab entry trong shop

    [Header("Shop Items")]
    public List<GameObject> _shopItemPrefabs; // danh sách prefab item có sẵn để bán

    [Header("Optional")]
    public Color _canBuyColor = Color.white;
    public Color _cannotBuyColor = Color.red;

    [SerializeField] private int _playerMoney = 500; // ví dụ có thể thay bằng PlayerStats sau này

    private void Start()
    {
        UpdateShopUI();
    }

    public void UpdateShopUI()
    {
        // Dọn entry cũ
        foreach (Transform child in _shopListContent)
        {
            Destroy(child.gameObject);
        }

        // Tạo entry mới cho từng item
        foreach (GameObject itemPrefab in _shopItemPrefabs)
        {
            Item item = itemPrefab.GetComponent<Item>();
            if (item == null)
            {
                Debug.LogWarning($"Item prefab {itemPrefab.name} không có script Item!");
                continue;
            }

            GameObject entry = Instantiate(_shopEntryPrefab, _shopListContent);

            // Gán UI component trong entry prefab
            TMP_Text itemName = entry.transform.Find("ItemName").GetComponent<TMP_Text>();
            TMP_Text itemPrice = entry.transform.Find("ItemPrice").GetComponent<TMP_Text>();
            Image itemIcon = entry.transform.Find("ItemIcon").GetComponent<Image>();
            Button buyButton = entry.transform.Find("BuyButton").GetComponent<Button>();

            // Cập nhật thông tin
            itemName.text = item.Name;
            int price = GetPrice(item);
            itemPrice.text = $"${price}";
            itemIcon.sprite = item.GetComponent<Image>()?.sprite;

            // Màu chữ tùy theo có đủ tiền hay không
            itemPrice.color = (_playerMoney >= price) ? _canBuyColor : _cannotBuyColor;

            // Gắn sự kiện mua
            buyButton.onClick.AddListener(() => BuyItem(itemPrefab, price));
        }
    }

    private int GetPrice(Item item)
    {
        // Giá tạm tính: id * 10 + 50 (hoặc bạn có thể tạo field "price" trong Item)
        return item.id * 10 + 50;
    }

    private void BuyItem(GameObject itemPrefab, int price)
    {
        if (_playerMoney < price)
        {
            Debug.Log("Không đủ tiền để mua vật phẩm này!");
            return;
        }

        bool added = InventoryController._instance.AddItem(itemPrefab);

        if (added)
        {
            _playerMoney -= price;
            Debug.Log($"Mua thành công {itemPrefab.GetComponent<Item>().Name} với giá {price}. Tiền còn lại: {_playerMoney}");
            UpdateShopUI(); // cập nhật lại màu giá
        }
        else
        {
            Debug.Log("Túi đồ đầy, không thể mua thêm!");
        }
    }
}

using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildingManager : MonoBehaviour
{
    public static BuildingManager Instance { get; private set; }

    private bool _isOpen = false;

    [Header("References")]
    [SerializeField] private GameObject buildingPanel;
    [SerializeField] private GameObject buildingEntryPrefab;  // prefab UI entry
    [SerializeField] private Transform contentParent;          // content của ScrollView
    [SerializeField] private InventoryController inventoryController;
    [SerializeField] private ItemDictionary itemDictionary;

    [Header("Building Items")]
    [SerializeField] private List<BuildingItemData> buildingItems = new List<BuildingItemData>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (buildingPanel != null)
            buildingPanel.SetActive(false); // ẩn khi bắt đầu
    }

    private void Start()
    {
        PopulateBuildingList();
    }

    private void PopulateBuildingList()
    {
        // Xóa entry cũ
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }

        // Tạo entry mới
        foreach (BuildingItemData data in buildingItems)
        {
            if (data.buildingItemPrefab == null) continue;

            GameObject entry = Instantiate(buildingEntryPrefab, contentParent);
            BuildingEntryUI entryUI = entry.GetComponent<BuildingEntryUI>();

            if (entryUI != null)
            {
                // ✅ Gọi setup và truyền thêm itemDictionary
                entryUI.SetupBuilding(data, inventoryController, OnBuildBuilding, itemDictionary);
            }
        }
    }


    private void OnBuildBuilding(BuildingItemData data, BuildingEntryUI entryUI)
    {
        if (inventoryController == null)
        {
            Debug.LogWarning("⚠️ InventoryController not assigned!");
            return;
        }

        // Lấy cache item
        var itemCounts = inventoryController._getItemCounts();

        // 1️⃣ Kiểm tra đủ nguyên liệu
        foreach (var req in data.requirements)
        {
            if (!itemCounts.ContainsKey(req.itemId) || itemCounts[req.itemId] < req.quantity)
            {
                Debug.Log($"❌ Không đủ nguyên liệu cho building {data.buildingItemPrefab.name}");
                return;
            }
        }

        // 2️⃣ Trừ nguyên liệu
        foreach (var req in data.requirements)
        {
            inventoryController.RemoveItemsFromInventory(req.itemId, req.quantity);
        }

        // 3️⃣ Thêm building item vào inventory
        bool success = inventoryController.AddItem(data.buildingItemPrefab);
        if (success)
        {
            Debug.Log($"✅ Đã chế tạo {data.buildingItemPrefab.name} thành công!");
        }
        else
        {
            Debug.LogWarning("⚠️ Inventory đầy, không thể thêm building item!");
        }

        inventoryController.RebuidItemCounts();
    }

    public void OpenBuildingPanel()
    {
        _isOpen = true;
        buildingPanel.SetActive(true);
        PauseController.SetPause(true);
        Debug.Log("[BuildingController] Opened!");
    }

    public void CloseBuildingPanel()
    {
        _isOpen = false;
        buildingPanel.SetActive(false);
        PauseController.SetPause(false);
        Debug.Log("[BuildingController] Closed!");
    }

    public bool IsOpen() => _isOpen;
}

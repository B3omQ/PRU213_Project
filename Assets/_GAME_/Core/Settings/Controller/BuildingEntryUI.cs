using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingEntryUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image buildingIcon;
    [SerializeField] private TMP_Text buildingNameText;
    [SerializeField] private TMP_Text requirementText;
    [SerializeField] private Button buildButton;

    private BuildingItemData buildingData;
    private InventoryController inventoryController;
    private System.Action<BuildingItemData, BuildingEntryUI> onBuildAction;

    public void SetupBuilding(
    BuildingItemData data,
    InventoryController inventory,
    System.Action<BuildingItemData, BuildingEntryUI> onBuild,
    ItemDictionary itemDictionary
)
    {
        buildingData = data;
        inventoryController = inventory;
        onBuildAction = onBuild;

        Item item = data.buildingItemPrefab.GetComponent<Item>();
        if (item != null)
        {
            buildingNameText.text = item.Name;
            buildingIcon.sprite = item.GetComponent<Image>()?.sprite;
        }

        requirementText.text = "";
        foreach (var req in data.requirements)
        {
            string itemName = itemDictionary != null
                ? itemDictionary.GetItemName(req.itemId)
                : $"Item ID {req.itemId}";

            requirementText.text += $" Require - {itemName} x{req.quantity}\n";
        }

        buildButton.onClick.RemoveAllListeners();
        buildButton.onClick.AddListener(() => onBuildAction?.Invoke(data, this));
    }


    public void UpdateBuildLimit(int newLimit)
    {
        // Optional: hiển thị số lần còn lại (nếu bạn dùng buildLimit)
    }
}

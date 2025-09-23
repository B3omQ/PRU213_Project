using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    private ItemDictionary _itemDictionary;
    public GameObject _inventoryPanel;
    public GameObject _slotPrefab;
    public int _slotCount;
    public GameObject[] _itemPrefabs;
    void Start()
    {
        _itemDictionary = FindAnyObjectByType<ItemDictionary>();

        for (int i = 0; i < _slotCount; i++)
        {
            Slot slot = Instantiate(_slotPrefab, _inventoryPanel.transform).GetComponent<Slot>();
            if (i < _itemPrefabs.Length)
            {
                GameObject item = Instantiate(_itemPrefabs[i], slot.transform);
                item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                slot._currentItem = item;
            }
        }
    }
    public List<InventorySaveData> GetInventoryItems()
    {
        List<InventorySaveData> invData = new List<InventorySaveData>();
        foreach (Transform slotTranform in _inventoryPanel.transform)
        {
            Slot slot = slotTranform.GetComponent<Slot>();
            if (slot._currentItem != null) 
            {
                Item item = slot._currentItem.GetComponent<Item>();
                invData.Add(new InventorySaveData { _itemId = item.id, _slotIndex = slotTranform.GetSiblingIndex() });
            }
        }
        return invData;
    }

    public void SetInventoryItems(List<InventorySaveData> inventorySaveData)
    {
        foreach (Transform child in _inventoryPanel.transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < _slotCount; i++)
        {
            Instantiate(_slotPrefab, _inventoryPanel.transform);
        }

        foreach(InventorySaveData data in inventorySaveData)
        {
            if (data._slotIndex < _slotCount)
            {
                Slot slot = _inventoryPanel.transform.GetChild(data._slotIndex).GetComponent<Slot>();
                GameObject itemPrefab = _itemDictionary.GetItemPrefab(data._itemId);
                if (itemPrefab != null)
                {
                    GameObject item = Instantiate(itemPrefab, slot.transform);
                    slot._currentItem = item;
                }
            }
        }
    }
}

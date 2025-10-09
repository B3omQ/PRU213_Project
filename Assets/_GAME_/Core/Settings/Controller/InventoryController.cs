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

    public static InventoryController _instance { get; private set; }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
    }
    void Start()
    {
        _itemDictionary = FindAnyObjectByType<ItemDictionary>();

        //for (int i = 0; i < _slotCount; i++)
        //{
        //    Slot slot = Instantiate(_slotPrefab, _inventoryPanel.transform).GetComponent<Slot>();
        //    if (i < _itemPrefabs.Length)
        //    {
        //        GameObject item = Instantiate(_itemPrefabs[i], slot.transform);
        //        item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        //        slot._currentItem = item;
        //    }
        //}
    }

    public bool AddItem(GameObject itemPrefab)
    {
        Item itemToAdd = itemPrefab.GetComponent<Item>();
        if (itemToAdd == null)
        {
            return false;
        }
        //check if item type in inventory ?
        foreach (Transform slotTransform in _inventoryPanel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();
            if (slot != null && slot._currentItem != null)
            {
                Item slotItem = slot._currentItem.GetComponent<Item>();
                if (slotItem != null && slotItem.id == itemToAdd.id)
                {
                    //same item == stack 
                    slotItem.AddToStack();
                    return true;
                }
            }
        }

        //Look for empty slot
        foreach (Transform slotTransform in _inventoryPanel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();
            if (slot != null && slot._currentItem == null)
            {
                GameObject newItem = Instantiate(itemPrefab, slotTransform);
                newItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                slot._currentItem = newItem;
                return true;
            }
        }
        Debug.Log("Inventory is full");
        return false;
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
                invData.Add(new InventorySaveData { _itemId = item.id, _slotIndex = slotTranform.GetSiblingIndex(), _qantity = item.quantity });
            }
        }
        return invData;
    }

    public void EnsureSlots()
    {
        if (_inventoryPanel.transform.childCount < _slotCount)
        {
            // clear tất cả slot cũ (nếu có)
            foreach (Transform child in _inventoryPanel.transform)
            {
                Destroy(child.gameObject);
            }

            // tạo lại slot mới
            for (int i = 0; i < _slotCount; i++)
            {
                Instantiate(_slotPrefab, _inventoryPanel.transform);
            }
        }
    }



    public void SetInventoryItems(List<InventorySaveData> inventorySaveData)
    {
        EnsureSlots(); // luôn chắc chắn có đủ slot

        // clear items trong slot
        foreach (Transform slotTransform in _inventoryPanel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();
            if (slot._currentItem != null)
            {
                Destroy(slot._currentItem);
                slot._currentItem = null;
            }
        }

        // đặt lại item
        foreach (InventorySaveData data in inventorySaveData)
        {
            if (data._slotIndex < _slotCount)
            {
                Slot slot = _inventoryPanel.transform.GetChild(data._slotIndex).GetComponent<Slot>();
                GameObject itemPrefab = _itemDictionary.GetItemPrefab(data._itemId);

                if (itemPrefab != null)
                {
                    GameObject item = Instantiate(itemPrefab, slot.transform);
                    item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

                    Item itemComponent = item.GetComponent<Item>();
                    if(itemComponent != null && data._qantity > 1)
                    {
                        itemComponent.quantity = data._qantity;
                        itemComponent.UpdateQuantityDisplay();
                    }

                    slot._currentItem = item;
                    Debug.Log($"Loaded item {data._itemId} in slot {data._slotIndex}");
                }
                else
                {
                    Debug.LogWarning($"Item id {data._itemId} not found in dictionary");
                }
            }
        }
    }



}

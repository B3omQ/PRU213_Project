using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HotBarController : MonoBehaviour
{

    public GameObject _hotBarPanel;
    public GameObject _slotPrefab;
    public int _slotCount = 8;

    private ItemDictionary _itemDictionary;
    private Key[] _hotbarKey;
    public Transform Player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        _itemDictionary = FindAnyObjectByType<ItemDictionary>();

        _hotbarKey = new Key[_slotCount];
        for (int i = 0; i < _slotCount; i++)
        {
            _hotbarKey[i] = i < 7 ? (Key)((int)Key.Digit1 + i) : Key.Digit0;
        }
        //for (int i = 0; i < _slotCount; i++)
        //{
        //    Slot slot = Instantiate(_slotPrefab, _hotBarPanel.transform).GetComponent<Slot>();
        //}

    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < _slotCount; i++)
        {
            if (Keyboard.current[_hotbarKey[i]].wasPressedThisFrame)
            {
                UseItemInSlot(i);
            }
        }
    }

    void UseItemInSlot(int index)
    {
        Slot slot = _hotBarPanel.transform.GetChild(index).GetComponent<Slot>();

        if (slot._currentItem != null)
        {
            Item item = slot._currentItem.GetComponent<Item>();
            item.UseItem(Player);
        }
    }

    public List<InventorySaveData> GetHotBarItems()
    {
        List<InventorySaveData> hotBarData = new List<InventorySaveData>();
        foreach (Transform slotTranform in _hotBarPanel.transform)
        {
            Slot slot = slotTranform.GetComponent<Slot>();
            if (slot._currentItem != null)
            {
                Item item = slot._currentItem.GetComponent<Item>();
                hotBarData.Add(new InventorySaveData { _itemId = item.id, _slotIndex = slotTranform.GetSiblingIndex() });
            }
        }
        return hotBarData;
    }

    public void EnsureSlotsHotBar()
    {
        if (_hotBarPanel.transform.childCount < _slotCount)
        {
            // clear tất cả slot cũ (nếu có)
            foreach (Transform child in _hotBarPanel.transform)
            {
                Destroy(child.gameObject);
            }

            // tạo lại slot mới
            for (int i = 0; i < _slotCount; i++)
            {
                Instantiate(_slotPrefab, _hotBarPanel.transform);
            }
        }
    }



    public void SetHotBarItems(List<InventorySaveData> inventorySaveData)
    {
        EnsureSlotsHotBar();

        // clear items trong slot
        foreach (Transform slotTransform in _hotBarPanel.transform)
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
                Slot slot = _hotBarPanel.transform.GetChild(data._slotIndex).GetComponent<Slot>();
                GameObject itemPrefab = _itemDictionary.GetItemPrefab(data._itemId);

                if (itemPrefab != null)
                {
                    GameObject item = Instantiate(itemPrefab, slot.transform);
                    item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
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

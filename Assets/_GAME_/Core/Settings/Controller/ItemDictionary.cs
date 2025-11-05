using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class ItemDictionary : MonoBehaviour
{
    public List<Item> _itemPrefab;
    public Dictionary<int, GameObject> _itemDictionary;

    private void Awake()
    {
        _itemDictionary = new Dictionary<int, GameObject>();

        for (int i = 0; i < _itemPrefab.Count; i++)
        {
            if (_itemPrefab[i] != null)
            {
                _itemPrefab[i].id = i + 1;
            }
        }

        foreach (Item item in _itemPrefab)
        {
            _itemDictionary[item.id] = item.gameObject;
        }
    }

    public GameObject GetItemPrefab(int itemId)
    {
        _itemDictionary.TryGetValue(itemId, out GameObject prefab);
        if (prefab == null)
        {
            Debug.LogWarning($"item id: {itemId} not found");
        }
        return prefab;
    }

    public string GetItemName(int itemId)
    {
        if (_itemDictionary.TryGetValue(itemId, out GameObject prefab))
        {
            Item item = prefab.GetComponent<Item>();
            if (item != null)
            {
                return item.Name;
            }
        }

        Debug.LogWarning($"⚠️ Item name for id {itemId} not found.");
        return "Unknown Item";
    }

}

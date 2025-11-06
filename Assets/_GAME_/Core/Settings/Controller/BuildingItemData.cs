using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BuildingRequirement
{
    public int itemId;
    public int quantity;
}

[System.Serializable]
public class BuildingItemData
{
    public GameObject buildingItemPrefab;        // prefab của item building (item dùng được)
    public List<BuildingRequirement> requirements = new List<BuildingRequirement>();
    public int buildLimit = 1;                   // Giới hạn số lần xây (nếu muốn)
}


using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public Vector3 _playerPosition;
    public string _mapBoundary;
    public List<InventorySaveData> _inventorySaveData;
}

using System.Collections.Generic;
using UnityEditor.XR;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public Vector3 _playerPosition;
    public string _mapBoundary;
    public List<InventorySaveData> _inventorySaveData;
    public List<InventorySaveData> _hotBarSaveData;
    public List<ChestSaveData> _chestSaveData;
    public List<Quest.QuestProgress> _questProgressData;
}

[System.Serializable]
public class ChestSaveData
{
    public string _chestId;
    public bool _isOpened;
}
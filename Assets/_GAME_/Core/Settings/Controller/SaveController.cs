using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Unity.Cinemachine;

public class SaveController : MonoBehaviour
{
    private string _saveLocation;
    private InventoryController _inventoryController;
    private HotBarController _HotBarController;

    private IEnumerator Start()
    {
        _saveLocation = Path.Combine(Application.persistentDataPath, "saveData.json");
        Debug.Log("Save file path: " + _saveLocation);

        _inventoryController = FindAnyObjectByType<InventoryController>();
        _HotBarController = FindAnyObjectByType<HotBarController>();

        // wait one frame so InventoryController can finish Start()
        yield return null;

        LoadGame();
    }

    public void SaveGame()
    {
        SaveData saveData = new SaveData
        {
            _playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position,
            _mapBoundary = FindAnyObjectByType<CinemachineConfiner2D>().BoundingShape2D.gameObject.name,
            _inventorySaveData = _inventoryController.GetInventoryItems(),
            _hotBarSaveData = _HotBarController.GetHotBarItems()
            
        };

        string json = JsonUtility.ToJson(saveData, true); // pretty print
        Debug.Log("Saving JSON:\n" + json);

        File.WriteAllText(_saveLocation, json);
    }

    public void LoadGame()
    {
        if (File.Exists(_saveLocation))
        {
            string json = File.ReadAllText(_saveLocation);
            Debug.Log("Loaded JSON:\n" + json);

            SaveData saveData = JsonUtility.FromJson<SaveData>(json);

            Debug.Log("Loaded items count: " + (saveData._inventorySaveData != null ? saveData._inventorySaveData.Count : 0));

            // move player
            GameObject.FindGameObjectWithTag("Player").transform.position = saveData._playerPosition;

            // set confiner boundary
            var confiner = FindAnyObjectByType<CinemachineConfiner2D>();
            var boundaryObj = GameObject.Find(saveData._mapBoundary);
            if (boundaryObj != null)
            {
                confiner.BoundingShape2D = boundaryObj.GetComponent<PolygonCollider2D>();
            }
            else
            {
                Debug.LogWarning("Boundary object not found: " + saveData._mapBoundary);
            }

            // rebuild inventory UI
            _inventoryController.SetInventoryItems(saveData._inventorySaveData);
            _HotBarController.SetHotBarItems(saveData._hotBarSaveData);
        }
        else
        {
            Debug.Log("No save file found. Creating one...");
            SaveGame();
        }
    }
}


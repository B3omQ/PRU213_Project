using System.IO;
using Unity.Cinemachine;
using UnityEditor.Overlays;
using UnityEngine;

public class SaveController : MonoBehaviour
{
    private string _saveLocation;
    private InventoryController _inventoryController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _saveLocation = Path.Combine(Application.persistentDataPath, "saveData.json");
        _inventoryController = FindAnyObjectByType<InventoryController>();
    }

    // Update is called once per frame
    public void SaveGame()
    {
        SaveData saveData = new SaveData
        {
            _playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position,
            _mapBoundary = FindAnyObjectByType<CinemachineConfiner2D>().BoundingShape2D.gameObject.name,
            _inventorySaveData = _inventoryController.GetInventoryItems()
        };

        File.WriteAllText(_saveLocation, JsonUtility.ToJson(saveData));
    }

    public void LoadGame()
    {
        if (File.Exists(_saveLocation))
        {
            SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(_saveLocation));

            GameObject.FindGameObjectWithTag("Player").transform.position = saveData._playerPosition;

            FindAnyObjectByType<CinemachineConfiner2D>().BoundingShape2D = GameObject.Find(saveData._mapBoundary).GetComponent<PolygonCollider2D>();

            _inventoryController.SetInventoryItems(saveData._inventorySaveData);
        }
        else
        {
            SaveGame();
        }
    }
}

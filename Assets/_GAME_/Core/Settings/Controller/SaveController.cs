using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveController : MonoBehaviour
{
    public static SaveController Instance { get; private set; }
    public static bool isLoadGame = false;
    private string _saveLocation;
    private InventoryController _inventoryController;
    private HotBarController _hotBarController;
    private Chest[] _chests;

    private void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        _saveLocation = Path.Combine(Application.persistentDataPath, "saveData.json");
        Debug.Log($"[SaveController] Save file path: {_saveLocation}");

        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }

    /// <summary>
    /// Gọi khi scene gameplay load xong để khởi tạo lại references
    /// </summary>
    public void InitializeGameplayComponents()
    {
        _inventoryController = FindAnyObjectByType<InventoryController>();
        _hotBarController = FindAnyObjectByType<HotBarController>();
        _chests = FindObjectsOfType<Chest>();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Gameplay" && isLoadGame)
        {
            isLoadGame = false;

            Debug.Log("[SaveController] Loading saved game after scene load...");
            SaveData saveData = LoadGameData();
            if (saveData != null)
            {
                // Đảm bảo Inventory, Hotbar, Quest, Player... đã spawn xong
                StartCoroutine(ApplySaveDelayed(saveData));
            }
        }
    }

    private System.Collections.IEnumerator ApplySaveDelayed(SaveData saveData)
    {
        Debug.Log("[SaveController] Waiting for controllers to initialize...");

        // Đợi cho tới khi tìm thấy InventoryController và HotBarController
        while (FindAnyObjectByType<InventoryController>() == null ||
               FindAnyObjectByType<HotBarController>() == null)
        {
            yield return null; // đợi 1 frame
        }

        InitializeGameplayComponents();
        ApplySaveData(saveData);
        Debug.Log("[SaveController] Save data applied successfully!");
    }

    // ---------------- SAVE ----------------
    public void SaveGame()
    {
        Debug.Log("[SaveController] Saving game...");

        var player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogWarning("[SaveController] Player not found, cannot save position!");
            return;
        }

        // get components
        _inventoryController ??= FindAnyObjectByType<InventoryController>();
        _hotBarController ??= FindAnyObjectByType<HotBarController>();
        _chests ??= FindObjectsOfType<Chest>();

        SaveData saveData = new SaveData
        {
            _playerPosition = player.transform.position,
            _mapBoundary = FindAnyObjectByType<CinemachineConfiner2D>()?.BoundingShape2D?.gameObject?.name ?? "DefaultBoundary",
            _inventorySaveData = _inventoryController != null ? _inventoryController.GetInventoryItems() : new List<InventorySaveData>(),
            _hotBarSaveData = _hotBarController != null ? _hotBarController.GetHotBarItems() : new List<InventorySaveData>(),
            _chestSaveData = GetChestsState(),
            _questProgressData = QuestController.Instance.activateQuests
        };

        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(_saveLocation, json);
        Debug.Log("[SaveController] Game saved successfully!");
    }

    // ---------------- LOAD ----------------
    public SaveData LoadGameData()
    {
        if (!File.Exists(_saveLocation))
        {
            Debug.LogWarning("[SaveController] No save file found!");
            return null;
        }

        string json = File.ReadAllText(_saveLocation);
        SaveData saveData = JsonUtility.FromJson<SaveData>(json);
        Debug.Log("[SaveController] Save data loaded!");
        return saveData;
    }

    public void ApplySaveData(SaveData saveData)
    {
        if (saveData == null)
        {
            Debug.LogWarning("[SaveController] No save data to apply!");
            return;
        }

        InitializeGameplayComponents();

        // Move player
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.transform.position = saveData._playerPosition;
        }

        // Set map boundary
        var confiner = FindAnyObjectByType<CinemachineConfiner2D>();
        if (confiner != null)
        {
            var boundaryObj = GameObject.Find(saveData._mapBoundary);
            if (boundaryObj != null)
            {
                confiner.BoundingShape2D = boundaryObj.GetComponent<PolygonCollider2D>();
            }
        }

        // Restore inventory + hotbar
        if (_inventoryController != null)
            _inventoryController.SetInventoryItems(saveData._inventorySaveData);

        if (_hotBarController != null)
            _hotBarController.SetHotBarItems(saveData._hotBarSaveData);

        LoadChestsSaveData(saveData._chestSaveData);

        if (QuestController.Instance != null)
            QuestController.Instance.LoadQuestProgress(saveData._questProgressData);
    }

    // ---------------- CHESTS ----------------
    private List<ChestSaveData> GetChestsState()
    {
        List<ChestSaveData> chestStates = new List<ChestSaveData>();
        if (_chests == null) return chestStates;

        foreach (Chest chest in _chests)
        {
            ChestSaveData chestSaveData = new ChestSaveData
            {
                _chestId = chest._chestId,
                _isOpened = chest._isOpened,
            };
            chestStates.Add(chestSaveData);
        }
        return chestStates;
    }

    private void LoadChestsSaveData(List<ChestSaveData> chestStates)
    {
        if (chestStates == null || _chests == null) return;

        foreach (Chest chest in _chests)
        {
            var chestData = chestStates.FirstOrDefault(c => c._chestId == chest._chestId);
            if (chestData != null)
            {
                chest.SetOpened(chestData._isOpened);
            }
        }
    }

    // ---------------- FILE MGMT ----------------
    public bool HasSaveFile() => File.Exists(_saveLocation);
    public string GetSaveFilePath() => _saveLocation;

    public void NewGame()
    {
        Debug.Log("[SaveController] Creating new save data...");

        SaveData newData = new SaveData
        {
            _playerPosition = Vector3.zero,
            _mapBoundary = "DefaultBoundary",
            _inventorySaveData = new List<InventorySaveData>(),
            _hotBarSaveData = new List<InventorySaveData>(),
            _chestSaveData = new List<ChestSaveData>(),
        };

        string json = JsonUtility.ToJson(newData, true);
        File.WriteAllText(_saveLocation, json);
        Debug.Log("[SaveController] New game data created!");
    }
}

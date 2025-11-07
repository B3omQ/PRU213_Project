using UnityEngine;

using UnityEngine;
using UnityEngine.UI;

public class SettingsMenuController : MonoBehaviour
{
    [SerializeField] private Button saveButton;
    [SerializeField] private Button loadButton;

    private void Start()
    {
        saveButton.onClick.AddListener(OnSaveGame);
        loadButton.onClick.AddListener(OnLoadGame);
    }

    private void OnSaveGame()
    {
        if (SaveController.Instance != null)
        {
            SaveController.Instance.SaveGame();
            Debug.Log("[SettingsMenu] Game saved!");
        }
        else
        {
            Debug.LogWarning("[SettingsMenu] SaveController not found!");
        }
    }

    private void OnLoadGame()
    {
        if (SaveController.Instance != null)
        {
            var data = SaveController.Instance.LoadGameData();
            SaveController.Instance.ApplySaveData(data);
            Debug.Log("[SettingsMenu] Game loaded!");
        }
        else
        {
            Debug.LogWarning("[SettingsMenu] SaveController not found!");
        }
    }
}


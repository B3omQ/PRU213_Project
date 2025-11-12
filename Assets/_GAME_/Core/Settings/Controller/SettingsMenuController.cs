using UnityEngine;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsMenuController : MonoBehaviour
{
    [SerializeField] private GameObject _DeadPanel;
    public void OnSaveGame()
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

    public void OnLoadGame()
    {
        if (SaveController.Instance != null)
        {
            var data = SaveController.Instance.LoadGameData();
            SaveController.Instance.ApplySaveData(data);
            Debug.Log("[SettingsMenu] Game loaded!");
            _DeadPanel.SetActive(false);
            PauseController.SetPause(false);
        }
        else
        {
            Debug.LogWarning("[SettingsMenu] SaveController not found!");
        }
    }

    public void OnNewGame()
    {
        //if (SaveController.Instance != null)
        //{
        //    SaveController.Instance.NewGame();
        //    Debug.Log("[SettingsMenu] New game!");
        //    _DeadPanel.SetActive(false);
        PauseController.SetPause(false);
        //}
        //else
        //{
        //    Debug.LogWarning("[SettingsMenu] SaveController not found!");
        //}

        SceneManager.LoadScene("StartScenes");
    }
}


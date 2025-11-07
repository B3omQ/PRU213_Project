using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuController : MonoBehaviour
{
    public void OnNewGame()
    {
        SaveController.isLoadGame = false;
        SaveController.Instance.NewGame();

        SceneManager.sceneLoaded += OnGameplayLoaded;
        SceneManager.LoadScene("Home");
    }

    public void OnLoadGame()
    {
        if (!SaveController.Instance.HasSaveFile())
        {
            Debug.LogWarning("[StartMenu] No save file found to load!");
            return;
        }

        SaveController.isLoadGame = true;
        SceneManager.sceneLoaded += OnGameplayLoaded;
        SceneManager.LoadScene("Home");
    }

    private void OnGameplayLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Home")
        {
            // Chạy coroutine chờ gameplay scene sẵn sàng
            SaveController.Instance.StartCoroutine(WaitAndApplySave());
        }

        // Hủy đăng ký để không bị gọi lại sau này
        SceneManager.sceneLoaded -= OnGameplayLoaded;
    }

    private IEnumerator WaitAndApplySave()
    {
        Debug.Log("[StartMenu] Waiting for scene initialization...");

        // Chờ vài frame cho tất cả controller load xong
        yield return null;
        yield return null;

        var saveData = SaveController.Instance.LoadGameData();
        if (saveData != null)
        {
            // Dùng coroutine apply dữ liệu an toàn (nếu bạn thêm ApplySaveDelayed như hướng dẫn)
            yield return SaveController.Instance.StartCoroutine(ApplySaveDelayed(saveData));
        }

        Debug.Log("[StartMenu] Save data applied successfully!");
    }

    private IEnumerator ApplySaveDelayed(SaveData saveData)
    {
        // Đợi cho đến khi InventoryController và HotBarController tồn tại
        while (FindAnyObjectByType<InventoryController>() == null ||
               FindAnyObjectByType<HotBarController>() == null)
        {
            yield return null;
        }

        SaveController.Instance.ApplySaveData(saveData);
    }

    public void OnExitClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}

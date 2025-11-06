using UnityEngine;

public class PauseController : MonoBehaviour
{
    public static bool _isGamePaused { get; private set; } = false;

    public static void SetPause(bool pause)
    {
        _isGamePaused = pause;
        Time.timeScale = pause ? 0f : 1f;
    }
}

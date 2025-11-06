using UnityEngine;

public class ExpController : MonoBehaviour
{
    public static ExpController instance;

    public delegate void ExpChangeHandler(int amount);
    public event ExpChangeHandler OnExpChange;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void AddExp(int amount)
    {
        Debug.Log($"Gained {amount} EXP!");
        OnExpChange?.Invoke(amount);
    }
}

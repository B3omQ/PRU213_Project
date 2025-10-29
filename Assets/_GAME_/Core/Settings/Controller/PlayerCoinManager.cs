using TMPro;
using UnityEngine;

public class PlayerCoinManager : MonoBehaviour
{
    [Header("Coin Settings")]
    [SerializeField] private int startingCoins = 500;  // số coin ban đầu

    [Header("UI Reference")]
    [SerializeField] private TMP_Text coinText;         // text hiển thị coin trên UI

    public static PlayerCoinManager Instance { get; private set; }

    private int _currentCoins;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        _currentCoins = startingCoins;
        UpdateUI();
    }

    public int GetCoins() => _currentCoins;

    public void AddCoins(int amount)
    {
        _currentCoins += Mathf.Max(0, amount);
        UpdateUI();
    }

    public bool SpendCoins(int amount)
    {
        if (_currentCoins < amount)
        {
            Debug.Log("❌ Không đủ tiền để mua!");
            return false;
        }

        _currentCoins -= amount;
        UpdateUI();
        return true;
    }

    private void UpdateUI()
    {
        if (coinText != null)
            coinText.text = $"COIN: {_currentCoins}";
    }
}

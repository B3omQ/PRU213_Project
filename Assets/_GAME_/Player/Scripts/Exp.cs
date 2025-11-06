using UnityEngine;

public class Exp : MonoBehaviour
{
    [SerializeField]
    public int _currentExp = 0, _maxExp = 100, _level = 1;
    public int healthIncrease = 1;

    public PlayerHealth _playerHealth;
    private void OnEnable()
    {
        ExpController.instance.OnExpChange += HandleExpChange;
    }

    private void OnDisable()
    {
        ExpController.instance.OnExpChange -= HandleExpChange;
    }

    private void HandleExpChange(int newExp)
    {
        _currentExp += newExp;
        if (_currentExp >= _maxExp)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        _playerHealth.IncreaseMaxHealth(healthIncrease);

        _level++;

        _currentExp = 0;
        _maxExp += 100;

        Debug.Log($"⭐ Level Up! Level: {_level}, Max Health: {_playerHealth._maxHealth}");
    }
}

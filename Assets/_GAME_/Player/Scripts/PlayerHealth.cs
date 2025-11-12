using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    
    public static event Action _OnplayerDamaged;
    public static event Action _OnplayerDeath;
    public static event Action _OnplayerHealed;
    public static event Action _OnPlayerDeath;

    [SerializeField] private GameObject _DeadPanel;
    public float _health, _maxHealth;

    void Start()
    {
        _health = _maxHealth;
        _DeadPanel.SetActive(false);
    }

    public void TakeDamage(float amount)
    {
       _health -= amount;
        _OnplayerDamaged?.Invoke();

        if (_health <= 0)
        {
            Die();
        }

    }

    public void HealToFull()
    {
        _health = _maxHealth;
        _OnplayerHealed?.Invoke();
    }
    public void Die()
    {
        Debug.Log("Player has died!");
        _OnPlayerDeath?.Invoke();
        _DeadPanel.SetActive(true);
        PauseController.SetPause(true);
    }
    public void IncreaseMaxHealth(float amount)
    {
        _maxHealth += amount;
        _health = _maxHealth;
        _OnplayerHealed?.Invoke();
    }
}

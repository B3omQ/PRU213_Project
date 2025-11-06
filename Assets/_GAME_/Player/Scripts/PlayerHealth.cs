using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    
    public static event Action _OnplayerDamaged;
    public static event Action _OnplayerDeath;
    public static event Action _OnplayerHealed;
    public float _health, _maxHealth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _health = _maxHealth;
    }

    // Update is called once per frame
    public void TakeDamage(float amount)
    {
       _health -= amount;
        _OnplayerDamaged?.Invoke();

    }

    public void HealToFull()
    {
        _health = _maxHealth;
        _OnplayerHealed?.Invoke();
    }

    public void IncreaseMaxHealth(float amount)
    {
        _maxHealth += amount;
        _health = _maxHealth;
        _OnplayerHealed?.Invoke();
    }
}

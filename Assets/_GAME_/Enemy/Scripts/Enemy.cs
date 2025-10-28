﻿using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D rigid;
    public Transform player;   
    public float speed = 2f;
    protected float maxHealth = 100f;
    protected float currentHealth;


    protected virtual void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;

        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    protected virtual void Update()
    {
        if (player == null) return;
        MoveTowardPlayer();
    }

    protected virtual void MoveTowardPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        rigid.linearVelocity = direction * speed;
    }

    public virtual void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
            Die();
    }

    protected virtual void Die()
    {
        gameObject.SetActive(false);    
    }
}
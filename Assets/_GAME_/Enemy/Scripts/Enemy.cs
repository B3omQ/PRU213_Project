using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D rigid;
    public Transform player;   
    public float speed = 2f;
    public float maxHealth = 100f;
    public float currentHealth;
    public int expAmount = 100;


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
        ExpController.instance.AddExp(expAmount);
    }
}
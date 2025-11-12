using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D rigid;
    public Transform player;   
    public float speed = 2f;
    public float maxHealth = 100f;
    public float currentHealth;
    public int expAmount = 100;
    public float attackRange = 1.2f;     // khoảng cách có thể tấn công
    public float attackCooldown = 1.0f;  // thời gian chờ giữa 2 đòn
    public float damage = 10f;           // lượng damage gây ra

    private float lastAttackTime = 0f;
    public PlayerHealth _playerHealth;

    protected virtual void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;

        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (_playerHealth == null && player != null)
            _playerHealth = player.GetComponent<PlayerHealth>();
    }

    protected virtual void Update()
    {
        if (player == null) return;
        MoveTowardPlayer();
        float distance = Vector2.Distance(transform.position, player.position);
        if (distance <= attackRange)
        {
            TryAttackPlayer();
        }
    }

    private void TryAttackPlayer()
    {
        // chỉ đánh nếu đã qua thời gian hồi chiêu
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            AttackPlayer();
            lastAttackTime = Time.time;
        }
    }

    protected virtual void AttackPlayer()
    {

        if (_playerHealth != null)
        {
            _playerHealth.TakeDamage(damage);
            Debug.Log("Enemy attack player!");
        }
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
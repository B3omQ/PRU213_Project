using UnityEngine;
using System.Linq;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D rigid;
    public Transform player;

    [Header("Stats")]
    public float speed = 2f;
    public float maxHealth = 100f;
    public float currentHealth;
    public int expAmount = 100;
    public int damage = 10;

    [Header("Ranges")]
    public float attackRange = 1.2f;
    public float attackCooldown = 1.0f;
    public float playerDetectRange = 5f;
    public float plantDetectRange = 5f;
    public float patrolRadius = 4f;
    public float maxChaseDistance = 8f;

    [Header("Knockback Settings")]
    public float knockbackForce = 5f;         // lực đẩy
    public float knockbackDuration = 0.2f;    // thời gian bị đẩy

    private float lastAttackTime = 0f;
    private Transform targetPlant;
    private PlayerHealth _playerHealth;

    private Transform targetItem;
    public float itemDetectRange = 5f;

    private Vector2 spawnPoint;
    private Vector2 patrolTarget;
    private float patrolWaitTime = 2f;
    private float patrolTimer = 0f;

    private bool isKnockedBack = false;
    private float knockbackTimer = 0f;

    private enum State { Patrol, ChasePlayer, ChasePlant, ChaseItem, Return }
    private State currentState = State.Patrol;

    protected virtual void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        spawnPoint = transform.position;

        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (_playerHealth == null && player != null)
            _playerHealth = player.GetComponent<PlayerHealth>();

        SetNewPatrolTarget();
    }

    protected virtual void Update()
    {
        if (isKnockedBack)
        {
            knockbackTimer += Time.deltaTime;
            if (knockbackTimer >= knockbackDuration)
            {
                isKnockedBack = false;
                knockbackTimer = 0f;
            }
            return; // tạm dừng hành động trong lúc knockback
        }

        switch (currentState)
        {
            case State.Patrol:
                PatrolBehavior();
                DetectTargets();
                break;

            case State.ChasePlayer:
                ChasePlayerBehavior();
                break;

            case State.ChasePlant:
                ChasePlantBehavior();
                break;

            case State.Return:
                ReturnToSpawn();
                break;

            case State.ChaseItem:
                ChaseItemBehavior();
                break;
        }
    }

    // -------------------- PATROL --------------------
    private void PatrolBehavior()
    {
        MoveToward(patrolTarget);

        if (Vector2.Distance(transform.position, patrolTarget) < 0.5f)
        {
            rigid.linearVelocity = Vector2.zero;
            patrolTimer += Time.deltaTime;

            if (patrolTimer >= patrolWaitTime)
            {
                SetNewPatrolTarget();
                patrolTimer = 0f;
            }
        }
    }

    private void SetNewPatrolTarget()
    {
        Vector2 randomPoint = spawnPoint + Random.insideUnitCircle * patrolRadius;
        patrolTarget = randomPoint;
    }

    // -------------------- DETECT --------------------
    private void DetectTargets()
    {
        float distToPlayer = Vector2.Distance(transform.position, player.position);
        if (distToPlayer <= playerDetectRange)
        {
            currentState = State.ChasePlayer;
            return;
        }

        GameObject[] plants = GameObject.FindGameObjectsWithTag("Planted");
        if (plants.Length > 0)
        {
            var nearest = plants
                .Select(p => new { obj = p, dist = Vector2.Distance(transform.position, p.transform.position) })
                .Where(x => x.dist <= plantDetectRange)
                .OrderBy(x => x.dist)
                .FirstOrDefault();

            if (nearest != null)
            {
                targetPlant = nearest.obj.transform;
                currentState = State.ChasePlant;
                return;
            }
        }

        GameObject[] items = GameObject.FindGameObjectsWithTag("Item");
        if (items.Length > 0)
        {
            var nearestItem = items
                .Select(i => new { obj = i, dist = Vector2.Distance(transform.position, i.transform.position) })
                .Where(x => x.dist <= itemDetectRange)
                .OrderBy(x => x.dist)
                .FirstOrDefault();

            if (nearestItem != null)
            {
                targetItem = nearestItem.obj.transform;
                currentState = State.ChaseItem;
                return;
            }
        }
    }

    // -------------------- CHASE PLAYER --------------------
    private void ChasePlayerBehavior()
    {
        float dist = Vector2.Distance(transform.position, player.position);
        if (dist > playerDetectRange * 1.5f || Vector2.Distance(transform.position, spawnPoint) > maxChaseDistance)
        {
            currentState = State.Return;
            return;
        }

        MoveToward(player.position);

        if (dist <= attackRange)
            TryAttackPlayer();
    }

    // -------------------- CHASE PLANT --------------------
    private void ChasePlantBehavior()
    {
        if (targetPlant == null)
        {
            currentState = State.Return;
            return;
        }

        float dist = Vector2.Distance(transform.position, targetPlant.position);
        if (dist > plantDetectRange * 1.5f || Vector2.Distance(transform.position, spawnPoint) > maxChaseDistance)
        {
            targetPlant = null;
            currentState = State.Return;
            return;
        }

        MoveToward(targetPlant.position);

        if (dist <= attackRange)
            TryAttackPlant(targetPlant);
    }

    // -------------------- CHASE ITEM --------------------
    private void ChaseItemBehavior()
    {
        if (targetItem == null)
        {
            currentState = State.Return;
            return;
        }

        float dist = Vector2.Distance(transform.position, targetItem.position);
        if (dist > itemDetectRange * 1.5f || Vector2.Distance(transform.position, spawnPoint) > maxChaseDistance)
        {
            targetItem = null;
            currentState = State.Return;
            return;
        }

        MoveToward(targetItem.position);

        if (dist <= 0.5f) // khoảng cách đủ gần để lấy item
        {
            Debug.Log("Enemy collected item!");
            Destroy(targetItem.gameObject);
            targetItem = null;
            currentState = State.Return;
        }
    }


    // -------------------- RETURN --------------------
    private void ReturnToSpawn()
    {
        MoveToward(spawnPoint);

        if (Vector2.Distance(transform.position, spawnPoint) < 0.5f)
        {
            rigid.linearVelocity = Vector2.zero;
            currentState = State.Patrol;
            SetNewPatrolTarget();
        }
    }

    // -------------------- ATTACK --------------------
    private void TryAttackPlayer()
    {
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            _playerHealth?.TakeDamage(damage);
            lastAttackTime = Time.time;
            Debug.Log("Enemy attacks player!");
        }
    }

    private void TryAttackPlant(Transform plant)
    {
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            var ph = plant.GetComponent<Crop>();
            if (ph != null)
            {
                ph.TakeDamage(damage);
                Debug.Log("Enemy attacks plant!");
            }
            lastAttackTime = Time.time;
        }
    }

    // -------------------- MOVE --------------------
    private void MoveToward(Vector2 targetPos)
    {
        Vector2 dir = (targetPos - (Vector2)transform.position).normalized;
        rigid.linearVelocity = dir * speed;
    }

    // -------------------- DAMAGE & KNOCKBACK --------------------
    public virtual void TakeDamage(float dmg, Vector2 attackerPos)
    {
        currentHealth -= dmg;

        // Knockback
        Vector2 knockDir = ((Vector2)transform.position - attackerPos).normalized;
        rigid.linearVelocity = Vector2.zero;
        rigid.AddForce(knockDir * knockbackForce, ForceMode2D.Impulse);

        isKnockedBack = true;
        knockbackTimer = 0f;

        if (currentHealth <= 0)
            Die();
    }

    protected virtual void Die()
    {
        gameObject.SetActive(false);
        ExpController.instance.AddExp(expAmount);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Item"))
        {
            Item item = collision.GetComponent<Item>();
            if (item != null && item.CanBePickedUp())
            {
                Destroy(collision.gameObject);
            }
        }
    }
}

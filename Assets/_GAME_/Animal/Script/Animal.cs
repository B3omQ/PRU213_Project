using UnityEngine;
using System.Linq;

public class Animal : MonoBehaviour
{
    private Rigidbody2D rigid;
    public Transform player;

    [Header("Stats")]
    public float speed = 2f;
    public float maxHealth = 100f;
    public float currentHealth;
    public int expAmount = 50; // Ít exp hơn enemy
    public float fleeSpeedMultiplier = 1.5f; // Chạy nhanh hơn khi sợ

    [Header("Ranges")]
    public float playerDetectRange = 5f; // Phát hiện player
    public float fleeRange = 7f; // Khoảng cách an toàn để dừng chạy
    public float patrolRadius = 4f;
    public float maxWanderDistance = 8f;

    [Header("Loot Settings")]
    public GameObject[] lootPrefabs; // Vật phẩm rơi ra khi chết (meat, fur, etc.)
    public int[] lootAmounts; // Số lượng mỗi loại vật phẩm
    public float lootDropChance = 1f; // Tỷ lệ rơi vật phẩm (1 = 100%)

    [Header("Knockback Settings")]
    public float knockbackForce = 5f;         // lực đẩy
    public float knockbackDuration = 0.2f;    // thời gian bị đẩy

    private Vector2 spawnPoint;
    private Vector2 patrolTarget;
    private float patrolWaitTime = 2f;
    private float patrolTimer = 0f;
    private float idleTime = 0f;
    private float idleDuration = 1f;

    private bool isKnockedBack = false;
    private float knockbackTimer = 0f;
    private bool isFleeing = false;

    private enum State { Patrol, Flee, Return, Idle }
    private State currentState = State.Patrol;

    protected virtual void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        spawnPoint = transform.position;

        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;

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
                DetectPlayer();
                break;

            case State.Flee:
                FleeBehavior();
                break;

            case State.Return:
                ReturnToSpawn();
                break;

            case State.Idle:
                IdleBehavior();
                DetectPlayer();
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
                // Có thể chuyển sang idle hoặc tìm điểm mới
                if (Random.value < 0.3f) // 30% chance để idle
                {
                    currentState = State.Idle;
                    idleTime = 0f;
                }
                else
                {
                    SetNewPatrolTarget();
                }
                patrolTimer = 0f;
            }
        }
    }

    private void SetNewPatrolTarget()
    {
        Vector2 randomPoint = spawnPoint + Random.insideUnitCircle * patrolRadius;
        patrolTarget = randomPoint;
    }

    // -------------------- IDLE --------------------
    private void IdleBehavior()
    {
        rigid.linearVelocity = Vector2.zero;
        idleTime += Time.deltaTime;

        if (idleTime >= idleDuration)
        {
            currentState = State.Patrol;
            SetNewPatrolTarget();
            idleTime = 0f;
        }
    }

    // -------------------- DETECT PLAYER --------------------
    private void DetectPlayer()
    {
        if (player == null) return;

        float distToPlayer = Vector2.Distance(transform.position, player.position);
        if (distToPlayer <= playerDetectRange)
        {
            currentState = State.Flee;
            isFleeing = true;
            return;
        }
    }

    // -------------------- FLEE FROM PLAYER --------------------
    private void FleeBehavior()
    {
        if (player == null)
        {
            currentState = State.Return;
            return;
        }

        float distToPlayer = Vector2.Distance(transform.position, player.position);
        float distFromSpawn = Vector2.Distance(transform.position, spawnPoint);

        // Nếu player xa hoặc đã chạy quá xa spawn point, quay về
        if (distToPlayer > fleeRange || distFromSpawn > maxWanderDistance)
        {
            isFleeing = false;
            currentState = State.Return;
            return;
        }

        // Chạy khỏi player
        Vector2 fleeDirection = ((Vector2)transform.position - (Vector2)player.position).normalized;
        Vector2 fleeTarget = (Vector2)transform.position + fleeDirection * 3f; // Chạy về phía đối diện player
        MoveToward(fleeTarget, speed * fleeSpeedMultiplier);
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

    // -------------------- MOVE --------------------
    private void MoveToward(Vector2 targetPos, float moveSpeed = -1f)
    {
        if (moveSpeed < 0)
            moveSpeed = speed;

        Vector2 dir = (targetPos - (Vector2)transform.position).normalized;
        rigid.linearVelocity = dir * moveSpeed;
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

        // Khi bị tấn công, chuyển sang trạng thái flee
        if (currentHealth > 0 && !isFleeing)
        {
            currentState = State.Flee;
            isFleeing = true;
        }

        if (currentHealth <= 0)
            Die();
    }

    protected virtual void Die()
    {
        // Rơi vật phẩm
        DropLoot();

        // Có thể cho exp nếu cần
        if (expAmount > 0 && ExpController.instance != null)
        {
            ExpController.instance.AddExp(expAmount);
        }

        gameObject.SetActive(false);
    }

    // -------------------- DROP LOOT --------------------
    private void DropLoot()
    {
        if (lootPrefabs == null || lootPrefabs.Length == 0) return;
        if (Random.value > lootDropChance) return; // Không rơi vật phẩm nếu không đủ tỷ lệ

        for (int i = 0; i < lootPrefabs.Length; i++)
        {
            if (lootPrefabs[i] == null) continue;

            int amount = 1;
            if (lootAmounts != null && i < lootAmounts.Length)
                amount = lootAmounts[i];

            for (int j = 0; j < amount; j++)
            {
                // Random vị trí xung quanh animal
                Vector2 dropOffset = Random.insideUnitCircle * 0.5f;
                Vector2 dropPosition = (Vector2)transform.position + dropOffset;

                GameObject loot = Instantiate(lootPrefabs[i], dropPosition, Quaternion.identity);

                // Có thể thêm bounce effect nếu có component
                BounceEffect bounce = loot.GetComponent<BounceEffect>();
                if (bounce != null)
                    bounce.StartBounce();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Animals không nhặt items như enemies
        // Có thể bỏ qua hoặc để trống
    }
}


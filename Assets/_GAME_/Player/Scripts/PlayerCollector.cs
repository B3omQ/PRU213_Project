using UnityEngine;

public class PlayerCollector : MonoBehaviour
{
    private InventoryController _inventoryController;

    [Header("Attack Settings")]
    public Transform attackPoint;     // điểm tấn công (child của Player)
    public float attackRadius = 0.6f; // bán kính vòng tròn tấn công
    public int damage = 1;            // sát thương mỗi hit
    public LayerMask treeLayer;       // chọn layer = "Tree" trong Inspector
    public float attackCooldown = 0.5f;
    float lastAttackTime;


    [Header("Health System")]
    public int maxHealth = 10;
    private int currentHealth;
    void Start()
    {
        _inventoryController = FindAnyObjectByType<InventoryController>();
    }
    void Update() 
    {
        if (InputManager.AttackPressed)
        {
            if (Time.time >= lastAttackTime + attackCooldown)
            {
                Attack();
                lastAttackTime = Time.time;
            }
        }
    }

    void Attack()
    {
        Collider2D[] hitTrees = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, treeLayer);
        Debug.Log($"Hit count: {hitTrees.Length}");

        foreach (Collider2D treeCollider in hitTrees)
        {
            Crop crop = treeCollider.GetComponent<Crop>();
            if (crop != null)
            {
                crop.TakeDamage(damage);
            }
            else
            {
                Debug.Log($"{treeCollider.name} không có Crop (kể cả ở parent)!");
            }
        }

        Debug.Log("Player attacked!");
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Item"))
        {
            Item item = collision.GetComponent<Item>();
            if (item != null)
            {
                bool itemAdded = _inventoryController.AddItem(collision.gameObject);
                if (itemAdded)
                {
                    item.PickUp();
                    Destroy(collision.gameObject);
                }
            }
        }
    }

}

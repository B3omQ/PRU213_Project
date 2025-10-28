using UnityEngine;

public class PlayerCollector : MonoBehaviour
{
    private InventoryController _inventoryController;

    [Header("Attack Settings")]
    public Transform attackPoint;
    public float attackRadius = 0.6f;
    public int damage = 1;
    public LayerMask treeLayer;
    public float attackCooldown = 0.5f;
    float lastAttackTime;

    

    void Start()
    {
        _inventoryController = FindAnyObjectByType<InventoryController>();

        // Nếu chưa gán trong Inspector thì tự lấy trên GameObject
       
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
            if (item != null && item.CanBePickedUp())
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

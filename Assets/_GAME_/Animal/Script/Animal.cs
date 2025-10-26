using UnityEngine;

namespace Game.Animal
{
    public class Animal : MonoBehaviour
    {
        [Header("Animal Properties")]
        public AnimalType animalType;
        public float moveSpeed = 2f;
        public float health = 100f;
        public float maxHealth = 100f;
        
        [Header("Components")]
        private SpriteRenderer spriteRenderer;
        private Animator animator;
        private Rigidbody2D rb;
        
        [Header("Movement")]
        public Vector2 moveDirection;
        public bool isMoving = false;
        
        [Header("Animation States")]
        public bool hasLyingAnimation = true;
        public float lyingThreshold = 0.1f; // Tốc độ dưới ngưỡng này sẽ nằm
        public float lyingDelay = 2f; // Thời gian chờ trước khi nằm
        private float stationaryTime = 0f;
        private bool isLying = false;
        
        [Header("Area Movement")]
        public Transform areaCenter;
        public float areaRadius = 10f;
        public bool stayInArea = true;
        public float changeDirectionInterval = 3f;
        private float lastDirectionChange;
        
        [Header("Drop Items")]
        public GameObject[] dropItems;
        public float[] dropChances = { 1f };
        public int minDropAmount = 1;
        public int maxDropAmount = 3;
        
        [Header("Pooling")]
        public bool isActive = false;
        
        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();
        }
        
        private void Start()
        {
            health = maxHealth;
        }
        
        private void Update()
        {
            if (isActive)
            {
                if (isMoving)
                {
                    Move();
                    CheckAreaBoundary();
                    CheckDirectionChange();
                }
                
                // Kiểm tra animation nằm/xuống
                CheckLyingAnimation();
            }
        }
        
        private void Move()
        {
            if (rb != null)
            {
                rb.linearVelocity = moveDirection * moveSpeed;
            }
            else
            {
                transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
            }
        }
        
        public void SetAnimalType(AnimalType type)
        {
            animalType = type;
            // Có thể thêm logic để thay đổi sprite dựa trên type
        }
        
        public void SetSprite(Sprite sprite)
        {
            if (spriteRenderer != null)
            {
                spriteRenderer.sprite = sprite;
            }
        }
        
        public void SetRandomDirection()
        {
            moveDirection = new Vector2(
                Random.Range(-1f, 1f),
                Random.Range(-1f, 1f)
            ).normalized;
        }
        
        public void TakeDamage(float damage)
        {
            health -= damage;
            if (health <= 0)
            {
                Die();
            }
        }
        
        private void Die()
        {
            // Drop items trước khi chết
            DropItems();
            
            isActive = false;
            // Gọi object pool để return object
            AnimalPool.Instance.ReturnToPool(this);
        }
        
        public void Activate()
        {
            isActive = true;
            gameObject.SetActive(true);
            health = maxHealth;
        }
        
        public void Deactivate()
        {
            isActive = false;
            gameObject.SetActive(false);
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            // Logic xử lý va chạm với player hoặc các object khác
            if (other.CompareTag("Player"))
            {
                // Có thể thêm logic tương tác với player
            }
        }
        
        // Kiểm tra biên giới vùng di chuyển
        private void CheckAreaBoundary()
        {
            if (!stayInArea || areaCenter == null) return;
            
            float distanceFromCenter = Vector2.Distance(transform.position, areaCenter.position);
            
            if (distanceFromCenter > areaRadius)
            {
                // Quay lại hướng về trung tâm
                Vector2 directionToCenter = (areaCenter.position - transform.position).normalized;
                moveDirection = directionToCenter;
            }
        }
        
        // Kiểm tra thay đổi hướng di chuyển
        private void CheckDirectionChange()
        {
            if (Time.time - lastDirectionChange >= changeDirectionInterval)
            {
                SetRandomDirection();
                lastDirectionChange = Time.time;
            }
        }
        
        // Drop items khi chết
        private void DropItems()
        {
            if (dropItems == null || dropItems.Length == 0) return;
            
            int dropAmount = Random.Range(minDropAmount, maxDropAmount + 1);
            
            for (int i = 0; i < dropAmount; i++)
            {
                // Chọn item ngẫu nhiên dựa trên drop chances
                int itemIndex = GetRandomItemIndex();
                if (itemIndex >= 0 && itemIndex < dropItems.Length)
                {
                    GameObject itemToDrop = dropItems[itemIndex];
                    if (itemToDrop != null)
                    {
                        // Tạo item tại vị trí animal
                        Vector3 dropPosition = transform.position + Random.insideUnitSphere * 2f;
                        dropPosition.z = 0; // 2D game
                        
                        GameObject droppedItem = Instantiate(itemToDrop, dropPosition, Quaternion.identity);
                        
                        // Thêm force ngẫu nhiên để item bay ra
                        Rigidbody2D itemRb = droppedItem.GetComponent<Rigidbody2D>();
                        if (itemRb != null)
                        {
                            Vector2 randomForce = Random.insideUnitCircle * 5f;
                            itemRb.AddForce(randomForce, ForceMode2D.Impulse);
                        }
                    }
                }
            }
        }
        
        // Chọn item index dựa trên drop chances
        private int GetRandomItemIndex()
        {
            if (dropChances == null || dropChances.Length == 0) return 0;
            
            float totalChance = 0f;
            foreach (float chance in dropChances)
            {
                totalChance += chance;
            }
            
            float randomValue = Random.Range(0f, totalChance);
            float currentChance = 0f;
            
            for (int i = 0; i < dropChances.Length; i++)
            {
                currentChance += dropChances[i];
                if (randomValue <= currentChance)
                {
                    return i;
                }
            }
            
            return 0; // Fallback
        }
        
        // Setup area movement
        public void SetAreaCenter(Transform center, float radius)
        {
            areaCenter = center;
            areaRadius = radius;
        }
        
        // Setup drop items
        public void SetDropItems(GameObject[] items, float[] chances, int minAmount, int maxAmount)
        {
            dropItems = items;
            dropChances = chances;
            minDropAmount = minAmount;
            maxDropAmount = maxAmount;
        }
        
        // Kiểm tra animation nằm/xuống
        private void CheckLyingAnimation()
        {
            if (!hasLyingAnimation || animator == null) return;
            
            // Kiểm tra tốc độ di chuyển
            float currentSpeed = 0f;
            if (rb != null)
            {
                currentSpeed = rb.linearVelocity.magnitude;
            }
            else
            {
                currentSpeed = moveDirection.magnitude * moveSpeed;
            }
            
            // Nếu di chuyển chậm hơn ngưỡng
            if (currentSpeed < lyingThreshold)
            {
                stationaryTime += Time.deltaTime;
                
                // Nếu đã đứng yên đủ lâu và chưa nằm
                if (stationaryTime >= lyingDelay && !isLying)
                {
                    SetLyingState(true);
                }
            }
            else
            {
                // Nếu đang di chuyển, reset thời gian và đứng dậy
                stationaryTime = 0f;
                if (isLying)
                {
                    SetLyingState(false);
                }
            }
        }
        
        // Đặt trạng thái nằm/xuống
        private void SetLyingState(bool lying)
        {
            isLying = lying;
            
            if (animator != null)
            {
                // Sử dụng Animator parameters
                animator.SetBool("IsLying", lying);
                animator.SetBool("IsMoving", !lying && isMoving);
            }
            else
            {
                // Fallback: thay đổi sprite trực tiếp
                // Có thể thêm logic thay đổi sprite ở đây
                Debug.Log($"Animal {animalType} is now {(lying ? "lying down" : "standing up")}");
            }
        }
        
        // Public methods để control animation
        public void ForceLying()
        {
            if (hasLyingAnimation)
            {
                SetLyingState(true);
            }
        }
        
        public void ForceStanding()
        {
            if (hasLyingAnimation)
            {
                SetLyingState(false);
                stationaryTime = 0f;
            }
        }
        
        public bool IsLying()
        {
            return isLying;
        }
    }
    
    public enum AnimalType
    {
        Cat,
        CatCyclop,
        Chicken,
        Cow,
        Cub,
        Dog,
        Dog2,
        Donkey,
        Fish,
        Frog,
        Horse,
        Lion,
        Lioness,
        Monkey,
        Parrot,
        Pig,
        Racoon
    }
}

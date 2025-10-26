using UnityEngine;

namespace Game.Animal
{
    /// <summary>
    /// Script để quản lý behavior của animals
    /// Hỗ trợ các behavior khác nhau cho từng loại animal
    /// </summary>
    public class AnimalBehavior : MonoBehaviour
    {
        [Header("Behavior Settings")]
        public AnimalType animalType;
        public BehaviorType behaviorType = BehaviorType.Passive;
        public float behaviorRadius = 5f;
        public float behaviorCooldown = 2f;
        
        [Header("Movement")]
        public float moveSpeed = 2f;
        public float rotationSpeed = 90f;
        public bool canMove = true;
        public bool canJump = false;
        public bool canSwim = false;
        
        [Header("Interaction")]
        public bool isFriendly = true;
        public bool isAggressive = false;
        public bool canBeTamed = false;
        public bool canBeFed = false;
        
        [Header("AI Settings")]
        public float detectionRange = 3f;
        public float escapeRange = 5f;
        public LayerMask playerLayer = 1;
        public LayerMask obstacleLayer = 1;
        
        private Animal animal;
        private Transform player;
        private float lastBehaviorTime;
        private Vector3 lastPosition;
        private float stuckTime;
        
        public enum BehaviorType
        {
            Passive,     // Di chuyển ngẫu nhiên
            Aggressive,  // Tấn công player
            Friendly,    // Tiến lại gần player
            Scared,      // Chạy trốn player
            Curious,     // Tiến lại gần nhưng giữ khoảng cách
            Territorial, // Bảo vệ khu vực
            Herd         // Di chuyển theo nhóm
        }
        
        private void Start()
        {
            animal = GetComponent<Animal>();
            if (animal == null)
            {
                Debug.LogWarning("AnimalBehavior requires Animal component!");
                return;
            }
            
            // Tìm player
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
            
            // Setup behavior dựa trên loại animal
            SetupBehaviorForAnimalType();
        }
        
        private void Update()
        {
            if (animal == null || !animal.isActive) return;
            
            UpdateBehavior();
            CheckStuck();
        }
        
        private void SetupBehaviorForAnimalType()
        {
            switch (animalType)
            {
                case AnimalType.Cat:
                    behaviorType = BehaviorType.Curious;
                    isFriendly = true;
                    canBeTamed = true;
                    canBeFed = true;
                    break;
                    
                case AnimalType.Dog:
                case AnimalType.Dog2:
                    behaviorType = BehaviorType.Friendly;
                    isFriendly = true;
                    canBeTamed = true;
                    canBeFed = true;
                    break;
                    
                case AnimalType.Lion:
                case AnimalType.Lioness:
                    behaviorType = BehaviorType.Aggressive;
                    isAggressive = true;
                    canBeTamed = false;
                    canBeFed = false;
                    break;
                    
                case AnimalType.Chicken:
                case AnimalType.Cow:
                case AnimalType.Pig:
                    behaviorType = BehaviorType.Passive;
                    isFriendly = true;
                    canBeTamed = true;
                    canBeFed = true;
                    break;
                    
                case AnimalType.Monkey:
                    behaviorType = BehaviorType.Curious;
                    isFriendly = false;
                    canBeTamed = true;
                    canBeFed = true;
                    break;
                    
                default:
                    behaviorType = BehaviorType.Passive;
                    break;
            }
        }
        
        private void UpdateBehavior()
        {
            if (Time.time - lastBehaviorTime < behaviorCooldown) return;
            
            switch (behaviorType)
            {
                case BehaviorType.Passive:
                    PassiveBehavior();
                    break;
                case BehaviorType.Aggressive:
                    AggressiveBehavior();
                    break;
                case BehaviorType.Friendly:
                    FriendlyBehavior();
                    break;
                case BehaviorType.Scared:
                    ScaredBehavior();
                    break;
                case BehaviorType.Curious:
                    CuriousBehavior();
                    break;
                case BehaviorType.Territorial:
                    TerritorialBehavior();
                    break;
                case BehaviorType.Herd:
                    HerdBehavior();
                    break;
            }
            
            lastBehaviorTime = Time.time;
        }
        
        private void PassiveBehavior()
        {
            if (Random.Range(0f, 1f) < 0.3f) // 30% chance to change direction
            {
                SetRandomDirection();
            }
        }
        
        private void AggressiveBehavior()
        {
            if (player != null && Vector3.Distance(transform.position, player.position) < detectionRange)
            {
                // Move towards player
                Vector3 direction = (player.position - transform.position).normalized;
                animal.moveDirection = new Vector2(direction.x, direction.z);
            }
            else
            {
                SetRandomDirection();
            }
        }
        
        private void FriendlyBehavior()
        {
            if (player != null && Vector3.Distance(transform.position, player.position) < detectionRange)
            {
                // Move towards player but stop at a distance
                Vector3 direction = (player.position - transform.position).normalized;
                float distance = Vector3.Distance(transform.position, player.position);
                
                if (distance > 2f) // Stop at 2 units distance
                {
                    animal.moveDirection = new Vector2(direction.x, direction.z);
                }
                else
                {
                    animal.moveDirection = Vector2.zero;
                }
            }
            else
            {
                SetRandomDirection();
            }
        }
        
        private void ScaredBehavior()
        {
            if (player != null && Vector3.Distance(transform.position, player.position) < detectionRange)
            {
                // Run away from player
                Vector3 direction = (transform.position - player.position).normalized;
                animal.moveDirection = new Vector2(direction.x, direction.z);
            }
            else
            {
                SetRandomDirection();
            }
        }
        
        private void CuriousBehavior()
        {
            if (player != null && Vector3.Distance(transform.position, player.position) < detectionRange)
            {
                // Move towards player but keep distance
                Vector3 direction = (player.position - transform.position).normalized;
                float distance = Vector3.Distance(transform.position, player.position);
                
                if (distance > 3f) // Keep 3 units distance
                {
                    animal.moveDirection = new Vector2(direction.x, direction.z);
                }
                else
                {
                    SetRandomDirection();
                }
            }
            else
            {
                SetRandomDirection();
            }
        }
        
        private void TerritorialBehavior()
        {
            // Stay in a specific area
            Vector3 center = transform.position;
            float distanceFromCenter = Vector3.Distance(transform.position, center);
            
            if (distanceFromCenter > behaviorRadius)
            {
                Vector3 direction = (center - transform.position).normalized;
                animal.moveDirection = new Vector2(direction.x, direction.z);
            }
            else
            {
                SetRandomDirection();
            }
        }
        
        private void HerdBehavior()
        {
            // Find nearby animals of same type
            Animal[] nearbyAnimals = FindObjectsOfType<Animal>();
            Vector3 herdCenter = Vector3.zero;
            int herdCount = 0;
            
            foreach (Animal nearbyAnimal in nearbyAnimals)
            {
                if (nearbyAnimal.animalType == animalType && 
                    Vector3.Distance(transform.position, nearbyAnimal.transform.position) < behaviorRadius)
                {
                    herdCenter += nearbyAnimal.transform.position;
                    herdCount++;
                }
            }
            
            if (herdCount > 0)
            {
                herdCenter /= herdCount;
                Vector3 direction = (herdCenter - transform.position).normalized;
                animal.moveDirection = new Vector2(direction.x, direction.z);
            }
            else
            {
                SetRandomDirection();
            }
        }
        
        private void SetRandomDirection()
        {
            Vector2 randomDirection = new Vector2(
                Random.Range(-1f, 1f),
                Random.Range(-1f, 1f)
            ).normalized;
            
            animal.moveDirection = randomDirection;
        }
        
        private void CheckStuck()
        {
            if (Vector3.Distance(transform.position, lastPosition) < 0.1f)
            {
                stuckTime += Time.deltaTime;
                if (stuckTime > 3f) // Stuck for 3 seconds
                {
                    SetRandomDirection();
                    stuckTime = 0f;
                }
            }
            else
            {
                stuckTime = 0f;
            }
            
            lastPosition = transform.position;
        }
        
        public void SetBehaviorType(BehaviorType newBehaviorType)
        {
            behaviorType = newBehaviorType;
        }
        
        public void SetFriendly(bool friendly)
        {
            isFriendly = friendly;
            isAggressive = !friendly;
        }
        
        public void SetAggressive(bool aggressive)
        {
            isAggressive = aggressive;
            isFriendly = !aggressive;
        }
        
        private void OnDrawGizmosSelected()
        {
            // Draw behavior radius
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, behaviorRadius);
            
            // Draw detection range
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, detectionRange);
            
            // Draw escape range
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, escapeRange);
        }
    }
}

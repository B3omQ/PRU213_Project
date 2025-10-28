using UnityEngine;

namespace Game.Animal
{
    /// <summary>
    /// Script cho drop items từ animals
    /// </summary>
    public class AnimalDropItem : MonoBehaviour
    {
        [Header("Item Properties")]
        public string itemName;
        public ItemType itemType;
        public int value = 1;
        public float lifetime = 30f;
        public bool canBePickedUp = true;
        
        [Header("Visual Effects")]
        public ParticleSystem pickupEffect;
        public AudioClip pickupSound;
        
        [Header("Physics")]
        public float bounceForce = 2f;
        public float friction = 0.95f;
        public bool useGravity = true;
        
        private Rigidbody2D rb;
        private Collider2D itemCollider;
        private SpriteRenderer spriteRenderer;
        private float spawnTime;
        private bool isPickedUp = false;
        
        public enum ItemType
        {
            Food,
            Material,
            Tool,
            Special
        }
        
        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            itemCollider = GetComponent<Collider2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            spawnTime = Time.time;
            
            // Setup physics
            if (rb != null)
            {
                rb.gravityScale = useGravity ? 1f : 0f;
                rb.linearDamping = 1f;
                rb.angularDamping = 2f;
            }
            
            // Setup collider
            if (itemCollider == null)
            {
                itemCollider = gameObject.AddComponent<CircleCollider2D>();
            }
            itemCollider.isTrigger = canBePickedUp;
        }
        
        private void Update()
        {
            // Kiểm tra lifetime
            if (Time.time - spawnTime >= lifetime)
            {
                DestroyItem();
            }
            
            // Apply friction
            if (rb != null && rb.linearVelocity.magnitude > 0.1f)
            {
                rb.linearVelocity *= friction;
            }
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (isPickedUp) return;
            
            if (other.CompareTag("Player"))
            {
                PickupItem(other.gameObject);
            }
        }
        
        private void PickupItem(GameObject picker)
        {
            if (isPickedUp) return;
            
            isPickedUp = true;
            
            // Play pickup effect
            if (pickupEffect != null)
            {
                ParticleSystem effect = Instantiate(pickupEffect, transform.position, Quaternion.identity);
                effect.Play();
                Destroy(effect.gameObject, 2f);
            }
            
            // Play pickup sound
            if (pickupSound != null)
            {
                AudioSource.PlayClipAtPoint(pickupSound, transform.position);
            }
            
            // Add to player inventory (nếu có inventory system)
            AddToInventory(picker);
            
            // Destroy item
            DestroyItem();
        }
        
        private void AddToInventory(GameObject player)
        {
            // Tìm inventory component trên player
            // Có thể thay đổi tên component tùy theo inventory system
            var inventory = player.GetComponent<Inventory>();
            if (inventory != null)
            {
                inventory.AddItem(itemName, value);
            }
            else
            {
                // Fallback: log thông tin item
                Debug.Log($"Player picked up: {itemName} x{value}");
            }
        }
        
        private void DestroyItem()
        {
            // Fade out effect
            if (spriteRenderer != null)
            {
                StartCoroutine(FadeOut());
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        private System.Collections.IEnumerator FadeOut()
        {
            float fadeTime = 1f;
            float elapsedTime = 0f;
            Color originalColor = spriteRenderer.color;
            
            while (elapsedTime < fadeTime)
            {
                elapsedTime += Time.deltaTime;
                float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeTime);
                spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
                yield return null;
            }
            
            Destroy(gameObject);
        }
        
        public void SetItemProperties(string name, ItemType type, int itemValue, float itemLifetime)
        {
            itemName = name;
            itemType = type;
            value = itemValue;
            lifetime = itemLifetime;
        }
        
        public void SetVisualProperties(Sprite sprite, Color color)
        {
            if (spriteRenderer != null)
            {
                spriteRenderer.sprite = sprite;
                spriteRenderer.color = color;
            }
        }
        
        public void SetPhysicsProperties(float bounce, float frictionValue, bool gravity)
        {
            bounceForce = bounce;
            friction = frictionValue;
            useGravity = gravity;
            
            if (rb != null)
            {
                rb.gravityScale = useGravity ? 1f : 0f;
            }
        }
        
        // Tạo item từ prefab
        public static GameObject CreateDropItem(GameObject itemPrefab, Vector3 position, string itemName, ItemType type, int value)
        {
            if (itemPrefab == null) return null;
            
            GameObject item = Instantiate(itemPrefab, position, Quaternion.identity);
            AnimalDropItem dropItem = item.GetComponent<AnimalDropItem>();
            
            if (dropItem != null)
            {
                dropItem.SetItemProperties(itemName, type, value, 30f);
            }
            
            return item;
        }
    }
    
    // Interface cho inventory system
    public interface Inventory
    {
        void AddItem(string itemName, int amount);
        bool HasItem(string itemName);
        int GetItemCount(string itemName);
    }
}

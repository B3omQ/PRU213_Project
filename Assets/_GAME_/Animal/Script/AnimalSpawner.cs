using UnityEngine;
using System.Collections;

namespace Game.Animal
{
    public class AnimalSpawner : MonoBehaviour
    {
        [Header("Spawn Settings")]
        public float spawnInterval = 2f;
        public int maxAnimals = 20;
        public float spawnRadius = 10f;
        public bool spawnOnStart = true;
        public bool continuousSpawning = true;
        
        [Header("Spawn Area")]
        public Transform spawnCenter;
        public LayerMask groundLayer = 1;
        public float minDistanceFromPlayer = 5f;
        
        [Header("Animal Behavior")]
        public bool randomizeAnimalType = true;
        public AnimalType specificAnimalType = AnimalType.Cat;
        
        private int currentAnimalCount = 0;
        private Transform player;
        private Coroutine spawnCoroutine;
        
        private void Start()
        {
            // Tìm player
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
            
            // Set spawn center nếu chưa có
            if (spawnCenter == null)
            {
                spawnCenter = transform;
            }
            
            if (spawnOnStart)
            {
                StartSpawning();
            }
        }
        
        public void StartSpawning()
        {
            if (spawnCoroutine == null)
            {
                spawnCoroutine = StartCoroutine(SpawnAnimalsCoroutine());
            }
        }
        
        public void StopSpawning()
        {
            if (spawnCoroutine != null)
            {
                StopCoroutine(spawnCoroutine);
                spawnCoroutine = null;
            }
        }
        
        private IEnumerator SpawnAnimalsCoroutine()
        {
            while (continuousSpawning)
            {
                if (currentAnimalCount < maxAnimals)
                {
                    SpawnAnimal();
                }
                
                yield return new WaitForSeconds(spawnInterval);
            }
        }
        
        public Animal SpawnAnimal()
        {
            Vector3 spawnPosition = GetRandomSpawnPosition();
            
            Animal animal = null;
            
            if (randomizeAnimalType)
            {
                animal = AnimalPool.Instance.GetRandomAnimal();
            }
            else
            {
                animal = AnimalPool.Instance.GetAnimal(specificAnimalType);
            }
            
            if (animal != null)
            {
                animal.transform.position = spawnPosition;
                animal.SetRandomDirection();
                currentAnimalCount++;
                
                // Subscribe to animal death event để update count
                StartCoroutine(MonitorAnimal(animal));
            }
            
            return animal;
        }
        
        private Vector3 GetRandomSpawnPosition()
        {
            Vector3 spawnPos;
            int attempts = 0;
            int maxAttempts = 10;
            
            do
            {
                // Random position trong spawn radius
                Vector2 randomCircle = Random.insideUnitCircle * spawnRadius;
                spawnPos = spawnCenter.position + new Vector3(randomCircle.x, 0, randomCircle.y);
                
                // Kiểm tra distance từ player
                if (player != null)
                {
                    float distanceFromPlayer = Vector3.Distance(spawnPos, player.position);
                    if (distanceFromPlayer < minDistanceFromPlayer)
                    {
                        // Nếu quá gần player, spawn ở vị trí khác
                        Vector3 directionFromPlayer = (spawnPos - player.position).normalized;
                        spawnPos = player.position + directionFromPlayer * minDistanceFromPlayer;
                    }
                }
                
                attempts++;
            } while (attempts < maxAttempts);
            
            return spawnPos;
        }
        
        private IEnumerator MonitorAnimal(Animal animal)
        {
            while (animal != null && animal.isActive)
            {
                yield return new WaitForSeconds(0.5f);
            }
            
            // Animal đã bị destroy hoặc deactivate
            currentAnimalCount = Mathf.Max(0, currentAnimalCount - 1);
        }
        
        public void SpawnAnimalAtPosition(Vector3 position, AnimalType animalType)
        {
            Animal animal = AnimalPool.Instance.GetAnimal(animalType);
            if (animal != null)
            {
                animal.transform.position = position;
                animal.SetRandomDirection();
                currentAnimalCount++;
                StartCoroutine(MonitorAnimal(animal));
            }
        }
        
        public void ClearAllAnimals()
        {
            // Tìm tất cả animals trong scene và return về pool
            Animal[] allAnimals = FindObjectsOfType<Animal>();
            foreach (Animal animal in allAnimals)
            {
                if (animal.isActive)
                {
                    AnimalPool.Instance.ReturnToPool(animal);
                }
            }
            currentAnimalCount = 0;
        }
        
        private void OnDrawGizmosSelected()
        {
            // Vẽ spawn area trong scene view
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(spawnCenter != null ? spawnCenter.position : transform.position, spawnRadius);
            
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(spawnCenter != null ? spawnCenter.position : transform.position, minDistanceFromPlayer);
        }
        
        private void OnDestroy()
        {
            StopSpawning();
        }
    }
}

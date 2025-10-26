using UnityEngine;
using System.Collections.Generic;

namespace Game.Animal
{
    public class AnimalManager : MonoBehaviour
    {
        public static AnimalManager Instance { get; private set; }
        
        [Header("Animal Settings")]
        public List<AnimalSpawner> spawners = new List<AnimalSpawner>();
        public bool globalSpawningEnabled = true;
        
        [Header("Animal Behavior")]
        public float globalMoveSpeed = 2f;
        public float globalHealth = 100f;
        public bool animalsCanMove = true;
        
        [Header("Debug")]
        public bool showDebugInfo = true;
        
        private List<Animal> activeAnimals = new List<Animal>();
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        private void Start()
        {
            // Tìm tất cả spawners trong scene
            if (spawners.Count == 0)
            {
                AnimalSpawner[] foundSpawners = FindObjectsOfType<AnimalSpawner>();
                spawners.AddRange(foundSpawners);
            }
            
            // Start spawning nếu enabled
            if (globalSpawningEnabled)
            {
                StartAllSpawning();
            }
        }
        
        public void StartAllSpawning()
        {
            foreach (AnimalSpawner spawner in spawners)
            {
                if (spawner != null)
                {
                    spawner.StartSpawning();
                }
            }
        }
        
        public void StopAllSpawning()
        {
            foreach (AnimalSpawner spawner in spawners)
            {
                if (spawner != null)
                {
                    spawner.StopSpawning();
                }
            }
        }
        
        public void ClearAllAnimals()
        {
            foreach (AnimalSpawner spawner in spawners)
            {
                if (spawner != null)
                {
                    spawner.ClearAllAnimals();
                }
            }
            
            // Clear active animals list
            activeAnimals.Clear();
        }
        
        public void RegisterAnimal(Animal animal)
        {
            if (!activeAnimals.Contains(animal))
            {
                activeAnimals.Add(animal);
            }
        }
        
        public void UnregisterAnimal(Animal animal)
        {
            if (activeAnimals.Contains(animal))
            {
                activeAnimals.Remove(animal);
            }
        }
        
        public void SetGlobalMoveSpeed(float speed)
        {
            globalMoveSpeed = speed;
            foreach (Animal animal in activeAnimals)
            {
                if (animal != null)
                {
                    animal.moveSpeed = speed;
                }
            }
        }
        
        public void SetGlobalHealth(float health)
        {
            globalHealth = health;
            foreach (Animal animal in activeAnimals)
            {
                if (animal != null)
                {
                    animal.maxHealth = health;
                    animal.health = health;
                }
            }
        }
        
        public void SetAnimalsCanMove(bool canMove)
        {
            animalsCanMove = canMove;
            foreach (Animal animal in activeAnimals)
            {
                if (animal != null)
                {
                    animal.isMoving = canMove;
                }
            }
        }
        
        public int GetActiveAnimalCount()
        {
            return activeAnimals.Count;
        }
        
        public List<Animal> GetAnimalsByType(AnimalType animalType)
        {
            List<Animal> animalsOfType = new List<Animal>();
            foreach (Animal animal in activeAnimals)
            {
                if (animal != null && animal.animalType == animalType)
                {
                    animalsOfType.Add(animal);
                }
            }
            return animalsOfType;
        }
        
        private void Update()
        {
            // Clean up null references
            activeAnimals.RemoveAll(animal => animal == null);
        }
        
        private void OnGUI()
        {
            if (showDebugInfo)
            {
                GUILayout.BeginArea(new Rect(10, 10, 300, 200));
                GUILayout.Label($"Active Animals: {GetActiveAnimalCount()}");
                GUILayout.Label($"Spawners: {spawners.Count}");
                GUILayout.Label($"Global Spawning: {globalSpawningEnabled}");
                GUILayout.Label($"Animals Can Move: {animalsCanMove}");
                
                if (GUILayout.Button("Start All Spawning"))
                {
                    StartAllSpawning();
                }
                
                if (GUILayout.Button("Stop All Spawning"))
                {
                    StopAllSpawning();
                }
                
                if (GUILayout.Button("Clear All Animals"))
                {
                    ClearAllAnimals();
                }
                
                GUILayout.EndArea();
            }
        }
    }
}

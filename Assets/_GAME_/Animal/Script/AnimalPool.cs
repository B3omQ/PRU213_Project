using System.Collections.Generic;
using UnityEngine;

namespace Game.Animal
{
    public class AnimalPool : MonoBehaviour
    {
        public static AnimalPool Instance { get; private set; }
        
        [Header("Pool Settings")]
        public GameObject animalPrefab;
        public int poolSize = 50;
        public Transform poolParent;
        
        [Header("Animal Types")]
        public List<AnimalData> animalTypes = new List<AnimalData>();
        
        private Dictionary<AnimalType, Queue<Animal>> pools = new Dictionary<AnimalType, Queue<Animal>>();
        private Dictionary<AnimalType, GameObject> prefabs = new Dictionary<AnimalType, GameObject>();
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                InitializePool();
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        private void InitializePool()
        {
            // Tạo pool parent nếu chưa có
            if (poolParent == null)
            {
                GameObject poolParentObj = new GameObject("AnimalPool");
                poolParent = poolParentObj.transform;
                poolParent.SetParent(transform);
            }
            
            // Khởi tạo pool cho mỗi loại animal
            foreach (var animalData in animalTypes)
            {
                if (animalData.prefab != null)
                {
                    prefabs[animalData.animalType] = animalData.prefab;
                    pools[animalData.animalType] = new Queue<Animal>();
                    
                    // Tạo objects cho pool
                    for (int i = 0; i < poolSize; i++)
                    {
                        GameObject animalObj = Instantiate(animalData.prefab, poolParent);
                        Animal animal = animalObj.GetComponent<Animal>();
                        
                        if (animal == null)
                        {
                            animal = animalObj.AddComponent<Animal>();
                        }
                        
                        animal.SetAnimalType(animalData.animalType);
                        animal.Deactivate();
                        pools[animalData.animalType].Enqueue(animal);
                    }
                }
            }
        }
        
        public Animal GetAnimal(AnimalType animalType)
        {
            if (pools.ContainsKey(animalType) && pools[animalType].Count > 0)
            {
                Animal animal = pools[animalType].Dequeue();
                animal.Activate();
                return animal;
            }
            else
            {
                // Nếu pool trống, tạo object mới
                if (prefabs.ContainsKey(animalType))
                {
                    GameObject animalObj = Instantiate(prefabs[animalType], poolParent);
                    Animal animal = animalObj.GetComponent<Animal>();
                    if (animal == null)
                    {
                        animal = animalObj.AddComponent<Animal>();
                    }
                    animal.SetAnimalType(animalType);
                    animal.Activate();
                    return animal;
                }
            }
            
            return null;
        }
        
        public void ReturnToPool(Animal animal)
        {
            if (animal != null)
            {
                animal.Deactivate();
                if (pools.ContainsKey(animal.animalType))
                {
                    pools[animal.animalType].Enqueue(animal);
                }
            }
        }
        
        public Animal GetRandomAnimal()
        {
            if (animalTypes.Count > 0)
            {
                AnimalData randomAnimalData = animalTypes[Random.Range(0, animalTypes.Count)];
                return GetAnimal(randomAnimalData.animalType);
            }
            return null;
        }
        
        public int GetPoolCount(AnimalType animalType)
        {
            if (pools.ContainsKey(animalType))
            {
                return pools[animalType].Count;
            }
            return 0;
        }
    }
    
    [System.Serializable]
    public class AnimalData
    {
        public AnimalType animalType;
        public GameObject prefab;
        public Sprite sprite;
        public float spawnWeight = 1f; // Trọng số spawn (để control tỷ lệ xuất hiện)
    }
}

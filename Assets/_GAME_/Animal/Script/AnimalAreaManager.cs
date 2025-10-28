using UnityEngine;
using System.Collections.Generic;

namespace Game.Animal
{
    /// <summary>
    /// Script để quản lý vùng di chuyển và drop items cho animals
    /// </summary>
    public class AnimalAreaManager : MonoBehaviour
    {
        [Header("Area Settings")]
        public Transform areaCenter;
        public float areaRadius = 10f;
        public bool showAreaGizmos = true;
        public Color areaColor = Color.green;
        
        [Header("Drop Items")]
        public GameObject[] commonItems;
        public GameObject[] rareItems;
        public float[] commonDropChances = { 0.7f, 0.3f };
        public float[] rareDropChances = { 0.3f, 0.7f };
        public int minDropAmount = 1;
        public int maxDropAmount = 3;
        
        [Header("Animal Settings")]
        public float changeDirectionInterval = 3f;
        public bool stayInArea = true;
        
        private List<Animal> animalsInArea = new List<Animal>();
        
        private void Start()
        {
            // Tìm tất cả animals trong scene và setup area
            SetupAnimalsInArea();
        }
        
        private void SetupAnimalsInArea()
        {
            Animal[] allAnimals = FindObjectsOfType<Animal>();
            foreach (Animal animal in allAnimals)
            {
                if (animal != null)
                {
                    SetupAnimalArea(animal);
                    SetupAnimalDropItems(animal);
                    animalsInArea.Add(animal);
                }
            }
        }
        
        private void SetupAnimalArea(Animal animal)
        {
            if (animal != null)
            {
                animal.SetAreaCenter(areaCenter, areaRadius);
                animal.stayInArea = stayInArea;
                animal.changeDirectionInterval = changeDirectionInterval;
            }
        }
        
        private void SetupAnimalDropItems(Animal animal)
        {
            if (animal == null) return;
            
            // Setup drop items dựa trên loại animal
            GameObject[] itemsToDrop;
            float[] dropChances;
            
            switch (animal.animalType)
            {
                case AnimalType.Chicken:
                    itemsToDrop = new GameObject[] { commonItems[0], rareItems[0] };
                    dropChances = new float[] { 0.8f, 0.2f };
                    break;
                    
                case AnimalType.Cow:
                    itemsToDrop = new GameObject[] { commonItems[1], rareItems[1] };
                    dropChances = new float[] { 0.7f, 0.3f };
                    break;
                    
                case AnimalType.Pig:
                    itemsToDrop = new GameObject[] { commonItems[2], rareItems[2] };
                    dropChances = new float[] { 0.6f, 0.4f };
                    break;
                    
                case AnimalType.Lion:
                    itemsToDrop = new GameObject[] { rareItems[0], rareItems[1] };
                    dropChances = new float[] { 0.5f, 0.5f };
                    break;
                    
                default:
                    itemsToDrop = commonItems;
                    dropChances = commonDropChances;
                    break;
            }
            
            animal.SetDropItems(itemsToDrop, dropChances, minDropAmount, maxDropAmount);
        }
        
        public void AddAnimalToArea(Animal animal)
        {
            if (animal != null && !animalsInArea.Contains(animal))
            {
                SetupAnimalArea(animal);
                SetupAnimalDropItems(animal);
                animalsInArea.Add(animal);
            }
        }
        
        public void RemoveAnimalFromArea(Animal animal)
        {
            if (animalsInArea.Contains(animal))
            {
                animalsInArea.Remove(animal);
            }
        }
        
        public void UpdateAreaSettings(float newRadius, bool newStayInArea)
        {
            areaRadius = newRadius;
            stayInArea = newStayInArea;
            
            // Cập nhật settings cho tất cả animals
            foreach (Animal animal in animalsInArea)
            {
                if (animal != null)
                {
                    animal.SetAreaCenter(areaCenter, areaRadius);
                    animal.stayInArea = stayInArea;
                }
            }
        }
        
        public void UpdateDropSettings(GameObject[] newCommonItems, GameObject[] newRareItems)
        {
            commonItems = newCommonItems;
            rareItems = newRareItems;
            
            // Cập nhật drop items cho tất cả animals
            foreach (Animal animal in animalsInArea)
            {
                if (animal != null)
                {
                    SetupAnimalDropItems(animal);
                }
            }
        }
        
        public int GetAnimalsInAreaCount()
        {
            return animalsInArea.Count;
        }
        
        public List<Animal> GetAnimalsInArea()
        {
            return new List<Animal>(animalsInArea);
        }
        
        public bool IsPositionInArea(Vector3 position)
        {
            if (areaCenter == null) return false;
            
            float distance = Vector2.Distance(position, areaCenter.position);
            return distance <= areaRadius;
        }
        
        public Vector3 GetRandomPositionInArea()
        {
            if (areaCenter == null) return Vector3.zero;
            
            Vector2 randomCircle = Random.insideUnitCircle * areaRadius;
            return areaCenter.position + new Vector3(randomCircle.x, randomCircle.y, 0);
        }
        
        private void OnDrawGizmos()
        {
            if (showAreaGizmos && areaCenter != null)
            {
                Gizmos.color = areaColor;
                Gizmos.DrawWireSphere(areaCenter.position, areaRadius);
                
                // Vẽ trung tâm
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(areaCenter.position, 0.5f);
            }
        }
        
        private void OnDrawGizmosSelected()
        {
            if (areaCenter != null)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(areaCenter.position, areaRadius);
                
                // Vẽ grid trong area
                Gizmos.color = Color.white;
                for (int i = 0; i < 10; i++)
                {
                    float angle = i * 36f * Mathf.Deg2Rad;
                    Vector3 start = areaCenter.position + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * areaRadius;
                    Vector3 end = areaCenter.position + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * (areaRadius * 0.5f);
                    Gizmos.DrawLine(start, end);
                }
            }
        }
    }
}

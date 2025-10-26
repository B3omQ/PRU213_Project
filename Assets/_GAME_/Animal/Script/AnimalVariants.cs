using UnityEngine;

namespace Game.Animal
{
    /// <summary>
    /// Script để quản lý các biến thể của cùng một loại animal
    /// Ví dụ: Chicken có 4 màu, Lion có 4 màu, etc.
    /// </summary>
    [System.Serializable]
    public class AnimalVariant
    {
        public string variantName;
        public Sprite sprite;
        public Color tintColor = Color.white;
        public float spawnWeight = 1f;
        public bool isRare = false;
    }
    
    public class AnimalVariants : MonoBehaviour
    {
        [Header("Animal Variants")]
        public AnimalType animalType;
        public AnimalVariant[] variants;
        
        [Header("Variant Settings")]
        public bool randomizeOnSpawn = true;
        public bool useWeightedRandom = true;
        
        private void Start()
        {
            // Validate variants
            if (variants == null || variants.Length == 0)
            {
                Debug.LogWarning($"No variants found for {animalType}");
            }
        }
        
        public AnimalVariant GetRandomVariant()
        {
            if (variants == null || variants.Length == 0)
            {
                return null;
            }
            
            if (variants.Length == 1)
            {
                return variants[0];
            }
            
            if (useWeightedRandom)
            {
                return GetWeightedRandomVariant();
            }
            else
            {
                return variants[Random.Range(0, variants.Length)];
            }
        }
        
        private AnimalVariant GetWeightedRandomVariant()
        {
            float totalWeight = 0f;
            foreach (var variant in variants)
            {
                totalWeight += variant.spawnWeight;
            }
            
            float randomValue = Random.Range(0f, totalWeight);
            float currentWeight = 0f;
            
            foreach (var variant in variants)
            {
                currentWeight += variant.spawnWeight;
                if (randomValue <= currentWeight)
                {
                    return variant;
                }
            }
            
            return variants[variants.Length - 1]; // Fallback
        }
        
        public AnimalVariant GetVariantByName(string variantName)
        {
            foreach (var variant in variants)
            {
                if (variant.variantName == variantName)
                {
                    return variant;
                }
            }
            return null;
        }
        
        public AnimalVariant GetRareVariant()
        {
            foreach (var variant in variants)
            {
                if (variant.isRare)
                {
                    return variant;
                }
            }
            return null;
        }
        
        public int GetVariantCount()
        {
            return variants != null ? variants.Length : 0;
        }
        
        public string[] GetVariantNames()
        {
            if (variants == null) return new string[0];
            
            string[] names = new string[variants.Length];
            for (int i = 0; i < variants.Length; i++)
            {
                names[i] = variants[i].variantName;
            }
            return names;
        }
    }
}

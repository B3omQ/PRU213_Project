using UnityEngine;
using System.Collections.Generic;

namespace Game.Animal
{
    /// <summary>
    /// Script để cấu hình spawn cho từng loại animal
    /// Hỗ trợ spawn theo thời gian, khu vực, và điều kiện
    /// </summary>
    [System.Serializable]
    public class AnimalSpawnConfig
    {
        [Header("Basic Settings")]
        public AnimalType animalType;
        public float spawnWeight = 1f;
        public int maxCount = 10;
        public bool canSpawn = true;
        
        [Header("Spawn Conditions")]
        public float minSpawnTime = 0f;
        public float maxSpawnTime = 60f;
        public bool spawnAtNight = true;
        public bool spawnAtDay = true;
        public bool spawnInRain = true;
        public bool spawnInSun = true;
        
        [Header("Spawn Area")]
        public Vector2 spawnArea = new Vector2(10f, 10f);
        public bool useCustomSpawnArea = false;
        public LayerMask allowedSpawnLayers = -1;
        
        [Header("Behavior")]
        public float moveSpeed = 2f;
        public float health = 100f;
        public bool isAggressive = false;
        public bool isFriendly = true;
    }
    
    public class AnimalSpawnConfigManager : MonoBehaviour
    {
        [Header("Spawn Configurations")]
        public List<AnimalSpawnConfig> spawnConfigs = new List<AnimalSpawnConfig>();
        
        [Header("Global Settings")]
        public bool useTimeOfDay = true;
        public bool useWeather = false;
        public float globalSpawnMultiplier = 1f;
        
        private void Start()
        {
            InitializeConfigs();
        }
        
        private void InitializeConfigs()
        {
            // Tạo config mặc định cho các loại animals
            if (spawnConfigs.Count == 0)
            {
                CreateDefaultConfigs();
            }
        }
        
        private void CreateDefaultConfigs()
        {
            // Farm Animals - Spawn nhiều vào ban ngày
            AddConfig(AnimalType.Chicken, 3f, 15, true, 0f, 30f, false, true, true, true);
            AddConfig(AnimalType.Cow, 1f, 5, true, 0f, 60f, false, true, true, true);
            AddConfig(AnimalType.Pig, 2f, 8, true, 0f, 45f, false, true, true, true);
            
            // Pets - Spawn ít hơn, cả ngày và đêm
            AddConfig(AnimalType.Cat, 1.5f, 6, true, 0f, 90f, true, true, true, true);
            AddConfig(AnimalType.Dog, 1f, 4, true, 0f, 120f, true, true, true, true);
            AddConfig(AnimalType.Cub, 0.5f, 2, true, 0f, 180f, true, true, true, true);
            
            // Wild Animals - Spawn ít, chủ yếu ban đêm
            AddConfig(AnimalType.Lion, 0.3f, 2, true, 0f, 300f, true, false, true, true);
            AddConfig(AnimalType.Monkey, 0.8f, 4, true, 0f, 150f, true, true, true, true);
            AddConfig(AnimalType.Racoon, 1f, 5, true, 0f, 120f, true, true, true, true);
        }
        
        private void AddConfig(AnimalType type, float weight, int maxCount, bool canSpawn, 
            float minTime, float maxTime, bool night, bool day, bool rain, bool sun)
        {
            AnimalSpawnConfig config = new AnimalSpawnConfig
            {
                animalType = type,
                spawnWeight = weight,
                maxCount = maxCount,
                canSpawn = canSpawn,
                minSpawnTime = minTime,
                maxSpawnTime = maxTime,
                spawnAtNight = night,
                spawnAtDay = day,
                spawnInRain = rain,
                spawnInSun = sun
            };
            
            spawnConfigs.Add(config);
        }
        
        public AnimalSpawnConfig GetConfig(AnimalType animalType)
        {
            foreach (var config in spawnConfigs)
            {
                if (config.animalType == animalType)
                {
                    return config;
                }
            }
            return null;
        }
        
        public bool CanSpawn(AnimalType animalType)
        {
            AnimalSpawnConfig config = GetConfig(animalType);
            if (config == null) return false;
            
            if (!config.canSpawn) return false;
            
            // Kiểm tra thời gian
            if (useTimeOfDay)
            {
                float currentTime = Time.time;
                if (currentTime < config.minSpawnTime || currentTime > config.maxSpawnTime)
                {
                    return false;
                }
            }
            
            // Kiểm tra thời tiết (nếu có)
            if (useWeather)
            {
                // Logic kiểm tra thời tiết
                // Có thể tích hợp với weather system
            }
            
            return true;
        }
        
        public float GetSpawnWeight(AnimalType animalType)
        {
            AnimalSpawnConfig config = GetConfig(animalType);
            if (config == null) return 0f;
            
            return config.spawnWeight * globalSpawnMultiplier;
        }
        
        public int GetMaxCount(AnimalType animalType)
        {
            AnimalSpawnConfig config = GetConfig(animalType);
            if (config == null) return 0;
            
            return config.maxCount;
        }
        
        public void UpdateConfig(AnimalType animalType, float newWeight, int newMaxCount)
        {
            AnimalSpawnConfig config = GetConfig(animalType);
            if (config != null)
            {
                config.spawnWeight = newWeight;
                config.maxCount = newMaxCount;
            }
        }
        
        public void EnableAnimalType(AnimalType animalType, bool enable)
        {
            AnimalSpawnConfig config = GetConfig(animalType);
            if (config != null)
            {
                config.canSpawn = enable;
            }
        }
        
        public List<AnimalType> GetEnabledAnimalTypes()
        {
            List<AnimalType> enabledTypes = new List<AnimalType>();
            foreach (var config in spawnConfigs)
            {
                if (config.canSpawn)
                {
                    enabledTypes.Add(config.animalType);
                }
            }
            return enabledTypes;
        }
    }
}

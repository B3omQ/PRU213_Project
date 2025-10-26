using UnityEngine;

namespace Game.Animal
{
    /// <summary>
    /// Script để quản lý tương tác giữa player và animals
    /// Hỗ trợ feeding, taming, và các tương tác khác
    /// </summary>
    public class AnimalInteraction : MonoBehaviour
    {
        [Header("Interaction Settings")]
        public bool canBeFed = false;
        public bool canBeTamed = false;
        public bool canBePetted = false;
        public bool canBeRidden = false;
        
        [Header("Feeding")]
        public string[] favoriteFoods;
        public float hungerLevel = 100f;
        public float maxHunger = 100f;
        public float hungerDecayRate = 1f;
        public bool isHungry = false;
        
        [Header("Taming")]
        public float tamingProgress = 0f;
        public float maxTamingProgress = 100f;
        public bool isTamed = false;
        public float tamingDecayRate = 0.5f;
        
        [Header("Mood")]
        public float moodLevel = 50f;
        public float maxMood = 100f;
        public bool isHappy = false;
        public float moodDecayRate = 0.2f;
        
        [Header("Effects")]
        public ParticleSystem happyEffect;
        public ParticleSystem hungryEffect;
        public AudioClip happySound;
        public AudioClip hungrySound;
        
        private Animal animal;
        private AnimalBehavior behavior;
        private AudioSource audioSource;
        private float lastInteractionTime;
        
        private void Start()
        {
            animal = GetComponent<Animal>();
            behavior = GetComponent<AnimalBehavior>();
            audioSource = GetComponent<AudioSource>();
            
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
            
            // Setup interaction dựa trên loại animal
            SetupInteractionForAnimalType();
        }
        
        private void Update()
        {
            UpdateHunger();
            UpdateTaming();
            UpdateMood();
            UpdateEffects();
        }
        
        private void SetupInteractionForAnimalType()
        {
            switch (animal.animalType)
            {
                case AnimalType.Cat:
                    canBeFed = true;
                    canBeTamed = true;
                    canBePetted = true;
                    favoriteFoods = new string[] { "Fish", "Meat" };
                    break;
                    
                case AnimalType.Dog:
                case AnimalType.Dog2:
                    canBeFed = true;
                    canBeTamed = true;
                    canBePetted = true;
                    canBeRidden = false;
                    favoriteFoods = new string[] { "Bone", "Meat" };
                    break;
                    
                case AnimalType.Chicken:
                    canBeFed = true;
                    canBeTamed = true;
                    canBePetted = false;
                    favoriteFoods = new string[] { "Seeds", "Corn" };
                    break;
                    
                case AnimalType.Cow:
                    canBeFed = true;
                    canBeTamed = true;
                    canBePetted = true;
                    favoriteFoods = new string[] { "Grass", "Hay" };
                    break;
                    
                case AnimalType.Pig:
                    canBeFed = true;
                    canBeTamed = true;
                    canBePetted = false;
                    favoriteFoods = new string[] { "Vegetables", "Fruits" };
                    break;
                    
                case AnimalType.Horse:
                    canBeFed = true;
                    canBeTamed = true;
                    canBePetted = true;
                    canBeRidden = true;
                    favoriteFoods = new string[] { "Carrots", "Apples" };
                    break;
                    
                case AnimalType.Lion:
                case AnimalType.Lioness:
                    canBeFed = false;
                    canBeTamed = false;
                    canBePetted = false;
                    break;
                    
                default:
                    canBeFed = false;
                    canBeTamed = false;
                    canBePetted = false;
                    break;
            }
        }
        
        private void UpdateHunger()
        {
            if (canBeFed)
            {
                hungerLevel -= hungerDecayRate * Time.deltaTime;
                hungerLevel = Mathf.Clamp(hungerLevel, 0f, maxHunger);
                
                isHungry = hungerLevel < 30f;
            }
        }
        
        private void UpdateTaming()
        {
            if (canBeTamed && !isTamed)
            {
                tamingProgress -= tamingDecayRate * Time.deltaTime;
                tamingProgress = Mathf.Clamp(tamingProgress, 0f, maxTamingProgress);
                
                if (tamingProgress >= maxTamingProgress)
                {
                    isTamed = true;
                    OnTamed();
                }
            }
        }
        
        private void UpdateMood()
        {
            moodLevel -= moodDecayRate * Time.deltaTime;
            moodLevel = Mathf.Clamp(moodLevel, 0f, maxMood);
            
            isHappy = moodLevel > 70f;
        }
        
        private void UpdateEffects()
        {
            if (isHappy && happyEffect != null && !happyEffect.isPlaying)
            {
                happyEffect.Play();
            }
            else if (!isHappy && happyEffect != null && happyEffect.isPlaying)
            {
                happyEffect.Stop();
            }
            
            if (isHungry && hungryEffect != null && !hungryEffect.isPlaying)
            {
                hungryEffect.Play();
            }
            else if (!isHungry && hungryEffect != null && hungryEffect.isPlaying)
            {
                hungryEffect.Stop();
            }
        }
        
        public bool TryFeed(string foodType)
        {
            if (!canBeFed) return false;
            
            bool isFavorite = false;
            foreach (string favorite in favoriteFoods)
            {
                if (favorite == foodType)
                {
                    isFavorite = true;
                    break;
                }
            }
            
            float hungerGain = isFavorite ? 30f : 15f;
            hungerLevel += hungerGain;
            hungerLevel = Mathf.Clamp(hungerLevel, 0f, maxHunger);
            
            // Increase mood
            moodLevel += isFavorite ? 20f : 10f;
            moodLevel = Mathf.Clamp(moodLevel, 0f, maxMood);
            
            // Increase taming progress
            if (canBeTamed && !isTamed)
            {
                tamingProgress += isFavorite ? 15f : 8f;
                tamingProgress = Mathf.Clamp(tamingProgress, 0f, maxTamingProgress);
            }
            
            PlaySound(happySound);
            OnFed(foodType, isFavorite);
            
            return true;
        }
        
        public bool TryPet()
        {
            if (!canBePetted) return false;
            
            moodLevel += 15f;
            moodLevel = Mathf.Clamp(moodLevel, 0f, maxMood);
            
            if (canBeTamed && !isTamed)
            {
                tamingProgress += 5f;
                tamingProgress = Mathf.Clamp(tamingProgress, 0f, maxTamingProgress);
            }
            
            PlaySound(happySound);
            OnPetted();
            
            return true;
        }
        
        public bool TryRide()
        {
            if (!canBeRidden) return false;
            
            // Logic for riding
            OnRidden();
            return true;
        }
        
        private void PlaySound(AudioClip clip)
        {
            if (audioSource != null && clip != null)
            {
                audioSource.PlayOneShot(clip);
            }
        }
        
        private void OnFed(string foodType, bool isFavorite)
        {
            Debug.Log($"{animal.animalType} was fed {foodType} (Favorite: {isFavorite})");
            lastInteractionTime = Time.time;
        }
        
        private void OnPetted()
        {
            Debug.Log($"{animal.animalType} was petted and is happy!");
            lastInteractionTime = Time.time;
        }
        
        private void OnRidden()
        {
            Debug.Log($"{animal.animalType} is being ridden!");
            lastInteractionTime = Time.time;
        }
        
        private void OnTamed()
        {
            Debug.Log($"{animal.animalType} has been tamed!");
            
            // Change behavior to friendly
            if (behavior != null)
            {
                behavior.SetFriendly(true);
            }
        }
        
        public float GetInteractionDistance()
        {
            return 2f; // Default interaction distance
        }
        
        public bool CanInteract()
        {
            return Time.time - lastInteractionTime > 1f; // 1 second cooldown
        }
        
        public string GetInteractionText()
        {
            if (isHungry && canBeFed)
            {
                return "Feed me!";
            }
            else if (canBePetted && isHappy)
            {
                return "Pet me!";
            }
            else if (canBeRidden && isTamed)
            {
                return "Ride me!";
            }
            else if (canBeTamed && !isTamed)
            {
                return $"Taming: {tamingProgress:F0}%";
            }
            else
            {
                return "Hello!";
            }
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                // Show interaction UI
                ShowInteractionUI();
            }
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                // Hide interaction UI
                HideInteractionUI();
            }
        }
        
        private void ShowInteractionUI()
        {
            // Logic to show interaction UI
            // This would typically involve a UI manager
        }
        
        private void HideInteractionUI()
        {
            // Logic to hide interaction UI
        }
    }
}

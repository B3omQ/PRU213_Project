using UnityEngine;

public class AnimalHealthUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animal animal;
    [SerializeField] private Transform fullHealthBar; // object thanh đỏ
    [SerializeField] private Transform emptyHealthBar;

    private Vector3 originalScale;

    private void Start()
    {
        if (animal == null)
            animal = GetComponentInParent<Animal>();

        if (fullHealthBar == null)
            Debug.LogWarning("Full health bar not assigned!");

        originalScale = fullHealthBar.localScale;
    }

    private void Update()
    {
        if (animal == null || fullHealthBar == null) return;

        float percent = Mathf.Clamp01(animal.currentHealth / animal.maxHealth);

        // Giữ nguyên hướng, chỉ scale theo trục X
        fullHealthBar.localScale = new Vector3(originalScale.x * percent, originalScale.y, originalScale.z);
    }
}


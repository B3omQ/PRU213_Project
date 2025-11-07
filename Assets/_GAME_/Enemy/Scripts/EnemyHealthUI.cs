using UnityEngine;

public class EnemyHealthUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Enemy enemy;
    [SerializeField] private Transform fullHealthBar; // object thanh đỏ
    [SerializeField] private Transform emptyHealthBar;

    private Vector3 originalScale;

    private void Start()
    {
        if (enemy == null)
            enemy = GetComponentInParent<Enemy>();

        if (fullHealthBar == null)
            Debug.LogWarning("Full health bar not assigned!");

        originalScale = fullHealthBar.localScale;
    }

    private void Update()
    {
        if (enemy == null || fullHealthBar == null) return;

        float percent = Mathf.Clamp01(enemy.currentHealth / enemy.maxHealth);

        // Giữ nguyên hướng, chỉ scale theo trục X
        fullHealthBar.localScale = new Vector3(originalScale.x * percent, originalScale.y, originalScale.z);
    }
}


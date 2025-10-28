using System.Collections;
using UnityEngine;

public class Crop : MonoBehaviour
{
    public int maxHealth = 1;
    int currentHealth;

    [Header("Drops")]
    public GameObject[] dropPrefabs; 
    public int minDrops = 1;
    public int maxDrops = 3;
    public float dropForce = 4f; 
    public float dropRadius = 0.5f;

    //[Header("Stump / Visuals")]
    //public GameObject stumpPrefab; // (tùy) spawn gốc cây sau khi chặt

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int dmg)
    {
        currentHealth -= dmg;
        Debug.Log($"Tree hit: {currentHealth}/{maxHealth}");
        if (currentHealth <= 0)
        {
            StartCoroutine(FallDown());
        }
    }

    IEnumerator FallDown()
    {
        Collider2D col = GetComponent<Collider2D>();
        if (col) col.enabled = false;

        yield return new WaitForSeconds(0.25f);

        SpawnDrops();

        //if (stumpPrefab)
        //{
        //    Instantiate(stumpPrefab, transform.position, Quaternion.identity);
        //}

        Destroy(gameObject);
    }

    void SpawnDrops()
    {
        if (dropPrefabs == null || dropPrefabs.Length == 0) return;

        int count = Random.Range(minDrops, maxDrops + 1);
        for (int i = 0; i < count; i++)
        {
            // chọn loại drop ngẫu nhiên
            GameObject prefab = dropPrefabs[Random.Range(0, dropPrefabs.Length)];
            Vector2 spawnPos = (Vector2)transform.position + Random.insideUnitCircle * dropRadius;
            GameObject go = Instantiate(prefab, spawnPos, Quaternion.identity);

            // apply random force nếu có Rigidbody2D
            Rigidbody2D rb = go.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 force = new Vector2(Random.Range(-1f, 1f), Random.Range(0.8f, 1.8f)).normalized * dropForce;
                rb.AddForce(force, ForceMode2D.Impulse);
                rb.AddTorque(Random.Range(-2f, 2f));
            }
        }
    }
}

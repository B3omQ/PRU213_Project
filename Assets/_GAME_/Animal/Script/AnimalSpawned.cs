using System.Collections.Generic;
using UnityEngine;

public class AnimalSpawned : MonoBehaviour
{
    [Header("Pooling Settings")]
    [SerializeField] public List<GameObject> prefabs;
    [SerializeField] public List<float> spawnRates;
    [SerializeField] float period = 2f; // Spawn chậm hơn enemy
    [SerializeField] int poolSize = 10;

    [Header("References")]
    [SerializeField] private WorldTime worldTime;
    [SerializeField] private PolygonCollider2D spawnArea; // 🌟 vùng spawn

    private float time;
    private List<GameObject> pool;

    void Start()
    {
        pool = new List<GameObject>();
        for (int i = 0; i < prefabs.Count; i++)
        {
            int count = Mathf.RoundToInt(poolSize * spawnRates[i]);
            for (int j = 0; j < count; j++)
            {
                GameObject o = Instantiate(prefabs[i]);
                o.SetActive(false);
                pool.Add(o);
            }
        }
    }

    void Update()
    {
        if (worldTime == null) return;

        int hour = worldTime.CurrentTime.Hours;
        // Animal spawn vào ban ngày (6h - 18h), ngược lại với enemy
        bool isDay = (hour >= 6 && hour < 18);
        if (!isDay) return;

        time += Time.deltaTime;
        if (time >= period)
        {
            GameObject animal = GetGame();
            if (animal != null)
            {
                Vector2 randomPos = GetRandomPointInPolygon(spawnArea);
                animal.transform.position = randomPos;
                animal.SetActive(true);
                animal.GetComponent<Animal>().currentHealth = animal.GetComponent<Animal>().maxHealth;

            }
            time = 0;
        }
    }

    GameObject GetGame()
    {
        foreach (var o in pool)
            if (!o.activeSelf) return o;
        return null;
    }

    // 🔹 Lấy điểm ngẫu nhiên trong polygon
    Vector2 GetRandomPointInPolygon(PolygonCollider2D collider)
    {
        Bounds bounds = collider.bounds;
        Vector2 point;

        int safety = 0;
        do
        {
            point = new Vector2(
                Random.Range(bounds.min.x, bounds.max.x),
                Random.Range(bounds.min.y, bounds.max.y)
            );
            safety++;
        }
        while (!collider.OverlapPoint(point) && safety < 100);

        return point;
    }

    // 🌈 Vẽ gizmo để thấy vùng spawn trong scene view
    private void OnDrawGizmos()
    {
        if (spawnArea != null)
        {
            Gizmos.color = Color.green; // Màu xanh lá để phân biệt với enemy (cyan)
            Vector2[] points = spawnArea.points;
            for (int i = 0; i < points.Length; i++)
            {
                Vector3 p1 = spawnArea.transform.TransformPoint(points[i]);
                Vector3 p2 = spawnArea.transform.TransformPoint(points[(i + 1) % points.Length]);
                Gizmos.DrawLine(p1, p2);
            }
        }
    }

    public void ActivateSpawner() => enabled = true;
    public void DeactivateSpawner() => enabled = false;
}


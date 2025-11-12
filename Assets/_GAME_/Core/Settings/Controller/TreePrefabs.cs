using UnityEngine;

public class TreePrefabs : MonoBehaviour
{
    [Header("Tree prefab để spawn")]
    public GameObject treePrefab; // Gán prefab cây (vd: tree1)

    [Header("Khu vực spawn (tọa độ min/max)")]
    public Vector2 spawnAreaMin = new Vector2(-10, -10);
    public Vector2 spawnAreaMax = new Vector2(10, 10);

    [Header("Thời gian spawn lại (giây)")]
    public float spawnInterval = 10f;

    [Header("Số lượng cây tối đa")]
    public int maxTrees = 20;

    [Header("Bán kính kiểm tra va chạm")]
    public float checkRadius = 1f;

    [Header("Layer của cây")]
    public string vegetationLayerName = "Vegetation";

    private float timer;

    void Start()
    {
        // Lần đầu spawn một ít cây
        for (int i = 0; i < 5; i++)
        {
            SpawnTree();
        }
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            timer = 0f;

            int currentCount = GameObject.FindGameObjectsWithTag("Tree").Length;
            if (currentCount < maxTrees)
            {
                SpawnTree();
            }
        }
    }

    void SpawnTree()
    {
        if (treePrefab == null) return;

        int attemptLimit = 30;
        for (int attempt = 0; attempt < attemptLimit; attempt++)
        {
            float x = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
            float y = Random.Range(spawnAreaMin.y, spawnAreaMax.y);
            Vector3 spawnPos = new Vector3(x, y, 0);

            // Kiểm tra các collider quanh vị trí
            Collider2D[] colliders = Physics2D.OverlapCircleAll(spawnPos, checkRadius);

            bool canSpawn = true;

            foreach (Collider2D col in colliders)
            {
                if (col == null) continue;

                // Nếu chạm phải Tree, NPC, hoặc House -> KHÔNG spawn
                if (col.CompareTag("Tree") || col.CompareTag("NPC") || col.CompareTag("House"))
                {
                    canSpawn = false;
                    break;
                }
            }

            // Nếu vùng spawn hợp lệ
            if (canSpawn)
            {
                GameObject newTree = Instantiate(treePrefab, spawnPos, Quaternion.identity);

                // Đặt layer Vegetation
                int vegetationLayer = LayerMask.NameToLayer(vegetationLayerName);
                if (vegetationLayer != -1)
                {
                    newTree.layer = vegetationLayer;
                }

                // Tag để quản lý
                newTree.tag = "Tree";
                return;
            }
        }

        Debug.Log("🌲 Không tìm được vị trí hợp lệ để spawn cây (vùng quá chật hoặc có NPC/House).");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Vector3 center = (spawnAreaMin + spawnAreaMax) / 2f;
        Vector3 size = new Vector3(spawnAreaMax.x - spawnAreaMin.x, spawnAreaMax.y - spawnAreaMin.y, 1);
        Gizmos.DrawWireCube(center, size);
    }
}

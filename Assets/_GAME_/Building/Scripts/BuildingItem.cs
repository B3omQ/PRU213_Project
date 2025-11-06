using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildingItem : Item
{
    [Header("Building Settings")]
    public GameObject buildingPrefab;
    public Color validColor = new Color(0, 1, 0, 0.5f);
    public Color invalidColor = new Color(1, 0, 0, 0.5f);

    private GameObject previewObject;
    private SpriteRenderer[] previewRenderers;
    private bool isBuildingMode = false;

    private Tilemap groundTilemap;

    public override void UseItem(Transform player, Tilemap tilledTilemap, TileBase tilledTile, Tilemap groundTilemap)
    {
        if (isBuildingMode)
        {
            CancelBuildingMode();
            return;
        }

        if (buildingPrefab == null) return;

        this.groundTilemap = groundTilemap;
        isBuildingMode = true;

        // 🔹 Tạo bản sao prefab để hiển thị preview (nhưng không có collider)
        previewObject = Instantiate(buildingPrefab);
        previewObject.name = "BuildingPreview";
        previewObject.layer = LayerMask.NameToLayer("Ignore Raycast");

        // 🔹 Lấy tất cả SpriteRenderers của prefab con
        previewRenderers = previewObject.GetComponentsInChildren<SpriteRenderer>();

        if (previewRenderers == null || previewRenderers.Length == 0)
        {
            Debug.LogWarning("Prefab không có SpriteRenderer nào — không thể tạo preview!");
            Destroy(previewObject);
            isBuildingMode = false;
            return;
        }

        // 🔹 Làm mờ và đảm bảo preview nằm trên các tile khác
        foreach (var sr in previewRenderers)
        {
            sr.color = validColor;
            sr.sortingOrder += 1;
        }

        // 🔹 Xóa tất cả collider trong preview (nếu có)
        foreach (var col in previewObject.GetComponentsInChildren<Collider2D>())
            Destroy(col);
    }

    void Update()
    {
        if (!isBuildingMode || previewObject == null || groundTilemap == null)
            return;

        Vector2 mousePos = InputManager.MousePosition;
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        Vector3Int cellPos = groundTilemap.WorldToCell(worldPos);
        Vector3 cellCenter = groundTilemap.GetCellCenterWorld(cellPos);

        previewObject.transform.position = cellCenter;

        bool canBuild = CanBuildHere(cellPos);

        // 🔹 Cập nhật màu tất cả sprite
        foreach (var sr in previewRenderers)
            sr.color = canBuild ? validColor : invalidColor;

        // 🔹 Đặt công trình thật
        if (InputManager.PlaceBuildingPressed && canBuild)
        {
            Instantiate(buildingPrefab, cellCenter, Quaternion.identity);
            RemoveFromStack(1);
            CancelBuildingMode();

            if (quantity <= 0)
            {
                Debug.Log($"Removing Building item {id} from inventory");
                InventoryController._instance.RemoveItemsFromInventory(id, 1);
                // hoặc nếu bạn có tham chiếu slot, có thể tự hủy gameObject:
                Destroy(gameObject);
            }
        }

        // 🔹 Thoát chế độ xây
        if (InputManager.ExitPressed)
            CancelBuildingMode();
    }

    private bool CanBuildHere(Vector3Int cellPos)
    {
        bool hasTile = groundTilemap.HasTile(cellPos);
        Vector3 worldCenter = groundTilemap.GetCellCenterWorld(cellPos);

        Debug.Log($"🧱 Checking tile at cell {cellPos} | WorldPos: {worldCenter} | HasTile: {hasTile}");

        if (!hasTile)
            return false;

        Collider2D hit = Physics2D.OverlapBox(worldCenter, Vector2.one * 0.9f, 0f);
        if (hit != null && !hit.CompareTag("MapBound"))
        {
            Debug.Log($"🚫 Bị chặn bởi {hit.name}");
            return false;
        }

        return true;
    }

    private void CancelBuildingMode()
    {
        if (previewObject != null)
            Destroy(previewObject);
        isBuildingMode = false;
    }   
}

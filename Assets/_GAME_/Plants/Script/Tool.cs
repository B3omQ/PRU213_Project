using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class HoeTool : Item
{
    private Tilemap groundTilemap;   // Tilemap gốc (RuleTile)
    private Tilemap tilledTilemap;   // Tilemap chồng lên
    private TileBase tilledTile;     // Tile đất đã cuốc
    private Transform player;


    [Header("Hoe Settings")]
    public float hoeRange = 1f;     // Khoảng cách cuốc

    [Header("Animation")]
    public Animator animator;       // Animator của người chơi

    public override void UseItem(Transform playerRef, Tilemap tilledTilemapRef, TileBase tilledTileRef, Tilemap groundTilemapRef)
    {
        player = playerRef;
        tilledTilemap = tilledTilemapRef;
        tilledTile = tilledTileRef;
        groundTilemap = groundTilemapRef;
        // Ưu tiên dùng tilemap trong item nếu có
        if (groundTilemap == null || tilledTilemap == null)
        {
            Debug.LogWarning("⚠️ HoeItem: Thiếu Tilemap!");
            return;
        }

        if (player == null)
        {
            Debug.LogWarning("⚠️ HoeItem: Player chưa được gán!");
            return;
        }

        // Gọi animation nếu có
        if (animator != null)
            animator.SetTrigger("Using");

        // Xác định vị trí tile phía trước player
        Vector3 targetPos = player.position + player.right * hoeRange;
        Vector3Int cellPos = groundTilemap.WorldToCell(targetPos);

        // Kiểm tra tile gốc
        TileBase groundTile = groundTilemap.GetTile(cellPos);
        if (groundTile == null)
        {
            Debug.Log("❌ Không thể cuốc ở đây (không có tile đất).");
            return;
        }

        // Kiểm tra tile đã cuốc chưa
        TileBase currentTilled = tilledTilemap.GetTile(cellPos);
        if (currentTilled != null)
        {
            Debug.Log("⚠️ Ô này đã được cuốc rồi!");
            return;
        }

        // Cuốc đất
        tilledTilemap.SetTile(cellPos, tilledTile);
        Debug.Log($"✅ Đã cuốc ô {cellPos}");
    }
}

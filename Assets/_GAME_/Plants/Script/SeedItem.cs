using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets._GAME_.Plants.Script
{
    public class SeedItem : Item
    {
        [Header("Seed Settings")]
        [SerializeField] private GameObject cropPrefab;
        [SerializeField] private float plantRange = 1f;

        private Transform player; // runtime player reference
        private Tilemap tilledTilemap;
        private TileBase tilledTile;

        /// <summary>
        /// Gọi khi người chơi dùng hạt giống từ hotbar hoặc inventory.
        /// </summary>
        public override void UseItem(Transform playerTransform, Tilemap tilemap, TileBase tile, Tilemap groundTilemap)
        {
            Debug.Log("Seed working");
            // Gán reference runtime
            player = playerTransform;
            tilledTilemap = tilemap;
            tilledTile = tile;

            if (player == null)
            {
                Debug.LogWarning("❌ SeedItem: Không tìm thấy Player để trồng hạt.");
                return;
            }

            if (tilledTilemap == null)
            {
                Debug.LogWarning("⚠️ SeedItem: Chưa truyền Tilemap đất đã cuốc vào UseItem().");
                return;
            }

            // Xác định vị trí tile phía trước player
            Vector3 targetPos = player.position + player.right * plantRange;
            Vector3Int cellPos = tilledTilemap.WorldToCell(targetPos);

            // Kiểm tra tile tại vị trí đó
            TileBase currentTile = tilledTilemap.GetTile(cellPos);
            if (currentTile == null)
            {
                itemPickupUI.Instance?.ShowWarning("❌ Cannot plant here (no tilled soil)");
                return;
            }

            // Kiểm tra đúng loại tile (nếu có chỉ định)
            if (tilledTile != null && currentTile != tilledTile)
            {
                itemPickupUI.Instance?.ShowWarning("❌ Cannot plant here (not tilled tile)");
                return;
            }

            // Xác định vị trí world chính giữa cell
            Vector3 plantWorldPos = tilledTilemap.GetCellCenterWorld(cellPos);

            // Tránh trồng chồng lên nhau
            Collider2D overlap = Physics2D.OverlapCircle(plantWorldPos, 0.1f);
            if (overlap != null && overlap.CompareTag("Planted"))
            {
                itemPickupUI.Instance?.ShowWarning("⚠️ A crop is already planted here!");
                return;
            }

            // Tiến hành trồng cây
            if (cropPrefab != null)
            {
                GameObject crop = Instantiate(cropPrefab, plantWorldPos, Quaternion.identity);
                crop.tag = "Planted";

                RemoveFromStack(1);
                Debug.Log($"🌱 Planted crop at tile {cellPos}");

                if (quantity <= 0)
                {
                    Debug.Log($"🧺 Removing seed {id} from inventory");
                    InventoryController._instance.RemoveItemsFromInventory(id, 1);
                    // hoặc nếu bạn có tham chiếu slot, có thể tự hủy gameObject:
                    Destroy(gameObject);
                }
            }
            else
            {
                Debug.LogWarning($"⚠️ Crop prefab chưa được gán cho {Name}");
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (player == null) return;
            Gizmos.color = Color.green;
            Vector3 targetPos = player.position + player.right * plantRange;
            Gizmos.DrawWireSphere(targetPos, 0.1f);
        }
    }
}

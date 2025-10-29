using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets._GAME_.Plants.Script
{
    public class SeedItem : Item
    {
        [Header("Seed Settings")]
        [SerializeField] private GameObject cropPrefab;
        [SerializeField] private LayerMask tilledSoilLayer;
        [SerializeField] private float checkRadius = 0.2f;
        [SerializeField] private float plantRange = 1f;

        private Transform player; // runtime player reference


        public override void UseItem(Transform playerTransform)
        {
            // Ưu tiên player được truyền vào, nếu null thì dùng player trong scene
            if (playerTransform != null)
                player = playerTransform;

            if (player == null)
            {
                Debug.LogWarning("❌ Không tìm thấy Player để trồng hạt.");
                return;
            }

            Debug.Log("Use item on SeedItem");

            Vector3 plantPosition = player.position + player.right * plantRange;

            Collider2D hit = Physics2D.OverlapCircle(plantPosition, checkRadius, tilledSoilLayer);
            if (hit == null || !hit.CompareTag("TilledSoil"))
            {
                itemPickupUI.Instance?.ShowWarning("Cannot plant here");
                return;
            }

            if (cropPrefab != null)
            {
                Instantiate(cropPrefab, hit.transform.position, Quaternion.identity);
                RemoveFromStack(1);
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
            Vector3 plantPosition = player.position + player.right * plantRange;
            Gizmos.DrawWireSphere(plantPosition, checkRadius);
        }
    }
}

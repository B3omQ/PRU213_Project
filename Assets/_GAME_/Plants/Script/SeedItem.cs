using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets._GAME_.Plants.Script
{
    public class SeedItem : Item
    {
        [SerializeField] private GameObject cropPrefab;
        [SerializeField] private GameObject Player;
        [SerializeField] private LayerMask tilledSoilLayer;
        [SerializeField] private float checkRadius = 0.2f;
        [SerializeField] private float plantRange = 1f;    

        public override void UseItem()
        {
            Debug.Log("Use item on seedItem");

            Vector3 plantPosition = Player.transform.position;

      

            Collider2D hit = Physics2D.OverlapCircle(plantPosition, checkRadius, tilledSoilLayer);
            if (hit == null || !hit.CompareTag("TilledSoil"))
            {

                if (itemPickupUI.Instance != null)
                {
                    itemPickupUI.Instance.ShowWarning("Can not plant");
                }
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
            if (Player == null) return;
            Gizmos.color = Color.green;
            Vector3 plantPosition = Player.transform.position + Player.transform.right * plantRange;
            Gizmos.DrawWireSphere(plantPosition, checkRadius);
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets._GAME_.Plants.Script
{
    public class SeedItem : Item
    {
        [SerializeField] private GameObject cropPrefab;
        [SerializeField] private GameObject Player;
       

        public override void UseItem()
        {
            Debug.Log("use item on seedItem");
            Vector3 plantPosition = Player.transform.position + Vector3.forward * 2f;

            // Tạo cây ở vị trí đó
            if (cropPrefab != null)
            {
                Instantiate(cropPrefab, plantPosition, Quaternion.identity);
                Debug.Log("Planted " + Name);
                RemoveFromStack(1);
            }
            else
            {
                Debug.LogWarning("Crop prefab is not assigned for " + Name);
            }
        }

        
    }
}

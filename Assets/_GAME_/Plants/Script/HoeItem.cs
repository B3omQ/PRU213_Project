using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;


namespace Assets._GAME_.Plants.Script
{
    public class HoeItem : Item
    {

        private HoeTool hoeTool;

        private void Awake()
        {
            hoeTool = GetComponent<HoeTool>();
        }

        public override void UseItem(Transform player, Tilemap tilemap, TileBase tile, Tilemap groundTilemap)
        {
            if (hoeTool == null)
            {
                Debug.Log("tool null");
                return;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


namespace Assets._GAME_.Plants.Script
{
    public class HoeItem : Item
    {

        private HoeTool hoeTool;

        private void Awake()
        {
            hoeTool = GetComponent<HoeTool>();
        }

        public override void UseItem(Transform player)
        {
            if (hoeTool == null)
            {
                Debug.Log("tool null");
                return;
            }
            hoeTool.UseHoe();
        }
    }
}

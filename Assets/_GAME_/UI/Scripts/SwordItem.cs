using UnityEngine;
using UnityEngine.Tilemaps;

public class SwordItem : Item
{
    private GameObject swordObject;          // Object kiếm trong Player
    private HotBarController hotbar;         // Tham chiếu đến HotBarController
    private Transform player;
    private bool isEquipped = false;
    private void Start()
    {
        hotbar = FindAnyObjectByType<HotBarController>();

        // Tìm player trong scene (nếu chưa có)
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }
    }

    public override void UseItem(Transform player, Tilemap tilledTilemap, TileBase tilledTile, Tilemap Groundmap)
    {
        if (player == null)
        {
            Debug.LogWarning("SwordItem: Player chưa được gán!");
            return;
        }

        // Nếu chưa có swordObject → tìm trong player
        if (swordObject == null)
        {
            Transform swordTransform = player.Find("Sword");
            if (swordTransform != null)
                swordObject = swordTransform.gameObject;
            else
            {
                Debug.LogWarning("SwordItem: Không tìm thấy object 'Sword' trong Player!");
                return;
            }
        }
        isEquipped = true;
        // Toggle bật/tắt kiếm
        bool newState = !swordObject.activeSelf;
        swordObject.SetActive(newState);
        Debug.Log(newState ? "Kiếm bật lên!" : "Kiếm tắt đi!");
    }

    public override void OnDropOutsideInventory()
    {
        if (swordObject != null)
        {
            swordObject.SetActive(false);
            isEquipped = false;
        }
    }
}

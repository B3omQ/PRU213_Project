using UnityEngine;
using UnityEngine.Tilemaps;

public class Coinitem : Item
{
    public override void UseItem(Transform player, Tilemap tilledTilemap, TileBase tilledTile, Tilemap groundTilemap)
    {
        RemoveFromStack(1);
        if (quantity <= 0)
        {
            InventoryController._instance.RemoveItemsFromInventory(id, 1);
            Destroy(gameObject);
        }
        Debug.Log("Add Coin into player");
        PlayerCoinManager.Instance.AddCoins(100);
        base.UseItem(player, tilledTilemap, tilledTile, groundTilemap);
    }
}

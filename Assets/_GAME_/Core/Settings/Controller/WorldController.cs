using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldController : MonoBehaviour
{
    [Header("Tilemap References")]
    [SerializeField] private Tilemap groundTilemap;
    [SerializeField] private Tilemap waterTilemap;

    [Header("Damage Settings")]
    [SerializeField] private float damageDelay = 1.5f;
    [SerializeField] private float damagePerSecond = 5f;

    [Header("Player Reference")]
    [SerializeField] private Transform player;
    [SerializeField] private PlayerHealth playerHealth;

    private bool isInWater = false;
    private Coroutine damageCoroutine;

    private void Update()
    {
        CheckPlayerTile();
    }

    private void CheckPlayerTile()
    {
        if (player == null || waterTilemap == null || groundTilemap == null) return;

        Vector3Int cellPos = waterTilemap.WorldToCell(player.position);

        bool onWater = waterTilemap.HasTile(cellPos);
        bool onGround = groundTilemap.HasTile(cellPos);

        // Chỉ tính là "đang ở dưới nước" nếu có water tile và KHÔNG có ground tile che lên
        bool inWater = onWater && !onGround;

        if (inWater && !isInWater)
        {
            isInWater = true;
            damageCoroutine = StartCoroutine(ApplyWaterDamage());
        }
        else if (!inWater && isInWater)
        {
            isInWater = false;
            if (damageCoroutine != null)
                StopCoroutine(damageCoroutine);
        }
    }


    private IEnumerator ApplyWaterDamage()
    {
        // Delay trước khi bắt đầu mất máu
        yield return new WaitForSeconds(damageDelay);

        while (isInWater)
        {
            if (playerHealth != null)
                playerHealth.TakeDamage(damagePerSecond);

            yield return new WaitForSeconds(1f);
        }
    }
}

using UnityEngine;

public class Sword : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float attackOffset = 0.5f;
    [SerializeField] private SpriteRenderer swordRenderer;

    private Vector2 lastMoveDir = Vector2.down; // hướng mặc định

    void Update()
    {
        Vector2 moveDir = InputManager.Movement;

        if (moveDir.sqrMagnitude > 0.01f)
        {
            lastMoveDir = moveDir.normalized;
        }

        // Cập nhật vị trí theo hướng nhìn
        transform.position = player.position + (Vector3)(lastMoveDir * attackOffset);

        // --- Không xoay, chỉ flip ---
        UpdateSwordVisual();
    }

    private void UpdateSwordVisual()
    {
        // Nếu hướng theo trục ngang
        if (Mathf.Abs(lastMoveDir.x) > Mathf.Abs(lastMoveDir.y))
        {
            // Nhìn phải
            if (lastMoveDir.x > 0)
            {
                swordRenderer.flipX = false;
                swordRenderer.flipY = false;
            }
            // Nhìn trái
            else
            {
                swordRenderer.flipX = true;
                swordRenderer.flipY = false;
            }
        }
        else
        {
            // Nhìn lên
            if (lastMoveDir.y > 0)
            {
                swordRenderer.flipX = false;
                swordRenderer.flipY = false;
            }
            // Nhìn xuống
            else
            {
                swordRenderer.flipX = false;
                swordRenderer.flipY = true;
            }
        }
    }
}

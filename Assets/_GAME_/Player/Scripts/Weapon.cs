using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float _damage = 1f;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();

        if (enemy != null)
        {
            // Tìm vị trí player (người tấn công)
            Transform playerTransform = transform.root;
            if (playerTransform == null)
                playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;

            if (playerTransform != null)
            {
                enemy.TakeDamage(_damage, playerTransform.position);
                Debug.Log($"Gây {_damage} sát thương cho {enemy.name} từ vị trí {playerTransform.position}");
            }
            else
            {
                Debug.LogWarning("Không tìm thấy vị trí player để truyền vào TakeDamage!");
            }
        }
    }


}

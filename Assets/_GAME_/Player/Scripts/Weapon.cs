using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float _damage = 1f;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if(enemy != null)
        {
            enemy.TakeDamage(_damage);
            Debug.Log($"⚔️ Gây {_damage} sát thương cho {enemy.name}");
        }
    }


}

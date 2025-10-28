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
            //goi ham gay dame
        }
    }


}

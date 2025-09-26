using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D rigid;
    public Transform player;   // gán trong Inspector hoặc tự tìm
    public float speed = 2f;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    void Update()
    {
        if (player == null) return;

        // hướng từ enemy đến player
        Vector2 direction = (player.position - transform.position).normalized;

        // di chuyển enemy về phía player
        rigid.linearVelocity = direction * speed;
    }
}
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HoeTool : MonoBehaviour
{
    [Header("Hoe Settings")]
    public GameObject tilledSoilPrefab;
    public LayerMask groundLayer;
    public float hoeRange = 1f;
    public Transform player;
    public int poolSize = 5;
    public float soilLifetime = 3f;

    [Header("Animation")]
    public Animator animator;

    private List<GameObject> soilPool = new List<GameObject>();
    itemPickupUI itemPickupUI;
    private void Start()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject soil = Instantiate(tilledSoilPrefab);
            soil.SetActive(false);
            soil.tag = "TilledSoil";
            SpriteRenderer sr = soil.GetComponent<SpriteRenderer>();
            if (sr != null)
                sr.sortingOrder = -1;
            soilPool.Add(soil);
        }

        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null)
                player = p.transform;
        }
        itemPickupUI = new itemPickupUI();
    }

    public void UseHoe()
    {
        if (player == null)
        {
            Debug.LogWarning("⚠️ HoeTool: player null");
            return;
        }

        TryHoeGround();

        if (animator != null)
            animator.SetTrigger("Using");
    }

    void TryHoeGround()
    {
        Vector2 targetPos = player.position + player.right * hoeRange;

        Collider2D hit = Physics2D.OverlapCircle(targetPos, 0.1f, groundLayer);
        if (hit != null && hit.CompareTag("TilledSoil"))
        {
            Debug.Log("⚠️ Đã cuốc rồi ở vị trí: " + targetPos);
            return;
        }

        GameObject soil = GetInactiveSoil();
        if (soil == null)
        {

            if (itemPickupUI.Instance != null)
            {
                itemPickupUI.Instance.ShowWarning("Soil is fully ");
            }
            return;
        }

        soil.transform.position = targetPos;
        soil.SetActive(true);
        StartCoroutine(DeactivateSoilAfterTime(soil, soilLifetime));
    }

    GameObject GetInactiveSoil()
    {
        foreach (var soil in soilPool)
            if (!soil.activeSelf)
                return soil;
        return null;
    }

    IEnumerator DeactivateSoilAfterTime(GameObject soil, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (soil != null)
            soil.SetActive(false);
    }
}

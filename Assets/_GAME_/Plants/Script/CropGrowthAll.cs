using UnityEditor.ShaderGraph;
using UnityEngine;

public class CropGrowthAll : MonoBehaviour
{
    [SerializeField] private GameObject[] growthStages;
    [SerializeField] private float[] stageDurations;

    private float timer = 0f;
    private int stage = 0;

    void Start()
    {
        Instantiate(growthStages[0], transform.position, Quaternion.identity, transform);
    }

    void Update()
    {
        if (stage >= growthStages.Length - 1) return;

        timer += Time.deltaTime;

        if (timer >= stageDurations[stage])
        {
            Destroy(transform.GetChild(0).gameObject);
            stage++;
            Instantiate(growthStages[stage], transform.position, Quaternion.identity, transform);
        }
    }
}

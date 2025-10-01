using UnityEngine;

public class GrowCrop : MonoBehaviour
{
    [SerializeField] private GameObject stage1Prefab;
    [SerializeField] private GameObject stage2Prefab;
    [SerializeField] private GameObject stage3Prefab;
    [SerializeField] private GameObject stage4Prefab;

    private float timer = 0f;
    private int stage = 1;

    void Start()
    {
        Instantiate(stage1Prefab, transform.position, Quaternion.identity, transform);
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (stage == 1 && timer >= 3f)
        {
            Destroy(transform.GetChild(0).gameObject);
            Instantiate(stage2Prefab, transform.position, Quaternion.identity, transform);
            stage = 2;
        }
        else if (stage == 2 && timer >= 6f)
        {
            Destroy(transform.GetChild(0).gameObject);
            Instantiate(stage3Prefab, transform.position, Quaternion.identity, transform);
            stage = 3;
        }
        else if (stage == 3 && timer >= 9f)
        {
            Destroy(transform.GetChild(0).gameObject);
            Instantiate(stage4Prefab, transform.position, Quaternion.identity, transform);
            stage = 4;
        }
    }
}

using System.Collections.Generic;
using UnityEngine;

public class EnemySpawned : MonoBehaviour
{
    [SerializeField]
    List<GameObject> prefabs;
    [SerializeField] List<float> spawnRates; 
    [SerializeField]
    float period = 1;
    [SerializeField]
    float time = 0;
    [SerializeField]
    int poolSize = 10;
    List<GameObject> pool;
    void Start()
    {
        pool = new List<GameObject>();

        for (int i = 0; i < prefabs.Count; i++)
        {
            int count = Mathf.RoundToInt(poolSize * spawnRates[i]);
            for (int j = 0; j < count; j++)
            {
                GameObject o = Instantiate(prefabs[i]);
                o.SetActive(false);
                pool.Add(o);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time >= period)
        {
            GameObject enemy = GetGame();
            if (enemy == null)
            {

            }
            else
            {
                enemy.SetActive(true);
            }

            time = 0;

        }
    }

    GameObject GetGame()
    {
        foreach (GameObject o in pool)

            if (o.activeSelf == false) return o;
        return null;
    }
}

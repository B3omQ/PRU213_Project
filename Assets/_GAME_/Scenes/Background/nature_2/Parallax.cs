using UnityEngine;

public class Parallax : MonoBehaviour
{
    Material _mat;
    float _distance;

    [Range(0f, 0.5f)]
    public float _speed = 0.2f;
    void Start()
    {
        _mat = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        _distance += Time.deltaTime * _speed;
        _mat.SetTextureOffset("_MainTex", Vector2.right * _distance);
    }   
}

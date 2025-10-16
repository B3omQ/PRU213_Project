using System.Collections;
using UnityEngine;

public class BounceEffect : MonoBehaviour
{
    public float _bounceHeight = 0.3f;
    public float _bounceDuration = 0.4f;
    public int _bounceCount = 2;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void StartBounce()
    {
        StartCoroutine(BounceHandler());
    }

    private IEnumerator BounceHandler()
    {
        Vector3 startPosition = transform.position;
        float localHeight = _bounceHeight;
        float localDuration = _bounceDuration;

        for (int i = 0; i < _bounceCount; i++)
        {
            yield return Bounce(startPosition, localHeight, localDuration / 2);
            localDuration *= 0.8f;
            localHeight *= 0.5f;
        }
        transform.position = startPosition;
    }

    private IEnumerator Bounce(Vector3 start, float height, float duration)
    {
        Vector3 peak = start + Vector3.up * height;
        float elasped = 0f;


        //move upwards
        while (elasped < duration)
        {
            transform.position = Vector3.Lerp(start, peak, elasped / duration);
            elasped += Time.deltaTime;
            yield return transform;
        }

        elasped = 0f;

        //move downward
        while (elasped < duration)
        {
            transform.position = Vector3.Lerp(start, peak, elasped / duration);
            elasped += Time.deltaTime;
            yield return transform;
        }
    }
}

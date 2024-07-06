using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyShake : MonoBehaviour
{
    private Transform spriteTransform;
    private Vector3 originalPosition;
    private Coroutine shakeCoroutine;

    private void Awake()
    {
        spriteTransform = transform;
        originalPosition = spriteTransform.localPosition;
    }

    public void StartShake(float strength)
    {
        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
        }
        shakeCoroutine = StartCoroutine(ShakeCoroutine(strength));
    }

    public void StopShake()
    {
        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
            shakeCoroutine = null;
            spriteTransform.localPosition = originalPosition;
        }
    }

    private IEnumerator ShakeCoroutine(float strength)
    {
        while (true)
        {
            float x = Random.Range(-1f, 1f) * strength;
            float y = Random.Range(-1f, 1f) * strength;

            spriteTransform.localPosition = new Vector3(originalPosition.x + x, originalPosition.y + y, originalPosition.z);

            yield return null;
        }
    }
}

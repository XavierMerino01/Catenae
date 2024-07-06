using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyShake : MonoBehaviour
{
    private Transform spriteTransform;
    private Quaternion originalRotation;
    private Coroutine shakeCoroutine;

    private void Awake()
    {
        spriteTransform = transform;
        originalRotation = spriteTransform.localRotation;
    }

    public void StartShake(float strength, float delay)
    {
        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
        }
        shakeCoroutine = StartCoroutine(ShakeCoroutine(strength, delay));
    }

    public void StopShake()
    {
        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
            shakeCoroutine = null;
            spriteTransform.localRotation = originalRotation;
        }
    }

    private IEnumerator ShakeCoroutine(float strength, float delay)
    {
        while (true)
        {
            float angle = Random.Range(-1f, 1f) * strength;

            spriteTransform.localRotation = Quaternion.Euler(0, 0, angle);

            yield return new WaitForSeconds(delay);
        }
    }
}

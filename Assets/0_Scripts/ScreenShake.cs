using System.Collections;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    private Transform cameraTransform;
    private Vector3 originalPosition;

    private void Awake()
    {
        cameraTransform = Camera.main.transform;
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.K))
        //{
        //    Shake(0.3f, 0.1f); 
        //}
    }
    public void Shake(float duration, float strength)
    {
        originalPosition = cameraTransform.localPosition;
        StartCoroutine(ShakeCoroutine(duration, strength));
    }

    private IEnumerator ShakeCoroutine(float duration, float strength)
    {
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * strength;
            float y = Random.Range(-1f, 1f) * strength;

            cameraTransform.localPosition = new Vector3(x, y, originalPosition.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        cameraTransform.localPosition = originalPosition;
    }
}

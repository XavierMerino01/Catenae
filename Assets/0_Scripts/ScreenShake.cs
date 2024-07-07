using System.Collections;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    private Transform cameraTransform;
    private Vector3 originalPosition;
    private Camera mainCamera;

    [SerializeField] private Vector3 targetFinalPosition;

    private void Awake()
    {
        cameraTransform = Camera.main.transform;
        mainCamera = Camera.main;
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

    public void FinalCameraTransition()
    {
        StartCoroutine(FinalCameraTransitionCoroutine(1.35f, targetFinalPosition, 6.0f));
    }

    private IEnumerator FinalCameraTransitionCoroutine(float targetSize, Vector3 targetPosition, float duration)
    {
        float elapsed = 0.0f;
        float initialSize = mainCamera.orthographicSize;
        Vector3 initialPosition = cameraTransform.position;

        while (elapsed < duration)
        {
            float t = elapsed / duration;

            mainCamera.orthographicSize = Mathf.Lerp(initialSize, targetSize, t);
            cameraTransform.position = Vector3.Lerp(initialPosition, targetPosition, t);

            elapsed += Time.deltaTime;
            yield return null;
        }

        mainCamera.orthographicSize = targetSize;
        cameraTransform.position = targetPosition;
        GameManager.instance.myUIManager.UIWin();
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Image fadePanelImage;
    [SerializeField] private Image spamButtonProgress;
    [SerializeField] private Slider timerBar;
    [SerializeField] private float levelDuration;
    [SerializeField] private Animator transitionMask;

    public float fillSpeed; 
    private Coroutine fillCoroutine;

    private void Start()
    {
        GameManager.instance.InitLevel();
        StartTimer();

        if (SceneManager.GetActiveScene().name != "StartScreen") return;

        StartFade(0, 2);

    }

    public void StartTimer()
    {
        StartCoroutine(TimerCoroutine(levelDuration)); // 60 seconds for one minute
    }

    private IEnumerator TimerCoroutine(float duration)
    {
        float startTime = Time.time;
        while (Time.time < startTime + duration)
        {
            float elapsed = Time.time - startTime;
            timerBar.value = elapsed / duration;
            yield return null;
        }
        timerBar.value = 1f; // Ensure the bar is full at the end
        Debug.Log("Timer ended");
        GameManager.instance.myButtonHandler.EndCurrentLevel();
    }

    public void UpdateSpamProgress(float newProgress)
    {
        if (fillCoroutine != null)
        {
            StopCoroutine(fillCoroutine);
        }
        fillCoroutine = StartCoroutine(SmoothFill(spamButtonProgress.fillAmount, spamButtonProgress.fillAmount + newProgress / 20));
    }

    private IEnumerator SmoothFill(float startValue, float endValue)
    {
        float elapsed = 0f;
        float duration = Mathf.Abs(endValue - startValue) / fillSpeed; // Calculate the duration based on fillSpeed

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            spamButtonProgress.fillAmount = Mathf.Lerp(startValue, endValue, elapsed / duration);
            yield return null;
        }

        // Ensure the final value is set
        spamButtonProgress.fillAmount = endValue;
        if (spamButtonProgress.fillAmount >= 1)
        {
            SetMask(true);
        }
    }

    public void SetMask(bool maskMode)
    {
        transitionMask.gameObject.SetActive(maskMode);
    }

    public void StartFade(float targetAlpha, float duration)
    {
        StartCoroutine(Fade(targetAlpha, duration));
    }

    #region Fade In and Out method

    private IEnumerator Fade(float targetAlpha, float duration)
    {
        // Get the current alpha value
        float startAlpha = fadePanelImage.color.a;

        // Calculate the increment value per frame
        float increment = (targetAlpha - startAlpha) / duration;

        // Adjust the alpha value gradually
        for (float time = 0; time < duration; time += Time.deltaTime)
        {
            Color newColor = fadePanelImage.color;
            newColor.a = startAlpha + increment * time;
            fadePanelImage.color = newColor;
            yield return null;
        }

        // Ensure the target alpha is set correctly
        Color finalColor = fadePanelImage.color;
        finalColor.a = targetAlpha;
        fadePanelImage.color = finalColor;
    }
    #endregion
}

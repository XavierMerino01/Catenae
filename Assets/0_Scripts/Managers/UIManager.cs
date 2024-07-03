using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Image fadePanelImage;
    [SerializeField] private Image timerBar;

    private void Start()
    {
        GameManager.instance.InitLevel();
        StartTimer();

        if (SceneManager.GetActiveScene().name != "StartScreen") return;

        StartFade(0, 2);

    }

    public void StartFade(float targetAlpha, float duration)
    {
        StartCoroutine(Fade(targetAlpha, duration));
    }

    public void StartTimer()
    {
        StartCoroutine(TimerCoroutine(60f)); // 60 seconds for one minute
    }

    private IEnumerator TimerCoroutine(float duration)
    {
        float startTime = Time.time;
        while (Time.time < startTime + duration)
        {
            float elapsed = Time.time - startTime;
            timerBar.fillAmount = elapsed / duration;
            yield return null;
        }
        timerBar.fillAmount = 1f; // Ensure the bar is full at the end
    }

    public void UpdateTimer()
    {
        
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

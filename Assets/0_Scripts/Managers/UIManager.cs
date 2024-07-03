using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Image fadePanelImage;

    private void Start()
    {
        if (SceneManager.GetActiveScene().name != "StartScreen") return;

        StartFade(0, 2);
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

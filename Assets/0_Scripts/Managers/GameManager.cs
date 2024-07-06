using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    void Awake()
    {
        if (instance != null)
        {

            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

    }


    //Create and grab references on starting game levels

    [HideInInspector] public GamepadManager myGamepadManager;
    [HideInInspector] public UIManager myUIManager;
    [HideInInspector] public ButtonCombinationHandler myButtonHandler;
    [HideInInspector] public AgesManager myAgeManager;

    public void InitLevel()
    {
        myGamepadManager = FindObjectOfType<GamepadManager>();
        myUIManager = FindObjectOfType<UIManager>();
        myButtonHandler = FindObjectOfType<ButtonCombinationHandler>();
        myAgeManager = FindObjectOfType<AgesManager>();
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void CloseGame()
    {
        Application.Quit();
    }

    public void GameOver()
    {

    }

    public void OnLevelTransitionEnd()
    {
        LevelData nextLevelData = myAgeManager.GetNextLevelData();
        myUIManager.SetNextLevelUI(nextLevelData.levelDuration, nextLevelData.buttonSpawnRate, nextLevelData.timerHandleSprite);
        myButtonHandler.SetButtonGenerationValues(nextLevelData.buttonsPerCombination, nextLevelData.buttonSpawnRate, nextLevelData.isLastAge);
        myAgeManager.SwapMaskInteraction();
    }
}

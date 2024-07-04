using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

    public void InitLevel()
    {
        myGamepadManager = FindObjectOfType<GamepadManager>();
        myUIManager = FindObjectOfType<UIManager>();
        myButtonHandler = FindObjectOfType<ButtonCombinationHandler>();
    }

    public void GameOver()
    {

    }

    public void LevelComplete()
    {
        myButtonHandler.StopButtonGeneration();
    }
}

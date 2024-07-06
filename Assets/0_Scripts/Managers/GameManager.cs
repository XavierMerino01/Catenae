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

    public void GameOver(bool win)
    {
        if (win)
        {
            myUIManager.StartFade(1, 2);
        }
        else
        {
            myUIManager.StartFade(1, 2);
        }
        
    }

    public void ChangeAgeMusic()
    {
        AgeData curentAge = myAgeManager.GetAgeData();
        FindObjectOfType<AudioManager>().RestoreInitalPitch("TeclaFinal");
        switch (curentAge.levelAge)
        {
            case (AgeData.HumanAge.Prehistory):
                FindObjectOfType<AudioManager>().Stop("Menu");
                FindObjectOfType<AudioManager>().Play("Prehistoria");
                break;
            case (AgeData.HumanAge.Egipt):
                Debug.Log("EGIPT");
                FindObjectOfType<AudioManager>().Stop("Prehistoria");
                FindObjectOfType<AudioManager>().Play("Egipto");
                break;
            case (AgeData.HumanAge.Rome):
                FindObjectOfType<AudioManager>().Stop("Egipto");
                FindObjectOfType<AudioManager>().Play("Roma");
                break;
            case (AgeData.HumanAge.MiddleAge):
                FindObjectOfType<AudioManager>().Stop("Roma");
                FindObjectOfType<AudioManager>().Play("Templarios");
                break;
            case (AgeData.HumanAge.Renacentism):
                FindObjectOfType<AudioManager>().Stop("Templarios");
                FindObjectOfType<AudioManager>().Play("Renacentismo");
                break;
            case (AgeData.HumanAge.ContemporaryAge):
                FindObjectOfType<AudioManager>().Stop("Renacentismo");
                FindObjectOfType<AudioManager>().Play("Actualidad");
                break;

        }
    }
    public void OnLevelTransitionEnd()
    {
        LevelData nextLevelData = myAgeManager.GetNextLevelData();

        myButtonHandler.SetButtonGenerationValues(nextLevelData.buttonsPerCombination, nextLevelData.buttonSpawnRate, nextLevelData.isLastAge);
        myUIManager.SetNextLevelUI(nextLevelData.levelDuration, nextLevelData.buttonSpawnRate, nextLevelData.timerHandleSprite);
        myAgeManager.SwapMaskInteraction();

        FindObjectOfType<AudioManager>().Play("PortalOut");
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    [HideInInspector] public UIManager myUIManager;
    [HideInInspector] public ButtonCombinationHandler myButtonHandler;
    [HideInInspector] public AgesManager myAgeManager;
    [HideInInspector] public TextAssigner myTextAssigner;

    private bool isPaused;

    public void InitLevel()
    {
        myUIManager = FindObjectOfType<UIManager>();
        myButtonHandler = FindObjectOfType<ButtonCombinationHandler>();
        myAgeManager = FindObjectOfType<AgesManager>();
        myTextAssigner = FindObjectOfType<TextAssigner>();
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
    public void PauseGame()
    {
        if (!isPaused)
        {
            Time.timeScale = 0;
            isPaused = true;
        }
        else
        {
            Time.timeScale = 1; 
            isPaused = false;
        }

        myButtonHandler.inputEnabled = !isPaused;
        myUIManager.TogglePausePanel(isPaused);
    }

    public void LoadMainMenu()
    {
        AudioSource[] sources = GetComponents<AudioSource>();
        foreach(AudioSource s in sources)
        {
            s.Stop();
        }

        FindObjectOfType<AudioManager>().Play("Menu");

        SceneManager.LoadScene(0);
    }

    public void CloseGame()
    {
        Application.Quit();
    }

    public void GameOver(bool win)
    {
        if (win)
        {
            myTextAssigner.FinalTransition();
            //myUIManager.UIWin();
        }
        else
        {
            FindObjectOfType<AudioManager>().Play("GameOver");
            myUIManager.UIGameOver();
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
    }

    public void RestartCurrentLevel()
    {
        ResetMusic();
        myButtonHandler.RestartButtonLevel();
    }

    public void ResetMusic()
    {

        foreach (AudioSource source in gameObject.GetComponents(typeof(AudioSource)))
        {
            if (source.isPlaying)
            {
                source.Stop();
            }
        }
        myAgeManager.currentAgeIndex--;
        ChangeAgeMusic();
        myAgeManager.currentAgeIndex++;
    }

    public void ChangeButtonSprites(InputManager.Controllers newController)
    {
        

        if (SceneManager.GetActiveScene().name == "MainMenu") 
        {
            GameObject[] textObjects = FindObjectOfType<MenuNavigation>().GetTextObjects();

            switch (newController)
            {
                case InputManager.Controllers.Xbox:
                    foreach (GameObject text in textObjects)
                    {
                        text.GetComponent<TMP_Text>().text = "<- B";
                    }
                break;
                case InputManager.Controllers.Keyboard:
                    foreach (GameObject text in textObjects)
                    {
                        text.GetComponent<TMP_Text>().text = "<- Esc";
                    }
                    break;
                case InputManager.Controllers.PlayStation:
                    foreach (GameObject text in textObjects)
                    {
                        text.GetComponent<TMP_Text>().text = "<- O";
                    }
                    break;
                default:
                    foreach (GameObject text in textObjects)
                    {
                        text.GetComponent<TMP_Text>().text = "<- B";
                    }
                    break;
            }
        }

        ButtonDisplay[] buttons = (ButtonDisplay[])Object.FindObjectsOfType(typeof(ButtonDisplay));

        switch (newController)
        {
            case InputManager.Controllers.Xbox:
                foreach (ButtonDisplay button in buttons)
                {
                    button.AssignXboxKeys();
                }
                break;
            case InputManager.Controllers.Keyboard:
                foreach (ButtonDisplay button in buttons)
                {
                    button.AssignPcKeys();
                }
                break;
            case InputManager.Controllers.PlayStation:
                foreach (ButtonDisplay button in buttons)
                {
                    button.AssignPlayKeys();
                }
                break;

            default:
                foreach (ButtonDisplay button in buttons)
                {
                    button.AssignXboxKeys();
                }
                break;

        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GamepadManager : MonoBehaviour
{

    [SerializeField] private GameObject[] gamepadOptions;

    // Start is called before the first frame update
    void Start()
    {
        //Move to level manager
        GameManager.instance.InitLevel();
        //

        SetGamepadVisuals(InputManager.instance.GetChosenController());
    }

    public void SetGamepadVisuals(InputManager.Controllers controllers)
    {
        for(int i = 0; i < gamepadOptions.Length; i++)
        {
            gamepadOptions[i].SetActive(false);
        }

        switch(controllers)
        {
            case InputManager.Controllers.Keyboard:
                gamepadOptions[0].SetActive(true); 
                break;

            case InputManager.Controllers.Xbox:
                gamepadOptions[1].SetActive(true);
                break;

            case InputManager.Controllers.PlayStation:
                gamepadOptions[2].SetActive(true);
                break;

            case InputManager.Controllers.Switch:
                gamepadOptions[3].SetActive(true);
                break;
        }
    }
}

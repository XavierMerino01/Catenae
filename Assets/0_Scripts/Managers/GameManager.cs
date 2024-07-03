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

    public void InitLevel()
    {
        myGamepadManager = FindObjectOfType<GamepadManager>();
    }
}

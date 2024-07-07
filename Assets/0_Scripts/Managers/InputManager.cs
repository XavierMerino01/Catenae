using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;
//using UnityEngine.InputSystem.Switch;
using UnityEngine.InputSystem.XInput;

public class InputManager : MonoBehaviour
{
    public static InputManager instance = null;
    GameControls myActionsManager;

    public enum Controllers
    {
        Keyboard,
        Xbox,
        PlayStation,
        Switch
    }
    public Controllers chosenController;

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

        myActionsManager = new GameControls();
        InitActions();

        InputSystem.onDeviceChange += CheckNewDevice;


    }
    private void Start()
    {
        CheckStartingDevice();
    }

    void InitActions()
    {

        myActionsManager.Gameplay.ArrowUp.Enable();
        myActionsManager.Gameplay.ArrowDown.Enable();
        myActionsManager.Gameplay.ArrowLeft.Enable();
        myActionsManager.Gameplay.ArrowRight.Enable();
        myActionsManager.Gameplay.Square.Enable();
        myActionsManager.Gameplay.Triangle.Enable();
        myActionsManager.Gameplay.Circle.Enable();
        myActionsManager.Gameplay.Cross.Enable();

    }

    private void Update()
    {
        CheckCurrentDevices();
    }

    public void CheckStartingDevice()
    {
        if (Gamepad.all.Count == 0) 
        {
            Debug.Log("Keyboard");
            SetChosenController(Controllers.Keyboard);
        }
        else
        {
            CheckNewDevice(Gamepad.all[0], InputDeviceChange.Added);
        }
    }

    public void CheckCurrentDevices()
    {
    }
    public void CheckNewDevice(InputDevice device, InputDeviceChange change)
    {
        switch (change)
        {
            case InputDeviceChange.Added:
                if (device is Gamepad gamepad)
                {
                    switch(gamepad)
                    {
                        case XInputController:
                            Debug.Log("Xbox connected");
                            SetChosenController(Controllers.Xbox);
                            break;

                        case DualShockGamepad:
                            Debug.Log("PS connected");
                            SetChosenController(Controllers.PlayStation);
                            break;
                    }
                    
                }
                break;
            case InputDeviceChange.Removed:
                //Debug.Log("Device removed: " + device);
                SetChosenController(Controllers.Keyboard);
                break;
        }
    }

    public void SetChosenController(Controllers newController)
    {
        chosenController = newController;
        GameManager.instance.ChangeButtonSprites(chosenController);
    }

    public Controllers GetChosenController()
    {
        return chosenController;
    }


    #region Game Inputs

    //public bool InputArrowUp()
    //{
    //    return myActionsManager.Gameplay.ArrowUp.WasPressedThisFrame();
    //}

    //public bool InputArrowDown()
    //{
    //    return myActionsManager.Gameplay.ArrowDown.WasPressedThisFrame();
    //}

    //public bool InputArrowLeft()
    //{
    //    return myActionsManager.Gameplay.ArrowLeft.WasPressedThisFrame();
    //}

    //public bool InputArrowRight()
    //{
    //    return myActionsManager.Gameplay.ArrowRight.WasPressedThisFrame();
    //}

    //public bool InputSquare()
    //{
    //    return myActionsManager.Gameplay.Square.WasPressedThisFrame();
    //}

    //public bool InputTriangle()
    //{
    //    return myActionsManager.Gameplay.Triangle.WasPressedThisFrame();
    //}

    //public bool InputCircle()
    //{
    //    return myActionsManager.Gameplay.Circle.WasPressedThisFrame();
    //}

    //public bool InputCross()
    //{
    //    return myActionsManager.Gameplay.Cross.WasPressedThisFrame();
    //}

    #endregion



    #region old_inputs

    //public bool ImpulseActionPressed()
    //{
    //    return myActionsManager.GameplayInputs.Impulse.IsPressed();
    //}

    //public bool ImpulseActionDown()
    //{
    //    return myActionsManager.GameplayInputs.Impulse.WasPressedThisFrame();
    //}
    //public bool ImpulseActionUp()
    //{
    //    return myActionsManager.GameplayInputs.Impulse.WasReleasedThisFrame();
    //}

    //public bool LeftRotationPressed()
    //{
    //    return myActionsManager.GameplayInputs.LeftRotation.IsPressed();
    //}

    //public bool RightRotationPressed()
    //{
    //    return myActionsManager.GameplayInputs.RightRotation.IsPressed();
    //}

    #endregion

    public bool isAnyKeyDown()
    {
        return Input.anyKeyDown;
    }
}

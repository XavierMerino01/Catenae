using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager instance = null;
    GameControls myActionsManager;

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

    void InitActions()
    {
        myActionsManager.GameplayInputs.Impulse.Enable();
        myActionsManager.GameplayInputs.RightRotation.Enable();
        myActionsManager.GameplayInputs.LeftRotation.Enable();

    }

    private void Update()
    {
        CheckCurrentDevices();
    }
    public void CheckCurrentDevices()
    {
    }
    public void CheckNewDevice(InputDevice device, InputDeviceChange change)
    {
        switch (change)
        {
            case InputDeviceChange.Added:
                //Debug.Log("Device added: " + device);
                break;
            case InputDeviceChange.Removed:
                //Debug.Log("Device removed: " + device);
                break;
        }
    }

    public bool ImpulseActionPressed()
    {
        return myActionsManager.GameplayInputs.Impulse.IsPressed();
    }

    public bool ImpulseActionDown()
    {
        return myActionsManager.GameplayInputs.Impulse.WasPressedThisFrame();
    }
    public bool ImpulseActionUp()
    {
        return myActionsManager.GameplayInputs.Impulse.WasReleasedThisFrame();
    }

    public bool LeftRotationPressed()
    {
        return myActionsManager.GameplayInputs.LeftRotation.IsPressed();
    }

    public bool RightRotationPressed()
    {
        return myActionsManager.GameplayInputs.RightRotation.IsPressed();
    }


    public bool isAnyKeyDown()
    {
        return Input.anyKeyDown;
    }
}

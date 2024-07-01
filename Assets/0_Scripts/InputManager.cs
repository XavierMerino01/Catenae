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
        myActionsManager.GameplayInputs.Jump.Enable();

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

    /*public bool IMRedAttackAction()
    {
        return myActionsManager.Gameplay.RedAttack.IsPressed();
    }
    public bool IMRedAttackActionDown()
    {
        return myActionsManager.Gameplay.RedAttack.WasPressedThisFrame();
    }
    public bool IMRedAttackActionUp()
    {
        return myActionsManager.Gameplay.RedAttack.WasReleasedThisFrame();
    }
    */
    public bool isAnyKeyDown()
    {
        return Input.anyKeyDown;
    }
}

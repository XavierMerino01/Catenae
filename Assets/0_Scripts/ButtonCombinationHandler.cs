using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using System;
using System.Diagnostics;

public class ButtonCombinationHandler : MonoBehaviour, GameControls.IGameplayActions
{
    public int buttonsPerCombination = 5;

    public GameObject buttonPrefab;
    public Vector2 initialPosition;
    public float spacing = 1.5f;

    private List<KeyCode> buttonCombination;
    private List<GameObject> buttonObjects = new List<GameObject>();
    private RandomButtonGenerator generator;

    private GameControls controls;
    private int currentButtonIndex = 0;

    public UnityEvent OnRowCleared;
    public UnityEvent OnRowFailed;

    void Awake()
    {
        controls = new GameControls();
        controls.Gameplay.SetCallbacks(this);
    }

    void OnEnable()
    {
        controls.Gameplay.Enable();
    }

    void OnDisable()
    {
        controls.Gameplay.Disable();
    }

    void Start()
    {
        generator = GetComponent<RandomButtonGenerator>();

        if (generator == null)
        {
            UnityEngine.Debug.LogError("RandomButtonGenerator is not assigned.");
            return;
        }

        buttonCombination = generator.GenerateButtonCombination(buttonsPerCombination);
        DisplayButtonCombination();
        PrintButtonCombination();
    }

    private void DisplayButtonCombination()
    {
        for (int i = 0; i < buttonCombination.Count; i++)
        {
            Vector2 position = new Vector2(initialPosition.x + i * spacing, initialPosition.y);
            GameObject button = Instantiate(buttonPrefab, position, Quaternion.identity);
            ButtonDisplay buttonDisplay = button.GetComponent<ButtonDisplay>();
            buttonDisplay.SetButton(buttonCombination[i]);
            buttonObjects.Add(button);
        }
    }

    public void HandleButtonPress(KeyCode key)
    {

        //UnityEngine.Debug.Log(key);
        //if (currentButtonIndex >= buttonObjects.Count)
        //{
        //    return;
        //}

        GameObject currentButton = buttonObjects[currentButtonIndex];
        ButtonDisplay buttonDisplay = currentButton.GetComponent<ButtonDisplay>();

        UnityEngine.Debug.Log(buttonDisplay.GetCurrentKey() + " " + key);

        if (buttonDisplay.GetCurrentKey() == key)
        {
            Destroy(currentButton);
            buttonObjects.RemoveAt(currentButtonIndex);

            currentButtonIndex++;
            //if (currentButtonIndex >= buttonObjects.Count)
            //{
            //    OnRowCleared?.Invoke();
            //}
        }
        else
        {
            //OnRowFailed?.Invoke();
        }
    }

    public void RowCleared()
    {
        UnityEngine.Debug.Log("Rowcleared");

    }

    public void RowFailed()
    {
        UnityEngine.Debug.Log("RowFailed");
    }
    private void PrintButtonCombination()
    {
        string combination = "Combination: ";
        for (int j = 0; j < buttonCombination.Count; j++)
        {
            combination += buttonCombination[j] + " ";
        }
        UnityEngine.Debug.Log(combination);
    }

    #region ButtonPresses
    public void OnArrowUp(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            HandleButtonPress(KeyCode.UpArrow);
        }
    }

    public void OnArrowDown(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            HandleButtonPress(KeyCode.DownArrow);
        }
    }

    public void OnArrowLeft(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            HandleButtonPress(KeyCode.LeftArrow);
        }
    }

    public void OnArrowRight(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            HandleButtonPress(KeyCode.RightArrow);
        }
    }

    public void OnSquare(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            HandleButtonPress(KeyCode.A);
        }
    }

    public void OnTriangle(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            HandleButtonPress(KeyCode.W);
        }
    }

    public void OnCircle(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            HandleButtonPress(KeyCode.D);
        }
    }

    public void OnCross(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            HandleButtonPress(KeyCode.S);
        }
    }
    #endregion
}

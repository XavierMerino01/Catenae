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
    public Vector2[] rowPositions;
    public float spacing = 1.5f;

    private List<KeyCode> currentButtonCombination;
    private List<GameObject>[] buttonCombinations;
    private List<GameObject> currentButtonObjects = new List<GameObject>();
    private RandomButtonGenerator generator;

    private GameControls controls;
    private int currentButtonIndex = 0;
    private int currentButtonRows = 0;

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

        buttonCombinations = new List<GameObject>[rowPositions.Length];

        InvokeRepeating("CreateAndDisplayNewButtonRow", 0.0f, 5.0f);
    }


    private void CreateAndDisplayNewButtonRow()
    {
        List<KeyCode> newButtons = generator.GenerateButtonCombination(buttonsPerCombination);
        buttonCombinations[currentButtonRows] = new List<GameObject>();
        DisplayButtonCombination(newButtons);
        PrintButtonCombination();
    }
    private void DisplayButtonCombination(List<KeyCode> ButtonsToDisplay)
    {
        for (int i = 0; i < ButtonsToDisplay.Count; i++)
        {
            Vector2 position = new Vector2(rowPositions[currentButtonRows].x + i * spacing, rowPositions[currentButtonRows].y);
            GameObject button = Instantiate(buttonPrefab, position, Quaternion.identity);
            ButtonDisplay buttonDisplay = button.GetComponent<ButtonDisplay>();
            buttonDisplay.SetButton(ButtonsToDisplay[i]);
            buttonCombinations[currentButtonRows].Add(button);
        }

        if (currentButtonObjects.Count == 0)
        {
            AssignCurrentCombination();
        }
        currentButtonRows++;
    }

    private void AssignCurrentCombination()
    {
        foreach (GameObject button in buttonCombinations[0])
        {
            currentButtonObjects.Add(button);
        }

    }

    public void HandleButtonPress(KeyCode key)
    {

        //UnityEngine.Debug.Log(key);
        //if (currentButtonIndex >= buttonObjects.Count)
        //{
        //    return;
        //}

        GameObject currentButton = currentButtonObjects[currentButtonIndex];
        ButtonDisplay buttonDisplay = currentButton.GetComponent<ButtonDisplay>();

        UnityEngine.Debug.Log(buttonDisplay.GetCurrentKey() + " " + key);

        if (buttonDisplay.GetCurrentKey() == key)
        {
            Destroy(currentButton);
            currentButtonObjects.RemoveAt(currentButtonIndex);

            //currentButtonIndex++;
            //if (currentButtonIndex >= buttonObjects.Count)
            //{
            //    OnRowCleared?.Invoke();
            //}
            if (currentButtonObjects.Count == 0) 
            {
                RowCleared();
            }
        }
        else
        {
            //OnRowFailed?.Invoke();
        }
    }

    public void RowCleared()
    {
        UnityEngine.Debug.Log("Rowcleared");

        currentButtonRows--;
        ReassignButtonCombinations();
        if (currentButtonRows > 0)
        {
            MoveAllButtonsDown(2);
        }
        AssignCurrentCombination();
    }

    public void RowFailed()
    {
        UnityEngine.Debug.Log("RowFailed");
    }

    private void ReassignButtonCombinations()
    {
        //Rearrange buttonCombinations array putting them one index value less (buttonComb[1] is now buttonComb[0])
        for (int i = 1; i < buttonCombinations.Length; i++)
        {
            buttonCombinations[i - 1] = buttonCombinations[i];
        }
        //Neteja l'últim element
        buttonCombinations[buttonCombinations.Length - 1] = new List<GameObject>();
    }

    private void MoveAllButtonsDown(float units)
    {
        foreach (var buttonList in buttonCombinations)
        {
            foreach (var button in buttonList)
            {
                Vector3 newPosition = button.transform.position;
                newPosition.y -= units;
                button.transform.position = newPosition;
            }
        }
    }


    private void PrintButtonCombination()
    {
        string combination = "Combination: ";
        for (int j = 0; j < currentButtonCombination.Count; j++)
        {
            combination += currentButtonCombination[j] + " ";
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

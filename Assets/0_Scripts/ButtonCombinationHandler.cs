using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ButtonCombinationHandler : MonoBehaviour, GameControls.IGameplayActions
{
    public int buttonsPerCombination = 5;
    public int numberOfCombinations = 4;

    public GameObject buttonPrefab;
    public Vector2[] rowPositions;
    public float spacing = 1.5f;

    private List<KeyCode>[,] buttonCombinations;
    private List<GameObject> currentButtonObjects = new List<GameObject>();
    private RandomButtonGenerator generator;

    private GameControls controls;
    private int currentButtonIndex = 0;
    private int currentButtonRows = 0;

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

        buttonCombinations = new List<KeyCode>[buttonsPerCombination, numberOfCombinations];

        //TREURE
        InvokeRepeating("CreateNewButtonRow", 0.0f, 5.0f);
    }

    private void CreateNewButtonRow()
    {
        List<KeyCode> newButtonCombination = generator.GenerateButtonCombination(buttonsPerCombination);

        if (currentButtonRows < numberOfCombinations)
        {
            for (int i = 0; i < buttonsPerCombination; i++)
            {
                if (buttonCombinations[i, currentButtonRows] == null)
                {
                    buttonCombinations[i, currentButtonRows] = new List<KeyCode>();
                }
                else
                {
                    buttonCombinations[i, currentButtonRows].Clear();
                }

                buttonCombinations[i, currentButtonRows].Add(newButtonCombination[i]);
            }

            Debug.Log("Button Combinations for row " + currentButtonRows);
            for (int i = 0; i < buttonsPerCombination; i++)
            {
                Debug.Log("Row " + currentButtonRows + ", Column " + i + ": " + string.Join(", ", buttonCombinations[i, currentButtonRows]));
            }

            DisplayButtonCombination(newButtonCombination);
            currentButtonRows++;
        }
        else
        {
            Debug.LogError("No more rows available for new button combinations.");
        }
    }

    private void DisplayButtonCombination(List<KeyCode> buttonsToDisplay)
    {
        for (int i = 0; i < buttonsToDisplay.Count; i++)
        {
            Vector2 position = new Vector2(rowPositions[currentButtonRows].x + i * spacing, rowPositions[currentButtonRows].y);
            GameObject button = Instantiate(buttonPrefab, position, Quaternion.identity);
            ButtonDisplay buttonDisplay = button.GetComponent<ButtonDisplay>();
            buttonDisplay.SetButton(buttonsToDisplay[i]);
            currentButtonObjects.Add(button);
        }
    }

    private void AssignCurrentCombination()
    {
        currentButtonObjects.Clear();
        for (int i = 0; i < buttonsPerCombination; i++)
        {
            if (buttonCombinations[i, 0] != null)
            {
                foreach (KeyCode keyCode in buttonCombinations[i, 0])
                {
                    Vector2 position = new Vector2(rowPositions[0].x + i * spacing, rowPositions[0].y);
                    GameObject button = Instantiate(buttonPrefab, position, Quaternion.identity);
                    ButtonDisplay buttonDisplay = button.GetComponent<ButtonDisplay>();
                    buttonDisplay.SetButton(keyCode);
                    currentButtonObjects.Add(button);
                }
            }
        }
    }

    public void HandleButtonPress(KeyCode key)
    {
        if (currentButtonIndex >= currentButtonObjects.Count)
        {
            return;
        }

        GameObject currentButton = currentButtonObjects[currentButtonIndex];
        ButtonDisplay buttonDisplay = currentButton.GetComponent<ButtonDisplay>();

        if (buttonDisplay.GetCurrentKey() == key)
        {
            Destroy(currentButton);
            currentButtonObjects.RemoveAt(currentButtonIndex);

            if (currentButtonObjects.Count == 0)
            {
                RowCleared();
            }
        }
        else
        {
            RowFailed();
        }
    }

    public void RowCleared()
    {
        Debug.Log("Row cleared");

        ReassignButtonCombinations();
        MoveAllButtonsDown();

        currentButtonRows--;
        AssignCurrentCombination();
    }

    private void MoveAllButtonsDown()
    {
        foreach (var buttonObject in currentButtonObjects)
        {
            Vector3 position = buttonObject.transform.position;
            position.y -= spacing; 
            buttonObject.transform.position = position;
        }
    }

    public void RowFailed()
    {
        Debug.Log("Row failed");
    }

    private void ReassignButtonCombinations()
    {
        for (int row = 1; row < numberOfCombinations; row++)
        {
            for (int col = 0; col < buttonsPerCombination; col++)
            {
                buttonCombinations[col, row - 1] = buttonCombinations[col, row];
            }
        }

        for (int col = 0; col < buttonsPerCombination; col++)
        {
            buttonCombinations[col, numberOfCombinations - 1] = null;
        }
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

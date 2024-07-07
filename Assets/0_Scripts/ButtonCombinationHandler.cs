using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class ButtonCombinationHandler : MonoBehaviour, GameControls.IGameplayActions
{
    public int buttonsPerCombination;
    public int numberOfCombinations;
    public float buttonSpawnRate;

    public bool isGeneratingButtons = false;
    private Coroutine buttonGenerationCoroutine;

    public GameObject buttonPrefab;
    public GameObject spamButton;
    public Vector2[] rowPositions;
    public float spacing;

    private List<KeyCode>[,] buttonCombinations;
    private List<GameObject> currentButtonObjects = new List<GameObject>();
    private RandomButtonGenerator generator;

    private GameControls controls;
    private int currentButtonCount = 0;
    private int currentButtonRows = 0;
    private int buttonRowOffset = 2;

    private bool isSpamingButton;
    public bool inputEnabled = true;
    private bool isLastLevel;

    public ScreenShake screen;

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
        spamButton.GetComponent<ButtonDisplay>().SetButton(KeyCode.W);
        spamButton.SetActive(false);
    }

    #region Start/Stop generating buttons
    public void StartButtonGeneration()
    {
        if (!isGeneratingButtons)
        {
            isGeneratingButtons = true;
            buttonGenerationCoroutine = StartCoroutine(ButtonGenerationLoop());
        }
    }

    private IEnumerator ButtonGenerationLoop()
    {
        while (isGeneratingButtons)
        {
            yield return new WaitForSeconds(buttonSpawnRate);
            CreateNewButtonRow();
            GameManager.instance.myUIManager.NextLineUI();
            
        }
    }

    public void StopButtonGeneration()
    {
        isGeneratingButtons = false;
        if (buttonGenerationCoroutine != null)
        {
            StopCoroutine(buttonGenerationCoroutine);
            buttonGenerationCoroutine = null;
        }
    }
    #endregion

    #region ButtonGeneration and Display
    private void CreateNewButtonRow()
    {
        List<KeyCode> newButtonCombination = generator.GenerateButtonCombination(buttonsPerCombination);

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
        if (currentButtonRows >= numberOfCombinations)
        {
            Debug.Log("GAME OVER");
            StopButtonGeneration();
            inputEnabled = false;
            GameManager.instance.GameOver(false);
        }
       // UpdateShakeEffect();

    }

    private void DisplayButtonCombination(List<KeyCode> buttonsToDisplay)
    {
        for (int i = 0; i < buttonsToDisplay.Count; i++)
        {
            //Vector2 position = new Vector2(rowPositions[currentButtonRows].x + i * spacing, rowPositions[currentButtonRows].y);
            Vector2 position = new Vector2(rowPositions[i + buttonRowOffset].x, rowPositions[currentButtonRows].y + spacing);
            GameObject button = Instantiate(buttonPrefab, position, Quaternion.identity);
            ButtonDisplay buttonDisplay = button.GetComponent<ButtonDisplay>();
            buttonDisplay.SetButton(buttonsToDisplay[i]);
            currentButtonObjects.Add(button);
        }
    }

    #endregion

    public void HandleButtonPress(KeyCode key)
    {
        if (!inputEnabled) return;

        if (currentButtonObjects[0] == null)
        {
            return;
        }

        GameObject currentButton = currentButtonObjects[0];
        KeyCode targetButtonKey = currentButton.GetComponent<ButtonDisplay>().GetCurrentKey();

        if (isSpamingButton)
        {

            if (targetButtonKey == key) 
            {
                GameManager.instance.myUIManager.UpdateSpamProgress(1);
                FindObjectOfType<AudioManager>().PlayAndChangePitch("TeclaFinal", 0.1f);
                screen.Shake(0.1f, 0.05f);
            }
            return;
        }

        if (targetButtonKey == key)
        {
            FindObjectOfType<AudioManager>().Play("Acierto");
            currentButtonCount++;
            Destroy(currentButton);
            currentButtonObjects.RemoveAt(0);

            if (currentButtonCount == buttonsPerCombination)
            {
                RowCleared();
                currentButtonCount = 0;
            }
        }
        else
        {
            FindObjectOfType<AudioManager>().Play("Error");
            GameManager.instance.myUIManager.ActivateErrorText(currentButtonCount + buttonRowOffset);
            RowFailed();
        }
    }

    public void RowCleared()
    {

        FindObjectOfType<AudioManager>().PlayRandom("Linea1", "Linea2", "Linea3", "Linea4");
        FindObjectOfType<AudioManager>().PlayRandom("EstirarCuerda1", "EstirarCuerda2", "EstirarCuerda3");
        FindObjectOfType<AudioManager>().PlayRandom("TirarCuerda1", "TirarCuerda2", "TirarCuerda3", "TirarCuerda4", "TirarCuerda5", "TirarCuerda6");

        GameManager.instance.myUIManager.DeactivateStartArrow();
        GameManager.instance.myAgeManager.PlayRowAnimation();
        MoveAllButtonsDown();
        ReassignButtonCombinations();
        
        currentButtonRows--;
        //UpdateShakeEffect();
    }

    private void MoveAllButtonsDown()
    {
        foreach (var buttonObject in currentButtonObjects)
        {
            //Vector3 position = buttonObject.transform.position;
            //position.y -= spacing; 
            //buttonObject.transform.position = position;
            buttonObject.GetComponent<ButtonDisplay>().MoveButton(spacing);
        }
    }


    public void RowFailed()
    {
        Debug.Log("Row failed");
        ClearLines(false);
        MoveAllButtonsDown();
        ReassignButtonCombinations();
        currentButtonRows--;

        CreateNewButtonRow();
        CreateNewButtonRow();

        FindObjectOfType<AudioManager>().PlayRandom("Dialogo1", "Dialogo2", "Dialogo3");
        FindObjectOfType<AudioManager>().PlayRandom("Exclamacion1", "Exclamacion2", "Exclamacion3");
        //UpdateShakeEffect();
    }

    private void ClearLines(bool clearAllLines)
    {
        if (currentButtonObjects.Count == 0) return;

        int buttonsToClear;

        if (clearAllLines)
        {
            buttonsToClear = currentButtonObjects.Count;
            currentButtonRows = 0;
        }
        else
        {
            buttonsToClear = buttonsPerCombination - currentButtonCount;
        }
        

        for (int i = 0; i < buttonsToClear; i++)
        {
            Destroy(currentButtonObjects[0]);
            currentButtonObjects.RemoveAt(0);
        }
        currentButtonCount = 0;
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

    public void EndCurrentLevel()
    {
        ClearLines(true);
        StopButtonGeneration();

        if (isLastLevel)
        {
            Debug.Log("YOU WIN");
            GameManager.instance.GameOver(true);
        }
        else
        {
            ActivateButtonSpam();
        }
        
    }

    private void ActivateButtonSpam()
    {
        GameManager.instance.myUIManager.ActivateStartArrow2();
        spamButton.SetActive(true);
        currentButtonObjects.Add(spamButton);
        isSpamingButton = true;
    }

    public void DeactivateButtonSpam()
    {
        GameManager.instance.myUIManager.DeactivateStartArrow2();
        spamButton.SetActive(false);
        currentButtonObjects.RemoveAt(0);
        isSpamingButton = false;
    }

    public void SetButtonGenerationValues(int newButtonsPerComb, float newSpawnRate, bool lastLevel)
    {
        isLastLevel = lastLevel;
        buttonsPerCombination = newButtonsPerComb;
        buttonCombinations = new List<KeyCode>[buttonsPerCombination, numberOfCombinations];
        buttonSpawnRate = newSpawnRate;

        if (buttonsPerCombination == 5)
        {
            buttonRowOffset = 1;
        }
        else if (buttonsPerCombination == 6) 
        {
            buttonRowOffset = 0;
        }
    }

    public void RestartButtonLevel()
    {
        ClearLines(true);
        StartButtonGeneration();
        inputEnabled = true;
    }


    public void UpdateShakeEffect()
    {
        foreach (var buttonObject in currentButtonObjects)
        {
            var spriteShake = buttonObject.GetComponent<KeyShake>();
            if (currentButtonRows >= 6)
            {
                spriteShake.StartShake(30f,0.05f); 
            }
            else if (currentButtonRows >= 5)
            {
                spriteShake.StartShake(20f,0.05f); 
            }
            else if (currentButtonRows >= 4)
            {
                spriteShake.StartShake(10f,0.05f); 
            }
            else
            {
                spriteShake.StopShake(); 
            }
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

    public void OnPauseButton(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            GameManager.instance.PauseGame();
        }
    }
    #endregion
}

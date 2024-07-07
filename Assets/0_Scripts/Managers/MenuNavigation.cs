using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using TMPro;

public class MenuNavigation : MonoBehaviour
{
    public Button[] menuOptions;
    public Transform[] positions;

    [Header("Screens")]
    public GameObject creditsScreen;
    public GameObject optionsScreen;
    public GameObject TemaScreen;

    [Header("Arrows")]
    public GameObject arrowRight;
    public GameObject arrowLeft;


    public float moveSpeed = 20f;
    public float spriteMove;
    public float moveDistance;
    public GameObject cuerda;

    public TMP_Text back1, back2, back3;

    private int currentIndex = 0;
    private bool canMove = true;
    private bool canSelect = true;
    private Vector3 targetPosition;

    private void Start()
    {
        if (menuOptions.Length > 0)
        {
            EventSystem.current.SetSelectedGameObject(menuOptions[2].gameObject); 
        }

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;


    }
    private void Update()
    {
        //Debug.Log(EventSystem.current.currentSelectedGameObject.name);
    }
    public void MoveRight()
    {
        if (canMove)
        {
            FindObjectOfType<AudioManager>().Play("Desplazamiento");
            StartCoroutine(MoveMenu(Vector3.right));
            StartCoroutine(Move(Vector3.left));
        }
    }

    public void MoveLeft()
    {
        if (canMove)
        {
            FindObjectOfType<AudioManager>().Play("Desplazamiento");
            StartCoroutine(MoveMenu(Vector3.left));
            StartCoroutine(Move(Vector3.left));
        }
    }

    public void SelectOption()
    {
        ///Debug.Log(canSelect);
        if (canSelect)
        {
            Debug.Log("SelectOption called");
            canSelect = false;
            StartCoroutine(CooldownSelectOption());
            int buttonInPosition2Index = (currentIndex + 2) % menuOptions.Length;
            

            switch (buttonInPosition2Index)
            {
                case 0:
                    FindObjectOfType<AudioManager>().Play("Click");
                    TemaScreen.SetActive(true);
                    canMove = false;
                    break;
                case 1:
                    if (optionsScreen.activeSelf)
                    {
                       
                        //FindObjectOfType<AudioManager>().Play("Atras");
                        //optionsScreen.SetActive(false);
                        //canMove = true;
                        
                    }
                    else
                    {
                        //DeactivateAllButtons();
                        FindObjectOfType<AudioManager>().Play("Click");
                        optionsScreen.SetActive(true);
                        canMove = false;
                        

                    }
                    break;
                case 2:
                    FindObjectOfType<AudioManager>().Play("Comenzar");
                    GameManager.instance.StartGame();
                    break;
                case 3:
                    if (creditsScreen.activeSelf)
                    {
                        
                        //creditsScreen.SetActive(false);
                        //canMove = true;
                        //FindObjectOfType<AudioManager>().Play("Atras");
                    }
                    else
                    {
                        //DeactivateAllButtons();

                        creditsScreen.SetActive(true);
                        canMove = false;
                        FindObjectOfType<AudioManager>().Play("Click");
                    }

                    break;
                case 4:
                    GameManager.instance.CloseGame();

                    break;

            }
        }
    }

    public void CancelOption()
    {
        if (canSelect)
        {
            canSelect = false;
            StartCoroutine(CooldownSelectOption());
            int buttonInPosition2Index = (currentIndex + 2) % menuOptions.Length;

            switch (buttonInPosition2Index)
            {
                case 0:
                    FindObjectOfType<AudioManager>().Play("Atras");
                    TemaScreen.SetActive(false);
                    canMove = true;
                    break;
                case 1:
                    if (optionsScreen.activeSelf)
                    {
                        FindObjectOfType<AudioManager>().Play("Atras");
                        optionsScreen.SetActive(false);
                        canMove = true;
                        //ActivateAllButtons();
                    }
                    
                    break;
                case 2:
                    
                    break;
                case 3:
                    if (creditsScreen.activeSelf)
                    {

                        creditsScreen.SetActive(false);
                        canMove = true;
                        FindObjectOfType<AudioManager>().Play("Atras");
                        //ActivateAllButtons();
                    }
                    break;
                case 4:
                    GameManager.instance.CloseGame();

                    break;

            }
        }
    }
    private IEnumerator CooldownSelectOption()
    {
        canSelect = false;
        yield return new WaitForSeconds(0.25f); // Cooldown duration, adjust as needed
        canSelect = true;
    }
    private IEnumerator MoveMenu(Vector3 direction)
    {
        canMove = false;
        arrowLeft.SetActive(false);
        arrowRight.SetActive(false);
        int previousIndex = currentIndex;
        
        if (direction == Vector3.right)
        {
            currentIndex = (currentIndex + 1) % menuOptions.Length;
        }
        else if (direction == Vector3.left)
        {
            currentIndex = (currentIndex - 1 + menuOptions.Length) % menuOptions.Length;
        }

        Vector3[] startPositions = new Vector3[menuOptions.Length];
        Vector3[] endPositions = new Vector3[menuOptions.Length];
        int[] newIndices = new int[menuOptions.Length];

        for (int i = 0; i < menuOptions.Length; i++)
        {
            startPositions[i] = menuOptions[i].transform.position;
            int newPositionIndex = (i - currentIndex + menuOptions.Length) % positions.Length;
            endPositions[i] = positions[newPositionIndex].position;
            newIndices[newPositionIndex] = i;
        }

        int wrapAroundIndex = -1;
        if (direction == Vector3.right)
        {
            wrapAroundIndex = (currentIndex - 1 + menuOptions.Length) % menuOptions.Length;
            menuOptions[wrapAroundIndex].gameObject.SetActive(false);
        }
        else if (direction == Vector3.left)
        {
            wrapAroundIndex = (currentIndex + 5) % menuOptions.Length;
            menuOptions[wrapAroundIndex].gameObject.SetActive(false);
        }

        float elapsedTime = 0f;
        float moveTime = 1f / moveSpeed; 

        while (elapsedTime < moveTime)
        {
            for (int i = 0; i < menuOptions.Length; i++)
            {
                if (i != wrapAroundIndex)
                {
                    menuOptions[i].transform.position = Vector3.Lerp(startPositions[i], endPositions[i], elapsedTime / moveTime);
                }
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        
        for (int i = 0; i < menuOptions.Length; i++)
        {
            menuOptions[i].transform.position = endPositions[i];
        }

        menuOptions[wrapAroundIndex].transform.position = endPositions[wrapAroundIndex];
        menuOptions[wrapAroundIndex].gameObject.SetActive(true);

        EventSystem.current.SetSelectedGameObject(menuOptions[newIndices[2]].gameObject);

        canMove = true;
        arrowLeft.SetActive(true);
        arrowRight.SetActive(true);
    }

    private IEnumerator Move(Vector3 direction)
    {
        canMove = false;
        Vector3 startPosition = cuerda.transform.position;
        targetPosition = startPosition + direction * moveDistance;

        float elapsedTime = 0f;
        float moveTime = moveDistance / moveSpeed;

        while (elapsedTime < moveTime)
        {
            cuerda.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / moveTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        cuerda.transform.position = targetPosition;
        canMove = true;
    }
    public void DeactivateAllButtons()
    {
        foreach (Button button in menuOptions)
        {
            if (button != menuOptions[1])
            {
                button.gameObject.SetActive(false);
            }
            
        }
    }

    public void ActivateAllButtons()
    {
        foreach (Button button in menuOptions)
        {
            //button.interactable = true;
            button.gameObject.SetActive(true);
        }
    }

    public GameObject[] GetTextObjects()
    {

        GameObject[] textObjectsArray = new GameObject[3] { back1.gameObject, back2.gameObject, back3.gameObject};
        

        return textObjectsArray;
    }
}

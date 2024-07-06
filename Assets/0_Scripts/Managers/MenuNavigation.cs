using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class MenuNavigation : MonoBehaviour
{
    public Button[] menuOptions;
    public Transform[] positions;

    [Header("Screens")]
    public GameObject creditsScreen;
    public GameObject optionsScreen;


    public float moveSpeed = 20f;
    private int currentIndex = 0;
    private bool canMove = true;
    private bool canSelect = true;

    private void Start()
    {
        if (menuOptions.Length > 0)
        {
            EventSystem.current.SetSelectedGameObject(menuOptions[2].gameObject); 
        }
    }

    public void MoveRight()
    {
        if (canMove)
        {
            FindObjectOfType<AudioManager>().Play("Desplazamiento");
            StartCoroutine(MoveMenu(Vector3.right));
        }
    }

    public void MoveLeft()
    {
        if (canMove)
        {
            FindObjectOfType<AudioManager>().Play("Desplazamiento");
            StartCoroutine(MoveMenu(Vector3.left));
        }
    }

    public void SelectOption()
    {
        Debug.Log(canSelect);
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
                    Debug.Log("we¡ll see");
                    break;
                case 1:
                    if (optionsScreen.activeSelf)
                    {
                        Debug.Log("DDD");
                        FindObjectOfType<AudioManager>().Play("Atras");
                        optionsScreen.SetActive(false);
                        canMove = true;
                    }
                    else
                    {
                        Debug.Log("AAA");
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

                        creditsScreen.SetActive(false);
                        canMove = true;
                        FindObjectOfType<AudioManager>().Play("Atras");
                    }
                    else
                    {
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
    private IEnumerator CooldownSelectOption()
    {
        canSelect = false;
        yield return new WaitForSeconds(0.25f); // Cooldown duration, adjust as needed
        canSelect = true;
    }
    private IEnumerator MoveMenu(Vector3 direction)
    {
        canMove = false;
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
    }


}

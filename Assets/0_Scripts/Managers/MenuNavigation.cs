using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

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
            StartCoroutine(MoveMenu(Vector3.right));
        }
    }

    public void MoveLeft()
    {
        if (canMove)
        {
            StartCoroutine(MoveMenu(Vector3.left));
        }
    }

    public void SelectOption()
    {
        int buttonInPosition2Index = (currentIndex + 2) % menuOptions.Length;

        
        switch (buttonInPosition2Index)
        {
            case 0:
                Debug.Log("we¡ll see");
                break;
            case 1:
                if (optionsScreen.activeSelf)
                {
                    optionsScreen.SetActive(false);
                    canMove= true;
                }
                else
                {
                    optionsScreen.SetActive(true);
                    canMove = false;

                }
                break;
            case 2:
                GameManager.instance.StartGame();
                break;
            case 3:
                if (creditsScreen.activeSelf)
                {
                    creditsScreen.SetActive(false);
                    canMove = true;

                }
                else
                {
                    creditsScreen.SetActive(true);
                    canMove = false;

                }

                break;
            case 4:
                GameManager.instance.CloseGame();
                break;
           
        }
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

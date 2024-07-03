using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreenControl : MonoBehaviour
{
    void Update()
    {
        if (Input.anyKeyDown)
        {
            LoadNextScene();
        }
    }

    void LoadNextScene()
    {
        // Make sure to add the next scene in the build settings
        SceneManager.LoadScene("MainMenu"); // Replace "GameScene" with your actual game scene name
    }
}

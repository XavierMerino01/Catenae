using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsRolll : MonoBehaviour
{
    public RectTransform creditsTransform; 
    public float speed = 20f; 

    private void Update()
    {
        
        creditsTransform.anchoredPosition += new Vector2(0, -speed * Time.deltaTime);
    }
}

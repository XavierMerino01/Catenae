using UnityEngine;
using UnityEngine.UI;

public class ArroMoovement : MonoBehaviour
{
    public RectTransform arrowTransform; 
    public float speed = 200f; 
    public float distance = 500f; 
    public bool movingRight;

    private float startX; 
    private float endX; 

    private void Start()
    {
        
        startX = arrowTransform.anchoredPosition.x - distance / 2;
        endX = arrowTransform.anchoredPosition.x + distance / 2;
    }

    private void Update()
    {
        
        if (movingRight)
        {
            arrowTransform.anchoredPosition += new Vector2(speed * Time.deltaTime, 0);
            if (arrowTransform.anchoredPosition.x >= endX)
            {
                movingRight = false; 
            }
        }
        else
        {
            arrowTransform.anchoredPosition -= new Vector2(speed * Time.deltaTime, 0);
            if (arrowTransform.anchoredPosition.x <= startX)
            {
                movingRight = true; 
            }
        }
    }
}

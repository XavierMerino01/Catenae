using UnityEngine;

public class ButtonDisplay : MonoBehaviour
{
    public Sprite wSprite;
    public Sprite aSprite;
    public Sprite sSprite;
    public Sprite dSprite;
    public Sprite upArrowSprite;
    public Sprite downArrowSprite;
    public Sprite leftArrowSprite;
    public Sprite rightArrowSprite;

    private SpriteRenderer spriteRenderer;
    private KeyCode currentKey;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetButton(KeyCode key)
    {
        currentKey = key;

        switch (key)
        {
            case KeyCode.W:
                spriteRenderer.sprite = wSprite;
                break;
            case KeyCode.A:
                spriteRenderer.sprite = aSprite;
                break;
            case KeyCode.S:
                spriteRenderer.sprite = sSprite;
                break;
            case KeyCode.D:
                spriteRenderer.sprite = dSprite;
                break;
            case KeyCode.UpArrow:
                spriteRenderer.sprite = upArrowSprite;
                break;
            case KeyCode.DownArrow:
                spriteRenderer.sprite = downArrowSprite;
                break;
            case KeyCode.LeftArrow:
                spriteRenderer.sprite = leftArrowSprite;
                break;
            case KeyCode.RightArrow:
                spriteRenderer.sprite = rightArrowSprite;
                break;
        }
    }

    public KeyCode GetCurrentKey()
    {
        return currentKey;
    }
}

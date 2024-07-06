using System.Collections;
using Unity.VisualScripting;
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

    public Sprite[] usedSprites;

    public Sprite[] keySprites;
    public Sprite[] xboxSprites;

    private SpriteRenderer spriteRenderer;
    private KeyCode currentKey;

    private Coroutine moveAnimCoroutine;
    private float currentYposTarget;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        SpawnAnimation();
    }

    public void SetButton(KeyCode key)
    {
        currentKey = key;

        switch (key)
        {
            case KeyCode.W:
                spriteRenderer.sprite = usedSprites[0];
                break;
            case KeyCode.A:
                spriteRenderer.sprite = usedSprites[1];
                break;
            case KeyCode.S:
                spriteRenderer.sprite = usedSprites[2];
                break;
            case KeyCode.D:
                spriteRenderer.sprite = usedSprites[3];
                break;
            case KeyCode.UpArrow:
                spriteRenderer.sprite = usedSprites[4];
                break;
            case KeyCode.DownArrow:
                spriteRenderer.sprite = usedSprites[5];
                break;
            case KeyCode.LeftArrow:
                spriteRenderer.sprite = usedSprites[6];
                break;
            case KeyCode.RightArrow:
                spriteRenderer.sprite = usedSprites[7];
                break;
        }
    }

    public void AssignPcKeys()
    {
        for (int i = 0; i < usedSprites.Length; i++)
        {
            usedSprites[i] = keySprites[i];
        }
        SetButton(currentKey);
    }

    public void AssignXboxKeys()
    {
        for (int i = 0; i < usedSprites.Length; i++)
        {
            usedSprites[i] = xboxSprites[i];
        }
        SetButton(currentKey);
    }

    public KeyCode GetCurrentKey()
    {
        return currentKey;
    }

    protected virtual void SpawnAnimation()
    {
        MoveButton(0.7f);
    }

    public void MoveButton(float distance)
    {
        if (moveAnimCoroutine != null)
        {
            StopCoroutine(moveAnimCoroutine);
            float newDistance = (transform.position.y - currentYposTarget) + distance;
            currentYposTarget = transform.position.y - newDistance;
            moveAnimCoroutine = StartCoroutine(MoveButtonCoroutine(newDistance, 0.5f));
            return;
        }

        currentYposTarget = transform.position.y - distance;
        moveAnimCoroutine = StartCoroutine(MoveButtonCoroutine(distance, 0.5f));
    }

    private IEnumerator MoveButtonCoroutine(float distance, float duration)
    {
        Vector3 startPosition = transform.position;
        Vector3 endPosition = new Vector3(startPosition.x, startPosition.y - distance, startPosition.z);
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = endPosition;
        moveAnimCoroutine = null; // Reset the coroutine reference once the movement is complete
        FindObjectOfType<ButtonCombinationHandler>().UpdateShakeEffect();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteGradient : MonoBehaviour
{
    public Gradient gradient;
    public int textureWidth;
    public int textureHeight;

    void Start()
    {
        Texture2D texture = new Texture2D(textureWidth, textureHeight);
        for (int y = 0; y < textureHeight; y++)
        {
            for (int x = 0; x < textureWidth; x++)
            {
                float t = (float)y / (textureHeight - 1);
                Color color = gradient.Evaluate(t);
                texture.SetPixel(x, y, color);
            }
        }
        texture.Apply();

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = Sprite.Create(texture, new Rect(0, 0, textureWidth, textureHeight), new Vector2(0.5f, 0.5f));
    }
}

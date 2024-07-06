using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData
{
    public float levelDuration;
    public float buttonsPerCombination;
    public float buttonSpawnRate;
}

[System.Serializable]
public class AgeData
{
    public enum HumanAge
    {
        Prehistory,
        Egipt,
        Rome,
        MiddleAge,
        Renacentism,
        ContemporaryAge
    }
    public HumanAge levelAge;

    public LevelData levelData;

    public GameObject ageSpritesParent;

    [HideInInspector] public SpriteRenderer[] ageSprites;

}
public class AgesManager : MonoBehaviour
{
    public AgeData[] Ages;
    private int currentAgeIndex = 1;

    private void Start()
    {
        AssignChildrenSprites();
    }

    private void AssignChildrenSprites()
    {
        Transform currentAgeParent = Ages[currentAgeIndex].ageSpritesParent.transform;
        Ages[currentAgeIndex].ageSprites = new SpriteRenderer[currentAgeParent.childCount];

        for (int i = 0; i < currentAgeParent.childCount; i++)
        {
            Ages[currentAgeIndex].ageSprites[i] = currentAgeParent.GetChild(i).GetComponent<SpriteRenderer>();
        }
    }

    public void SwapMaskInteraction()
    {
        foreach (SpriteRenderer sprite in Ages[currentAgeIndex].ageSprites)
        {
            sprite.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
        }

        Ages[currentAgeIndex - 1].ageSpritesParent.SetActive(false);

        currentAgeIndex++;
        if (currentAgeIndex < Ages.Length - 1)
        {
            //TO DO: Next Level
            AssignChildrenSprites();
        }
        else
        {
            Debug.Log("YOU WIN");
            //TO DO: YOU WIN
        }
    }
}

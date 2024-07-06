using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData
{
    public float levelDuration;
    public int buttonsPerCombination;
    public float buttonSpawnRate;
    public Sprite timerHandleSprite;
    public bool isLastAge;
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
    public string pullAnimName;

}
public class AgesManager : MonoBehaviour
{
    public AgeData[] Ages;
    private int currentAgeIndex = 1;

    private List<Animator> currentAgeAnimators = new List<Animator>();
    private string animationName;

    private void Start()
    {
        AssignChildrenSprites();
    }

    private void AssignChildrenSprites()
    {
        PopulateAnimators();
        Transform currentAgeParent = Ages[currentAgeIndex].ageSpritesParent.transform;
        Ages[currentAgeIndex].ageSprites = new SpriteRenderer[currentAgeParent.childCount];

        for (int i = 0; i < currentAgeParent.childCount; i++)
        {
            Ages[currentAgeIndex].ageSprites[i] = currentAgeParent.GetChild(i).GetComponent<SpriteRenderer>();
        }
    }

    private void PopulateAnimators()
    {
        currentAgeAnimators.Clear();
        animationName = Ages[currentAgeIndex - 1].pullAnimName;
        Transform currentAgeParent = Ages[currentAgeIndex-1].ageSpritesParent.transform;

        for (int i = 0; i < currentAgeParent.childCount; i++)
        {
            Animator newAnim = currentAgeParent.GetChild(i).GetComponent<Animator>();
            if (newAnim != null)
            {
                currentAgeAnimators.Add(newAnim);
            }
        }
    }

    public void PlayRowAnimation()
    {
        foreach (Animator anim in currentAgeAnimators)
        {
            anim.Play(animationName);
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

        if (currentAgeIndex != Ages.Length)
        {
            Ages[currentAgeIndex].ageSpritesParent.SetActive(true);
            AssignChildrenSprites();
        }

    }

    public LevelData GetNextLevelData()
    {
        return Ages[currentAgeIndex].levelData;
    }

    public AgeData GetAgeData()
    {
        return Ages[currentAgeIndex];
    }
}

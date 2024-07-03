using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomButtonGenerator : MonoBehaviour
{
    private readonly List<KeyCode> possibleButtons = new List<KeyCode>
    {
        KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D,
        KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.LeftArrow, KeyCode.RightArrow
    };

    public List<KeyCode> GenerateButtonCombination(int buttonsPerCombination)
    {
        List<KeyCode> buttonCombination = new List<KeyCode>();
        for (int j = 0; j < buttonsPerCombination; j++)
        {
            buttonCombination.Add(possibleButtons[Random.Range(0, possibleButtons.Count)]);
        }
        return buttonCombination;
    }

}

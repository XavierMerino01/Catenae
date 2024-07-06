using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskHandler : MonoBehaviour
{
    public void MaskAnimationOver()
    {
        GameManager.instance.OnLevelTransitionEnd();
    }
}

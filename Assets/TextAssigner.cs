using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextAssigner : MonoBehaviour
{
    public TextData[] textDatas;
    public AgesManager ageManager;
    public TextMeshProUGUI[] errorObjTexts;

    private void Start()
    {
        ageManager = GetComponent<AgesManager>();
    }

    public void AssignRandomAgeText(int missedButtonIndex)
    {
        AgeData.HumanAge currentAge = ageManager.GetPreviousAgeData().levelAge;
        int textDatasIndex = 0;

        switch (currentAge)
        {
            case AgeData.HumanAge.Prehistory: 
                textDatasIndex = 0; 
                break;
            case AgeData.HumanAge.Egipt:
                textDatasIndex = 1;
                break;
            case AgeData.HumanAge.Rome:
                textDatasIndex = 2;
                break;
            case AgeData.HumanAge.MiddleAge:
                textDatasIndex = 3;
                break;
            case AgeData.HumanAge.Renacentism:
                textDatasIndex = 4;
                break;
            case AgeData.HumanAge.ContemporaryAge:
                textDatasIndex = 5;
                break;
        }

        int rand = Random.Range(0, textDatas[textDatasIndex].Texts.Length);
        string textToShow = textDatas[textDatasIndex].Texts[rand];
        errorObjTexts[missedButtonIndex].text = textToShow;
    }

    public void FinalTransition()
    {
        AssignFinalTexts();
        StartCoroutine(ActivateErrorTextsSequentially());
    }

    private void AssignFinalTexts()
    {
        for (int i = 0; i < textDatas[6].Texts.Length; i++) 
        {
            errorObjTexts[i].text = textDatas[6].Texts[i];
        }
    }

    private IEnumerator ActivateErrorTextsSequentially()
    {
        Transform previousGrandparent = null;

        for (int i = 0; i < errorObjTexts.Length; i++)
        {
            if (previousGrandparent != null)
            {
                previousGrandparent.gameObject.SetActive(false);
            }

            Transform currentGrandparent = errorObjTexts[i].transform.parent?.parent;
            if (currentGrandparent != null)
            {
                currentGrandparent.gameObject.SetActive(true);
                previousGrandparent = currentGrandparent;
            }

            yield return new WaitForSeconds(3.0f);
        }
        if (previousGrandparent != null)
        {
            previousGrandparent.gameObject.SetActive(false);
        }
        GameManager.instance.myUIManager.UIWin();
    }
}

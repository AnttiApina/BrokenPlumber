using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text timeTxt;
    public Text scoreTxt;
    public Text mushroomTimeTxt;
    public Text mushroomCountTxt;

    public delegate int UpdateUI();
    public static event UpdateUI OnUpdateTime;
    public static event UpdateUI OnUpdateScore;
    public static event UpdateUI OnUpdateMushroomTime;
    public static event UpdateUI OnUpdateMushroomCount;
    


    // Update is called once per frame
    void Update()
    {
        if (OnUpdateScore != null)
        {
            UpdateText(scoreTxt, "Score:", OnUpdateScore());
            OnUpdateScore = null;
        }

        if (OnUpdateTime != null)
        {
            UpdateText(timeTxt, "Time:", OnUpdateTime());
            OnUpdateTime = null;
        }

        if (OnUpdateMushroomCount != null)
        {
            UpdateText(mushroomCountTxt, "Mushrooms:", OnUpdateMushroomCount());
            OnUpdateMushroomCount = null;
        }

        if (OnUpdateMushroomTime != null)
        {
            UpdateText(mushroomTimeTxt, "Mushroom time:", OnUpdateMushroomTime());
            OnUpdateMushroomTime = null;
        }
        
    }


    private void UpdateText(Text uiText, String prefix, int value)
    {
        uiText.text = prefix + " " + value;
    }

}

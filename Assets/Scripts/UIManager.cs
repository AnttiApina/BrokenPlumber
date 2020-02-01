using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text timeTxt;
    public Text scoreTxt;

    public delegate int UpdateScore();
    public static event UpdateScore OnUpdateScore;
    
    public delegate int UpdateTime();
    public static event UpdateTime OnUpdateTime;

    // Update is called once per frame
    void Update()
    {
        if (OnUpdateScore != null)
        {
            int score = OnUpdateScore();
            scoreTxt.text = "Score: " + score;
            OnUpdateScore = null;
        }

        if (OnUpdateTime != null)
        {
            int time = OnUpdateTime();
            timeTxt.text = "Time: " + time;
            OnUpdateTime = null;
        }
    }

}

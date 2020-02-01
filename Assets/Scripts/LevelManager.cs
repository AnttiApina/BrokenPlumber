using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public delegate Appliance OnApplianceRepaired();
    public static event OnApplianceRepaired ApplianceRepairedEvent;

    private Appliance[] _appliances;
    private int _applianceToFixIndex;
    
    public int playerScore;
    public int time = 60 * 8;

    private void Start()
    {
        _appliances = FindObjectsOfType<Appliance>().OrderBy(app => app.order).ToArray();
        _appliances[_applianceToFixIndex].SetBroken();
        StartCoroutine(Tick());
    }

    // Update is called once per frame
    void Update()
    {
        if (ApplianceRepairedEvent != null)
        {
            // Appliance appliance = ApplianceRepairedEvent();
            ApplianceRepairedEvent = null;
            _applianceToFixIndex = _applianceToFixIndex < _appliances.Length - 1 ? _applianceToFixIndex + 1 : 0;
            _appliances[_applianceToFixIndex].SetBroken();
            playerScore++;
        }
    }

    IEnumerator Tick()
    {
        while (time > 0)
        {
            yield return new WaitForSeconds(1);
            time -= 5;
        }
        EndGame();
    }

    private void EndGame()
    {
        SceneManager.LoadScene(0);
    }
}

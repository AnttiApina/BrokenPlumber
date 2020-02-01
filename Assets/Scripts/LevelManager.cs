using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public delegate Appliance OnApplianceRepaired();
    public static event OnApplianceRepaired ApplianceRepairedEvent;

    private Appliance[] _appliances;
    private int _applianceToFixIndex;
    
    public int _playerScore;
    public int _time = 60 * 8;

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
            _playerScore++;
        }
    }

    IEnumerator Tick()
    {
        yield return new WaitForSeconds(1);
        _time -= 10;
        
    }
}

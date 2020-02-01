using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public delegate Appliance OnApplianceRepaired();
    public static event OnApplianceRepaired ApplianceRepairedEvent;

    private Appliance[] _appliances;

    private void Start()
    {
        _appliances = FindObjectsOfType<Appliance>();
        Debug.Log("Number of appliances: " + _appliances.Length);
    }

    // Update is called once per frame
    void Update()
    {
        if (ApplianceRepairedEvent != null)
        {
            Appliance appliance = ApplianceRepairedEvent();
            Vector3 appliancePos = appliance.transform.position;
            Debug.Log(appliancePos.x + " " + appliancePos.y);
            ApplianceRepairedEvent = null;
        }
    }
}

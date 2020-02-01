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

    public delegate void OnMushroomEffectChange(bool state);

    public static event OnMushroomEffectChange MushroomEffectChangeEvent;
    private bool _pressed_mushroom_key = false;

    private Appliance[] _appliances;
    private int _applianceToFixIndex;
    
    public int playerScore;
    public int time = 60 * 8;
    
    public int mushroomEffectTime = 3;
    private int mushroomEffectDefaultTime;
    private int mushrooms = 10; 
    
    private void Start()
    {
        // MushroomEffectChangeEvent = defaultFn;
        _appliances = FindObjectsOfType<Appliance>().OrderBy(app => app.order).ToArray();
        _appliances[_applianceToFixIndex].SetBroken();
        mushroomEffectDefaultTime = mushroomEffectTime;
        StartCoroutine(Tick());

        UIManager.OnUpdateTime += () => time;
        UIManager.OnUpdateScore += () => playerScore;
        UIManager.OnUpdateMushroomTime += () => 0;
        UIManager.OnUpdateMushroomCount += () => mushrooms;
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
            UIManager.OnUpdateScore += () => playerScore;
        }
        
        _pressed_mushroom_key |= Input.GetKeyDown(KeyCode.T);
        if (_pressed_mushroom_key && mushroomEffectTime == mushroomEffectDefaultTime && mushrooms > 0)
        {
            MushroomEffectChangeEvent?.Invoke(true);
            mushrooms--;
            UIManager.OnUpdateMushroomCount += () => mushrooms;
            StartCoroutine(StartMushroomTimer());
        }
        _pressed_mushroom_key = false;
    }

    IEnumerator Tick()
    {
        while (time > 0)
        {
            yield return new WaitForSeconds(1);
            time -= 5;
            UIManager.OnUpdateTime += () => time;
        }
        EndGame();
    }

    IEnumerator StartMushroomTimer()
    {
        while (mushroomEffectTime > 0)
        {
            mushroomEffectTime -= 1;
            UIManager.OnUpdateMushroomTime += () => mushroomEffectTime + 1;
            yield return new WaitForSeconds(1);
            UIManager.OnUpdateMushroomTime += () => mushroomEffectTime + 1;
        }
        UIManager.OnUpdateMushroomTime += () => 0;

        MushroomEffectChangeEvent?.Invoke(false);
        mushroomEffectTime = mushroomEffectDefaultTime;
    }

    private void EndGame()
    {
        SceneManager.LoadScene(0);
    }
}

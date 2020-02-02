using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

public class IntroContro : MonoBehaviour
{
    // Start is called before the first frame update
    public void GoToStart()
    {
        SceneManager.LoadScene(0);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            GoToStart();
        }
    }
}

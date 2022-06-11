using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

public class TL_Manager : MonoBehaviour
{

    private TX_Manager manager => FindObjectOfType<TX_Manager>();
    private bool done = false;

    private void Update()
    {
        if (Keyboard.current.zKey.wasPressedThisFrame)
        {
            // next hand
            if(!done)
                done = manager.NextHand();
        }

        else if (Keyboard.current.nKey.wasPressedThisFrame)
        {
            // start t0
            manager.SwitchScene();
        }
        else if (Keyboard.current.kKey.wasPressedThisFrame)
        {
            // skip to part 2 game scene
            StartCoroutine(SceneSwitchTimer());

        }
    }

    IEnumerator SceneSwitchTimer()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("MainGame");
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TX_Manager : MonoBehaviour
{
    Settings settings => FindObjectOfType<Settings>();
    [SerializeField] private string NextSceneName;

    Coroutine sswitch = null;
    public void SwitchScene()
    {
        if(sswitch == null)
            sswitch = StartCoroutine(SceneSwitchTimer());
    }

    IEnumerator SceneSwitchTimer()
    {
        yield return new WaitForSeconds(3f);
        settings.GoToScene(NextSceneName);
    }

    public bool NextHand()
    {
        return settings.FinishT_Task();
    }
}

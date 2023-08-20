using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class PressToPlay : MonoBehaviour
{
    public bool isPressable = false;

    public string sceneName = "Monday";
    void Update()
    {
        if(isPressable) {
            if(Input.GetKeyDown(KeyCode.Return)) {
                // CutToPlay();
                DayManager.instance.StartFirstDay();
                // WipeToPlay();
            }
        }
    }

    public void CutToPlay()
    {
        SceneController.TransitToSceneCut(sceneName,3.0f,Interpolation.Easing.Style.Quart,Color.black);
    }

    public void FadeToPlay()
    {
        SceneController.TransitToSceneFade(sceneName,3.0f,Interpolation.Easing.Style.Quart,Color.black,null,Transition.IOType.OutIn);
    }

    public void WipeToPlay()
    {
        SceneController.TransitToSceneWipe(sceneName,3.0f,Interpolation.Easing.Style.Quart,Color.black);
    }

    public void Quit()
    {
        Application.Quit();
    }
}

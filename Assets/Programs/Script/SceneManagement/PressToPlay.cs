using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class PressToPlay : MonoBehaviour
{
    public bool isPressable = false;

    public string sceneName = "SampleScene";
    void Update()
    {
        if(isPressable) {
            if(Input.GetKeyDown(KeyCode.Space)) {
                // CutToPlay();
                FadeToPlay();
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
        SceneController.TransitToSceneFade(sceneName,3.0f,Interpolation.Easing.Style.Quart,Color.black);
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

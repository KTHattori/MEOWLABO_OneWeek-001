using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedButton : MonoBehaviour
{
    AnimatedUI.AnimatedImage image;
    AnimatedUI.AnimatedTMPro text;
    void Reset()
    {
        
    }

    void AddDefaultState()
    {
        image.AddState("Selected");
        image.AddState("Pressed");
        image.AddState("Disabled");
    }
}

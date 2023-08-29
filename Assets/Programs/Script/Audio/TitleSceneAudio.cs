using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleSceneAudio : MonoBehaviour
{
    [SerializeField]
    private AudioClip bgmClip;
    

    // Start is called before the first frame update
    void Start()
    {
        Invoke("Play",0.1f);
    }

    public void Play()
    {
        AudioManager.instance.PlayBGM(bgmClip,0.2f,true,AudioManager.ChannelState.ChannelA);
    }
}

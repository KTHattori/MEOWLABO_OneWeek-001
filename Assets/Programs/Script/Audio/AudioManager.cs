using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;

public class AudioManager : MonoSingleton<AudioManager>
{
    public enum ChannelState
    {
        Silent,
        ChannelA,
        ChannelB
    }

    [System.Serializable]
    public class Channel
    {
        public string channelName;
        public AudioMixerGroup bgmMixerGroup;
        public AudioMixerSnapshot[] snapshotList;
        public AudioSource bgmAudioSource;
    }


    [SerializeField]
    private string audioResourcePath = "Sounds/";
    [SerializeField]
    private AudioMixer audioMixer;
    [SerializeField]
    private Channel channelA;
    [SerializeField]
    private Channel channelB;
    [SerializeField,Range(0.0f,1.0f)]
    private float channelWeightB = 1.0f;

    [SerializeField]
    private AudioMixerSnapshot silentSnapshot;

    [SerializeField,Header("Main Mixer Group")]
    private AudioMixerGroup bgmMixerGroup;
    [SerializeField]
    private AudioMixerGroup sfxMixerGroup;
    [SerializeField,Header("Zero Disance Audio Source")]
    private AudioSource sfxAudioSource;
    
    private int currentSnapshotIndex = 0;

    [SerializeField]
    private ChannelState channelType = ChannelState.Silent;

    [SerializeField,Header("Initial Channel")]
    private ChannelState initialChannel = ChannelState.ChannelA;
    [SerializeField]
    private float entryTime = 1.0f;
    [SerializeField]
    private float entryDelay = 0.0f;
    private bool entry = false;

    public AudioMixerSnapshot CurrentSnapShot
    {
        get 
        {
            switch(channelType)
            {
                case ChannelState.Silent:
                    return silentSnapshot;
                case ChannelState.ChannelA:
                    return channelA.snapshotList[currentSnapshotIndex];
                case ChannelState.ChannelB:
                    return channelB.snapshotList[currentSnapshotIndex];
                default:
                    return silentSnapshot;
            }
        }
    }

    public AudioMixerSnapshot OppositeSnapShot
    {
        get 
        {
            switch(channelType)
            {
                case ChannelState.Silent:
                    return silentSnapshot;
                case ChannelState.ChannelA:
                    return channelB.snapshotList[currentSnapshotIndex];
                case ChannelState.ChannelB:
                    return channelA.snapshotList[currentSnapshotIndex];
                default:
                    return silentSnapshot;
            }
        }
    }
    
    void Start()
    {
        channelType = ChannelState.Silent;
        switch(initialChannel)
        {
            case ChannelState.Silent:
                channelType = ChannelState.Silent;
                break;
            case ChannelState.ChannelA:
                channelType = ChannelState.ChannelA;
                CurrentSnapShot.TransitionTo(entryTime);
                break;
            case ChannelState.ChannelB:
                channelType = ChannelState.ChannelB;
                CurrentSnapShot.TransitionTo(entryTime);
                break;
        }
    }
    
    public void FadeOut(float time = 1.0f)
    {
        AudioMixerSnapshot[] snapshots = new AudioMixerSnapshot[2] { CurrentSnapShot, silentSnapshot };
        float[] weights = new float[2] { 0.0f, 1.0f };
        audioMixer.TransitionToSnapshots(snapshots, weights, time);
        channelType = ChannelState.Silent;
    }

    public void FadeIn(float time = 1.0f)
    {
        AudioMixerSnapshot[] snapshots = new AudioMixerSnapshot[2] { silentSnapshot, CurrentSnapShot };
        float[] weights = new float[2] { 0.0f, 1.0f };
        audioMixer.TransitionToSnapshots(snapshots, weights, time);
    }

    public void CrossFadeChannel(float time = 0.0f)
    {
        if(channelType == ChannelState.Silent) return;

        if (channelType == ChannelState.ChannelA)
        {
            AudioMixerSnapshot[] snapshots = new AudioMixerSnapshot[2] { channelA.snapshotList[currentSnapshotIndex], channelB.snapshotList[currentSnapshotIndex] };
            float[] weights = new float[2] { 0.0f, 1.0f };
            audioMixer.TransitionToSnapshots(snapshots, weights, time);
            channelType = ChannelState.ChannelB;
        }
        else
        {
            AudioMixerSnapshot[] snapshots = new AudioMixerSnapshot[2] { channelB.snapshotList[currentSnapshotIndex], channelA.snapshotList[currentSnapshotIndex] };
            float[] weights = new float[2] { 0.0f, 1.0f };
            audioMixer.TransitionToSnapshots(snapshots, weights, time);
            channelType = ChannelState.ChannelA;
        }
    }

    public void SetBlendLevel(float weight,float time = 0.0f)
    {
        if(channelType == ChannelState.Silent) return;

        AudioMixerSnapshot[] snapshots = new AudioMixerSnapshot[2] { CurrentSnapShot, OppositeSnapShot };
        float[] weights = new float[2] { weight, 1.0f - weight };
        audioMixer.TransitionToSnapshots(snapshots, weights, time);
    }

    public void PlaySFX(string clipName)
    {
        AudioClip clip = Resources.Load<AudioClip>(audioResourcePath + clipName);
        sfxAudioSource.PlayOneShot(clip);
    }

    public void PlaySFX(AudioClip clip,float volume = 1.0f)
    {
        sfxAudioSource.PlayOneShot(clip,volume);
    }

    public void PlayBGM(string clipName,ChannelState type = ChannelState.ChannelA)
    {
        AudioClip clip = Resources.Load<AudioClip>(audioResourcePath + clipName);
        if(type == ChannelState.ChannelA)
        {
            channelA.bgmAudioSource.clip = clip;
            channelA.bgmAudioSource.Play();
        }
        else
        {
            channelB.bgmAudioSource.clip = clip;
            channelB.bgmAudioSource.Play();
        }
    }

    public void PlayBGM(AudioClip clip,float volume = 1.0f,bool isLoop = true,ChannelState type = ChannelState.ChannelA,float fadeTime = 0.0f)
    {
        if(type == ChannelState.ChannelA)
        {
            channelA.bgmAudioSource.clip = clip;
            channelA.bgmAudioSource.volume = volume;
            channelA.bgmAudioSource.loop = isLoop;
            channelA.bgmAudioSource.Play();
            channelType = ChannelState.ChannelA;
        }
        else
        {
            channelB.bgmAudioSource.clip = clip;
            channelB.bgmAudioSource.volume = volume;
            channelB.bgmAudioSource.loop = isLoop;
            channelB.bgmAudioSource.Play();
            channelType = ChannelState.ChannelB;
        }

        FadeIn(fadeTime);
    }

    public void StopBGM(float fadeTime)
    {
        if(fadeTime > 0.0f)
        {
            FadeOut(fadeTime);
            Invoke("StopBGM",fadeTime);
        }
        else
        {
            StopBGM();
        }
    }

    public void StopBGM()
    {
        switch(channelType)
        {
            case ChannelState.ChannelA:
                channelA.bgmAudioSource.Stop();
                channelType = ChannelState.Silent;
                break;
            case ChannelState.ChannelB:
                channelB.bgmAudioSource.Stop();
                channelType = ChannelState.Silent;
                break;
        }
    }

    public void StopBGMCompletely()
    {
        channelA.bgmAudioSource.Stop();
        channelB.bgmAudioSource.Stop();
    }

    public void SetBGMVolume(float volume,ChannelState type)
    {
        if(type == ChannelState.ChannelA)
        {
            channelA.bgmAudioSource.volume = volume;
        }
        else
        {
            channelB.bgmAudioSource.volume = volume;
        }
    }

    public void SetSFXVolume(float volume)
    {
        sfxAudioSource.volume = volume;
    }

    public void MuteBGM(ChannelState type = ChannelState.ChannelA)
    {
        if(type == ChannelState.ChannelA)
        {
            channelA.bgmAudioSource.mute = true;
        }
        else
        {
            channelB.bgmAudioSource.mute = true;
        }
    }

    public void SetSFXVolume(bool isOn)
    {
        sfxAudioSource.mute = !isOn;
    }

}

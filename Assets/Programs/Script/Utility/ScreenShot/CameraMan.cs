using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CameraMan : MonoSingleton<CameraMan>
{

    [Header("保存先の設定")]
    [SerializeField]
    string folderName = "ScreenShots";
    [Header("撮影に使用するキー")]
    [SerializeField]
    KeyCode key = KeyCode.None;

    bool isCreatingScreenShot = false;
    string path;

    void Start()
    {
        path = Application.dataPath + "/Private/" + folderName + "/";
    }

    void Update()
    {
        if(Input.GetKeyDown(key))
        {
            PrintScreen();
        }
    }

    public void PrintScreen()
    {
        StartCoroutine("PrintScreenInternal");
    }

    IEnumerator PrintScreenInternal()
    {
        if (isCreatingScreenShot)
        {
            yield break;
        }

        isCreatingScreenShot = true;

        yield return null;

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        string date = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        string fileName = path + "ScreenShot_" + date + ".png";

        ScreenCapture.CaptureScreenshot(fileName);

        yield return new WaitUntil(() => File.Exists(fileName));

        isCreatingScreenShot = false;
    }

}
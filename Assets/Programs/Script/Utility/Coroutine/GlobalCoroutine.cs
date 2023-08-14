// 引用元 : https://hacchi-man.hatenablog.com/entry/2020/12/04/220000

using System;
using System.Collections;
using UnityEngine;
 
public static class GlobalCoroutine
{
    private static readonly CoroutineManager _i;
 
    static GlobalCoroutine()
    {
        var obj = new GameObject("CoroutineManager");
        _i = obj.AddComponent<CoroutineManager>();
        GameObject.DontDestroyOnLoad(_i);
    }
 
    /// <summary>
    /// Coroutine を開始する
    /// </summary>
    public static void Play(IEnumerator e, Action end = null, string key = "")
    {
        _i.Play(e, end, key);
    }
 
    public static void Stop(string key)
    {
        _i.Stop(key);
    }
 
    public static void StopAll()
    {
        _i.StopAll();
    }
}
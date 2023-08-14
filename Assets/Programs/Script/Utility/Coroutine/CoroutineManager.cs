// 引用元 : https://hacchi-man.hatenablog.com/entry/2020/12/04/220000

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
internal class CoroutineManager : MonoBehaviour
{
    private Dictionary<string, Coroutine> _dic = new Dictionary<string, Coroutine>();
 
    internal void Stop(string key)
    {
        if (!_dic.ContainsKey(key))
        {
            Debug.LogWarning($"{key} not found");
            return;
        }
 
        StopCoroutine(_dic[key]);
        _dic[key] = null;
        _dic.Remove(key);
    }
 
    internal void StopAll()
    {
        StopAllCoroutines();
    }
 
    internal void Play(IEnumerator e, Action end = null, string key = "")
    {
        var isCache = !string.IsNullOrEmpty(key) && !_dic.ContainsKey(key);
        if (isCache)
        {
            end += () =>
            {
                if (_dic.ContainsKey(key))
                    _dic.Remove(key);
            };
        }
        var c = StartCoroutine(Coroutine(e, end));
        if (isCache)
        {
            _dic.Add(key, c);
        }
    }
 
    private IEnumerator Coroutine(IEnumerator e, Action end = null)
    {
        yield return e;
        end?.Invoke();
    }
}
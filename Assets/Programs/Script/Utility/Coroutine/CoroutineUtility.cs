// 一部引用：https://kan-kikuchi.hatenablog.com/entry/DelayMethod

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CoroutineUtility
{
    static public IEnumerator DelayedCoroutine(float seconds, Action<int> action,int arg = 0)
    {
        yield return new WaitForSeconds(seconds);
        action?.Invoke(arg);
    }

    /// <summary>
    /// 渡されたメソッドを指定時間後に実行する
    /// </summary>
    public static IEnumerator DelayMethod<T1, T2>(float waitTime, Action<T1, T2> action, T1 t1, T2 t2)
    {
        yield return new WaitForSeconds(waitTime);
        action(t1, t2);
    }

    /// <summary>
    /// 渡されたメソッドを指定時間後に実行する
    /// </summary>
    public static IEnumerator DelayMethod<T>(float waitTime, Action<T> action, T t)
    {
        yield return new WaitForSeconds(waitTime);
        action(t);
    }

    /// <summary>
    /// 渡されたメソッドを指定時間後に実行する
    /// </summary>
    public static IEnumerator DelayMethod(float waitTime, Action action)
    {
        yield return new WaitForSeconds(waitTime);
        action();
    }
}

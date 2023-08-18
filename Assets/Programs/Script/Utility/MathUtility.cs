using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathUtility
{
    public static void Swap<T>(ref T a,ref T b)
    {
        T temp = a;
        a = b;
        b = temp;
    }

    public static float Posterize(float t,int step)
    {
        return Mathf.Floor(t * step) / step;
    }
}

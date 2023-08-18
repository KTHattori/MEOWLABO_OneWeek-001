using System.Collections.Generic;
using UnityEngine;

public static class GameObjectUtility
{
    /// <summary>
    /// 指定されたインターフェイスを実装したコンポーネントを持つ複数のオブジェクトを検索します
    /// </summary>
    public static T[] FindObjectsOfInterface<T>() where T : class
    {
        var result = new List<T>();
        foreach ( var n in GameObject.FindObjectsOfType<Component>() )
        {
            var component = n as T;
            if ( component != null )
            {
                result.Add( component );
            }
        }
        return result.ToArray();
    }

    public static GameObject[] FindGameObjectsWithLayerMask(LayerMask layerMask)
    {
        GameObject[] goArray = GameObject.FindObjectsOfType<GameObject>();
        List<GameObject> goList = new List<GameObject>();
        foreach (GameObject go in goArray) {
            // LayerMask bit check
            if (((1 << go.layer) & 1 << layerMask) != 0) {
                goList.Add(go);
            }
        }
        if (goList.Count == 0) {
            return null;
        }
        return goList.ToArray();
    }
}



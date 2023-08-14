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
}

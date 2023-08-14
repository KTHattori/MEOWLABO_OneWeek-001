// EnumIndexAttribute.cs
// 利用例は下部に

using System;
using System.Linq;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Showing an array with Enum as keys in the property inspector. (Supported children)<br/>
/// インスペクタ上で配列のキーとして列挙体を表示する属性を定義します。（子要素も対応）<br/>
/// 下記サイトから引用：https://goropocha.hatenablog.com/entry/2021/02/11/232617
/// </summary>
public class EnumIndexAttribute : PropertyAttribute {
    private string[] _names;

    /// <summary>
    /// Constructor
    /// コンストラクタ
    /// </summary>
    /// <param name="enumType"></param>
    public EnumIndexAttribute(Type enumType) => _names = Enum.GetNames(enumType);

#if UNITY_EDITOR
    /// <summary>
    /// Show inspector
    /// インスペクタを表示
    /// </summary>
    [CustomPropertyDrawer(typeof(EnumIndexAttribute))]
    private class Drawer : PropertyDrawer {
        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label) {
            var names = ((EnumIndexAttribute)attribute)._names;
            // propertyPath returns something like hogehoge.Array.data[0]
            // so get the index from there.
            var index = int.Parse(property.propertyPath.Split('[', ']').Where(c => !string.IsNullOrEmpty(c)).Last());
            if (index < names.Length) label.text = names[index];
            EditorGUI.PropertyField(rect, property, label, includeChildren: true);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            return EditorGUI.GetPropertyHeight(property, label, includeChildren: true);
        }
    }
#endif
}

/**
----- 利用例(サイトから引用) -------------------------------------------------
    // EnumIndex属性で表示したいEnumを設定することでInspector上に反映されます
    [SerializeField, EnumIndex(typeof(ButtonType))]
    private ButtonArray[] buttonArray;
----------------------------------------------------------------------------
*/

// Dialog.cs

using UnityEngine;

[System.Serializable]
public class Dialog
{
    public enum DialogType
    {
        Normal,
        Question,
        Answer
    }

    public enum FontStyle
    {
        Normal,
        Bold,
        Italic,
        BoldItalic
    }

    public enum FontSize
    {
        Small = 24,
        Medium = 36,
        Large = 72,
        ExtraLarge = 144
    }

    [TextArea(5,10)] public string content;
    public Color backgroundColor;
    public Color fontColor;
    public Sprite backgroundImage;


    public Dialog()
    {
        content = string.Empty;
    }

    public Dialog(string _content)
    {
        content = _content;
    }
}

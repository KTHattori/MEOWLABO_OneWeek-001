using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Interpolation;

[System.Serializable]
public class WipeTransition : Transition
{
    [SerializeField,Header("ワイプデフォルト画像")]
    private SCO_SpriteScriptableObject defaultScreenImage;

    [SerializeField]
    public enum WipeDirection
    {
        Down = 0,
        Right = 1,
        Up = 2,
        Left = 3,
    }

    [System.Serializable]
    public class UniqueSettings
    {
        [SerializeField,Tooltip("ワイプに使用する画像")]
        public Sprite imageSprite = null;
        [SerializeField,Tooltip("ワイプ画像の色")]
        public Color color = Color.white;
        [SerializeField,Tooltip("ワイプの方向")]
        public WipeDirection direction = WipeDirection.Down;
    }

    [SerializeField,Header("ワイプ固有設定")]
    private UniqueSettings wipeSettings;

    // private
    private GameObject panel = null;
    private Image image = null;
    private CanvasRenderer canvasRenderer = null;
    private RectTransform rect = null;
    private Color tempColor = Color.white;
    public UniqueSettings Settings { get {return wipeSettings;} set { this.wipeSettings = value;} }

    void Reset()
    {
        // 遷移のタイプをフェードに設定
        type = Transition.Type.Wipe;
    }
    
    public override void Initialize()
    {
        // パネル用オブジェクトを生成
        panel = new GameObject("WipePanel");
        if(onGoingAmount > 0) panel.name += "(" + onGoingAmount + ")";
        panel.transform.SetParent(TargetCanvas.transform);
        if(baseInfo.dontDestroyOnLoad) GameObject.DontDestroyOnLoad(TargetCanvas);

        // コンポーネントを追加・取得
        rect = panel.AddComponent<RectTransform>();
        canvasRenderer = panel.AddComponent<CanvasRenderer>();
        image = panel.AddComponent<Image>();

        // キャンバスと同サイズに設定し、原点へ移動
        rect.sizeDelta = TargetCanvas.GetComponent<RectTransform>().sizeDelta;
        rect.localPosition = new Vector2(0.0f,0.0f);

        // 画像と色を設定
        if(wipeSettings.imageSprite) image.sprite = wipeSettings.imageSprite;
        else image.sprite = defaultScreenImage.sprite;
        image.color = wipeSettings.color;

        // 画像タイプをフィルに設定
        image.type = Image.Type.Filled;

        // 方向に応じたフィル方式と原点を設定
        switch(wipeSettings.direction)
        {
            case WipeDirection.Down:
            image.fillMethod = Image.FillMethod.Vertical;
            image.fillOrigin = (int)Image.OriginVertical.Top;
            break;
            case WipeDirection.Right:
            image.fillMethod = Image.FillMethod.Horizontal;
            image.fillOrigin = (int)Image.OriginHorizontal.Left;
            break;
            case WipeDirection.Up:
            image.fillMethod = Image.FillMethod.Vertical;
            image.fillOrigin = (int)Image.OriginVertical.Bottom;
            break;
            case WipeDirection.Left:
            image.fillMethod = Image.FillMethod.Horizontal;
            image.fillOrigin = (int)Image.OriginHorizontal.Right;
            break;
        }

        // フィルを0.0fにリセット
        image.fillAmount = 0.0f;

        onGoingAmount++;
    }

    public override void Uninitialize()
    {
        GameObject.Destroy(panel);
        image = null;
        rect = null;
        panel = null;

        onGoingAmount--;
    }

    public override void TransitIn(float value)
    {
        image.fillAmount = 1.0f - value;
        image.color = Easing.EaseColor(wipeSettings.color,tempColor,value,1.0f,EaseType);
    }
    public override void TransitOut(float value)
    {
        image.fillAmount = value;
        image.color = Easing.EaseColor(tempColor,wipeSettings.color,value,1.0f,EaseType);
    }

    protected override void TransitSwitch()
    {
        // 方向に応じたフィルを設定
        switch(wipeSettings.direction)
        {
            case WipeDirection.Down:
            image.fillOrigin = (int)Image.OriginVertical.Bottom;
            break;
            case WipeDirection.Right:
            image.fillOrigin = (int)Image.OriginHorizontal.Right;
            break;
            case WipeDirection.Up:
            image.fillOrigin = (int)Image.OriginVertical.Top;
            break;
            case WipeDirection.Left:
            image.fillOrigin = (int)Image.OriginHorizontal.Left;
            break;
        }
    }
}


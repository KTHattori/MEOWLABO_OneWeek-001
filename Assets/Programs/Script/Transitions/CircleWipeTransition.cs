using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Interpolation;

[System.Serializable]
public class CircleWipeTransition : Transition
{
    [SerializeField,Header("ワイプデフォルト画像")]
    private SCO_SpriteScriptableObject defaultScreenImage;

    [SerializeField]
    public enum CircleOrigin
    {
        TopLeft,
        TopCenter,
        TopRight,
        MiddleLeft,
        MiddleCenter,
        MiddleRight,
        BottomLeft,
        BottomCenter,
        BottomRight, 
    }

    [System.Serializable]
    public class UniqueSettings
    {
        [SerializeField,Tooltip("円ワイプに使用する画像")]
        public Sprite imageSprite = null;
        [SerializeField,Tooltip("円ワイプ画像の色")]
        public Color color = Color.white;
        [SerializeField,Tooltip("円ワイプの原点")]
        public CircleOrigin origin = CircleOrigin.MiddleCenter;
        [SerializeField,Tooltip("360度ワイプの時の開始地点")]
        public Image.Origin360 circleStart = Image.Origin360.Top;
        [SerializeField,Tooltip("時計回り")]
        public bool clockwise = true;
        [SerializeField,Tooltip("イン・アウト切り替え時に回転方向を反転する")]
        public bool revertRotation = true;
    }

    [SerializeField,Header("ワイプ固有設定")]
    private UniqueSettings circleWipeSettings;

    // private
    private GameObject panel = null;
    private Image image = null;
    private CanvasRenderer canvasRenderer = null;
    private RectTransform rect = null;
    private Color tempColor = Color.white;
    public UniqueSettings Settings { get {return circleWipeSettings;} set { this.circleWipeSettings = value;} }

    void Reset()
    {
        // 遷移のタイプを円ワイプに設定
        type = Transition.Type.CircleWipe;
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
        if(circleWipeSettings.imageSprite) image.sprite = circleWipeSettings.imageSprite;
        else image.sprite = defaultScreenImage.sprite;
        image.color = circleWipeSettings.color;

        // 画像タイプをフィルに設定
        image.type = Image.Type.Filled;

        // 方向に応じたフィル方式と原点を設定
        switch(circleWipeSettings.origin)
        {
            case CircleOrigin.TopLeft:
            image.fillMethod = Image.FillMethod.Radial90;
            image.fillOrigin = (int)Image.Origin90.TopLeft;
            break;
            case CircleOrigin.TopCenter:
            image.fillMethod = Image.FillMethod.Radial180;
            image.fillOrigin = (int)Image.Origin180.Top;
            break;
            case CircleOrigin.TopRight:
            image.fillMethod = Image.FillMethod.Radial90;
            image.fillOrigin = (int)Image.Origin90.TopRight;
            break;
            case CircleOrigin.MiddleLeft:
            image.fillMethod = Image.FillMethod.Radial180;
            image.fillOrigin = (int)Image.Origin180.Left;
            break;
            case CircleOrigin.MiddleCenter:
            image.fillMethod = Image.FillMethod.Radial360;
            image.fillOrigin = (int)circleWipeSettings.circleStart;
            break;
            case CircleOrigin.MiddleRight:
            image.fillMethod = Image.FillMethod.Radial180;
            image.fillOrigin = (int)Image.Origin180.Right;
            break;
            case CircleOrigin.BottomLeft:
            image.fillMethod = Image.FillMethod.Radial90; 
            image.fillOrigin = (int)Image.Origin90.BottomLeft;
            break;
            case CircleOrigin.BottomCenter:
            image.fillMethod = Image.FillMethod.Radial180;
            image.fillOrigin = (int)Image.Origin180.Bottom;
            break;
            case CircleOrigin.BottomRight:
            image.fillMethod = Image.FillMethod.Radial90;
            image.fillOrigin = (int)Image.Origin90.BottomRight;
            break; 
        }

        // 回転方向の設定
        image.fillClockwise = circleWipeSettings.clockwise;

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
        image.color = Easing.EaseColor(circleWipeSettings.color,tempColor,value,1.0f,EaseType);
    }
    public override void TransitOut(float value)
    {
        image.fillAmount = value;
        image.color = Easing.EaseColor(tempColor,circleWipeSettings.color,value,1.0f,EaseType);
    }

    protected override void TransitSwitch()
    {
        // NOT XOR
        image.fillClockwise = !(circleWipeSettings.clockwise ^ circleWipeSettings.revertRotation);
    }
}


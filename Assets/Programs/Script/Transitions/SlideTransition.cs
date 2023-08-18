using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Interpolation;

[System.Serializable]
public class SlideTransition : Transition
{
    [SerializeField,Header("デフォルト画像")]
    private SCO_SpriteScriptableObject defaultScreenImage;

    [SerializeField]
    public enum SlideDirection
    {
        Up,
        Right,
        Down,
        Left,
        UpLeft,
        UpRight,
        DownLeft,
        DownRight, 
    }

    [System.Serializable]
    public class UniqueSettings
    {
        [SerializeField,Tooltip("使用する画像")]
        public Sprite imageSprite = null;
        [SerializeField,Tooltip("画像の色")]
        public Color color = Color.white;
        [SerializeField,Tooltip("スライドの方向")]
        public SlideDirection direction = SlideDirection.Up;
        //[SerializeField,Tooltip("画像の回転角度")]
        // public float imageAngle;
        [SerializeField,Tooltip("イン・アウト切り替え時に移動方向を反転する")]
        public bool revertDirection = false;
    }

    [SerializeField,Header("スライド固有設定")]
    private UniqueSettings slideSettings;

    // private
    private GameObject panel = null;
    private Image image = null;
    private CanvasRenderer canvasRenderer = null;
    private RectTransform rect = null;
    private Color tempColor = Color.white;
    private Vector2 imageSize = new Vector2();
    private Vector2 startPos = new Vector2();
    private Vector2 endPos = new Vector2();
    public UniqueSettings Settings { get {return slideSettings;} set { this.slideSettings = value;} }

    void Reset()
    {
        // 遷移のタイプをスライドに設定
        type = Transition.Type.Slide;
    }
    
    public override void Initialize()
    {
        // パネル用オブジェクトを生成
        panel = new GameObject("SlidePanel");
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

        // 画像サイズを取得
        imageSize = rect.sizeDelta;

        // 画像と色を設定
        if(slideSettings.imageSprite) image.sprite = slideSettings.imageSprite;
        else image.sprite = defaultScreenImage.sprite;
        image.color = slideSettings.color;

        // 画像タイプをシンプルに設定
        image.type = Image.Type.Simple;

        // 方向に応じた開始位置を設定
        switch(slideSettings.direction)
        {
            case SlideDirection.Up:
            startPos.y = -imageSize.y;
            break;
            case SlideDirection.Right:
            startPos.x = -imageSize.x;
            break;
            case SlideDirection.Down:
            startPos.y = imageSize.y;
            break;
            case SlideDirection.Left:
            startPos.x = imageSize.x;
            break;
            case SlideDirection.UpLeft:
            startPos.x = imageSize.x;
            startPos.y = -imageSize.y;
            break;
            case SlideDirection.UpRight:
            startPos.x = -imageSize.x;
            startPos.y = -imageSize.y;
            break;
            case SlideDirection.DownLeft:
            startPos.x = imageSize.x;
            startPos.y = imageSize.y;
            break;
            case SlideDirection.DownRight:
            startPos.x = -imageSize.x;
            startPos.y = imageSize.y;
            break;
        }

        endPos = rect.localPosition;
        rect.localPosition = startPos;

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
        rect.localPosition = Easing.Ease(startPos,endPos,value,1.0f,EaseType);
        image.color = Easing.EaseColor(slideSettings.color,tempColor,value,1.0f,EaseType);
    }
    public override void TransitOut(float value)
    {
        rect.localPosition = Easing.Ease(startPos,endPos,value,1.0f,EaseType);
        image.color = Easing.EaseColor(tempColor,slideSettings.color,value,1.0f,EaseType);
    }

    protected override void TransitSwitch()
    {
        // 方向反転設定に応じて終了位置を変更
        if(!slideSettings.revertDirection)
        {
            startPos = -startPos;
        }
        // 方向に応じた終了位置を設定
        switch(slideSettings.direction)
        {
            case SlideDirection.Up:
            endPos.y = startPos.y;
            break;
            case SlideDirection.Right:
            endPos.x = startPos.x;
            break;
            case SlideDirection.Down:
            endPos.y = startPos.y;
            break;
            case SlideDirection.Left:
            endPos.x = startPos.x;
            break;
            case SlideDirection.UpLeft:
            endPos.x = startPos.x;
            endPos.y = startPos.y;
            break;
            case SlideDirection.UpRight:
            endPos.x = startPos.x;
            endPos.y = startPos.y;
            break;
            case SlideDirection.DownLeft:
            endPos.x = startPos.x;
            endPos.y = startPos.y;
            break;
            case SlideDirection.DownRight:
            endPos.x = startPos.x;
            endPos.y = startPos.y;
            break;
        }

        // 開始位置を中心に
        startPos.x = 0.0f;
        startPos.y = 0.0f;
    }
}


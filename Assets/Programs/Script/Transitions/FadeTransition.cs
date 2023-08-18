using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Interpolation;

[System.Serializable]
public class FadeTransition : Transition
{
    [SerializeField,Header("フェードデフォルト画像")]
    private SCO_SpriteScriptableObject defaultScreenImage;

    [System.Serializable]
    public class UniqueSettings
    {
        [SerializeField,Tooltip("フェードに使用する画像")]
        public Sprite imageSprite = null;
        [SerializeField,Tooltip("フェード色")]
        public Color color = Color.white;
    }

    [SerializeField,Header("フェード固有設定")]
    private UniqueSettings fadeSettings;

    // private
    private GameObject panel = null;
    private Image image = null;
    private CanvasRenderer canvasRenderer = null;
    private RectTransform rect = null;

    private Color tempColor = Color.white;
    
    public UniqueSettings Settings { get {return fadeSettings;} set { this.fadeSettings = value;} }

    void Reset()
    {
        // 遷移のタイプをフェードに設定
        type = Transition.Type.Fade;
    }
    
    public override void Initialize()
    {
        // パネル用オブジェクトを生成
        panel = new GameObject("FadePanel");
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

        // 画像を設定し、色の初期設定を行う
        if(fadeSettings.imageSprite) image.sprite = fadeSettings.imageSprite;
        else image.sprite = defaultScreenImage.sprite;
        tempColor = fadeSettings.color;
        tempColor.a = 0.0f;
        image.color = tempColor;

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
        tempColor.a = 1.0f - value;
        image.color = Easing.EaseColor(fadeSettings.color,tempColor,value,1.0f,EaseType);
    }
    public override void TransitOut(float value)
    {
        tempColor.a = value;
        image.color = Easing.EaseColor(tempColor,fadeSettings.color,value,1.0f,EaseType);
    }

    protected override void TransitSwitch()
    {

    }
}

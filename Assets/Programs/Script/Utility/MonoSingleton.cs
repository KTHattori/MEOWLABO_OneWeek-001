/// MonoSingleton.cs


using UnityEngine;

/**
<summary>
シングルトン化した MonoBehavior 抽象クラス
dontDestroyOnLoadの部分と、各コメント以外は下記サイトを引用してます
参考：https://caitsithware.com/wordpress/archives/118
</summary>
*/
public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    // メンバ インスタンス
    static T m_Instance = null;

    // シーンロード時に破棄を行わないかどうか
    [SerializeField]
    protected bool dontDestroyOnLoad;
 
    public static T instance
    {
        // ゲッター
        get
        {
            if( m_Instance != null )
            {   // インスタンスが存在していれば取得
                return m_Instance;
            }

            // 型を取得
            System.Type type = typeof(T);

            // T型のMonoBehaviorスクリプトがアタッチされたオブジェクトが存在するかチェック
            T instance = GameObject.FindObjectOfType(type,true) as T;
            if( instance == null )
            {   // 見つからない場合
                string typeName = type.ToString();

                // 新たにこのスクリプトがアタッチされたオブジェクトを生成
                GameObject gameObject = new GameObject( typeName, type );
                instance = gameObject.GetComponent<T>();

                if( instance == null )
                {   // それでも失敗した場合エラーログを残す
                    Debug.LogError(typeName + " インスタンスの生成に失敗しました。\nProblem during the creation of " + typeName,gameObject );
                }
            }
            else
            {   // 初期化を行う
                Initialize(instance);
            }
            return m_Instance;
        }
    }
 
    /// 初期化関数
    static void Initialize(T instance)
    {
        if( m_Instance == null )
        {   // メンバインスタンスが空なら代入し、インスタンス初期化時関数をコール
            m_Instance = instance;

            if(m_Instance.dontDestroyOnLoad)
            {   // dontDestroyOnLoadが有効
                DontDestroyOnLoad(m_Instance);
            }

            m_Instance.OnInitialize();  // インスタンス初期化時関数
        }
        else if( m_Instance != instance )
        {
            // オブジェクトが見つかったが重複している場合、破棄
            DestroyImmediate( instance.gameObject );
        }
    }
 
    /// オブジェクト破棄時関数
    static void Destroyed(T instance)
    {
        if( m_Instance == instance )
        {   // 正しいインスタンスであればインスタンス破棄時関数をコール
            m_Instance.OnFinalize();
 
            m_Instance = null;
        }
    }
 
    /// インスタンス初期化時関数
    public virtual void OnInitialize() {}

    /// インスタンス破棄時関数
    public virtual void OnFinalize() {}
 
    void Awake()
    {
        // 初期化
        Initialize( this as T );
    }
 
    void OnDestroy()
    {
        // オブジェクトが破棄された場合の処理
        Destroyed( this as T );
    }
 
    void OnApplicationQuit()
    {
        // アプリケーションが終了した場合も破棄時の処理
        Destroyed( this as T );
    }
}

/**
----- 利用例(サイトから引用) -------------------------------------------------
    // シングルトンクラス定義時
    public class EnemyManager : MonoSingleton<EnemyManager>
    {
        public int HP = 100;
        public int Attack = 15;

        public override void OnInitialize()
        {
            // インスタンス初期化時処理
        }

        public override void OnFinalize()
        {
            // インスタンス破棄時処理
        }

        void Update()
        {
            // 通常スクリプトと同様にUpdateもコールされます
        }

        public void Spawn()
        {
            // 敵の生成処理
        }

                    .
                    .
                    .
    }

    // 呼び出し(他クラス内)
                    .
                    .
                    .
        
        // 敵を15秒ごとに生成
        if(enemySpawnTimer < 0.0f)
        {
            // インスタンスを介してpublicなメンバ・メソッドにアクセス可能
            EnemyManager.instance.Spawn();
            enemySpawnTimer = 15.0f;
        }


----------------------------------------------------------------------------
*/

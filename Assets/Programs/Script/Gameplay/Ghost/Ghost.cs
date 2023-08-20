using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoSingleton<Ghost>
{
    public enum State
    {
        [InspectorName("出現")]
        Emerge,
        [InspectorName("待機")]
        Idle,
        [InspectorName("移動")]
        Move,
        [InspectorName("インタラクト中")]
        Interact,
        [InspectorName("イベント中")]
        Event,
        [InspectorName("消滅")]
        Disappear,
    }

    [System.Serializable]
    public class EmergeData
    {
        public Renderer renderer;
        public float time = 1.0f;
        public Vector3 position = Vector3.zero;
        public Interpolation.Easing.Curve curve = Interpolation.Easing.Curve.Linear;
    }

    [System.Serializable]
    public class MoveData
    {
        public float minSpeed = 1.0f;
        public float maxSpeed = 2.0f;
        public float arrivedRange = 0.01f;
        public float interpolationSpeed = 0.05f;
        public float rotationThreshold = 5.0f;
        public float rotationTime = 1.0f;
        public string targetLayerMaskName = "PlayerMovementGuide";
        public LayerMask TargetLayerMask { get { return LayerMask.NameToLayer(targetLayerMaskName); } }
    }

    [System.Serializable]
    public class InteractData
    {
        [SerializeField]
        public Transform locator;
        
        public float time = 1.0f;
        public float range = 1.0f;
    }

    [System.Serializable]
    public class EventData
    {

    }

    [System.Serializable]
    public class DisappearData
    {
        public float time = 1.0f;
    }

    [SerializeField]
    private State state = State.Emerge;

    public void SetState(State state)
    {
        this.state = state;
    }

    public State GetState()
    {
        return state;
    }

    [SerializeField]
    private EmergeData emergeData = new EmergeData();

    [SerializeField]
    private MoveData moveData = new MoveData();

    [SerializeField]
    private InteractData interactData = new InteractData();

    [SerializeField]
    private EventData eventData = new EventData();

    [SerializeField]
    private DisappearData disappearData = new DisappearData();

    public MoveData Move
    {
        get { return moveData; }
    }

    public InteractData Interact
    {
        get { return interactData; }
    }

    public EventData Event
    {
        get { return eventData; }
    }

    public EmergeData Emerge
    {
        get { return emergeData; }
    }

    public DisappearData Disappear
    {
        get { return disappearData; }
    }


    private GhostMove ghostMove = null;
    private GhostSelect ghostSelect = null;
    private GhostInteract ghostInteract = null;

    [SerializeField]
    private bool isEmerged = false;

    private float timer = 0.0f;

    void OnValidate()
    {
        emergeData.position = transform.position;
    }

    void Reset()
    {
        ghostMove = GetComponent<GhostMove>();
        ghostSelect = GetComponent<GhostSelect>();
        ghostInteract = GetComponent<GhostInteract>();
        emergeData.renderer = GetComponentInChildren<Renderer>();
    }



    // Start is called before the first frame update
    void Start()
    {
        ghostMove = GetComponent<GhostMove>();
        ghostSelect = GetComponent<GhostSelect>();
        ghostInteract = GetComponent<GhostInteract>();
        emergeData.renderer = GetComponentInChildren<Renderer>();
        SetTransparency(0.0f);
        timer = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (isEmerged)
        { 
            Act(state);
        }
    }

    void Act(State state)
    {
        switch (state)
        {
            case State.Emerge:
                OnEmerge();
                break;
            case State.Idle:
                OnIdle();
                break;
            case State.Move:
                OnMove();
                break;
            case State.Interact:
                OnInteract();
                break;
            case State.Event:
                OnEvent();
                break;
            case State.Disappear:
                OnDisappear();
                break;
        }
    }

    

    void OnEmerge()
    {
        UpdateEmerge();
    }
    
    void OnIdle()
    {
        CastRay();
        // マウスが動いたら移動状態に遷移
        if (true || ghostSelect.GetMouseMoved())
        {
            IndicatePoint();
            SetState(State.Move);
        }
        
        CheckInteract();
    }

    void OnMove()
    {
        CastRay();
        IndicatePoint();
        Locomote();
        // 一定の距離まで近づいたら待機状態に遷移
        if (ghostMove.IsArrived())
        {
            ghostMove.Stop();
            SetState(State.Idle);
        }

        CheckInteract();
    }

    void OnInteract()
    {
        if(!ghostInteract.HasTarget())
        {
            SetState(State.Idle);
            return;
        }

        if(ghostInteract.IsTargetInRange())
        {
            ghostInteract.Interact();
            SetState(State.Idle);
        }
        else
        {
            ghostMove.IndicatePoint(ghostInteract.TargetObject.transform.position);
            Locomote();
        }
    }

    void OnEvent()
    {

    }

    void OnDisappear()
    {

    }

    
    bool TryGetPointedObject(out GameObject indicatedObject)
    {
        GameObject hit = ghostSelect.GetHitObject();
        if (hit)
        {
            indicatedObject = hit;
            return true;
        }
        indicatedObject = null;
        return false;
    }

    void UpdateEmerge()
    {
        timer += Time.deltaTime;
        float progress = timer / emergeData.time;
        if (progress >= 1.0f)
        {
            timer = 0.0f;
            progress = 1.0f;
            TimeShift.ActivateShift();
            SetState(State.Idle);
        }
        progress = Interpolation.Easing.Ease(0.0f,1.0f,progress,emergeData.curve);
        SetTransparency(progress);
    }

    void SetTransparency(float alpha)
    {
        emergeData.renderer.sharedMaterial.SetFloat("_Alpha", alpha);
    }

    void CastRay()
    {
        ghostSelect.RaycastTerrain();
        ghostSelect.RaycastProp();
    }

    void IndicatePoint()
    {
        Vector3 mousePoint = ghostSelect.GetMousePoint();
        ghostMove.IndicatePoint(mousePoint);
    }

    void Locomote()
    {
        ghostMove.Rotate();
        ghostMove.Move();
    }

    void ResetTimer()
    {
        timer = 0.0f;
    }

    void CheckInteract()
    {
        if(ghostSelect.GetMouseClicked())
        {
            if(ghostInteract.HasTarget()) ghostInteract.Cancel();

            if (PropManager.instance.highlightedObject)
            {
                ghostInteract.UpdateTarget(PropManager.instance.highlightedObject);
                SetState(State.Interact);
            }
        }
    }

    static public void StartEmerge()
    {
        instance.isEmerged = true;
        instance.SetState(State.Emerge);
        instance.SetTransparency(0.0f);
        Debug.Log("StartEmerge");
    }

    static public void StartDisappear()
    {
        instance.isEmerged = false;
        instance.SetState(State.Disappear);
        instance.SetTransparency(1.0f);
        Debug.Log("StartDisappear");
    }
}

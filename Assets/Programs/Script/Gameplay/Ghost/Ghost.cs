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
        public float time = 1.0f;
        public Vector3 position = Vector3.zero;
    }

    [System.Serializable]
    public class MoveData
    {
        public float minSpeed = 1.0f;
        public float maxSpeed = 2.0f;
        public float tiltRange = 30.0f;
        public string targetLayerMaskName = "PlayerMovementGuide";
        public float arrivedRange = 0.1f;
        public float interpolationSpeed = 0.15f;
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
    private bool isAppear = false;

    void Reset()
    {
        ghostMove = GetComponent<GhostMove>();
        ghostSelect = GetComponent<GhostSelect>();
        ghostInteract = GetComponent<GhostInteract>();
    }



    // Start is called before the first frame update
    void Start()
    {
        ghostMove = GetComponent<GhostMove>();
        ghostSelect = GetComponent<GhostSelect>();
        ghostInteract = GetComponent<GhostInteract>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isAppear)
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

    }
    
    void OnIdle()
    {
        ghostSelect.RaycastTerrain();
        ghostSelect.RaycastProp();
        Vector3 mousePoint = ghostSelect.GetMousePoint();
        // マウスが動いたら移動状態に遷移
        if (ghostSelect.GetMouseMoved())
        {
            ghostMove.IndicatePoint(mousePoint);
            SetState(State.Move);
        }
    }

    void OnMove()
    {
        ghostSelect.RaycastTerrain();
        ghostSelect.RaycastProp();
        Vector3 mousePoint = ghostSelect.GetMousePoint();
        ghostMove.IndicatePoint(mousePoint);
        ghostMove.Rotate();
        ghostMove.Move();
        // 一定の距離まで近づいたら待機状態に遷移
        if (ghostMove.IsArrived())
        {
            ghostMove.Stop();
            SetState(State.Idle);
        }
        
    }

    void OnInteract()
    {
        ghostMove.Rotate();
        ghostMove.Move();
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
}

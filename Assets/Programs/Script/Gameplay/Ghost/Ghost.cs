using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoSingleton<Ghost>
{
    public enum State
    {
        [InspectorName("出現")]
        Appear,
        [InspectorName("移動(通常時)")]
        Move,
        [InspectorName("インタラクト中")]
        Interact,
        [InspectorName("イベント中")]
        Event,
        [InspectorName("消滅")]
        Disappear,
    }

    [System.Serializable]
    public class AppearData
    {
        public float time = 1.0f;
        public Vector3 position = Vector3.zero;
    }

    [System.Serializable]
    public class MoveData
    {
        public float speed = 1.0f;
    }

    [System.Serializable]
    public class InteractData
    {
        public float time = 1.0f;
        public float range = 1.0f;
        public GameObject target = null;
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
    private State state = State.Appear;

    public void SetState(State state)
    {
        this.state = state;
    }

    public State GetState()
    {
        return state;
    }

    [SerializeField]
    private AppearData appearData = new AppearData();

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

    public AppearData Appear
    {
        get { return appearData; }
    }

    public DisappearData Disappear
    {
        get { return disappearData; }
    }


    private GhostMove ghostMove = null;
    private GhostInput ghostInput = null;
    private GhostInteract ghostInteract = null;

    [SerializeField]
    private bool isAppear = false;

    void Reset()
    {
        ghostMove = GetComponent<GhostMove>();
        ghostInput = GetComponent<GhostInput>();
        ghostInteract = GetComponent<GhostInteract>();
    }



    // Start is called before the first frame update
    void Start()
    {
        ghostMove = GetComponent<GhostMove>();
        ghostInput = GetComponent<GhostInput>();
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
            case State.Appear:
                OnAppear();
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

    

    void OnAppear()
    {

    }

    void OnMove()
    {
        Vector3 mousePoint = ghostInput.GetMousePoint();
        ghostMove.Rotate(mousePoint);
        ghostMove.Move(mousePoint);
        if(CheckClicked(out GameObject clickedObject))
        {
            SetTarget(clickedObject);
            SetState(State.Interact);
        }
    }

    void OnInteract()
    {
        ghostMove.Rotate(interactData.target.transform.position);
        ghostMove.Move(interactData.target.transform.position);
        if(CheckClicked(out GameObject clickedObject))
        {
            SetTarget(clickedObject);
            SetState(State.Interact);
        }
    }

    void OnEvent()
    {

    }

    void OnDisappear()
    {

    }

    bool CheckClicked(out GameObject clickedObject)
    {
        if (ghostInput.GetMouseClicked())
        {
            clickedObject = ghostInput.GetClickedObject();
            return true;
        }
        clickedObject = null;
        return false;
    }

    void SetTarget(GameObject target)
    {
        interactData.target = target;
    }
}

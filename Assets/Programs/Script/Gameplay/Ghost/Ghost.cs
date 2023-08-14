using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoSingleton<Ghost>
{
    public enum State
    {
        [InspectorName("出現時")]
        Appear,
        [InspectorName("待機中")]
        Idle,
        [InspectorName("移動中")]
        Move,
        [InspectorName("インタラクト中")]
        Interact,
        [InspectorName("イベント中")]
        Event,
        [InspectorName("消滅中")]
        Disappear,
    }

    [System.Serializable]
    public class AppearData
    {
        public float time = 1.0f;
        public Vector3 position = Vector3.zero;
    }

    [System.Serializable]
    public class IdleData
    {

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
    private State state = State.Idle;

    public void SetState(State state)
    {
        this.state = state;
    }

    public State GetState()
    {
        return state;
    }

    [SerializeField]
    private AppearData appear = new AppearData();

    [SerializeField]
    private IdleData idle = new IdleData();

    [SerializeField]
    private MoveData move = new MoveData();

    [SerializeField]
    private InteractData interact = new InteractData();

    [SerializeField]
    private EventData ghostEvent = new EventData();

    [SerializeField]
    private DisappearData disappear = new DisappearData();

    public MoveData Move
    {
        get { return move; }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

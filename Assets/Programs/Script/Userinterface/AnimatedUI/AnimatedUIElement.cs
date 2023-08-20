using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Interpolation;

namespace AnimatedUI
{
    public interface IAnimatable
    {
        public void Entry();
        public void Exit();
        public void SetState(int targetIndex);
        public void SetState(string targetStateName);
    }

    public enum CommonProperty
    {
        Position = 0,
        Rotation = 1,
        Scaling = 2,
        Size = 3,
        Offset_Position = 4,
        Offset_Rotation = 5,
        Offset_Scaling = 6,
        Offset_Size = 7,
        Color = 8,
        MAX = 9
    }

    public enum CommonFlag
    {
        UseDefaultPosition = 0,
        UseDefaultRotation = 1,
        UseDefaultScaling = 2,
        UseDefaultSize = 3,
        UseDefaultColor = 4,
        MAX = 5,
    }
    
    static public class CommonState
    {
        // --- const variables ---
        public const int Uninitialized = -2;
        public const int Entry = -1;
        public const int Idle = 0;
        public const int ExitCompleted = 99;
        public const int ReEntry = 100;
        public const int Destroy = -99;
    }

    static public class ButtonState
    {
        // --- const variables ---
        public const int Selected = 1;
        public const int Pressed = 2;
    }

    static public class MenuState
    {
        // --- const variables ---
        public const int Selected = 1;
    }

    [RequireComponent(typeof(RectTransform))]    public abstract class AnimatedUIElement : MonoBehaviour,IAnimatable
    {
        [System.Serializable]
        public class Property : IEquatable<Property>
        {
            // variables
            [ReadOnly]
            public string propertyName = "";
            public float x;
            public float y;
            public float z;
            public float w;

            // properties
            public int Integer { get { return (int)x;} set { this.x = (int)value;} }
            public float Float {get { return x;} set { this.x = value;} }
            public Vector2 Vector2 { get { return new Vector2(x,y);} set { this.x = value.x; this.y = value.y;} }
            public Vector3 Vector3 { get { return new Vector3(x,y,z);} set { this.x = value.x; this.y = value.y; this.z = value.z;} }
            public Vector4 Vector4 { get { return new Vector4(x,y,z,w);} set { this.x = value.x; this.y = value.y; this.z = value.z; this.w = value.w;}}
            public Vector4 Vector { get { return new Vector4(x,y,z,w);} set { this.x = value.x; this.y = value.y; this.z = value.z; this.w = value.w;} }
            public Color ColorRGB { get { return new Color(x,y,z,1.0f);}set { this.x = value.r; this.y = value.g; this.z = value.b; this.w = 1.0f;}}
            public Color Color { get { return new Color(x,y,z,w);} set { this.x = value.r; this.y = value.g; this.z = value.b; this.w = value.a;}}

            public void SetValues(Property value)
            {
                this.x = value.x;
                this.y = value.y;
                this.z = value.z;
                this.w = value.w;
            }

            public void GetInterpolated(Property start,Property end,float ratio)
            {
                this.x = Easing.Ease(start.x,end.x,ratio,1.0f,Easing.Curve.Linear);
                this.y = Easing.Ease(start.y,end.y,ratio,1.0f,Easing.Curve.Linear);
                this.z = Easing.Ease(start.z,end.z,ratio,1.0f,Easing.Curve.Linear);
                this.w = Easing.Ease(start.w,end.w,ratio,1.0f,Easing.Curve.Linear);
            }

            public Property(string name,int val)
            {
                this.propertyName = name;
                this.x = val;
                this.y = 0;
                this.z = 0;
                this.w = 0;
            }
            public Property(string name,float val)
            {
                this.propertyName = name;
                this.x = val;
                this.y = 0.0f;
                this.z = 0.0f;
                this.w = 0.0f;
            }
            public Property(string name,Vector2 val)
            {
                this.propertyName = name;
                this.x = val.x;
                this.y = val.y;
                this.z = 0.0f;
                this.w = 0.0f;
            }
            public Property(string name,Vector3 val)
            {
                this.propertyName = name;
                this.x = val.x;
                this.y = val.y;
                this.z = val.z;
                this.w = 0.0f;
            }
            public Property(string name,Vector4 val)
            {
                this.propertyName = name;
                this.x = val.x;
                this.y = val.y;
                this.z = val.z;
                this.w = val.w;
            }
            public Property(string name,float x = 0.0f,float y= 0.0f,float z= 0.0f,float w = 0.0f)
            {
                this.propertyName = name;
                this.x = x;
                this.y = y;
                this.z = z;
                this.w = w;
            }

            public bool Equals(Property other)
            {
                if (other == null)
                {
                    Debug.Log("");
                    return false;
                }

                return x.Equals(other.x) && y.Equals(other.y) && z.Equals(other.z) && w.Equals(other.w);
            }
        }

        [System.Serializable]
        protected class Flag
        {
            // variables
            [ReadOnly]
            public string flagName = "";
            public bool flag;

            // properties
            

            public Flag(string name,bool flag)
            {
                this.flagName = name;
                this.flag = flag;
            }

            public bool IsChecked
            {
                get {return this.flag;}
            }
            
            public bool Equals(bool other)
            {
                return this.flag == other;
            }
        }

        
        [System.Serializable]
        public class TransitionInfo
        {
            [SerializeField,Header("遷移先Stateの番号")]
            public int targetState = 0;
            [SerializeField,Header("遷移にかかる時間")]
            public float duration = 1.0f;
            [SerializeField,Header("遷移に使用するイージングカーブ")]
            public Easing.Curve easeCurve = Easing.Curve.EaseInCubic;
            [SerializeField,Header("割り込み遷移を許可する")]
            public bool enableInterrupt = true;

            public TransitionInfo(int defaultTargetState,bool defaultInterruptState = true)
            {
                duration = 1.0f;
                easeCurve = Easing.Curve.EaseInCubic;
                targetState = defaultTargetState;
                enableInterrupt = defaultInterruptState;
            }

            public void OnTransitStart()
            {
                
            }

            public void OnTransitProgress(float ratio)
            {
                
            }

            public void OnTransitEnd()
            {
                
            }

            public void OnTransitAbort(float ratio)
            {
                
            }
        }

        [System.Serializable]
        protected class StateInfo : IDisposable
        {
            [SerializeField,Header("ステート名称")]
            public string name = "CustomState";
            [SerializeField,Header("ステート番号"),ReadOnly]
            public int index;
            [SerializeField,Header("パラメータ")]
            public List<Property> properties = null;
            [SerializeField,Header("フラグ")]
            public List<Flag> flags = null;
            [SerializeField,Header("自動遷移を無効化")]
            public bool disableAutomaticTransit = false;
            [SerializeField,Header("デフォルト遷移情報"),Tooltip("遷移時にカスタム遷移情報が登録されていない場合、こちらが呼出されます。")]
            public TransitionInfo defaultGoingTransit = new TransitionInfo(0,true);
            [SerializeField,Header("カスタム遷移情報")]
            public List<TransitionInfo> customizedGoingTransitions = new List<TransitionInfo>();

            public StateInfo()
            {
                defaultGoingTransit = new TransitionInfo(0,true);
            }

            public StateInfo(string stateName,int stateIndex,int defaultTargetState,bool defaultInterruptState)
            {
                name = stateName;
                index = stateIndex;
                defaultGoingTransit = new TransitionInfo(defaultTargetState,defaultInterruptState);
            }

            void ReleaseProperties()
            {
                for(int i = 0,max = properties.Count;i < max;i++)
                {
                    properties[i] = null;
                }
                properties.Clear();
            }

            void ReleaseTransitions()
            {
                defaultGoingTransit = null;
                for(int i = 0,max = customizedGoingTransitions.Count;i < max;i++)
                {
                    customizedGoingTransitions[i] = null;
                }
                customizedGoingTransitions.Clear();
            }
            
            public void ClampTargetStateIndex(int min,int max)
            {
                if(defaultGoingTransit.targetState < min) defaultGoingTransit.targetState = min;
                if(defaultGoingTransit.targetState > max) defaultGoingTransit.targetState = max;
            }

            public void Dispose()
            {
                ReleaseProperties();
                ReleaseTransitions();
            }

            public void OnStateEntry()
            {
                
            }

            public void OnStateUpdate()
            {
                
            }

            public void OnStateExit()
            {
                
            } 
        }

        // --- static variables ---
        static private int instanceAmount;

        // --- protected variables ---
        protected GameObject origin;
        protected RectTransform originRect;
        protected RectTransform rect;
        protected Dictionary<int,StateInfo> stateInfoDict = new Dictionary<int, StateInfo>();
        protected Dictionary<string,int> nameIndexDict = new Dictionary<string, int>();
        [SerializeField]
        protected float transitRatio = 0.0f;
        protected float transitTimer = 0.0f;
        protected float transitDuration = 0.0f;
        protected bool isInTransit = false;
        [SerializeField]
        protected int currentStateIndex = CommonState.Uninitialized;
        protected int nextStateIndex = 0;
        protected int exitStateIndex = 0;
        protected TransitionInfo currentTransition = null;
        protected List<Property> currentProperties = new List<Property>();
        protected List<Property> startProperties = new List<Property>();
        protected List<Property> endProperties = new List<Property>();

        protected int entryStateIndexBackup = CommonState.Idle;
        protected int addStateCount = 0;

        // --- properties ---
        public List<Property> CurrentProperties { get{return currentProperties;}}

        // --- serialized protected variables ---
        [SerializeField]
        protected bool useUnscaledTimeOnTransit = false;
        [SerializeField]
        protected bool entryOnStart = false;
        [SerializeField]
        protected bool resetOnAwake = true;
        [SerializeField]
        protected bool destroyOnExit = false;
        [SerializeField]
        protected float transitSpeedMultiplier = 1.0f;
        [SerializeField]
        protected int firstStateIndex = CommonState.Idle;
        [SerializeField]
        protected StateInfo entryStateInfo = new StateInfo("Entry",CommonState.Entry,CommonState.Idle,false);   // -1
        [SerializeField]
        protected StateInfo idleStateInfo = new StateInfo("Idle",CommonState.Idle,CommonState.Idle + 1,true);    // 0
        [SerializeField]
        protected List<StateInfo> additionalStateInfos = new List<StateInfo>();   // 1 ~ n
        [SerializeField]
        protected StateInfo exitStateInfo = new StateInfo("Exit",CommonState.Idle + 1,CommonState.ExitCompleted,false);    // n + 1

        /// <summary>継承メソッド：各AnimatedUI用の処理</summary>
        protected abstract void ResetContent();
        protected abstract void SetAdditionalProperties(ref List<Property> properties);
        protected abstract void SetAdditionalFlags(ref List<Flag> flags,bool defaultFlag);
        protected abstract void OnValidateContent(StateInfo info);
        protected abstract void TransitContent(float ratio);
        protected abstract void InitializeContent();
        protected abstract void UninitializeContent();
        protected abstract void StartTransitContent(int targetIndex);
        protected abstract void InterruptContent(int targetIndex);
        protected abstract void EndTransitContent();
        protected abstract void ApplyAdditionalValues();

        public void AddState(string stateName)
        {
            additionalStateInfos.Add(new StateInfo(stateName,0,0,true));
        }

        void Reset()
        {
            addStateCount = 0;
            ResetContent();
            rect = GetComponent<RectTransform>();
            SetupState(entryStateInfo,true);
            SetupState(idleStateInfo,true);
            foreach(StateInfo info in additionalStateInfos)
            {
                SetupState(info,true);
            }
            SetupState(exitStateInfo,true);
        }

        void Start()
        {
            currentStateIndex = CommonState.Uninitialized;
            Initialize();
        }

        void Awake()
        {
            if(currentStateIndex != CommonState.Uninitialized && resetOnAwake)
            {
                transitRatio = 0.0f;
                transitTimer = 0.0f;
                transitDuration = 0.0f;
                isInTransit = false;
                currentTransition = null;
                currentStateIndex = CommonState.Entry;
                ApplyTransformValues();
            }
        }

        void OnValidate()
        {
            if(!rect) rect = GetComponent<RectTransform>();
            if(origin == null)
            {
                addStateCount = additionalStateInfos.Count;
                SetDefaultTransformInfo(entryStateInfo);
                OnValidateContent(entryStateInfo);
                entryStateInfo.ClampTargetStateIndex(CommonState.Idle,CommonState.Idle);
                SetDefaultTransformInfo(idleStateInfo);
                OnValidateContent(idleStateInfo);
                idleStateInfo.ClampTargetStateIndex(CommonState.Idle,addStateCount + 1);
                for(int i = 0;i < addStateCount;i++)
                {
                    SetupState(additionalStateInfos[i],true);
                    SetDefaultTransformInfo(additionalStateInfos[i]);
                    OnValidateContent(additionalStateInfos[i]);
                    additionalStateInfos[i].index = i + 1;
                    additionalStateInfos[i].ClampTargetStateIndex(CommonState.Idle,addStateCount + 1);
                }
                SetDefaultTransformInfo(exitStateInfo);
                OnValidateContent(exitStateInfo);

                exitStateInfo.index = addStateCount + 1;

                if(firstStateIndex != entryStateIndexBackup)
                {
                    entryStateIndexBackup = firstStateIndex;
                    entryStateInfo.defaultGoingTransit.targetState = entryStateIndexBackup;
                }
                else if(entryStateInfo.defaultGoingTransit.targetState != entryStateIndexBackup)
                {
                    entryStateIndexBackup = entryStateInfo.defaultGoingTransit.targetState;
                    firstStateIndex = entryStateIndexBackup;
                }

                if(destroyOnExit)
                {
                    if(exitStateInfo.defaultGoingTransit.targetState != CommonState.Destroy) exitStateInfo.defaultGoingTransit.targetState = CommonState.Destroy;
                }
                else
                {
                    if(exitStateInfo.defaultGoingTransit.targetState != CommonState.ExitCompleted) exitStateInfo.defaultGoingTransit.targetState = CommonState.ExitCompleted;
                }
            }
        }

        void OnDestroy()
        {
            Uninitialize();
            instanceAmount--;
        }

        void Initialize()
        {
            origin = new GameObject("AnimeUIOrigin_" + instanceAmount++,typeof(RectTransform));
            originRect = origin.GetComponent<RectTransform>();
            origin.transform.SetParent(gameObject.transform.parent,true);
            originRect.localScale = Vector3.one;
            gameObject.transform.SetParent(origin.transform);
            if(!rect) rect = GetComponent<RectTransform>();

            transitRatio = 0.0f;
            transitTimer = 0.0f;
            transitDuration = 0.0f;
            isInTransit = false;
            nextStateIndex = 0;
            exitStateIndex = 0;
            currentTransition = null;
            currentStateIndex = CommonState.Entry;

            SetupProperties(ref startProperties);
            SetupProperties(ref endProperties);
            SetupProperties(ref currentProperties);

            InitializeContent();
            RegisterInfosToDictionary();
            SetPropertieValues(ref currentProperties,ref entryStateInfo.properties);
            ApplyTransformValues();
            origin.SetActive(false);

            if(entryOnStart) Invoke("Entry",entryStateInfo.defaultGoingTransit.duration);
        }

        void Uninitialize()
        {
            UninitializeContent();

            if(entryStateInfo != null) entryStateInfo.Dispose();
            entryStateInfo = null;

            if(idleStateInfo != null) idleStateInfo.Dispose();
            idleStateInfo = null;

            for(int i = 0,max = additionalStateInfos.Count;i < max;i++)
            {
                if(additionalStateInfos[i] != null) additionalStateInfos[i].Dispose();
                additionalStateInfos[i] = null;
            }

            if(exitStateInfo != null) exitStateInfo.Dispose();
            exitStateInfo = null;

            for(int i = 0,max = startProperties.Count;i < max;i++)
            {
                if(startProperties != null) startProperties[i] = null;
            }
            for(int i = 0,max = currentProperties.Count;i < max;i++)
            {
                currentProperties[i] = null;
            }
            for(int i = 0,max = endProperties.Count;i < max;i++)
            {
                endProperties[i] = null;
            }
            
            startProperties.Clear();
            currentProperties.Clear();
            endProperties.Clear();

            stateInfoDict.Clear();
            nameIndexDict.Clear();

            if(originRect) originRect = null;
            if(rect) rect = null;

            CancelInvoke();
        }

        void Destroy()
        {
            if(origin) GameObject.Destroy(origin);
            origin = null;
        }

        void RegisterInfosToDictionary()
        {
            int addCount = additionalStateInfos.Count;
            stateInfoDict.Clear();
            nameIndexDict.Clear();

            stateInfoDict.Add(CommonState.Entry,entryStateInfo);
            nameIndexDict.Add(entryStateInfo.name,entryStateInfo.index);

            stateInfoDict.Add(CommonState.Idle,idleStateInfo);
            nameIndexDict.Add(idleStateInfo.name,idleStateInfo.index);

            for(int i = 0;i < addCount;i++)
            {
                stateInfoDict.Add(i + 1,additionalStateInfos[i]);
                nameIndexDict.Add(additionalStateInfos[i].name,additionalStateInfos[i].index);
            }

            stateInfoDict.Add(addCount + 1,exitStateInfo);
            nameIndexDict.Add(exitStateInfo.name,exitStateInfo.index);

            exitStateIndex = addCount + 1;
        }

        void SetDefaultTransformInfo(StateInfo info)
        {
            if(!rect) return;
            if(info.flags[(int)CommonFlag.UseDefaultPosition].IsChecked) info.properties[(int)CommonProperty.Position].Vector = rect.localPosition;
            if(info.flags[(int)CommonFlag.UseDefaultRotation].IsChecked) info.properties[(int)CommonProperty.Rotation].Vector = rect.localEulerAngles;
            if(info.flags[(int)CommonFlag.UseDefaultScaling].IsChecked) info.properties[(int)CommonProperty.Scaling].Vector = rect.localScale;
            if(info.flags[(int)CommonFlag.UseDefaultSize].IsChecked) info.properties[(int)CommonProperty.Size].Vector = rect.sizeDelta;
        }

        void SetupState(StateInfo info,bool defaultFlag = false)
        {
            SetupProperties(ref info.properties);
            SetupFlags(ref info.flags,defaultFlag);
        }

        void SetupProperties(ref List<Property> propertyList)
        {
            if(propertyList == null || propertyList.Count <= 0) propertyList = new List<Property>();
            else return;
            propertyList.Add(new Property(CommonProperty.Position.ToString(),Vector3.zero));           // "Position"
            propertyList.Add(new Property(CommonProperty.Rotation.ToString(),rect.localEulerAngles));  // "Rotation"
            propertyList.Add(new Property(CommonProperty.Scaling.ToString(),rect.localScale));         // "Scaling"
            propertyList.Add(new Property(CommonProperty.Size.ToString(),rect.sizeDelta));             // "Size"
            propertyList.Add(new Property(CommonProperty.Offset_Position.ToString(),Vector3.zero));    // "Offset_Position"
            propertyList.Add(new Property(CommonProperty.Offset_Rotation.ToString(),Vector3.zero));    // "Offset_Rotation"
            propertyList.Add(new Property(CommonProperty.Offset_Scaling.ToString(),Vector2.zero));     // "Offset_Scaling"
            propertyList.Add(new Property(CommonProperty.Offset_Size.ToString(),Vector2.zero));        // "Offset_Size"
            propertyList.Add(new Property(CommonProperty.Color.ToString(),Color.white));               // "Color"
            SetAdditionalProperties(ref propertyList);
        }

        void SetupFlags(ref List<Flag> flagList,bool defaultFlag = false)
        {
            if(flagList == null || flagList.Count <= 0) flagList = new List<Flag>();
            else return;
            flagList.Add(new Flag(CommonFlag.UseDefaultPosition.ToString(),defaultFlag));  // "UseDefaultPosition"
            flagList.Add(new Flag(CommonFlag.UseDefaultRotation.ToString(),defaultFlag));  // "UseDefaultRotation"
            flagList.Add(new Flag(CommonFlag.UseDefaultScaling.ToString(),defaultFlag));  // "UseDefaultScaling"
            flagList.Add(new Flag(CommonFlag.UseDefaultSize.ToString(),defaultFlag));  // "UseDefaultSize"
            flagList.Add(new Flag(CommonFlag.UseDefaultColor.ToString(),defaultFlag));  // "UseDefaultColor"
            SetAdditionalFlags(ref flagList,defaultFlag);
        }

        void FixedUpdate()
        {
            if(!isInTransit)
            {
                stateInfoDict[currentStateIndex].OnStateUpdate();
            }
            else
            {
                UpdateTimer();
                UpdateTransitRatio(currentTransition);
                Transit(transitRatio);
                CheckTransitEnd();
            }
        }

        public void Entry()
        {
            origin.SetActive(true);
            StartTransit(firstStateIndex);
        }

        public void ReEntry()
        {
            currentStateIndex = CommonState.Entry;
            SetPropertieValues(ref currentProperties,ref entryStateInfo.properties);
            Entry();
        }

        public void Exit()
        {
            StartTransit(exitStateIndex);
        }

        public void SetState(int targetIndex)
        {
            if(!stateInfoDict.ContainsKey(targetIndex)) return;
            StartTransit(targetIndex);
        }

        public void SetState(string targetStateName)
        {
            if(!nameIndexDict.TryGetValue(targetStateName,out int targetIndex)) return;
            StartTransit(targetIndex);
        }

        void UpdateTimer()
        {
            if(useUnscaledTimeOnTransit) transitTimer += transitSpeedMultiplier * TimeUtility.UnscaledDeltaTime;
            else transitTimer += transitSpeedMultiplier * Time.fixedDeltaTime;
        }

        void UpdateTransitRatio(TransitionInfo transition)
        {
            transitRatio = Easing.Ease(transitTimer,transitDuration,transition.easeCurve);
        }

        TransitionInfo GetTransitInfoByTargetIndex(List<TransitionInfo> transitList,int targetIndex)
        {
            int max = transitList.Count;
            
            for(int i = 0;i < max;i++)
            {
                if(transitList[i].targetState == targetIndex) return transitList[i];
            }

            return null;
        }

        void StartTransit(int targetIndex)
        {
            if(isInTransit)
            {
                if(currentTransition.enableInterrupt)
                {
                    InterruptTransit(targetIndex);
                    return;
                }
                else
                {
                    return;
                }
            }
            else
            {
                StartTransitNormaly(targetIndex);
            }
        }

        void StartTransitNormaly(int targetIndex)
        {
            isInTransit = true;
            nextStateIndex = targetIndex;

            SetPropertieValues(ref startProperties,ref stateInfoDict[currentStateIndex].properties);
            endProperties = stateInfoDict[nextStateIndex].properties;

            StartTransitContent(targetIndex);

            SetTransition(targetIndex);
            SetTimer(currentTransition.duration);
            Transit(0.0f);
        }

        void InterruptTransit(int targetIndex)
        {
            isInTransit = true;
            nextStateIndex = targetIndex;

            currentTransition.OnTransitAbort(transitRatio);

            SetPropertieValues(ref startProperties,ref currentProperties);
            endProperties = stateInfoDict[nextStateIndex].properties;

            InterruptContent(targetIndex);

            SetTransition(targetIndex);
            SetTimer(currentTransition.duration);
        }

        void SetTransition(int targetIndex)
        {
            if(GetTransitInfoByTargetIndex(stateInfoDict[currentStateIndex].customizedGoingTransitions,targetIndex) != null)
            {
                currentTransition = GetTransitInfoByTargetIndex(stateInfoDict[currentStateIndex].customizedGoingTransitions,targetIndex);
            }
            else
            {
                currentTransition = stateInfoDict[currentStateIndex].defaultGoingTransit;
                currentTransition.targetState = targetIndex;
            }

            currentTransition.OnTransitStart();
        }

        void SetTimer(float duration)
        {
            transitTimer = 0.0f;
            transitDuration = duration;
        }

        void EndTransit()
        {
            if(!isInTransit) return;
            Transit(1.0f);
            EndTransitContent();
            stateInfoDict[currentStateIndex].OnStateExit();
            currentTransition.OnTransitEnd();
            stateInfoDict[nextStateIndex].OnStateEntry();
            isInTransit = false;
            currentTransition = null;
            currentStateIndex = nextStateIndex;
            
            if(currentStateIndex == exitStateIndex)
            {
                currentStateIndex = exitStateInfo.defaultGoingTransit.targetState;
            }

            switch(currentStateIndex)
            {
                case CommonState.ExitCompleted:
                currentStateIndex = CommonState.Entry;
                origin.SetActive(false);
                break;
                case CommonState.ReEntry:
                Invoke("ReEntry",exitStateInfo.defaultGoingTransit.duration);
                break;
                case CommonState.Destroy:
                Invoke("Destroy",exitStateInfo.defaultGoingTransit.duration);
                break;
                default:
                if(!stateInfoDict[currentStateIndex].disableAutomaticTransit) {StartTransit(stateInfoDict[currentStateIndex].defaultGoingTransit.targetState);}
                break;
            }
        }

        void CheckTransitEnd()
        {
            if(transitTimer > transitDuration)
            {
                EndTransit();
            }
        }

        void Transit(float ratio)
        {
            TransitCommonInfo(ratio);
            TransitContent(ratio);
            ApplyTransformValues();
            ApplyAdditionalValues();
            currentTransition.OnTransitProgress(ratio);
        }

        void ApplyTransformValues()
        {
            // 絶対
            originRect.localPosition = currentProperties[(int)CommonProperty.Position].Vector;
            originRect.localEulerAngles = currentProperties[(int)CommonProperty.Rotation].Vector;

            // 相対
            rect.localPosition = currentProperties[(int)CommonProperty.Offset_Position].Vector;
            rect.localEulerAngles = currentProperties[(int)CommonProperty.Offset_Rotation].Vector;

            // スケール・サイズ
            rect.sizeDelta = currentProperties[(int)CommonProperty.Size].Vector + currentProperties[(int)CommonProperty.Offset_Size].Vector;
            rect.localScale = currentProperties[(int)CommonProperty.Scaling].Vector + currentProperties[(int)CommonProperty.Offset_Scaling].Vector;
        }

        void TransitCommonInfo(float ratio)
        {
            TransitProperties(ref currentProperties,ref startProperties,ref endProperties,ratio);
        }

        void SetPropertieValues(ref List<Property> targetList,ref List<Property> originalList)
        {
            for(int i = 0,max = targetList.Count;i < max;i++)
            {
                targetList[i].SetValues(originalList[i]);
            }
        }

        void TransitProperties(ref List<Property> targetList,ref List<Property> currentList,ref List<Property> nextList,float ratio)
        {
            for(int i = 0,max = targetList.Count;i < max;i++)
            {
                if(currentList[i].Equals(nextList[i])) continue;    // 値が全く同じならスキップ
                targetList[i].GetInterpolated(currentList[i],nextList[i],ratio);    // 補正値を取得
            }
        }

        protected void TransitInt(ref int target,ref int current,ref int next,float ratio)
        {
            target = (int)Easing.Ease((float)current,(float)next,ratio,1.0f,Easing.Curve.Linear);
        }

        protected void TransitFloat(ref float target,ref float current,ref float next,float ratio)
        {
            target = Easing.Ease(current,next,ratio,1.0f,Easing.Curve.Linear);
        }

        protected void TransitVector2(ref Vector2 target,ref Vector2 current,ref Vector2 next,float ratio)
        {
            target = Easing.Ease(current,next,ratio,1.0f,Easing.Curve.Linear);
        }

        protected void TransitVector3(ref Vector3 target,ref Vector3 current,ref Vector3 next,float ratio)
        {
            target = Easing.Ease(current,next,ratio,1.0f,Easing.Curve.Linear);
        }

        protected void TransitVector4(ref Vector4 target,ref Vector4 current,ref Vector4 next,float ratio)
        {
            target = Easing.Ease(current,next,ratio,1.0f,Easing.Curve.Linear);
        }

        protected void TransitColor(ref Color target,ref Color current,ref Color next,float ratio)
        {
            target = Easing.Ease(current,next,ratio,1.0f,Easing.Curve.Linear);
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace AnimatedUI
{
    public class AnimatedUIGroup : MonoBehaviour,IAnimatable
    {
        [System.Serializable]
        class AnimatedUIList : IAnimatable
        {
            [SerializeField]
            float actionDelay = 0.0f;

            [SerializeField]
            public List<AnimatedUIElement> elements = new List<AnimatedUIElement>();

            public void Entry()
            {
                float delay = actionDelay;
                for(int i = 0,max = elements.Count;i < max;i++)
                {
                    GlobalCoroutine.Play(CoroutineUtility.DelayMethod(actionDelay * (float)i,elements[i].Entry));
                }
            }

            public void ReEntry()
            {
                float delay = actionDelay;
                for(int i = 0,max = elements.Count;i < max;i++)
                {
                    GlobalCoroutine.Play(CoroutineUtility.DelayMethod(actionDelay * (float)i,elements[i].ReEntry));
                }
            }

            public void Exit()
            {
                for(int i = 0,max = elements.Count;i < max;i++)
                {
                    GlobalCoroutine.Play(CoroutineUtility.DelayMethod(actionDelay * (float)i,elements[i].Exit));
                }
            }

            public void SetState(int targetIndex)
            {
                for(int i = 0,max = elements.Count;i < max;i++)
                {
                    GlobalCoroutine.Play(CoroutineUtility.DelayMethod(actionDelay * (float)i,elements[i].SetState,targetIndex));
                }
            }

            public void SetState(string targetStateName)
            {
                for(int i = 0,max = elements.Count;i < max;i++)
                {
                    GlobalCoroutine.Play(CoroutineUtility.DelayMethod(actionDelay * (float)i,elements[i].SetState,targetStateName));
                }
            }
        }

        [SerializeField]
        float actionDelay = 0.0f;

        [SerializeField]
        List<AnimatedUIList> groups = new List<AnimatedUIList>();

        [SerializeField]
        public bool entryOnStart = false;
        [SerializeField]
        public bool resetOnAwake = false;

        void Reset()
        {
            Fetch();
        }

        void Start()
        {
            if(entryOnStart) Entry();
        }

        void Awake()
        {
            if(resetOnAwake) ReEntry();
        }

        [ContextMenu("ENTRYALL")]
        public void Entry()
        {
            float delay = actionDelay;
            for(int i = 0,max = groups.Count;i < max;i++)
            {
                GlobalCoroutine.Play(CoroutineUtility.DelayMethod(actionDelay * (float)i,groups[i].Entry));
            }
        }

        public void ReEntry()
        {
            float delay = actionDelay;
            for(int i = 0,max = groups.Count;i < max;i++)
            {
                GlobalCoroutine.Play(CoroutineUtility.DelayMethod(actionDelay * (float)i,groups[i].ReEntry));
            }
        }

        [ContextMenu("EXITALL")]
        public void Exit()
        {
            for(int i = 0,max = groups.Count;i < max;i++)
            {
                GlobalCoroutine.Play(CoroutineUtility.DelayMethod(actionDelay * (float)i,groups[i].Exit));
            }
        }

        public void SetState(int targetIndex)
        {
            for(int i = 0,max = groups.Count;i < max;i++)
            {
                GlobalCoroutine.Play(CoroutineUtility.DelayMethod<int>(actionDelay * (float)i,groups[i].SetState,targetIndex));
            }
        }

        public void SetState(string targetStateName)
        {
            for(int i = 0,max = groups.Count;i < max;i++)
            {
                GlobalCoroutine.Play(CoroutineUtility.DelayMethod<string>(actionDelay * (float)i,groups[i].SetState,targetStateName));
            }
        }


        [ContextMenu("Fetch")]
        public void Fetch()
        {
            groups.Clear();
            groups.Add(new AnimatedUIList());
            groups[0].elements.AddRange(GetComponentsInChildren<AnimatedUIElement>());
        }
    }

}
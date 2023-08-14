using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interpolation;
using AnimatedUI;

namespace AnimatedUI
{
    [RequireComponent(typeof(AnimatedUIElement))]
    public abstract class AnimatedUIEffect : MonoBehaviour
    {
        [SerializeField]
        protected float intensity = 1.0f;
        [SerializeField]
        protected float duration = 0.0f;
        [SerializeField]
        protected bool enableDecreaseIntensity = true;
        [SerializeField]
        protected Easing.Curve decreaseCurve = Easing.Curve.Linear;
        [SerializeField]
        protected float animateSpeedMultiplier = 1.0f;
        [SerializeField]
        protected bool useUnscaledTime = false;
        [SerializeField]
        protected Easing.Curve easeCurve = Easing.Curve.Linear;

        AnimatedUIElement animeUI = null;
        protected bool isInEffect = false;
        protected float progressRatio = 0.0f;
        protected float timer = 0.0f;
        protected float currentIntensity = 0.0f;
        protected float totalDuration = 0.0f;


        protected abstract void Animate(float ratio,float intensity);

        void Reset()
        {
            if(!animeUI) animeUI = GetComponent<AnimatedUIElement>();
        }

        void FixedUpdate()
        {
            if(isInEffect)
            {
                UpdateProgressRatio();
                Animate(progressRatio,currentIntensity);
                UpdateTimer();
                CheckProgressEnd();
            }
        }

        void UpdateTimer()
        {
            if(useUnscaledTime) timer += animateSpeedMultiplier * TimeUtility.UnscaledDeltaTime;
            else timer += animateSpeedMultiplier * Time.fixedDeltaTime;
        }

        void UpdateProgressRatio()
        {
            progressRatio = Mathf.Clamp01(Easing.Ease(timer,totalDuration,easeCurve));
        }

        void DecreaseIntensity()
        {
            if(enableDecreaseIntensity) currentIntensity *= (1.0f - Easing.Ease(timer,totalDuration,decreaseCurve));
        }

        void CheckProgressEnd()
        {
            if(timer >= totalDuration)
            {
                isInEffect = false;
            }
        }

        public void Play(float additionalDuration = 0.0f,float additionalIntensity = 0.0f)
        {
            isInEffect = true;
            this.totalDuration = duration + additionalDuration;
            this.currentIntensity = intensity + additionalIntensity;
            timer = 0.0f;
        }

        public void AddIntensity(float additionalIntensity)
        {
            this.currentIntensity += additionalIntensity;
        }

        public void ScaleIntensity(float scale)
        {
            this.currentIntensity = currentIntensity * scale;
        }

        public void Extend()
        {
            this.totalDuration += duration;
        }

        public void Extend(float extendDuration)
        {
            this.totalDuration += extendDuration;
        }

        public void Pause()
        {
            isInEffect = false;
        }

        public void Abort()
        {
            isInEffect = false;
            progressRatio = 0.0f;
            timer = 0.0f;
        }

        void Stop()
        {
            isInEffect = false;
            progressRatio = 1.0f;
        }
    }
}


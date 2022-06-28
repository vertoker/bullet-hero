using System.Collections;
using System.Collections.Generic;
using LevelEditor.UI;
using UnityEngine;
using UnityEngine.Events;
using Game.Provider;
using Utils;
using UI;

namespace LevelEditor.Windows
{
    public class TimelineRuntime : Window, ITimelineController
    {
        private Runtime runtime;
        [SerializeField] private Timeline timeline;
        [SerializeField] private Slider slider;

        private UnityAction<int, int> updateActive;
        private UnityAction<float> updateCall;

        private void Awake()
        {
            GamePreview.InitEvent += Init;
        }
        public void Init(Runtime runtime)
        {
            GamePreview.InitEvent -= Init;
            this.runtime = runtime;
            timeline.Init(runtime.Length, this);
        }

        public bool IsPlay => runtime.IsPlay;
        public float SecCurrent { get => runtime.Timer; set => runtime.SetSec(value); }

        public void Add(UnityAction<float> update)
        {
            if (updateActive != null)
                Runtime.UpdateFrame -= updateActive;
            updateActive = UpdateTimeline;
            updateCall = update;
            Runtime.UpdateFrame += updateActive;
        }
        public void Play()
        {
            runtime.PlaySafe();
        }
        public void Pause()
        {
            runtime.Pause();
        }

        public void UpdateTimeline(int frame, int maxFrame)
        {
            updateCall.Invoke(Static.Frame2Sec(frame));
        }
    }
}
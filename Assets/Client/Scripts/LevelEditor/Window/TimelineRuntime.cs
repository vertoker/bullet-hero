using System.Collections;
using System.Collections.Generic;
using LevelEditor.UI;
using UnityEngine;
using UnityEngine.Events;
using Game.Provider;
using Utils;

namespace LevelEditor.Windows
{
    public class TimelineRuntime : Window, ITimelineController
    {
        private Runtime runtime;
        private Timeline timeline;
        private UnityAction<int, int> updateActive;
        private UnityAction<float> updateCall;

        private void Awake()
        {
            timeline = GetComponent<Timeline>();
        }
        public void Init(Runtime runtime)
        {
            this.runtime = runtime;
            timeline.Init(runtime.Length, this);
        }

        public bool IsPlay => runtime.Play;
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
            runtime.StartMove();
        }
        public void Pause()
        {
            runtime.StopMove();
        }

        public void UpdateTimeline(int frame, int maxFrame)
        {
            updateCall.Invoke(Static.Frame2Sec(frame));
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using LevelEditor.UI;
using UnityEngine;
using UnityEngine.Events;
using Game.Provider;
using Utils;

namespace LevelEditor.Windows
{
    public class TimelinePrefab : Window, ITimelineController
    {
        private Runtime runtime;
        private Timeline timeline;
        private UnityAction<int, int> updateActive;
        private UnityAction<float> updateCall;

        private void Awake()
        {
            timeline = GetComponent<Timeline>();
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
            runtime.Play();
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
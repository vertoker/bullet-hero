using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

namespace LevelEditor.UI
{
    public interface ITimelineController
    {
        bool IsPlay { get; }
        float SecCurrent { get; set; }

        void Add(UnityAction<float> update);
        void Play();
        void Pause();
    }
}
using UnityEngine;
using System;

namespace Game.Components
{
    [Serializable]
    public struct Zoom
    {
        [SerializeField] private float t;// Время
        [SerializeField] private float s;// Коэффициент приближения

        public float Time { get { return t; } set { t = value; } }
        public float Size { get { return s; } set { s = value; } }

        public Zoom(float t, float s)
        {
            this.t = t;
            this.s = s;
        }
    }
}
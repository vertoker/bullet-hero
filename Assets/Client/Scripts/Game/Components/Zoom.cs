using UnityEngine;
using System;

namespace Game.Components
{
    [Serializable]
    public struct Zoom
    {
        [SerializeField] private float t;// �����
        [SerializeField] private float s;// ����������� �����������

        public float Time { get { return t; } set { t = value; } }
        public float Size { get { return s; } set { s = value; } }

        public Zoom(float t, float s)
        {
            this.t = t;
            this.s = s;
        }
    }
}
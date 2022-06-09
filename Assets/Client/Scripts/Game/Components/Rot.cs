using UnityEngine;
using System;

using Data;

namespace Game.Components
{
    [Serializable]
    public struct Rot
    {
        [SerializeField] private int f; // ���������� ����� �� ������� �� ������ (frame)
        [SerializeField] private FloatRandomType r;// ��� ������� � ���� (type of random)
        [SerializeField] private EasingType e;// ��� ������� (easing)
        // ��������� ����
        [SerializeField] private float sa;// start angle
        [SerializeField] private float ea;// end angle
        [SerializeField] private float i; // interval

        public int Frame { get { return f; } set { f = value; } }
        public FloatRandomType RandomType { get { return r; } set { r = value; } }
        public EasingType Easing { get { return e; } set { e = value; } }
        public float SA { get { return sa; } set { sa = value; } }
        public float EA { get { return ea; } set { ea = value; } }
        public float Interval { get { return i; } set { i = value; } }
    }
}
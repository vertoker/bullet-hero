using UnityEngine;
using System;

using Data;

namespace Game.Components
{
    [Serializable]
    public struct Clr
    {
        [SerializeField] private int f; // Глобальная метка во времени по кадрам (frame)
        [SerializeField] private ColorRandomType r;// Тип рандома у угла (type of random)
        [SerializeField] private EasingType e;// Тип функции (easing)
        // Параметры угла
        [SerializeField] private float sr;// start r
        [SerializeField] private float sg;// start g
        [SerializeField] private float sb;// start b
        [SerializeField] private float sa;// start a
        [SerializeField] private float er;// end r
        [SerializeField] private float eg;// end g
        [SerializeField] private float eb;// end b
        [SerializeField] private float ea;// end a
        [SerializeField] private float i; // interval

        public int Frame { get { return f; } set { f = value; } }
        public ColorRandomType RandomType { get { return r; } set { r = value; } }
        public EasingType Easing { get { return e; } set { e = value; } }
        public float SR { get { return sr; } set { sr = value; } }
        public float SG { get { return sg; } set { sg = value; } }
        public float SB { get { return sb; } set { sb = value; } }
        public float SA { get { return sa; } set { sa = value; } }
        public float ER { get { return er; } set { er = value; } }
        public float EG { get { return eg; } set { eg = value; } }
        public float EB { get { return eb; } set { eb = value; } }
        public float EA { get { return ea; } set { ea = value; } }
        public float Interval { get { return i; } set { i = value; } }
    }
}
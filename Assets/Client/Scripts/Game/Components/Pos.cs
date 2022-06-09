using UnityEngine;
using System;

using Data;

namespace Game.Components
{
    [Serializable]
    public struct Pos
    {
        [SerializeField] private int f; // Глобальная метка во времени по кадрам (frame)
        [SerializeField] private VectorRandomType r;// Тип рандома у позиции (type of random)
        [SerializeField] private EasingType e;// Тип функции (easing)
        // Параметры позиции
        [SerializeField] private float sx;// start x
        [SerializeField] private float sy;// start y
        [SerializeField] private float ex;// end x
        [SerializeField] private float ey;// end y
        [SerializeField] private float i; // interval

        public int Frame { get { return f; } set { f = value; } }
        public VectorRandomType RandomType { get { return r; } set { r = value; } }
        public EasingType Easing { get { return e; } set { e = value; } }
        public float SX { get { return sx; } set { sx = value; } }
        public float SY { get { return sy; } set { sy = value; } }
        public float EX { get { return ex; } set { ex = value; } }
        public float EY { get { return ey; } set { ey = value; } }
        public float Interval { get { return i; } set { i = value; } }
    }
}
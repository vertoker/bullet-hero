using UnityEngine;
using System;

namespace Data
{
    /// <summary>
    /// Сохранение игрока на уровне
    /// <summary>
    [Serializable]
    public struct Checkpoint
    {
        [SerializeField] private bool a;// Активность чекпоинта (active)
        [SerializeField] private string n;// Название чекпоинта (name)
        [SerializeField] private int f;// На каком моменте времени находиться чекпоинт по кадрам (frame)
        [SerializeField] private VectorRandomType r;// Тип рандома у позиции (type of random)

        // Параметры позиции
        [SerializeField] private float sx;// start x
        [SerializeField] private float sy;// start y
        [SerializeField] private float ex;// end x
        [SerializeField] private float ey;// end y
        [SerializeField] private float i; // internal

        public bool Active { get { return a; } set { a = value; } }
        public string Name { get { return n; } set { n = value; } }
        public int Frame { get { return f; } set { f = value; } }
        public VectorRandomType RandomType { get { return r; } set { r = value; } }
        public float SX { get { return sx; } set { sx = value; } }
        public float SY { get { return sy; } set { sy = value; } }
        public float EX { get { return ex; } set { ex = value; } }
        public float EY { get { return ey; } set { ey = value; } }
        public float Interval { get { return i; } set { i = value; } }

        public Checkpoint(bool active,
            string name,
            int frame,
            VectorRandomType random_type,
            float sx, float sy,
            float ex = 0, float ey = 0,
            float i = 0)
        {
            a = active;
            n = name;
            f = frame;
            r = random_type;

            this.sx = sx;
            this.sy = sy;
            this.ex = ex;
            this.ey = ey;
            this.i = i;
        }
    }
}
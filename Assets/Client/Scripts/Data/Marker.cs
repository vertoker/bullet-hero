using UnityEngine;
using System;

namespace Data
{
    /// <summary>
    /// Маркеры для редактора
    /// <summary>
    [Serializable]
    public struct Marker
    {
        [SerializeField] private string n;// Имя маркера (name)
        [SerializeField] private string d;// Описание маркера (description)
        [SerializeField] private int f;// На каком моменте времени находиться метка по кадрам (frame)
        [SerializeField] private float r;// Цвет маркера (красное) (red)
        [SerializeField] private float g;// Цвет маркера (синее) (green)
        [SerializeField] private float b;// Цвет маркера (зелёное) (blue)

        public string Name { get { return n; } set { n = value; } }
        public string Description { get { return d; } set { d = value; } }
        public int Frame { get { return f; } set { f = value; } }
        public float Red { get { return r; } set { r = value; } }
        public float Green { get { return g; } set { g = value; } }
        public float Blue { get { return b; } set { b = value; } }

        public Marker(string name,
            string description,
            int frame,
            float red,
            float green,
            float blue)
        {
            n = name;
            d = description;
            f = frame;
            r = red;
            g = green;
            b = blue;
        }
    }
}
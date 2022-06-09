using System.Collections.Generic;
using UnityEngine;
using System;

using Data;

namespace Game.Components
{
    /// <summary>
    /// Объекты в уровне с данными для игры
    /// <summary>
    [Serializable]
    public struct Prefab
    {
        [SerializeField] private string n;// Имя у объекта (name)
        [SerializeField] private bool a;// Активность объекта (active)
        [SerializeField] private bool c;// Наличие коллайдера у объекта (collider)
        [SerializeField] private List<Pos> pos;// Список с метками позиции (position)
        [SerializeField] private List<Sca> sca;// Список с метками размера (scale)
        [SerializeField] private List<Rot> rot;// Список с метками поворота (rotation)
        [SerializeField] private List<Clr> clr;// Список с метками цветов (color)
        [SerializeField] private SpriteType st;// Тип спрайта у объекта (sprite type)
        [SerializeField] private int sf;// На этом моменте времени создаётся объект (start frame)
        [SerializeField] private int ef;// На этом моменте времени удаляется объект (end frame)
        [SerializeField] private AnchorPresets ap;// Якорь объекта (anchor)
        [SerializeField] private int id, idp;// ID объекта и его родителя (id, idp)
        [SerializeField] private int l;// Слой на котором находиться объект, относительно других (layer) (выше или ниже)
        [SerializeField] private int h;//На каком потоке (в редакторе) находиться объект (height)

        public string Name { get { return n; } set { n = value; } }
        public bool Active { get { return a; } set { a = value; } }
        public bool Collider { get { return c; } set { c = value; } }
        public List<Pos> Pos { get { return pos ?? new List<Pos>(); ; } set { pos = value; } }
        public List<Sca> Sca { get { return sca; } set { sca = value; } }
        public List<Rot> Rot { get { return rot; } set { rot = value; } }
        public List<Clr> Clr { get { return clr; } set { clr = value; } }
        public SpriteType SpriteType { get { return st; } set { st = value; } }
        public int StartFrame { get { return sf; } set { sf = value; } }
        public int EndFrame { get { return ef; } set { ef = value; } }
        public AnchorPresets Anchor { get { return ap; } set { ap = value; } }
        public int ID { get { return id; } set { id = value; } }
        public int ParentID { get { return idp; } set { idp = value; } }
        public int Layer { get { return l; } set { l = value; } }
        public int Height { get { return h; } set { h = value; } }
        public int GetFrameLength { get { return ef - sf; } }

        public Prefab(string name,
            bool active,
            bool collider,
            List<Pos> pos,
            List<Sca> sca,
            List<Rot> rot,
            List<Clr> clr,
            SpriteType st,
            int startFrame,
            int endFrame,
            AnchorPresets anchor,
            int id, int idParent,
            int layer,
            int height)
        {
            n = name;
            a = active;
            c = collider;
            this.pos = pos;
            this.sca = sca;
            this.rot = rot;
            this.clr = clr;

            this.st = st;
            sf = startFrame;
            ef = endFrame;

            idp = idParent;
            this.id = id;

            ap = anchor;
            h = height;
            l = layer;
        }
    }
}
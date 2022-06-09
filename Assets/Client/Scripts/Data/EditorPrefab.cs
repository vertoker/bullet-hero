using System.Collections.Generic;
using UnityEngine;
using System;

using Game.Components;

namespace Data
{
    [Serializable]
    public struct EditorPrefab
    {
        [SerializeField] private List<Prefab> p;//ќбъекты, наход€€щиес€ в наследственности с главный объектом (главный объект на 0 позиции)

        public List<Prefab> Prefabs { get { return p; } set { p = value; } }
        public int CountObjects { get { return p.Count; } }
        public int GetFrameLength { get { return p[0].EndFrame - p[0].StartFrame; } }

        public EditorPrefab(List<Prefab> prefabs)
        {
            p = prefabs;
        }
    }
}
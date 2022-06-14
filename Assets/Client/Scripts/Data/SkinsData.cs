using System.Collections.Generic;
using UnityEngine;
using System;

namespace Data
{
    [CreateAssetMenu(menuName = "Skin/New Skin Data", fileName = "Skin Data", order = 1)]
    public class SkinsData : ScriptableObject
    {
        [SerializeField] private List<Skin> skins;

        public int Length => skins.Count;
        public Skin GetSkin(int id)
        {
            return skins[id];
        }

        public string[] LoadSkinList()
        {
            string[] list = new string[skins.Count];
            for (int i = 0; i < skins.Count; i++)
                list[i] = string.Format("{0} : {1}", skins[i].GetName(), skins[i].Price);
            return list;
        }
    }
}

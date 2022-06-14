using System.Collections.Generic;
using Game.SerializationSaver;
using UnityEngine.Events;
using UnityEngine;
using System.IO;
using System;

namespace Data
{
    [Serializable]
    public class ShopData
    {
        [SerializeField] private int money;
        [SerializeField] private bool[] skinStatuses;
        [SerializeField] private int selected;

        public int Money { get { return money; } set { money = value; } }
        public bool[] SkinStatuses { get { return skinStatuses; } set { skinStatuses = value; } }
        public int Selected { get { return selected; } set { selected = value; } }

        private static UnityEvent<ShopData> dataUpdate = new UnityEvent<ShopData>();
        public static event UnityAction<ShopData> DataUpdate
        {
            add => dataUpdate.AddListener(value);
            remove => dataUpdate.RemoveListener(value);
        }

        public ShopData(int money, int selected)
        {
            this.money = money;
            this.selected = selected;
        }
        public void Fill(SkinsData skinsData)
        {
            if (skinStatuses.Length == 0) {
                skinStatuses = new bool[skinsData.Length];
                for (int i = 0; i < skinStatuses.Length; i++)
                    skinStatuses[i] = false;
                skinStatuses[0] = true;
            }
        }

        public static ShopData DEFAULT = new ShopData(25, 0);

        public static ShopData Loader
        {
            get
            {
                if (!Exists())
                    Save(DEFAULT);
                return Load();
            }
            set
            {
                Save(value);
                dataUpdate.Invoke(value);
            }
        }

        public static ShopData Load()
        {
            return Saver.Load<ShopData>(Application.persistentDataPath, "shop_data.json");
        }
        public static bool Exists()
        {
            return File.Exists(Path.Combine(Application.persistentDataPath, "shop_data.json"));
        }
        public static void Save(ShopData shopData)
        {
            string path = Path.Combine(Application.persistentDataPath, "shop_data.json");
            Saver.Save(shopData, path);
        }
    }
}
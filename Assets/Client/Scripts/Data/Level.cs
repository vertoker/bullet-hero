using System.Collections.Generic;
using UnityEngine;
using System;

using Game.Components;

namespace Data
{
    /// Это класс нужно сохранять в JSON
    /// Основной класс, хранящиц всю информацию об уровне
    [Serializable]
    public class Level
    {
        public int CURRENT_VERSION = 1;

        [SerializeField] private LevelData ld;// Базовая информация об уровне (level data)
        [SerializeField] private List<Marker> m;// Маркеры для редактора (markers)
        [SerializeField] private List<Checkpoint> c;// Сохранения игрока на уровне (checkpoints)
        [SerializeField] private List<Prefab> p;// Все объекты на сцене (prefabs)
        [SerializeField] private List<EditorPrefab> ep;// Объекты редактора (editor prefabs)
        [SerializeField] private CameraData cd;// Эффект движущейся камеры в игре (effects)

        public LevelData LevelData { get { return ld; } set { ld = value; } }
        public List<Marker> Markers { get { return m; } set { m = value; } }
        public List<Checkpoint> Checkpoints { get { return c; } set { c = value; } }
        public List<Prefab> Prefabs { get { return p; } set { p = value; } }
        public List<EditorPrefab> EditorPrefabs { get { return ep; } set { ep = value; } }
        public CameraData CameraData { get { return cd; } set { cd = value; } }

        public Level(LevelData level_data,
            List<Marker> markers,
            List<Checkpoint> checkpoints,
            List<Prefab> prefabs,
            List<EditorPrefab> editor_prefabs,
            CameraData camera_data)
        {
            ld = level_data;
            m = markers;
            c = checkpoints;
            p = prefabs;
            ep = editor_prefabs;
            cd = camera_data;
        }
    }
}

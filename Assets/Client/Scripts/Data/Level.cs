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
        public Level Copy()
        {
            return new Level(ld, m, c, p, ep, cd);
        }

        #region Static Default
        private static readonly LevelData levelData = new LevelData()
        {
            LevelName = "0 demo",
            EditorVersion = 1,
            MusicTitle = "1up muncher",
            MusicAuthor = "DUNDERPATRULLEN",
            LevelAuthor = "vertog",
            StartFadeOut = 184.5f,
            EndFadeOut = 188f
        };
        private static readonly List<Marker> markers = new List<Marker>()
        {
            new Marker()
            {
                Name = "marker 1",
                Description = string.Empty,
                Frame = 100,
                Red = 1, Green = 0, Blue = 0
            },
            new Marker()
            {
                Name = "marker 2",
                Description = string.Empty,
                Frame = 500,
                Red = 1, Green = 1, Blue = 1
            }
        };
        private static readonly List<Checkpoint> checkpoints = new List<Checkpoint>()
        {
            new Checkpoint()
            {
                Active = true,
                Name = "checkpoint 1",
                Frame = 500,
                RandomType = VectorRandomType.N,
                SX = 0, SY = 0
            },
            new Checkpoint()
            {
                Active = false,
                Name = "checkpoint 2",
                Frame = 700,
                RandomType = VectorRandomType.N,
                SX = 0, SY = 0
            },
            new Checkpoint()
            {
                Active = true,
                Name = "checkpoint 1",
                Frame = 1000,
                RandomType = VectorRandomType.MM,
                SX = -5, SY = -5, EX = 5, EY = 5
            }
        };
        private static readonly List<Prefab> prefabs = new List<Prefab>()
        {
            new Prefab()
            {
                Active = true,
                Collider = true,
                StartFrame = 100,
                EndFrame = 700,
                ID = 0, ParentID = 0,
                SpriteType = SpriteType.Square,

                Pos = new Pos[2]
                {
                    new Pos()
                    {
                        Frame = 100,
                        RandomType = VectorRandomType.N,
                        SX = -5, SY = -5
                    },
                    new Pos()
                    {
                        Frame = 700,
                        RandomType = VectorRandomType.N,
                        SX = 5, SY = 5
                    }
                },
                Rot = new Rot[2]
                {
                    new Rot()
                    {
                        Frame = 100,
                        RandomType = FloatRandomType.N,
                        SA = 0
                    },
                    new Rot()
                    {
                        Frame = 700,
                        RandomType = FloatRandomType.N,
                        SA = 360
                    },
                },
                Sca = new Sca[1]
                {
                    new Sca()
                    {
                        Frame = 100,
                        RandomType = VectorRandomType.N,
                        SX = 1, SY = 1
                    }
                },
                Clr = new Clr[1]
                {
                    new Clr()
                    {
                        Frame = 100,
                        RandomType = ColorRandomType.N,
                        SR = 1, SG = 1, SB = 1, SA = 1
                    }
                },
                Anchor = AnchorPresets.Center_Middle,
                Layer = 1
            },
            new Prefab()
            {
                Active = true,
                Collider = true,
                StartFrame = 200,
                EndFrame = 500,
                ID = 1, ParentID = 0,
                SpriteType = SpriteType.Square,

                Pos = new Pos[1]
                {
                    new Pos()
                    {
                        Frame = 200,
                        RandomType = VectorRandomType.N,
                        SX = 0, SY = 7
                    }
                },
                Rot = new Rot[1]
                {
                    new Rot()
                    {
                        Frame = 200,
                        RandomType = FloatRandomType.N,
                        SA = 0
                    }
                },
                Sca = new Sca[1]
                {
                    new Sca()
                    {
                        Frame = 100,
                        RandomType = VectorRandomType.N,
                        SX = 1, SY = 1
                    }
                },
                Clr = new Clr[1]
                {
                    new Clr()
                    {
                        Frame = 100,
                        RandomType = ColorRandomType.N,
                        SR = 1, SG = 0, SB = 0, SA = 1
                    }
                },
                Anchor = AnchorPresets.Center_Middle,
                Layer = 1
            }
        };
        public static readonly Level DEFAULT_LEVEL = new Level(levelData, markers, checkpoints, prefabs, null, new CameraData());
        #endregion
    }
}

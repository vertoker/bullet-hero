using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using System.IO;

class LevelManager// Convert JSON to Track Animation and reverse
{
    public static Level level;
    public static AudioClip clip;

    #region Static Delegates
    // Level Data
    public static UnityAction<string> LevelName = new UnityAction<string>((string value) => { level.LevelData.LevelName = value; });
    public static UnityAction<string> EditorVersion = new UnityAction<string>((string value) => { level.LevelData.EditorVersion = Utils.String2Int(value); });
    public static UnityAction<string> MusicTitle = new UnityAction<string>((string value) => { level.LevelData.MusicTitle = value; });
    public static UnityAction<string> MusicAuthor = new UnityAction<string>((string value) => { level.LevelData.MusicAuthor = value; });
    public static UnityAction<string> LevelAuthor = new UnityAction<string>((string value) => { level.LevelData.LevelAuthor = value; });
    public static UnityAction<string> StartFadeOut = new UnityAction<string>((string value) => { level.LevelData.StartFadeOut = Utils.String2Float(value); });
    public static UnityAction<string> EndFadeOut = new UnityAction<string>((string value) => { level.LevelData.EndFadeOut = Utils.String2Float(value); });

    // Markers
    public static UnityAction<string> MarkerName(int index)
    { return new UnityAction<string>((string value) => { level.Markers[index].Name = value; }); }
    public static UnityAction<string> MarkerDescription(int index)
    { return new UnityAction<string>((string value) => { level.Markers[index].Description = value; }); }
    public static UnityAction<string> MarkerFrame(int index)
    { return new UnityAction<string>((string value) => { level.Markers[index].Frame = Utils.String2Int(value); }); }
    public static UnityAction<float> MarkerColorR(int index)
    { return new UnityAction<float>((float value) => { level.Markers[index].Red = value; }); }
    public static UnityAction<float> MarkerColorG(int index)
    { return new UnityAction<float>((float value) => { level.Markers[index].Green = value; }); }
    public static UnityAction<float> MarkerColorB(int index)
    { return new UnityAction<float>((float value) => { level.Markers[index].Blue = value; }); }

    // Checkpoint
    public static UnityAction<bool> CheckpointActive(int index)
    { return new UnityAction<bool>((bool value) => { level.Checkpoints[index].Active = value; }); }
    public static UnityAction<string> CheckpointName(int index)
    { return new UnityAction<string>((string value) => { level.Checkpoints[index].Name = value; }); }
    public static UnityAction<string> CheckpointFrame(int index)
    { return new UnityAction<string>((string value) => { level.Checkpoints[index].Frame = Utils.String2Int(value); }); }
    public static UnityAction<int> CheckpointRandom(int index)
    { return new UnityAction<int>((int value) => { level.Checkpoints[index].RandomType = (VectorRandomType)value; }); }
    public static UnityAction<string> CheckpointSX(int index)
    { return new UnityAction<string>((string value) => { level.Checkpoints[index].SX = Utils.String2Float(value); }); }
    public static UnityAction<string> CheckpointSY(int index)
    { return new UnityAction<string>((string value) => { level.Checkpoints[index].SY = Utils.String2Float(value); }); }
    public static UnityAction<string> CheckpointEX(int index)
    { return new UnityAction<string>((string value) => { level.Checkpoints[index].EX = Utils.String2Float(value); }); }
    public static UnityAction<string> CheckpointEY(int index)
    { return new UnityAction<string>((string value) => { level.Checkpoints[index].EY = Utils.String2Float(value); }); }
    public static UnityAction<string> CheckpointInterval(int index)
    { return new UnityAction<string>((string value) => { level.Checkpoints[index].Interval = Utils.String2Float(value); }); }
    // Prefabs
    public static UnityAction<string> PrefabName(int index)
    { return new UnityAction<string>((string value) => { level.Prefabs[index].Name = value; }); }
    public static UnityAction<bool> PrefabActive(int index)
    { return new UnityAction<bool>((bool value) => { level.Prefabs[index].Active = value; }); }
    public static UnityAction<bool> PrefabCollider(int index)
    { return new UnityAction<bool>((bool value) => { level.Prefabs[index].Collider = value; }); }
    public static UnityAction<int> PrefabSpriteType(int index)
    { return new UnityAction<int>((int value) => { level.Prefabs[index].SpriteType = (SpriteType)value; }); }
    public static UnityAction PrefabSpriteTypeButton(int index, int idButton)
    { return () => { level.Prefabs[index].SpriteType = (SpriteType)idButton; }; }
    public static UnityAction<string> PrefabStartFrame(int index)
    { return new UnityAction<string>((string value) => { level.Prefabs[index].StartFrame = Utils.String2Int(value); }); }
    public static UnityAction<string> PrefabEndFrame(int index)
    { return new UnityAction<string>((string value) => { level.Prefabs[index].EndFrame = Utils.String2Int(value); }); }
    public static UnityAction<int> PrefabAnchorPresets(int index)
    { return new UnityAction<int>((int value) => { level.Prefabs[index].Anchor = (AnchorPresets)value; }); }
    public static UnityAction<string> PrefabID(int index)
    { return new UnityAction<string>((string value) => { level.Prefabs[index].ID = Utils.String2Int(value); }); }
    public static UnityAction<string> PrefabParentID(int index)
    { return new UnityAction<string>((string value) => { level.Prefabs[index].ParentID = Utils.String2Int(value); }); }
    public static UnityAction<string> PrefabLayer(int index)
    { return new UnityAction<string>((string value) => { level.Prefabs[index].Layer = Utils.String2Int(value); }); }
    public static UnityAction<string> PrefabHeight(int index)
    { return new UnityAction<string>((string value) => { level.Prefabs[index].Height = Utils.String2Int(value); }); }
    #endregion

    #region GetList
    public static GetList EditorPrefabStandardList()
    { return new GetList(() => { return PrefabStandard.GetPrefabStandard().ConvertAll(LevelConverter.EditorPrefabsConverter); }); }
    public static GetList EditorPrefabLevelList()
    { return new GetList(() => { return level.EditorPrefabs.ConvertAll(LevelConverter.EditorPrefabsConverter); }); }
    public static GetList EditorPrefabMemoryList()
    { return new GetList(() => { return PrefabMemory.GetPrefabMemory().ConvertAll(LevelConverter.EditorPrefabsConverter); }); }
    public static GetList MarkerList()
    { return new GetList(() => { return level.Markers.ConvertAll(LevelConverter.MarkersConverter); }); }
    public static GetList CheckpointList()
    { return new GetList(() => { return level.Checkpoints.ConvertAll(LevelConverter.CheckpointsConverter); }); }
    public static GetList PrefabList()
    { return new GetList(() => { return level.Prefabs.ConvertAll(LevelConverter.PrefabsConverter); }); }
    #endregion

    #region Save/Load
    public static void CreateTestLevel()
    {
        Prefab prefab1 = new Prefab("cube1", true, true,
            new List<Pos>()
            {
                new Pos(0, VectorRandomType.N, EasingType.Linear, -5, -5),
                new Pos(30, VectorRandomType.N, EasingType.Linear, 5, -5),
                new Pos(50, VectorRandomType.N, EasingType.Linear, 5, 5),
                new Pos(70, VectorRandomType.N, EasingType.Linear, 5, -5)
            },
            new List<Sca>()
            {
                new Sca(0, VectorRandomType.N, EasingType.Linear, 1, 1)
            },
            new List<Rot>()
            {
                new Rot(0, FloatRandomType.N, EasingType.Linear, 0)
            },
            new List<Clr>()
            {
                new Clr(0, ColorRandomType.N, EasingType.Linear, 0f, 0f, 0f, 1f),
                new Clr(10, ColorRandomType.N, EasingType.Linear, 1f, 0.2f, 0.3f, 1f)
            },
            SpriteType.Square, 0, 900, AnchorPresets.Left_Top, 1, 0, 1, 1);
        Prefab prefab2 = new Prefab("cube2", true, true,
            new List<Pos>()
            {
                new Pos(0, VectorRandomType.N, EasingType.Linear, -5, -5),
                new Pos(80, VectorRandomType.N, EasingType.Linear, 5, -5),
                new Pos(100, VectorRandomType.N, EasingType.Linear, 5, 5),
                new Pos(120, VectorRandomType.N, EasingType.Linear, -5, 5)
            },
            new List<Sca>()
            {
                new Sca(0, VectorRandomType.N, EasingType.Linear, 1, 1)
            },
            new List<Rot>()
            {
                new Rot(0, FloatRandomType.N, EasingType.Linear, 0)
            },
            new List<Clr>()
            {
                new Clr(0, ColorRandomType.N, EasingType.Linear, 0f, 0f, 0f, 1f),
                new Clr(70, ColorRandomType.N, EasingType.Linear, 1f, 0.2f, 0.3f, 1f)
            },
            SpriteType.Square, 0, 1200, AnchorPresets.Right_Middle, 2, 0, 1, 2);
        Prefab prefab3 = new Prefab("circle1", true, true,
            new List<Pos>()
            {
                new Pos(0, VectorRandomType.N, EasingType.Linear, -5, -5),
                new Pos(130, VectorRandomType.N, EasingType.Linear, 5, -5),
                new Pos(150, VectorRandomType.N, EasingType.Linear, 5, 5),
                new Pos(170, VectorRandomType.N, EasingType.Linear, -5, 5)
            },
            new List<Sca>()
            {
                new Sca(0, VectorRandomType.N, EasingType.Linear, 1, 1)
            },
            new List<Rot>()
            {
                new Rot(0, FloatRandomType.N, EasingType.Linear, 0)
            },
            new List<Clr>()
            {
                new Clr(0, ColorRandomType.N, EasingType.Linear, 0f, 0f, 0f, 1f),
                new Clr(110, ColorRandomType.N, EasingType.Linear, 1f, 0.2f, 0.3f, 1f)
            },
            SpriteType.Circle, 0, 1800, AnchorPresets.Left_Bottom, 3, 0, 2, 3);
        Prefab prefab4 = new Prefab("empty", true, false,
            new List<Pos>()
            {
                new Pos(0, VectorRandomType.N, EasingType.Linear, 5, 5),
                new Pos(100, VectorRandomType.N, EasingType.Linear, -5, 5),
                new Pos(150, VectorRandomType.N, EasingType.Linear, 5, 5),
                new Pos(200, VectorRandomType.N, EasingType.Linear, -5, 5),
                new Pos(350, VectorRandomType.N, EasingType.Linear, 5, 5),
                new Pos(400, VectorRandomType.N, EasingType.Linear, -5, 5)
            },
            new List<Sca>()
            {
                new Sca(0, VectorRandomType.N, EasingType.Linear, 1, 1)
            },
            new List<Rot>()
            {
                new Rot(0, FloatRandomType.N, EasingType.Linear, 0)
            },
            new List<Clr>()
            {
                new Clr(0, ColorRandomType.N, EasingType.Linear, 0f, 0f, 0f, 1f),
                new Clr(100, ColorRandomType.N, EasingType.Linear, 1f, 0.2f, 0.3f, 1f)
            },
            SpriteType.None, 0, 2400, AnchorPresets.Center_Middle, 4, 1, 3, 4);
        Prefab prefab5 = new Prefab("empty", true, false,
            new List<Pos>()
            {
                new Pos(0, VectorRandomType.N, EasingType.Linear, 5, 5),
                new Pos(100, VectorRandomType.N, EasingType.Linear, -5, 5),
                new Pos(150, VectorRandomType.N, EasingType.Linear, 5, 5),
                new Pos(200, VectorRandomType.N, EasingType.Linear, -5, 5),
                new Pos(350, VectorRandomType.N, EasingType.Linear, 5, 5),
                new Pos(400, VectorRandomType.N, EasingType.Linear, -5, 5)
            },
            new List<Sca>() { new Sca(0, VectorRandomType.N, EasingType.Linear, 1, 1) },
            new List<Rot>() { new Rot(0, FloatRandomType.N, EasingType.Linear, 0) },
            new List<Clr>() { new Clr(0, ColorRandomType.N, EasingType.Linear, 1f, 0.2f, 0.3f, 1f) },
            SpriteType.None, 0, 2400, AnchorPresets.Center_Middle, 5, 1, 3, 5);
        Prefab prefab6 = new Prefab("empty", true, false,
            new List<Pos>()
            {
                new Pos(0, VectorRandomType.N, EasingType.Linear, 5, 5),
                new Pos(100, VectorRandomType.N, EasingType.Linear, -5, 5),
                new Pos(150, VectorRandomType.N, EasingType.Linear, 5, 5),
                new Pos(200, VectorRandomType.N, EasingType.Linear, -5, 5),
                new Pos(350, VectorRandomType.N, EasingType.Linear, 5, 5),
                new Pos(400, VectorRandomType.N, EasingType.Linear, -5, 5)
            },
            new List<Sca>() { new Sca(0, VectorRandomType.N, EasingType.Linear, 1, 1) },
            new List<Rot>() { new Rot(0, FloatRandomType.N, EasingType.Linear, 0) },
            new List<Clr>() { new Clr(0, ColorRandomType.N, EasingType.Linear, 1f, 0.2f, 0.3f, 1f) },
            SpriteType.None, 0, 2400, AnchorPresets.Center_Middle, 6, 1, 3, 6);

        Level level = new Level(
            new LevelData("0 demo level", 1, "1up muncher", "DUNDERPATRULLEN", "vertog", 184.5f, 188f),
            new List<Marker>()
            {
                new Marker("here is block", "", 30, 1, 1, 1),
                new Marker("here is another block", "", 50, 1, 0, 0),
                new Marker("here is circle", "", 150, 0, 1, 0)
            },
            new List<Checkpoint>()
            {
                new Checkpoint(true, "checkPoint 1", 30, VectorRandomType.N, 0, 0),
                new Checkpoint(true, "checkPoint 2", 600, VectorRandomType.N, 0, 0),
                new Checkpoint(true, "cHEckPoint 3", 1200, VectorRandomType.N, 0, 0)
            },
            new List<Prefab>() { prefab1, prefab2, prefab3, prefab4, prefab5, prefab6 },
            new List<EditorPrefab>());

        Save(level);
    }

    public static void Save()
    {
        string json = JsonUtility.ToJson(level, true);
        string path = GetPathLevel(level.LevelData.LevelName);
        File.WriteAllText(path, json);
        Debug.Log(json.Length);
    }
    public static void Save(Level level)
    {
        string json = JsonUtility.ToJson(level, true);
        string path = GetPathLevel(level.LevelData.LevelName);
        File.WriteAllText(path, json);
        Debug.Log(json.Length);
    }
    public static bool Load()
    {
        if (PlayerPrefs.HasKey("level_editor"))
        {
            Load(PlayerPrefs.GetString("level_editor"));
            return true;
        }
        return false;
    }
    public static void Load(string name)
    {
        string path = GetPathLevel(name);
        level = JsonUtility.FromJson<Level>(File.ReadAllText(path));
    }
    public static string GetPathLevel(string name)
    {
        return Path.Combine(Application.dataPath, "levels", name, "level.json");
    }
    #endregion
}

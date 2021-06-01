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
    public static UnityAction<string> EditorVersion = new UnityAction<string>((string value) => { level.LevelData.EditorVersion = String2Int(value); });
    public static UnityAction<string> MusicTitle = new UnityAction<string>((string value) => { level.LevelData.MusicTitle = value; });
    public static UnityAction<string> MusicAuthor = new UnityAction<string>((string value) => { level.LevelData.MusicAuthor = value; });
    public static UnityAction<string> LevelAuthor = new UnityAction<string>((string value) => { level.LevelData.LevelAuthor = value; });
    public static UnityAction<string> StartFadeOut = new UnityAction<string>((string value) => { level.LevelData.StartFadeOut = String2Float(value); });
    public static UnityAction<string> EndFadeOut = new UnityAction<string>((string value) => { level.LevelData.EndFadeOut = String2Float(value); });

    // Markers
    public static UnityAction<string> MarkerName(int index)
    { return new UnityAction<string>((string value) => { level.Markers[index].Name = value; }); }
    public static UnityAction<string> MarkerDescription(int index)
    { return new UnityAction<string>((string value) => { level.Markers[index].Description = value; }); }
    public static UnityAction<string> MarkerTime(int index)
    { return new UnityAction<string>((string value) => { level.Markers[index].Time = String2Int(value); }); }
    public static UnityAction<string> MarkerColorR(int index)
    { return new UnityAction<string>((string value) => { level.Markers[index].Red = String2Float(value); }); }
    public static UnityAction<string> MarkerColorG(int index)
    { return new UnityAction<string>((string value) => { level.Markers[index].Green = String2Float(value); }); }
    public static UnityAction<string> MarkerColorB(int index)
    { return new UnityAction<string>((string value) => { level.Markers[index].Blue = String2Float(value); }); }

    // Checkpoint
    public static UnityAction<bool> CheckpointActive(int index)
    { return new UnityAction<bool>((bool value) => { level.Checkpoints[index].Active = value; }); }
    public static UnityAction<string> CheckpointName(int index)
    { return new UnityAction<string>((string value) => { level.Checkpoints[index].Name = value; }); }
    public static UnityAction<string> CheckpointTime(int index)
    { return new UnityAction<string>((string value) => { level.Checkpoints[index].Time = String2Float(value); }); }
    public static UnityAction<int> CheckpointRandom(int index)
    { return new UnityAction<int>((int value) => { level.Checkpoints[index].RandomType = (VectorRandomType)value; }); }
    public static UnityAction<string> CheckpointSX(int index)
    { return new UnityAction<string>((string value) => { level.Checkpoints[index].SX = String2Float(value); }); }
    public static UnityAction<string> CheckpointSY(int index)
    { return new UnityAction<string>((string value) => { level.Checkpoints[index].SY = String2Float(value); }); }
    public static UnityAction<string> CheckpointEX(int index)
    { return new UnityAction<string>((string value) => { level.Checkpoints[index].EX = String2Float(value); }); }
    public static UnityAction<string> CheckpointEY(int index)
    { return new UnityAction<string>((string value) => { level.Checkpoints[index].EY = String2Float(value); }); }
    public static UnityAction<string> CheckpointInterval(int index)
    { return new UnityAction<string>((string value) => { level.Checkpoints[index].Interval = String2Float(value); }); }
    // Prefabs
    public static UnityAction<string> PrefabName(int index)
    { return new UnityAction<string>((string value) => { level.Prefabs[index].Name = value; }); }
    public static UnityAction<bool> PrefabActive(int index)
    { return new UnityAction<bool>((bool value) => { level.Prefabs[index].Active = value; }); }
    public static UnityAction<bool> PrefabCollider(int index)
    { return new UnityAction<bool>((bool value) => { level.Prefabs[index].Collider = value; }); }

    #region Pos
    public static Pos PrefabPos(int index, int posIndex)
    { return level.Prefabs[index].Pos[posIndex]; }
    public static UnityAction<string> PosTime(Pos pos)
    { return new UnityAction<string>((string value) => { pos.Time = String2Float(value); }); }
    public static UnityAction<int> PosRandom(Pos pos)
    { return new UnityAction<int>((int value) => { pos.RandomType = (VectorRandomType)value; }); }
    public static UnityAction<int> PosEasing(Pos pos)
    { return new UnityAction<int>((int value) => { pos.Easing = (EasingType)value; }); }
    public static UnityAction<string> PosSX(Pos pos)
    { return new UnityAction<string>((string value) => { pos.SX = String2Float(value); }); }
    public static UnityAction<string> PosSY(Pos pos)
    { return new UnityAction<string>((string value) => { pos.SY = String2Float(value); }); }
    public static UnityAction<string> PosEX(Pos pos)
    { return new UnityAction<string>((string value) => { pos.EX = String2Float(value); }); }
    public static UnityAction<string> PosEY(Pos pos)
    { return new UnityAction<string>((string value) => { pos.EY = String2Float(value); }); }
    public static UnityAction<string> PosInterval(Pos pos)
    { return new UnityAction<string>((string value) => { pos.Interval = String2Float(value); }); }
    #endregion

    #region Sca
    public static Sca PrefabSca(int index, int scaIndex)
    { return level.Prefabs[index].Sca[scaIndex]; }
    public static UnityAction<string> ScaTime(Sca sca)
    { return new UnityAction<string>((string value) => { sca.Time = String2Float(value); }); }
    public static UnityAction<int> ScaRandom(Sca sca)
    { return new UnityAction<int>((int value) => { sca.RandomType = (VectorRandomType)value; }); }
    public static UnityAction<int> ScaEasing(Sca sca)
    { return new UnityAction<int>((int value) => { sca.Easing = (EasingType)value; }); }
    public static UnityAction<string> ScaSX(Sca sca)
    { return new UnityAction<string>((string value) => { sca.SX = String2Float(value); }); }
    public static UnityAction<string> ScaSY(Sca sca)
    { return new UnityAction<string>((string value) => { sca.SY = String2Float(value); }); }
    public static UnityAction<string> ScaEX(Sca sca)
    { return new UnityAction<string>((string value) => { sca.EX = String2Float(value); }); }
    public static UnityAction<string> ScaEY(Sca sca)
    { return new UnityAction<string>((string value) => { sca.EY = String2Float(value); }); }
    public static UnityAction<string> ScaInterval(Sca sca)
    { return new UnityAction<string>((string value) => { sca.Interval = String2Float(value); }); }
    #endregion

    #region Rot
    public static Rot PrefabRot(int index, int rotIndex)
    { return level.Prefabs[index].Rot[rotIndex]; }
    public static UnityAction<string> RotTime(Rot rot)
    { return new UnityAction<string>((string value) => { rot.Time = String2Float(value); }); }
    public static UnityAction<int> RotRandom(Rot rot)
    { return new UnityAction<int>((int value) => { rot.RandomType = (FloatRandomType)value; }); }
    public static UnityAction<int> RotEasing(Rot rot)
    { return new UnityAction<int>((int value) => { rot.Easing = (EasingType)value; }); }
    public static UnityAction<string> RotSA(Rot rot)
    { return new UnityAction<string>((string value) => { rot.SA = String2Float(value); }); }
    public static UnityAction<string> RotEA(Rot rot)
    { return new UnityAction<string>((string value) => { rot.EA = String2Float(value); }); }
    public static UnityAction<string> RotInterval(Rot rot)
    { return new UnityAction<string>((string value) => { rot.Interval = String2Float(value); }); }
    #endregion

    #region Clr
    public static Clr PrefabClr(int index, int clrIndex)
    { return level.Prefabs[index].Clr[clrIndex]; }
    public static UnityAction<string> ClrTime(Clr clr)
    { return new UnityAction<string>((string value) => { clr.Time = String2Float(value); }); }
    public static UnityAction<int> ClrRandom(Clr clr)
    { return new UnityAction<int>((int value) => { clr.RandomType = (ColorRandomType)value; }); }
    public static UnityAction<int> ClrEasing(Clr clr)
    { return new UnityAction<int>((int value) => { clr.Easing = (EasingType)value; }); }
    public static UnityAction<float> ClrSR(Clr clr)
    { return new UnityAction<float>((float value) => { clr.SR = value; }); }
    public static UnityAction<float> ClrSG(Clr clr)
    { return new UnityAction<float>((float value) => { clr.SG = value; }); }
    public static UnityAction<float> ClrSB(Clr clr)
    { return new UnityAction<float>((float value) => { clr.SB = value; }); }
    public static UnityAction<float> ClrSA(Clr clr)
    { return new UnityAction<float>((float value) => { clr.SA = value; }); }
    public static UnityAction<float> ClrER(Clr clr)
    { return new UnityAction<float>((float value) => { clr.ER = value; }); }
    public static UnityAction<float> ClrEG(Clr clr)
    { return new UnityAction<float>((float value) => { clr.EG = value; }); }
    public static UnityAction<float> ClrEB(Clr clr)
    { return new UnityAction<float>((float value) => { clr.EB = value; }); }
    public static UnityAction<float> ClrEA(Clr clr)
    { return new UnityAction<float>((float value) => { clr.EA = value; }); }
    public static UnityAction<string> ClrInterval(Clr clr)
    { return new UnityAction<string>((string value) => { clr.Interval = String2Float(value); }); }
    #endregion

    public static UnityAction<int> PrefabSpriteType(int index)
    { return new UnityAction<int>((int value) => { level.Prefabs[index].SpriteType = (SpriteType)value; }); }
    public static UnityAction<string> PrefabStartFrame(int index)
    { return new UnityAction<string>((string value) => { level.Prefabs[index].StartFrame = String2Int(value); }); }
    public static UnityAction<string> PrefabEndFrame(int index)
    { return new UnityAction<string>((string value) => { level.Prefabs[index].EndFrame = String2Int(value); }); }
    public static UnityAction<int> PrefabAnchorPresets(int index)
    { return new UnityAction<int>((int value) => { level.Prefabs[index].Anchor = (AnchorPresets)value; }); }
    public static UnityAction<string> PrefabID(int index)
    { return new UnityAction<string>((string value) => { level.Prefabs[index].ID = String2Int(value); }); }
    public static UnityAction<string> PrefabParentID(int index)
    { return new UnityAction<string>((string value) => { level.Prefabs[index].ParentID = String2Int(value); }); }
    public static UnityAction<string> PrefabLayer(int index)
    { return new UnityAction<string>((string value) => { level.Prefabs[index].Layer = String2Int(value); }); }
    public static UnityAction<string> PrefabHeight(int index)
    { return new UnityAction<string>((string value) => { level.Prefabs[index].Height = String2Int(value); }); }
    #endregion

    #region Static
    private static int String2Int(string value)
    {
        if (int.TryParse(value, out int result))
            return result;
        return 0;
    }
    private static float String2Float(string value)
    {
        if (float.TryParse(value, out float result))
            return result;
        return 0;
    }
    #endregion

    #region Save/Load
    public static void CreateTestLevel()
    {
        Prefab prefab1 = new Prefab("cube1", true, true,
            new List<Pos>()
            {
                new Pos(0, VectorRandomType.N, EasingType.Linear, -5, -5),
                new Pos(3, VectorRandomType.N, EasingType.Linear, 5, -5),
                new Pos(5, VectorRandomType.N, EasingType.Linear, 5, 5),
                new Pos(7, VectorRandomType.N, EasingType.Linear, 5, -5)
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
                new Clr(1, ColorRandomType.N, EasingType.Linear, 1f, 0.2f, 0.3f, 1f)
            },
            SpriteType.Square, 0, 900, AnchorPresets.Left_Top, 1, 0, 1, 1);
        Prefab prefab2 = new Prefab("cube2", true, true,
            new List<Pos>()
            {
                new Pos(0, VectorRandomType.N, EasingType.Linear, -5, -5),
                new Pos(8, VectorRandomType.N, EasingType.Linear, 5, -5),
                new Pos(10, VectorRandomType.N, EasingType.Linear, 5, 5),
                new Pos(12, VectorRandomType.N, EasingType.Linear, -5, 5)
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
                new Clr(7, ColorRandomType.N, EasingType.Linear, 1f, 0.2f, 0.3f, 1f)
            },
            SpriteType.Square, 0, 1200, AnchorPresets.Right_Middle, 2, 0, 1, 2);
        Prefab prefab3 = new Prefab("circle1", true, true,
            new List<Pos>()
            {
                new Pos(0, VectorRandomType.N, EasingType.Linear, -5, -5),
                new Pos(13, VectorRandomType.N, EasingType.Linear, 5, -5),
                new Pos(15, VectorRandomType.N, EasingType.Linear, 5, 5),
                new Pos(17, VectorRandomType.N, EasingType.Linear, -5, 5)
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
                new Clr(11, ColorRandomType.N, EasingType.Linear, 1f, 0.2f, 0.3f, 1f)
            },
            SpriteType.Circle, 0, 1800, AnchorPresets.Left_Bottom, 3, 0, 2, 3);
        Prefab prefab4 = new Prefab("empty", true, false,
            new List<Pos>()
            {
                new Pos(0, VectorRandomType.N, EasingType.Linear, 5, 5),
                new Pos(10, VectorRandomType.N, EasingType.Linear, -5, 5),
                new Pos(15, VectorRandomType.N, EasingType.Linear, 5, 5),
                new Pos(20, VectorRandomType.N, EasingType.Linear, -5, 5),
                new Pos(35, VectorRandomType.N, EasingType.Linear, 5, 5),
                new Pos(40, VectorRandomType.N, EasingType.Linear, -5, 5)
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
                new Clr(0, ColorRandomType.N, EasingType.Linear, 1f, 0.2f, 0.3f, 1f)
            },
            SpriteType.None, 0, 2400, AnchorPresets.Center_Middle, 4, 1, 3, 4);
        Prefab prefab5 = new Prefab("empty", true, false,
            new List<Pos>()
            {
                new Pos(0, VectorRandomType.N, EasingType.Linear, 5, 5),
                new Pos(10, VectorRandomType.N, EasingType.Linear, -5, 5),
                new Pos(15, VectorRandomType.N, EasingType.Linear, 5, 5),
                new Pos(20, VectorRandomType.N, EasingType.Linear, -5, 5),
                new Pos(35, VectorRandomType.N, EasingType.Linear, 5, 5),
                new Pos(40, VectorRandomType.N, EasingType.Linear, -5, 5)
            },
            new List<Sca>() { new Sca(0, VectorRandomType.N, EasingType.Linear, 1, 1) },
            new List<Rot>() { new Rot(0, FloatRandomType.N, EasingType.Linear, 0) },
            new List<Clr>() { new Clr(0, ColorRandomType.N, EasingType.Linear, 1f, 0.2f, 0.3f, 1f) },
            SpriteType.None, 0, 2400, AnchorPresets.Center_Middle, 5, 1, 3, 5);
        Prefab prefab6 = new Prefab("empty", true, false,
            new List<Pos>()
            {
                new Pos(0, VectorRandomType.N, EasingType.Linear, 5, 5),
                new Pos(10, VectorRandomType.N, EasingType.Linear, -5, 5),
                new Pos(15, VectorRandomType.N, EasingType.Linear, 5, 5),
                new Pos(20, VectorRandomType.N, EasingType.Linear, -5, 5),
                new Pos(35, VectorRandomType.N, EasingType.Linear, 5, 5),
                new Pos(40, VectorRandomType.N, EasingType.Linear, -5, 5)
            },
            new List<Sca>() { new Sca(0, VectorRandomType.N, EasingType.Linear, 1, 1) },
            new List<Rot>() { new Rot(0, FloatRandomType.N, EasingType.Linear, 0) },
            new List<Clr>() { new Clr(0, ColorRandomType.N, EasingType.Linear, 1f, 0.2f, 0.3f, 1f) },
            SpriteType.None, 0, 2400, AnchorPresets.Center_Middle, 6, 1, 3, 6);

        Level level = new Level(
            new LevelData("0 demo level", 1, "1up muncher", "DUNDERPATRULLEN", "vertog", 184.5f, 188f),
            new List<Marker>()
            {
                new Marker("here is block", "", 3, 1, 1, 1),
                new Marker("here is another block", "", 5, 1, 0, 0),
                new Marker("here is circle", "", 15, 0, 1, 0)
            },
            new List<Checkpoint>()
            {
                new Checkpoint(true, "checkPoint 1", 3, VectorRandomType.N, 0, 0),
                new Checkpoint(true, "checkPoint 2", 60, VectorRandomType.N, 0, 0),
                new Checkpoint(true, "cHEckPoint 3", 120, VectorRandomType.N, 0, 0)
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
    public static void Load(string name)
    {
        string path = GetPathLevel(name);
        level = JsonUtility.FromJson<Level>(File.ReadAllText(path));
    }
    public static string GetPathLevel(string name)
    {
        return Application.dataPath + "/levels/" + name + "/level.json";
    }
    #endregion
}

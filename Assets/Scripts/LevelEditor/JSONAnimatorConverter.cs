using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JSONAnimatorConverter : MonoBehaviour// Convert JSON to Track Animation and reverse
{
    public void Start()
    {
        CreateTestLevel();
    }

    public static void CreateTestLevel()
    {
        Prefab prefab1 = new Prefab("cube1", true, true, 
            new List<Pos>() { new Pos(1, VectorRandomType.N, -5, -5), new Pos(3, VectorRandomType.N, 5, -5), new Pos(5, VectorRandomType.N, 5, 5), new Pos(7, VectorRandomType.N, 5, -5)}, 
            new List<Sca>() { new Sca(3, VectorRandomType.N, 1, 1), new Sca(5, VectorRandomType.N, 1, 2), new Sca(7, VectorRandomType.N, 2, 2), new Sca(9, VectorRandomType.N, 2, 1) }, 
            new List<Rot>() { new Rot(5, FloatRandomType.N, 0), new Rot(7, FloatRandomType.N, 180), new Rot(9, FloatRandomType.N, 90), new Rot(11, FloatRandomType.N, -90) },
            new List<Clr>() { new Clr(11, ColorRandomType.N, 1f, 0.2f, 0.3f, 1f) },
            SpriteType.Square, 1, 15, 0, 1, AnchorPresets.Center_Middle, AnchorPresets.Center_Middle, 3, 1, 0);
        Prefab prefab2 = new Prefab("cube2", true, true,
            new List<Pos>() { new Pos(11, VectorRandomType.N, -5, -5), new Pos(13, VectorRandomType.N, 5, -5), new Pos(15, VectorRandomType.N, 5, 5), new Pos(17, VectorRandomType.N, 5, -5) },
            new List<Sca>() { new Sca(13, VectorRandomType.N, 1, 1), new Sca(15, VectorRandomType.N, 1, 2), new Sca(17, VectorRandomType.N, 2, 2), new Sca(19, VectorRandomType.N, 2, 1) },
            new List<Rot>() { new Rot(15, FloatRandomType.N, 0), new Rot(17, FloatRandomType.N, 180), new Rot(19, FloatRandomType.N, 90), new Rot(21, FloatRandomType.N, -90), }, 
            new List<Clr>() { new Clr(11, ColorRandomType.N, 1f, 0.2f, 0.3f, 1f) },
            SpriteType.Square, 7, 20, 0, 1, AnchorPresets.Center_Middle, AnchorPresets.Center_Middle, 2, 2, 0);
        Prefab prefab3 = new Prefab("circle1", true, true,
            new List<Pos>() { new Pos(6, VectorRandomType.N, -5, -5), new Pos(8, VectorRandomType.N, 5, -5), new Pos(10, VectorRandomType.N, 5, 5), new Pos(12, VectorRandomType.N, 5, -5) },
            new List<Sca>() { new Sca(8, VectorRandomType.N, 1, 1), new Sca(10, VectorRandomType.N, 1, 2), new Sca(12, VectorRandomType.N, 2, 2), new Sca(13, VectorRandomType.N, 2, 1) },
            new List<Rot>() { new Rot(10, FloatRandomType.N, 0), new Rot(13, FloatRandomType.N, 180), new Rot(14, FloatRandomType.N, 90), new Rot(16, FloatRandomType.N, -90) },
            new List<Clr>() { new Clr(11, ColorRandomType.N, 1f, 0.2f, 0.3f, 1f) },
            SpriteType.Square, 13, 20, 0, 1, AnchorPresets.Center_Middle, AnchorPresets.Center_Middle, 1, 3, 0);

        Level level = new Level(
            new LevelData("0 demo level", 1, "1up muncher", "DUNDERPATRULLEN", "vertog", 184.5f, 188f),
            new List<Marker>() { new Marker(true, 0, "here is block", 3), new Marker(false, 0, "here is another block", 5), new Marker(true, 1, "here is circle", 15) },
            new List<Checkpoint>() { new Checkpoint(true, "checkPoint 1", 3), new Checkpoint(true, "checkPoint 2", 60), new Checkpoint(true, "cHEckPoint 3", 120) },
            new List<NestedPrefab>(), new List<Prefab>() { prefab1, prefab2, prefab3 },
            new Effects(new List<Zoom>() { new Zoom(5, 10), new Zoom(6, 9), new Zoom(7, 10), new Zoom(8, 9), new Zoom(9, 10)}, new List<Pos>() { }, 
                        new List<Rot>() { new Rot(1, FloatRandomType.N, 0), new Rot(2, FloatRandomType.N, 10), new Rot(5, FloatRandomType.N, -50), new Rot(6, FloatRandomType.N, 0)})
            );

        Save(level);
    }

    public static void Save(Level level)
    {
        File.WriteAllText(GetPath(level.data.levelName), JsonUtility.ToJson(level, true));
        print(JsonUtility.ToJson(level).Length);
    }

    public static Level Load(string name)
    {
        return JsonUtility.FromJson<Level>(File.ReadAllText(GetPath(name)));
    }

    public static string GetPath(string name)
    {
        return Application.dataPath + "/" + name + "/level.json";
    }
}

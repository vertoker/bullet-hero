using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileManager : MonoBehaviour// Convert JSON to Track Animation and reverse
{
    public void Start()
    {
        CreateTestLevel();
    }

    public static void CreateTestLevel()
    {
        Prefab prefab1 = new Prefab("cube1", true, true, 
            new List<Pos>() 
            { 
                new Pos(0, VectorRandomType.N, AnchorPresets.Center_Middle, EasingType.Linear, -5, -5), 
                new Pos(3, VectorRandomType.N, AnchorPresets.Left_Middle, EasingType.Linear, 5, -5), 
                new Pos(5, VectorRandomType.N, AnchorPresets.Left_Bottom, EasingType.Linear, 5, 5), 
                new Pos(7, VectorRandomType.N, AnchorPresets.Left_Top, EasingType.Linear, 5, -5)
            }, 
            new List<Sca>() 
            {
                new Sca(0, VectorRandomType.N, EasingType.Linear, 1, 1)
            }, 
            new List<Rot>() 
            {
                new Rot(0, FloatRandomType.N, EasingType.Linear, 0)
            },
            new List<Clr>() { new Clr(1, ColorRandomType.N, EasingType.Linear, 1f, 0.2f, 0.3f, 1f) },
            SpriteType.Square, 0, 900, 900, 2, 1, AnchorPresets.Left_Top, 3, 1, 0);
        Prefab prefab2 = new Prefab("cube2", true, true,
            new List<Pos>() 
            {
                new Pos(0, VectorRandomType.N, AnchorPresets.Center_Middle, EasingType.Linear, -5, -5), 
                new Pos(8, VectorRandomType.N, AnchorPresets.Center_Middle, EasingType.Linear, 5, -5), 
                new Pos(10, VectorRandomType.N, AnchorPresets.Center_Middle, EasingType.Linear, 5, 5), 
                new Pos(12, VectorRandomType.N, AnchorPresets.Center_Middle, EasingType.Linear, -5, 5)
            },
            new List<Sca>() 
            {
                new Sca(0, VectorRandomType.N, EasingType.Linear, 1, 1)
            },
            new List<Rot>() 
            {
                new Rot(0, FloatRandomType.N, EasingType.Linear, 0)
            },
            new List<Clr>() { new Clr(7, ColorRandomType.N, EasingType.Linear, 1f, 0.2f, 0.3f, 1f) },
            SpriteType.Square, 0, 1200, 1200, 6, 2, AnchorPresets.Right_Middle, 2, 2, 1);
        Prefab prefab3 = new Prefab("circle1", true, true,
            new List<Pos>() 
            {
                new Pos(0, VectorRandomType.N, AnchorPresets.Right_Top, EasingType.Linear, -5, -5), 
                new Pos(13, VectorRandomType.N, AnchorPresets.Left_Top, EasingType.Linear, 5, -5), 
                new Pos(15, VectorRandomType.N, AnchorPresets.Left_Bottom, EasingType.Linear, 5, 5),
                new Pos(17, VectorRandomType.N, AnchorPresets.Right_Bottom, EasingType.Linear, -5, 5)
            },
            new List<Sca>() 
            {
                new Sca(0, VectorRandomType.N, EasingType.Linear, 1, 1)
            },
            new List<Rot>() 
            {
                new Rot(0, FloatRandomType.N, EasingType.Linear, 0)
            },
            new List<Clr>() { new Clr(11, ColorRandomType.N, EasingType.Linear, 1f, 0.2f, 0.3f, 1f) },
            SpriteType.Circle, 0, 1800, 1800, 5, 3, AnchorPresets.Left_Bottom, 1, 3, 2);
        Prefab prefab4 = new Prefab("empty", true, false,
            new List<Pos>()
            {
                new Pos(0, VectorRandomType.N, AnchorPresets.Center_Middle, EasingType.Linear, 5, 5),
                new Pos(10, VectorRandomType.N, AnchorPresets.Right_Bottom, EasingType.Linear, -5, 5),
                new Pos(15, VectorRandomType.N, AnchorPresets.Left_Top, EasingType.Linear, 5, 5),
                new Pos(20, VectorRandomType.N, AnchorPresets.Right_Top, EasingType.Linear, -5, 5),
                new Pos(35, VectorRandomType.N, AnchorPresets.Left_Bottom, EasingType.Linear, 5, 5),
                new Pos(40, VectorRandomType.N, AnchorPresets.Center_Middle, EasingType.Linear, -5, 5)
            },
            new List<Sca>() { new Sca(0, VectorRandomType.N, EasingType.Linear, 1, 1) },
            new List<Rot>() { new Rot(0, FloatRandomType.N, EasingType.Linear, 0) },
            new List<Clr>() { new Clr(0, ColorRandomType.N, EasingType.Linear, 1f, 0.2f, 0.3f, 1f) },
            SpriteType.None, 0, 2400, 2400, 0, 4, AnchorPresets.Center_Middle, 1, 3, 2);
        Prefab prefab5 = new Prefab("empty", true, false,
            new List<Pos>()
            {
                new Pos(0, VectorRandomType.N, AnchorPresets.Center_Middle, EasingType.Linear, 5, 5),
                new Pos(10, VectorRandomType.N, AnchorPresets.Right_Bottom, EasingType.Linear, -5, 5),
                new Pos(15, VectorRandomType.N, AnchorPresets.Left_Top, EasingType.Linear, 5, 5),
                new Pos(20, VectorRandomType.N, AnchorPresets.Right_Top, EasingType.Linear, -5, 5),
                new Pos(35, VectorRandomType.N, AnchorPresets.Left_Bottom, EasingType.Linear, 5, 5),
                new Pos(40, VectorRandomType.N, AnchorPresets.Center_Middle, EasingType.Linear, -5, 5)
            },
            new List<Sca>() { new Sca(0, VectorRandomType.N, EasingType.Linear, 1, 1) },
            new List<Rot>() { new Rot(0, FloatRandomType.N, EasingType.Linear, 0) },
            new List<Clr>() { new Clr(0, ColorRandomType.N, EasingType.Linear, 1f, 0.2f, 0.3f, 1f) },
            SpriteType.None, 0, 2400, 2400, 4, 5, AnchorPresets.Center_Middle, 1, 3, 2);
        Prefab prefab6 = new Prefab("empty", true, false,
            new List<Pos>()
            {
                new Pos(0, VectorRandomType.N, AnchorPresets.Center_Middle, EasingType.Linear, 5, 5),
                new Pos(10, VectorRandomType.N, AnchorPresets.Right_Bottom, EasingType.Linear, -5, 5),
                new Pos(15, VectorRandomType.N, AnchorPresets.Left_Top, EasingType.Linear, 5, 5),
                new Pos(20, VectorRandomType.N, AnchorPresets.Right_Top, EasingType.Linear, -5, 5),
                new Pos(35, VectorRandomType.N, AnchorPresets.Left_Bottom, EasingType.Linear, 5, 5),
                new Pos(40, VectorRandomType.N, AnchorPresets.Center_Middle, EasingType.Linear, -5, 5)
            },
            new List<Sca>() { new Sca(0, VectorRandomType.N, EasingType.Linear, 1, 1) },
            new List<Rot>() { new Rot(0, FloatRandomType.N, EasingType.Linear, 0) },
            new List<Clr>() { new Clr(0, ColorRandomType.N, EasingType.Linear, 1f, 0.2f, 0.3f, 1f) },
            SpriteType.None, 0, 2400, 2400, 5, 6, AnchorPresets.Center_Middle, 1, 3, 2);

        Level level = new Level(
            new LevelData("0 demo level", 1, "1up muncher", "DUNDERPATRULLEN", "vertog", 3, 184.5f, 188f),
            new List<Marker>() { new Marker(0, "here is block", "", 3), new Marker(0, "here is another block", "", 5), new Marker(1, "here is circle", "", 15) },
            new List<Checkpoint>() { new Checkpoint(true, "checkPoint 1", 3), new Checkpoint(true, "checkPoint 2", 60), new Checkpoint(true, "cHEckPoint 3", 120) },
            new List<Prefab>() { prefab1, prefab2, prefab3, prefab4, prefab5, prefab6 },
            new List<GameEvent>(), new List<EditorPrefab>(),
            new Effects(new CamData(new List<Zoom>() { new Zoom(5, 10), new Zoom(6, 9), new Zoom(7, 10), new Zoom(8, 9), new Zoom(9, 10) }, new List<Pos>() { },
                        new List<Rot>() 
                        {
                            new Rot(1, FloatRandomType.N, EasingType.Linear, 0), 
                            new Rot(2, FloatRandomType.N, EasingType.Linear, 10),
                            new Rot(5, FloatRandomType.N, EasingType.Linear, -50), 
                            new Rot(6, FloatRandomType.N, EasingType.Linear, 0) 
                        }))
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

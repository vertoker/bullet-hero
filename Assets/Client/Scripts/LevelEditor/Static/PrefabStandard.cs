using System.Collections.Generic;
using System;

public static class PrefabStandard
{
    private static float mainR = 1f, mainG = 0.3f, mainB = 0.3f, mainA = 1f;

    private static readonly Prefab circle = new Prefab("circle", true, true,
        new List<Pos>() { new Pos(0, VectorRandomType.N, EasingType.Linear, 0, 0) },
        new List<Sca>() { new Sca(0, VectorRandomType.N, EasingType.Linear, 1, 1) },
        new List<Rot>() { new Rot(0, FloatRandomType.N, EasingType.Linear, 0) },
        new List<Clr>() { new Clr(0, ColorRandomType.N, EasingType.Linear, mainR, mainG, mainB, mainA) },
        SpriteType.Circle, 0, 60, AnchorPresets.Center_Middle, 0, 0, 1, 0);
    private static readonly Prefab box = new Prefab("box", true, true,
        new List<Pos>() { new Pos(0, VectorRandomType.N, EasingType.Linear, 0, 0) },
        new List<Sca>() { new Sca(0, VectorRandomType.N, EasingType.Linear, 1, 1) },
        new List<Rot>() { new Rot(0, FloatRandomType.N, EasingType.Linear, 0) },
        new List<Clr>() { new Clr(0, ColorRandomType.N, EasingType.Linear, mainR, mainG, mainB, mainA) },
        SpriteType.Square, 0, 60, AnchorPresets.Center_Middle, 0, 0, 1, 0);

    private static List<EditorPrefab> prefab_standard = new List<EditorPrefab>()
    {
        new EditorPrefab(new List<Prefab>(){ circle }),
        new EditorPrefab(new List<Prefab>(){ box })//, 
        //new EditorPrefab(new List<Prefab>(){ box }),
    };
    public static EditorPrefab GetPrefabStandard(int index) { return prefab_standard[index]; }
    public static List<EditorPrefab> GetPrefabStandard() { return prefab_standard; }
}

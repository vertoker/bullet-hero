using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CreateObject
{
    public static void CreateEditorPrefab(EditorPrefab prefabs)
    {

    }
    public static void CreateEditorPrefab(List<Prefab> prefabs)
    {

    }
    public static void CreatePrefab(Prefab prefab)
    {
        LevelManager.level.Prefabs.Add(prefab);
    }
    public static void CreateMarker(Marker marker)
    {

    }
    public static void CreateCheckpoint(Checkpoint checkpoint)
    {

    }
}

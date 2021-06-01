using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameEditor : MonoBehaviour
{

}

#if UNITY_EDITOR
[CustomEditor(typeof(GameEditor))]
public class CustomButtons : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        GameEditor editor = (GameEditor)target;

        if (GUILayout.Button("Save Test Level"))
        {
            LevelManager.CreateTestLevel();
        }
    }
}
#endif
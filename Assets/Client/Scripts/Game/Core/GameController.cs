using System.Collections;
using System.Collections.Generic;
using Game.SerializationSaver;
using Game.Provider;
using UnityEngine;
using Data;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Game.Core
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private Player[] players;
        [SerializeField] private Level level;

        private void Start()
        {
            DataProvider.Load(level, players);
        }
        public void Load(string name)
        {
            level = LevelSaver.Load(name);
            //Debug.Log(level);
            DataProvider.Load(level, players);
        }
        public void Save()
        {
            LevelSaver.Save(level);
        }
    }
    
#if UNITY_EDITOR
    [CustomEditor(typeof(GameController))]
    public class ControllerGUI : Editor
    {
        private string _name;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            GameController controller = (GameController)target;

            EditorGUILayout.Space(10);
            if (GUILayout.Button("Save"))
            {
                controller.Save();
            }
            //_name = GUILayout.TextArea(_name);
            if (GUILayout.Button("Load"))
            {
                controller.Load("0 demo level");
            }
        }
    }
#endif
}
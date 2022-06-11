using System.Collections;
using System.Collections.Generic;
using Game.SerializationSaver;
using Game.Provider;
using UnityEngine;
using Game.UI;
using Data;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Game.Core
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private HealthBar healthBar;
        [SerializeField] private Player player;
        [SerializeField] private Level level;

        public Player Player => player;
        public static GameController Instance;

        private void Awake()
        {
            Instance = this;
        }
        private void Start()
        {
            healthBar.SetPlayer(player);
            player.SetPlayerPreset(1);
            DataProvider.Load(level, player);
        }
        public void Load(string name)
        {
            level = LevelSaver.Load(name);
            DataProvider.Load(level, player);
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
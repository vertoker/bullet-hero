using System.Collections;
using UnityEngine.SceneManagement;
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
        [SerializeField] private GameRules gameRules;

        [Space]
        [SerializeField] private Sprite[] playerSkins;
        private bool isLoaded = false;

        public Player Player => player;
        public static GameController Instance;

        private void Awake()
        {
            Instance = this;
            SceneManager.sceneLoaded += LoadedProvider;
        }
        private void Start()
        {
            LoadLevel(GameRules.standard, Level.DEFAULT_LEVEL);
        }
        public void LoadLevel(GameRules rules, Level level)
        {
            if (isLoaded)
                return;
            isLoaded = true;

            gameRules = rules;

            SceneManager.LoadScene(1, LoadSceneMode.Additive);

            healthBar.SetPlayerRules(rules.immortality ? 0 : rules.lifeCount);
            player.SetPlayerPreset(playerSkins[rules.skinID], rules.immortality, rules.lifeCount);
        }
        private void LoadedProvider(Scene scene, LoadSceneMode loadSceneMode)
        {
            if (loadSceneMode == LoadSceneMode.Additive)
            {
                DataProvider.Start(level, player);
                SceneManager.sceneLoaded -= LoadedProvider;
            }
        }

        public void Play()
        {
            DataProvider.Runtime.StartGame();
        }
        public void Pause()
        {
            DataProvider.Runtime.StopGame();
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

            //EditorGUILayout.Space(10);
            //_name = GUILayout.TextArea(_name);
            /*if (GUILayout.Button("Load"))
            {
                controller.Load("0 demo level");
            }*/
        }
    }
#endif
}
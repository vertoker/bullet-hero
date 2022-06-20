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
        [SerializeField] private Player player;
        [SerializeField] private HealthBar healthBar;
        [SerializeField] private InputController inputController;
        [SerializeField] private SkinsData skinsData;

        [SerializeField] private GameRules gameRules;
        [SerializeField] private Level level;
        private UIController controllerUI;
        private bool isLoaded = false;

        public Player Player => player;
        public static GameController Instance;

        private void Awake()
        {
            Instance = this;
            controllerUI = GetComponent<UIController>();
            SceneManager.sceneLoaded += LoadedProvider;
        }
        private void Start()
        {
            if (GameDataProvider.IsSet)
                LoadLevel(GameDataProvider.Rules, GameDataProvider.Level);
            else
                LoadLevel(GameRules.standard, Level.DEFAULT_LEVEL);
        }
        public void LoadLevel(GameRules rules, Level level)
        {
            if (isLoaded)
                return;
            isLoaded = true;

            gameRules = rules.Copy();

            SceneManager.LoadScene(1, LoadSceneMode.Additive);

            RestartController();
        }
        private void LoadedProvider(Scene scene, LoadSceneMode loadSceneMode)
        {
            if (loadSceneMode == LoadSceneMode.Additive)
            {
                DataProvider.Start(level, player, gameRules);
                SceneManager.sceneLoaded -= LoadedProvider;
                controllerUI.Enable();
            }
        }
        private void OnDisable()
        {
            controllerUI.Disable();
        }

        public void Play()
        {
            DataProvider.Runtime.Play();
        }
        public void Pause()
        {
            DataProvider.Runtime.Pause();
        }
        public void Restart()
        {
            RestartController();
            DataProvider.Runtime.Restart();
        }

        private void RestartController()
        {
            player.SetPlayerPreset(skinsData.GetSkin(gameRules.skinID), gameRules.immortality, gameRules.lifeCount);
            healthBar.SetPlayerRules(gameRules.immortality ? 0 : gameRules.lifeCount);
            inputController.Restart();
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
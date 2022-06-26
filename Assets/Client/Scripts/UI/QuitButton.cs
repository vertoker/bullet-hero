using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using Game.Core;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UI
{
    public class QuitButton : MonoBehaviour
    {
        [SerializeField] private QuitType quitType;

        public void SetQuitType(QuitType quit_type)
        {
            quitType = quit_type;
        }

        public void Tap()
        {
            if (quitType == QuitType.FromGame)
            {
                Application.Quit();
#if UNITY_EDITOR
                EditorApplication.isPlaying = false;
#endif
            }
            if (quitType == QuitType.ToMenu)
            {
                SceneManager.LoadScene(0);
            }
            if (quitType == QuitType.ToEditor)
            {
                GameDataProvider.Set(GameDataProvider.Rules, GameDataProvider.Level);
                SceneManager.LoadScene(3);
            }
        }
    }

    public enum QuitType : byte
    {
        FromGame = 0,
        ToMenu = 1,
        ToEditor = 2
    }
}
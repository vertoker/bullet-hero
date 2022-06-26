using System.Collections;
using UnityEngine.SceneManagement;
using Game.Provider;
using UnityEngine;
using Game.UI;
using Data;
using UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Game.Core
{
    public class QuitButtonSetter : MonoBehaviour
    {
        [SerializeField] private QuitButton quitButton;

        private void Start()
        {
            quitButton.SetQuitType(GameDataProvider.QuitType);
        }
    }
}
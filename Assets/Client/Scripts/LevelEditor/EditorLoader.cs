using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace LevelEditor
{
    public class EditorLoader : MonoBehaviour
    {
        private void OnEnable()
        {
            var operation = SceneManager.LoadSceneAsync(3);
        }
    }
}
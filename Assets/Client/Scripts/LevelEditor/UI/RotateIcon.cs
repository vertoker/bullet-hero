using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace LevelEditor.UI
{
    public class RotateIcon : MonoBehaviour
    {
        public float speed = 1f;
        private RectTransform tr;

        private void Awake()
        {
            tr = GetComponent<RectTransform>();
        }

        private void FixedUpdate()
        {
            tr.localEulerAngles = new Vector3(0f, 0f, tr.localEulerAngles.z + speed);
        }
    }
}
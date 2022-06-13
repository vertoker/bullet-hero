using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class SwitcherActivity : MonoBehaviour
    {
        [SerializeField] private bool startActivity = false;

        private void Awake()
        {
            gameObject.SetActive(startActivity);
        }
        public void Switch()
        {
            gameObject.SetActive(!gameObject.activeSelf);
        }
    }
}
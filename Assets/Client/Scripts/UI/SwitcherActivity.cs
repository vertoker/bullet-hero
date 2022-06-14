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
            gameObject.SetActive(true);
        }
        private void Start()
        {
            gameObject.SetActive(startActivity);
        }
        public void Switch()
        {
            SetActive(!gameObject.activeSelf);
        }
        public void SetActive(bool value)
        {
            gameObject.SetActive(value);
        }
    }
}
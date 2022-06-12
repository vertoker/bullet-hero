using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

namespace UI
{
    public class ToggleImageActivity : MonoBehaviour
    {
        [SerializeField] private Toggle toggle;
        private Image image;

        private void Awake()
        {
            image = GetComponent<Image>();
        }
        private void OnEnable()
        {
            toggle.ToggleUpdate += SetActivity;
        }
        private void OnDisable()
        {
            toggle.ToggleUpdate -= SetActivity;
        }
        private void SetActivity(bool value)
        {
            image.enabled = value;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;

namespace UI
{
    public class Toggle : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private bool isOn = false;
        [SerializeField] private Color backgroundOff, iconOff;
        [SerializeField] private Color backgroundOn, iconOn;
        private Image background, icon;
        private bool isInited = false;

        private UnityEvent<bool> toogleUpdate = new UnityEvent<bool>();

        public event UnityAction<bool> ToggleUpdate
        {
            add => toogleUpdate.AddListener(value);
            remove => toogleUpdate.RemoveListener(value);
        }

        public bool IsOn => isOn;

        private void Awake()
        {
            background = GetComponent<Image>();
            icon = transform.GetChild(0).GetComponent<Image>();
            isInited = true;
            UpdateToggle();
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            PressToggle();
        }

        public void PressToggle(bool value)
        {
            isOn = value;
            UpdateToggle();
        }
        public void PressToggle()
        {
            isOn = !isOn;
            UpdateToggle();
        }

        private void UpdateToggle()
        {
            if (!isInited)
                return;

            if (isOn)
            {
                background.color = backgroundOn;
                icon.color = iconOn;
            }
            else
            {
                background.color = backgroundOff;
                icon.color = iconOff;
            }
            toogleUpdate.Invoke(isOn);
        }
    }
}
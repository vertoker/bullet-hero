using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;

namespace LevelEditor.UI
{
    public class IconDropdown : MonoBehaviour
    {
        [SerializeField] private int startSelect = 0;
        [SerializeField] private Sprite[] icons;
        [SerializeField] private Image originSpawn;
        [SerializeField] private GameObject template;
        [SerializeField] private Image selectedIcon;

        private Image[] objects;

        [Space]
        [SerializeField]
        private UnityEvent<int> selectEvent = new UnityEvent<int>();
        public event UnityAction<int> SelectEvent
        {
            add => selectEvent.AddListener(value);
            remove => selectEvent.RemoveListener(value);
        }

        private void Awake()
        {
            objects = new Image[icons.Length];
            for (int i = 0; i < icons.Length; i++)
            {
                objects[i] = Instantiate(originSpawn, template.transform);
                int local = i;
                objects[i].GetComponent<Button>().onClick.AddListener(() => { Select(local); });
                objects[i].sprite = icons[i];
            }
        }
        private void OnEnable()
        {
            Select(startSelect);
        }

        public void SwitchActivity()
        {
            template.SetActive(!template.activeSelf);
        }
        public void Open()
        {
            template.SetActive(true);
        }
        public void Close()
        {
            template.SetActive(false);
        }
        public void Select(int id)
        {
            template.SetActive(false);
            selectedIcon.sprite = icons[id];
            selectEvent.Invoke(id);
        }
    }
}
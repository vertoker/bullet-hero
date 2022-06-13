using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;
using Utils.Pool;

namespace UI
{
    public class HorizontalSliderInt : MonoBehaviour
    {
        [SerializeField] private Color selected;
        [SerializeField] private Color notFilled;
        [SerializeField] private Color filled;

        private int length = 0;
        private bool isInited = false;
        private PoolSpawner poolSpawner;
        private Image[] blocks;

        private int selectedSlot = 0;
        public int Value => selectedSlot;

        private UnityEvent<int, int> sliderUpdate = new UnityEvent<int, int>();
        public event UnityAction<int, int> SliderUpdate
        {
            add => sliderUpdate.AddListener(value);
            remove => sliderUpdate.RemoveListener(value);
        }

        private void Awake()
        {
            poolSpawner = GetComponent<PoolSpawner>();
        }
        private void OnEnable()
        {
            Initialize(5);
        }

        private void Initialize(int length)
        {
            if (isInited)
                return;
            isInited = true;

            EnqueueAll();
            this.length = length;
            DequeueAll();
        }
        private void EnqueueAll()
        {
            for (int i = 0; i < length; i++)
            {
                poolSpawner.Enqueue(blocks[i].gameObject);
            }
        }
        private void DequeueAll()
        {
            blocks = new Image[length];
            for (int i = 0; i < length; i++)
            {
                blocks[i] = poolSpawner.Dequeue().GetComponent<Image>();
                blocks[i].GetComponent<HorizontalSliderBlock>().Init(this, i);
            }
        }

        public void UpdateSlider(int selectedSlot)
        {
            if ((!isInited) || length <= selectedSlot)
                return;

            this.selectedSlot = selectedSlot;
            for (int i = 0; i < selectedSlot; i++)
            {
                blocks[i].color = filled;
            }
            blocks[selectedSlot].color = selected;
            for (int i = selectedSlot + 1; i < length; i++)
            {
                blocks[i].color = notFilled;
            }

            sliderUpdate.Invoke(selectedSlot, length);
        }
    }
}
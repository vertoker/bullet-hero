using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;
using Utils.Pool;

namespace UI
{
    public class HorizontalSliderBlock : MonoBehaviour, IPointerEnterHandler
    {
        private HorizontalSliderInt slider;
        private int id;

        public void Init(HorizontalSliderInt slider, int id)
        {
            this.slider = slider;
            this.id = id;
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            slider.UpdateSlider(id);
        }
    }
}
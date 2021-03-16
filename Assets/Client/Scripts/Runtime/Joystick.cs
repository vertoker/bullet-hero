using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// Скрипт виртуального джостика для UI
public class Joystick : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public Image bgImg;
    public Image jsImg;
    private Vector2 inputVector;

    public void OnPointerDown(PointerEventData p) { OnDrag(p); }

    public void OnDrag(PointerEventData p)
    {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(bgImg.rectTransform, p.position, p.pressEventCamera, out Vector2 pos))
        {
            inputVector = pos / bgImg.rectTransform.sizeDelta * 2f;
            inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;
            jsImg.rectTransform.anchoredPosition = inputVector * (bgImg.rectTransform.sizeDelta / 2f);
        }
    }

    public void OnPointerUp(PointerEventData p)
    {
        jsImg.rectTransform.anchoredPosition = new Vector2();
        inputVector = Vector2.zero;
    }

    public void ResetJoystick()
    {
        jsImg.rectTransform.anchoredPosition = new Vector2();
        inputVector = Vector2.zero;
    }

    public Vector2 Joy() { return inputVector; }
    public Vector3 JoyVector3() { return inputVector; }
    public void SetActivity(bool enabled) 
    { bgImg.enabled = jsImg.enabled = enabled; }
}
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine;

namespace Game.Core
{
    public class InputController : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        [SerializeField] private ControlType controlType = ControlType.Mouse;
        [SerializeField] private Camera cam;
        [SerializeField] private Transform target;
        [SerializeField] private Player player;

        private bool active = true;
        private Vector3 borders;
        private Vector3 startPos;
        private Vector3 playerStartPos;

        private void Start()
        {
            borders = new Vector2(9 * cam.aspect - Player.RADIUS, 9 - Player.RADIUS);
        }
        private void OnEnable()
        {
            player.DeathCaller += Death;
        }
        private void OnDisable()
        {
            player.DeathCaller -= Death;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!active)
                return;
            if (controlType == ControlType.Finger)
                FingerDown();
            if (controlType == ControlType.Mouse)
                MouseControl();
        }
        public void OnDrag(PointerEventData eventData)
        {
            if (!active)
                return;
            if (controlType == ControlType.Finger)
                FingerDrag();
            if (controlType == ControlType.Mouse)
                MouseControl();
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            startPos = playerStartPos = new Vector3();
        }

        private void Death()
        {
            active = false;
            startPos = playerStartPos = new Vector3();
            target.gameObject.SetActive(false);
        }
        private void MouseControl()
        {
            Vector2 v = cam.ScreenToWorldPoint(Input.mousePosition);
            float x = Mathf.Clamp(v.x, -borders.x, borders.x);
            float y = Mathf.Clamp(v.y, -borders.y, borders.y);
            target.localPosition = new Vector3(x, y);
        }
        private void FingerDown()
        {
            startPos = cam.ScreenToWorldPoint(Input.mousePosition);
            playerStartPos = target.position;
        }
        private void FingerDrag()
        {
            Vector3 offset = cam.ScreenToWorldPoint(Input.mousePosition) - startPos;
            Vector3 v = playerStartPos + offset;

            float x = Mathf.Clamp(v.x, -borders.x, borders.x);
            float y = Mathf.Clamp(v.y, -borders.y, borders.y);
            target.localPosition = new Vector3(x, y);
        }
    }

    public enum ControlType : byte
    {
        Mouse = 0,
        Finger = 1
    }
}
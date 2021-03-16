using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine;

public class InputController : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public Transform player;
    public Transform target;
    public Joystick joystick;
    private Vector2 borders;
    private Vector2 startPos;
    private Vector2 playerStartPos;
    private float couf = 1f;
    private Vector2 posStable;
    private float rotVelocity;
    private float speed = 0.3f;
    public bool isFinger { get; set; } = true;
    public bool isJoystick { get; set; } = true;

    public void StartInputController(float aspect)
    {
        borders = new Vector2(9 * aspect - 0.25f, 9 - 0.25f);
        Vector2 sCoordinate = new Vector2(1080f * aspect, 1080f);
        Vector2 sResolution = new Vector2(Screen.width, Screen.height);
        Vector2 sCouf = new Vector2(sCoordinate.x / sResolution.x, sCoordinate.y / sResolution.y);
        posStable = new Vector2(Screen.width, Screen.height) / 2f;
        couf = (sCouf.x + sCouf.y) / 2f * 0.01852222f;
    }

    public void Update()
    {
        float targetLocal = 0;
        Vector2 offset = player.position - target.localPosition;

        if (offset.sqrMagnitude > 0.01f)
        { targetLocal = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg + 90; }

        player.localEulerAngles = new Vector3(0, 0, Mathf.SmoothDampAngle(player.localEulerAngles.z, targetLocal, ref rotVelocity, 0.05f));
    }

    public void FixedUpdate()
    {
        if (isJoystick)
        {
            target.localPosition += joystick.JoyVector3() * speed;
        }
        Vector2 v = target.localPosition;
        if (v.x > borders.x) { v = new Vector2(borders.x, v.y); }
        else if (v.x < -borders.x) { v = new Vector2(-borders.x, v.y); }
        if (v.y > borders.y) { v = new Vector2(v.x, borders.y); }
        else if (v.y < -borders.y) { v = new Vector2(v.x, -borders.y); }
        target.localPosition = v;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isFinger) 
        {
            startPos = ((Vector2)Input.mousePosition - posStable) * couf;
            playerStartPos = target.localPosition;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isFinger)
        {
            Vector2 offset = ((Vector2)Input.mousePosition - posStable) * couf - startPos;
            Vector2 v = playerStartPos + offset;

            if (v.x > borders.x) { v = new Vector2(borders.x, v.y); }
            else if (v.x < -borders.x) { v = new Vector2(-borders.x, v.y); }
            if (v.y > borders.y) { v = new Vector2(v.x, borders.y); }
            else if (v.y < -borders.y) { v = new Vector2(v.x, -borders.y); }
            target.localPosition = v;
        }
        else
        {
            Vector2 v = ((Vector2)Input.mousePosition - posStable) * couf;
            if (v.x > borders.x) { v = new Vector2(borders.x, v.y); }
            else if (v.x < -borders.x) { v = new Vector2(-borders.x, v.y); }
            if (v.y > borders.y) { v = new Vector2(v.x, borders.y); }
            else if (v.y < -borders.y) { v = new Vector2(v.x, -borders.y); }
            target.localPosition = v;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        startPos = playerStartPos = new Vector2();
    }
}

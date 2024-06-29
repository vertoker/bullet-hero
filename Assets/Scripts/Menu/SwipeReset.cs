using UnityEngine.UI;
using UnityEngine;
using Custom;

public class SwipeReset : GlobalBehaviour
{
    public Menu menu;
    public Image arrowIm;
    public Text info, bye;
    public RectTransform arrow;
    private Vector2 startPos;
    private bool isSwiped = true;
    private int progress = 1;
    public void Down()
    {
        isSwiped = true;
        startPos = Input.mousePosition;
    }
    public void Drag()
    {
        if (!isSwiped) { return; }
        Vector2 offset = (Vector2)Input.mousePosition - startPos;
        if (offset.magnitude >= 150f)
        {
            if (Mathf.Abs(offset.x) > Mathf.Abs(offset.y))
            {
                if (offset.x > 0)
                    Change(Swipe.Right);
                else
                    Change(Swipe.Left);
            }
            else
            {
                if (offset.y > 0)
                    Change(Swipe.Up);
                else
                    Change(Swipe.Down);
            }
            isSwiped = false;
        }
    }

    public void StartReset()
    {
        info.enabled = true;
        bye.enabled = false;
        arrowIm.enabled = true;
        arrow.localEulerAngles = new Vector3(0, 0, -90);
        return;
    }
    public bool Change(Swipe swipe)
    {
        switch (progress)
        {
            case 1:
                if (swipe == Swipe.Right)
                {
                    arrow.localEulerAngles = new Vector3(0, 0, 0);
                    info.text = "SWIPE UP";
                    progress++;
                    return false;
                }
                break;
            case 2:
                if (swipe == Swipe.Up)
                {
                    arrow.localEulerAngles = new Vector3(0, 0, 90);
                    info.text = "SWIPE LEFT";
                    progress++;
                    return false;
                }
                break;
            case 3:
                if (swipe == Swipe.Left)
                {
                    arrow.localEulerAngles = new Vector3(0, 0, 180);
                    info.text = "SWIPE DOWN";
                    progress++;
                    return false;
                }
                break;
            case 4:
                if (swipe == Swipe.Down)
                {
                    progress++;
                    info.text = "";
                    bye.enabled = true;
                    arrowIm.enabled = false;
                    menu.ResetProgress();
                    return false;
                }
                break;
        }
        return true;
    }
    public void ExitReset()
    {
        progress = 1;
        info.text = "";
        bye.enabled = true;
        arrowIm.enabled = false;
        return;
    }
}

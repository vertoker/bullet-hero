using UnityEngine.Experimental.Rendering.Universal;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Custom;

public class MainAnimator : GlobalBehaviour
{
    public Sprite[] sprites;
    public ShadowCaster2D[] shadowCasters;
    public List<IAnimItem> Items { get; private set; }
    private Generate generate;

    void Start()
    {
        generate = GetComponent<Generate>();
        Items = new List<IAnimItem>();
    }

    void FixedUpdate()
    {
        for (int i = 0; i < Items.Count; i++)
        {
            if (Items[i].FrameAction())
            {
                IAnimItem item = Items[i];
                Items.Remove(Items[i]);
                item.EndAction();
            }
        }
    }
    public Sprite GetSprite(SpriteType spriteType)
    {
        return sprites[(int)spriteType];
    }
    public ShadowCaster2D GetShadowCaster2D(SpriteType spriteType)
    {
        return shadowCasters[(int)spriteType];
    }

    #region StartsAnim
    //Animate Color in SpriteRenderer
    public void StartAnimColor(float time, Color start, Color end, SpriteRenderer sr, EndAnim endAnim = null)
    {
        sr.color = start;
        Items.Add(new ColorItem(time, start, end, sr, endAnim));
    }
    //Animate Color in SpriteRenderer (Destroy in end)
    public void StartAnimColor(float time, Color start, Color end, SpriteRenderer sr)
    {
        sr.color = start;
        Items.Add(new ColorItem(time, start, end, sr, generate.destroyDelegate));
    }
    //Animate Text in SpriteRenderer
    public void StartAnimText(AnimText animText)
    {
        Items.Add(new TextCurveItem(animText, generate.destroyDelegate));
    }
    //Animate effect in SpriteRenderer
    public void StartAnimColor(AnimSprite animSprite)
    {
        Items.Add(new ColorCurveItem(animSprite, generate.destroyDelegate));
    }

    //Animate Position in Transform
    public void StartAnimPosRepeat(float time, Vector2 start, Vector2 end, Transform tr, EndAnim endAnim = null)
    {
        tr.localPosition = start;
        Items.Add(new PosItem(time, start, end, tr, endAnim));
    }
    //Animate Progress bar in Transform
    public void StartAnimFilled(float time, float progressTarget, float width, Direction direction, Vector2 pos, Vector2 border, Transform tr, EndAnim endAnim = null)
    {
        Items.Add(new FilledItem(time, progressTarget, width, direction, tr, pos, border, generate.cam.cam.aspect, endAnim));
    }

    //Animate Position in Transform with velocity
    public void StartAnimPosVel(Vector2 start, Vector2 vel, Vector2 border, Transform tr)
    {
        tr.localPosition = start;
        Items.Add(new PosVelItem(vel, border, tr, generate.destroyDelegate));
    }
    //Animate Position in Transform
    public void StartAnimPos(float time, Vector2 start, Vector2 end, Transform tr, EndAnim endAnim = null)
    {
        tr.localPosition = start;
        Items.Add(new PosItem(time, start, end, tr, endAnim));
    }

    //Animate Rotation in Transform
    public void StartAnimRot(float time, float speed, Transform tr, EndAnim endAnim = null)
    {
        Items.Add(new RotItem(time, speed, tr, endAnim));
    }
    //Animate Rotation in Transform with velocity
    public void StartAnimRot(float speed, Vector2 border, Transform tr, EndAnim endAnim = null)
    {
        Items.Add(new RotItem(speed, border, tr, endAnim));
    }

    //Animate Scale in Transform
    public void StartAnimScale(float time, float start, float end, Transform tr, EndAnim endAnim = null)
    {
        tr.localScale = new Vector3(start, start, 1);
        Items.Add(new ScaleItem(time, start, end, tr, endAnim));
    }
    //Animate Scale in Transform with shaking (radius)
    public void StartAnimScale(float time, float start, float end, float radius, Transform tr, EndAnim endAnim = null)
    {
        tr.localScale = new Vector3(start, start, 1);
        Items.Add(new ScaleItem(time, radius, start, end, tr, endAnim));
    }

    #endregion
}
/// /////////////////////////////
public interface IAnimItem
{
    bool FrameAction();
    void EndAction();
}
/// /////////////////////////////
class RectPosItem : IAnimItem// For UI Transform
{
    readonly EndAnim endAnim;
    readonly RectTransform tr;
    Vector3 frameRule;
    int frameCounter;

    public RectPosItem(float time, Vector2 start, Vector2 end, RectTransform tr, EndAnim endAnim)
    {
        frameCounter = (int)(time * 60f);
        float x = (end.x - start.x) / frameCounter;
        float y = (end.y - start.y) / frameCounter;
        frameRule = new Vector2(x, y);

        this.endAnim = endAnim;
        this.tr = tr;
    }
    public bool FrameAction()
    {
        frameCounter--;
        tr.localPosition += frameRule;
        return frameCounter < 0;
    }

    public void EndAction()
    {
        endAnim?.Invoke();
        return;
    }
}
/// /////////////////////////////
class PlayerItem : IAnimItem/// ///////////////////////////////////////////////////////////////смтри в input manager
{
    readonly EndAnim endAnim;
    readonly Transform tr;
    Vector3 frameRule;
    int frameCounter;

    public PlayerItem(float time, Vector2 start, Vector2 end, Transform tr, EndAnim endAnim)
    {
        frameCounter = (int)(time * 60f);
        float x = (end.x - start.x) / frameCounter;
        float y = (end.y - start.y) / frameCounter;
        frameRule = new Vector2(x, y);

        this.endAnim = endAnim;
        this.tr = tr;
    }
    public bool FrameAction()
    {
        frameCounter--;
        tr.localPosition += frameRule;
        return frameCounter < 0;
    }

    public void EndAction()
    {
        endAnim?.Invoke();
        return;
    }
}
/// /////////////////////////////
class FilledItem : IAnimItem
{
    readonly EndAnim endAnim;
    readonly Transform tr;
    float target, max, width;
    int frameCounter = 0;
    Direction direction;
    Vector2 border, pos;
    float aspect;

    public FilledItem(float time, float target, float width, Direction direction, Transform tr, Vector2 pos, Vector2 border, float aspect, EndAnim endAnim)
    {
        max = (int)(time * 60f);
        this.target = target;
        this.aspect = aspect;
        this.direction = direction;
        this.endAnim = endAnim; 
        this.border = border;
        this.width = width;
        this.pos = pos;
        this.tr = tr;
    }
    public bool FrameAction()
    {
        frameCounter++;
        Progress();
        return frameCounter == max;
    }

    public void Progress()
    {
        float progress = frameCounter / max;
        Vector2 scale = target * progress * border;
        switch (direction)
        {
            case Direction.Up:
                tr.localScale = new Vector2(width, scale.y);
                tr.localPosition = new Vector2(pos.x, -border.y + scale.y / 2);
                break;
            case Direction.Down:
                tr.localScale = new Vector2(width, scale.y);
                tr.localPosition = new Vector2(pos.x, border.y - scale.y / 2);
                break;
            case Direction.Left:
                tr.localScale = new Vector2(scale.x, width);
                tr.localPosition = new Vector2(-border.x + scale.x / 2, pos.y);
                break;
            case Direction.Right:
                tr.localScale = new Vector2(scale.x, width);
                tr.localPosition = new Vector2(border.x - scale.x / 2, pos.y);
                break;
        }
        return;
    }
    public void EndAction()
    {
        endAnim?.Invoke();
        return;
    }
}
/// ///////////////////////////////////
class ColorCurveItem : IAnimItem
{
    DestroyDelegate destroy;
    AnimationCurve curve;
    SpriteRenderer sr;
    Color offset, start;
    int counter = 0, max;
    int mStart, mEnd;
    float offsetAStart, offsetAEnd, A = 0;

    public ColorCurveItem(AnimSprite animSprite, DestroyDelegate destroy)
    {
        mStart = (int)(animSprite.time2Start.Get() * 60f);
        int mExist = (int)(animSprite.time2Exist.Get() * 60f);
        int mEnd = (int)(animSprite.time2End.Get() * 60f);
        max = mStart + mExist + mEnd;
        this.mEnd = max - mEnd;

        if (mStart != 0) 
        { offsetAStart = 1f / mStart; }
        else { A = 1; }
        offsetAEnd = 1f / mEnd;
        this.destroy = destroy;
        curve = animSprite.curve;
        sr = animSprite.GetSR();
        start = animSprite.color0;
        offset = animSprite.color1 - start;
    }
    public bool FrameAction()
    {
        Color c = start + curve.Evaluate((float)counter / max) * offset;
        if (counter < mStart) { A += offsetAStart; }
        else if (counter > mEnd) { A -= offsetAEnd; }
        sr.color = new Color(c.r, c.g, c.b, A);
        counter++; return counter == max;
    }
    public void EndAction()
    {
        destroy?.Invoke(sr.gameObject);
        return;
    }
}
/// /////////////////////////////
class TextCurveItem : IAnimItem
{
    DestroyDelegate destroy;
    AnimationCurve curve;
    TextMesh tm;
    Color offset, start;
    int counter = 0, max;
    int mStart, mEnd;
    float offsetAStart, offsetAEnd, A = 0;

    public TextCurveItem(AnimText animText, DestroyDelegate destroy)
    {
        mStart = (int)(animText.time2Start.Get() * 60f);
        int mExist = (int)(animText.time2Exist.Get() * 60f);
        int mEnd = (int)(animText.time2End.Get() * 60f);
        max = mStart + mExist + mEnd;
        this.mEnd = max - mEnd;

        if (mStart != 0)
        { offsetAStart = 1f / mStart; }
        else { A = 1; }
        offsetAEnd = 1f / mEnd;
        this.destroy = destroy;
        curve = animText.curve;
        tm = animText.GetTM();
        start = animText.color0;
        offset = animText.color1 - start;
    }
    public bool FrameAction()
    {
        Color c = start + curve.Evaluate((float)counter / max) * offset;
        if (counter < mStart) { A += offsetAStart; }
        else if (counter > mEnd) { A -= offsetAEnd; }
        tm.color = new Color(c.r, c.g, c.b, A);
        counter++; return counter == max;
    }
    public void EndAction()
    {
        destroy?.Invoke(tm.gameObject);
        return;
    }
}
/// /////////////////////////////
class ColorItem : IAnimItem
{
    readonly DestroyDelegate destroy;
    readonly EndAnim endAnim;
    readonly SpriteRenderer sr;
    Vector4 frameRule;
    int frameCounter;
    Color end;

    public ColorItem(float time, Color start, Color end, SpriteRenderer sr, EndAnim endAnim)
    {
        frameCounter = (int)(time * 60f);
        float r = (end.r - start.r) / frameCounter;
        float g = (end.g - start.g) / frameCounter;
        float b = (end.b - start.b) / frameCounter;
        float a = (end.a - start.a) / frameCounter;
        frameRule = new Vector4(r, g, b, a);

        this.endAnim = endAnim;
        this.end = end;
        this.sr = sr;
    }
    public ColorItem(float time, Color start, Color end, SpriteRenderer sr, DestroyDelegate destroy)
    {
        frameCounter = (int)(time * 60f);
        float r = (end.r - start.r) / frameCounter;
        float g = (end.g - start.g) / frameCounter;
        float b = (end.b - start.b) / frameCounter;
        float a = (end.a - start.a) / frameCounter;
        frameRule = new Vector4(r, g, b, a);

        this.destroy = destroy;
        this.end = end;
        this.sr = sr;
    }

    public bool FrameAction()
    {
        frameCounter--;
        float r = end.r - frameRule.x * frameCounter;
        float g = end.g - frameRule.y * frameCounter;
        float b = end.b - frameRule.z * frameCounter;
        float a = end.a - frameRule.w * frameCounter;
        Color color = new Color(r, g, b, a);
        sr.color = color;
        return color == end;
    }

    public void EndAction()
    {
        destroy?.Invoke(sr.gameObject);
        endAnim?.Invoke();
        return;
    }
}
/// /////////////////////////////
class PosItem : IAnimItem
{
    readonly EndAnim endAnim;
    readonly Transform tr;
    Vector3 frameRule;
    int frameCounter;

    public PosItem(float time, Vector2 start, Vector2 end, Transform tr, EndAnim endAnim)
    {
        frameCounter = (int)(time * 60f);
        float x = (end.x - start.x) / frameCounter;
        float y = (end.y - start.y) / frameCounter;
        frameRule = new Vector2(x, y);

        this.endAnim = endAnim;
        this.tr = tr;
    }
    public bool FrameAction()
    {
        frameCounter--;
        tr.localPosition += frameRule;
        return frameCounter < 0;
    }

    public void EndAction()
    {
        endAnim?.Invoke();
        return;
    }
}
/// /////////////////////////////
class RotItem : IAnimItem
{
    readonly EndAnim endAnim;
    readonly Transform tr;
    int frameCounter;
    Vector2 border;
    Vector3 speed;

    public RotItem(float time, float speed, Transform tr, EndAnim endAnim)
    {
        frameCounter = (int)(time * 60f);
        this.endAnim = endAnim;
        this.speed = new Vector3(0, 0, speed);
        this.tr = tr;
    }
    public RotItem(float speed, Vector2 border, Transform tr, EndAnim endAnim)
    {
        frameCounter = 0;
        this.border = border;
        this.endAnim = endAnim;
        this.speed = new Vector3(0, 0, speed);
        this.tr = tr;
    }
    public bool FrameAction()
    {
        frameCounter--;
        tr.localEulerAngles -= speed;
        if (frameCounter < 0) 
        { return Math.OutOfBorder(tr.localPosition, border); }
        return frameCounter == 0;
    }

    public void EndAction()
    {
        endAnim?.Invoke();
        return;
    }
}
/// /////////////////////////////
class ScaleItem : IAnimItem
{
    readonly EndAnim endAnim;
    readonly Transform tr;
    Vector2 frameRule;
    int frameCounter;
    Vector2 end;
    readonly bool shake = false;
    readonly float radius;

    public ScaleItem(float time, float start, float end, Transform tr, EndAnim endAnim)
    {
        frameCounter = (int)(time * 60f);
        float x = (end - start) / frameCounter;
        frameRule = new Vector2(x, x);

        this.endAnim = endAnim;
        this.end = new Vector2(end, end);
        this.tr = tr;
    }
    public ScaleItem(float time, float radius, float start, float end, Transform tr, EndAnim endAnim)
    {
        frameCounter = (int)(time * 60f);
        float x = (end - start) / frameCounter;
        frameRule = new Vector2(x, x);

        this.endAnim = endAnim;
        this.radius = radius;
        this.end = new Vector2(end, end);
        this.tr = tr;
    }
    public bool FrameAction()
    {
        frameCounter--;
        float x = end.x - frameRule.x * frameCounter;
        float y = end.y - frameRule.y * frameCounter;
        Vector2 scale = new Vector2(x, y);
        if (shake)
        {
            scale += (Vector2)(Quaternion.Euler(0, 0, Random.Range(0, 360)) * Vector2.up) * radius;
        }
        tr.localScale = scale;
        return scale == end;
    }

    public void EndAction()
    {
        endAnim?.Invoke();
        return;
    }
}
/// /////////////////////////////
class PosVelItem : IAnimItem
{
    readonly DestroyDelegate destroy;
    readonly Transform tr;
    Vector2 vel, border;
    int activeEndAction = 0;

    public PosVelItem(Vector2 vel, Vector2 border, Transform tr, DestroyDelegate destroy)
    {
        this.destroy = destroy;
        this.border = border;
        this.vel = vel;
        this.tr = tr;
    }
    public bool FrameAction()
    {
        activeEndAction++;
        tr.localPosition += (Vector3)vel;
        return Math.OutOfBorder(tr.localPosition, border) && activeEndAction >= 5;
    }

    public void EndAction()
    {
        destroy(tr.gameObject);
        return;
    }
}
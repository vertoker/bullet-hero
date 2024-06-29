using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Custom;

public enum DataItem
{
    Bullet = 0, BulletRow = 1, BulletExplosion = 2, BulletDivision = 3, LineSimple = 4,
    CircleSimple = 5, StandardGun = 6, SquareSnake = 7, Pillar = 8, AnimSprite = 9,
    AnimText = 10
}
[System.Serializable]
public class PrefabItem { }

public class Bullet : PrefabItem
{
    public FloatMod speed;
    public FloatMod size;
    public SpriteType sprite;
    public FloatMod spinSpeed;

    public Vector2Mod startPoint = new Vector2Mod();
    public Vector2Mod velocity = new Vector2Mod(CalculateFunctionVector2.Velocity);

    public Bullet(FloatMod speed, FloatMod size, SpriteType sprite, FloatMod spinSpeed, Vector2Mod startPoint, Vector2Mod velocity)
    {
        this.speed = speed;
        this.size = size;
        this.sprite = sprite;
        this.spinSpeed = spinSpeed;

        this.startPoint = startPoint;
        this.velocity = velocity;
    }

    public Bullet(FloatMod speed, FloatMod size, SpriteType sprite, FloatMod spinSpeed)
    {
        this.speed = speed;
        this.size = size;
        this.sprite = sprite;
        this.spinSpeed = spinSpeed;
    }
}
public class BulletRow : PrefabItem
{
    public IntMod count;
    public FloatMod speed;
    public FloatMod sizeBullet;

    public Vector2Mod startPoint = new Vector2Mod();
    public Vector2Mod velocity = new Vector2Mod(CalculateFunctionVector2.Velocity);

    public BulletRow(IntMod count, FloatMod speed, FloatMod sizeBullet, Vector2Mod startPoint, Vector2Mod velocity)
    {
        this.count = count;
        this.speed = speed;
        this.sizeBullet = sizeBullet;
        this.startPoint = startPoint;
        this.velocity = velocity;
    }
}
public class BulletExplosion : PrefabItem
{
    public float time2point;
    public float time2charging;
    public float time2spread;
    public float time2end;
    public CamMod camMod;

    public FloatMod size;
    public FloatMod spinSpeed;
    public SpriteType bomb;
    public FloatMod sizeExp;

    public Vector2Mod startPoint = new Vector2Mod();
    public Vector2Mod endPoint = new Vector2Mod(Direction.Random);

    public BulletExplosion(FloatMod size, FloatMod spinSpeed, SpriteType bomb, FloatMod sizeExp, float time2point, float time2charging, float time2spread, float time2end, CamMod camMod, Vector2Mod startPoint, Vector2Mod endPoint)
    {
        this.size = size;
        this.spinSpeed = spinSpeed;
        this.bomb = bomb;
        this.sizeExp = sizeExp;

        this.time2point = time2point;
        this.time2charging = time2charging;
        this.time2spread = time2spread;
        this.time2end = time2end;
        this.camMod = camMod;

        this.startPoint = startPoint;
        this.endPoint = endPoint;
    }
}
public class BulletDivision : PrefabItem
{
    public float time2point;
    public float time2charging;
    public float time2spread;
    public float time2end;
    public CamMod camMod;

    public FloatMod size;
    public FloatMod spinSpeed;
    public SpriteType sprite;
    public IntMod[] bulletCounts;
    public Bullet bulletDivide;

    public Vector2Mod startPoint = new Vector2Mod();
    public Vector2Mod endPoint = new Vector2Mod(Direction.Random);

    public BulletDivision(FloatMod size, FloatMod spinSpeed, SpriteType sprite, IntMod[] bulletCounts, Bullet bullet, float time2point, float time2charging, float time2spread, float time2end, CamMod camMod, Vector2Mod startPoint, Vector2Mod endPoint)
    {
        this.size = size;
        this.spinSpeed = spinSpeed;
        this.sprite = sprite;
        this.bulletCounts = bulletCounts;
        bulletDivide = bullet;

        this.time2point = time2point;
        this.time2charging = time2charging;
        this.time2spread = time2spread;
        this.time2end = time2end;
        this.camMod = camMod;

        this.startPoint = startPoint;
        this.endPoint = endPoint;
    }
}
public class LineSimple : PrefabItem
{
    public float time2charging;
    public float time2exist;
    public float time2end;
    public CamMod camMod;
    public FloatMod throttling;
    public FloatMod width;

    public Vector2Mod point = new Vector2Mod();
    public FloatMod angle;

    public LineSimple(FloatMod width, FloatMod throttling, float time2charging, float time2exist, float time2end, CamMod camMod, Vector2Mod point, FloatMod angle)
    {
        this.width = width;
        this.throttling = throttling;
        this.time2charging = time2charging;
        this.time2exist = time2exist;
        this.time2end = time2end;
        this.camMod = camMod;

        this.point = point;
        this.angle = angle;
    }
}
public class CircleSimple : PrefabItem
{
    public float time2wait;
    public float time2charging;
    public float time2end;
    public CamMod camMod;
    public FloatMod spinSpeed;
    public FloatMod throttling;
    public FloatMod size;

    public Vector2Mod point = new Vector2Mod();

    public CircleSimple(FloatMod size, FloatMod throttling, FloatMod spinSpeed, float time2wait, float time2charging, float time2end, CamMod camMod, Vector2Mod point)
    {
        this.size = size;
        this.throttling = throttling;
        this.spinSpeed = spinSpeed;
        this.time2wait = time2wait;
        this.time2charging = time2charging;
        this.time2end = time2end;
        this.camMod = camMod;

        this.point = point;
    }
}
public class StandardGun : PrefabItem
{
    public float time2move;
    public float time2charging;
    public float time2shoot;
    public float time2wait;
    public float time2end;

    public FloatMod size;
    public FloatMod recoil;
    public FloatMod speedMove;
    public FloatMod speedWait;
    public IntMod[] bulletCounts;
    public Bullet bullet;

    public Vector2Mod startPoint;
    public Vector2Mod startWaitPoint;
    public Vector2Mod endWaitPoint;
    public Vector2Mod endPoint;
    public StandardGun(FloatMod speedMove, FloatMod speedWait, FloatMod recoil, FloatMod size, IntMod[] bulletCounts, Bullet bullet, float time2move, float time2charging, float time2shoot, float time2wait, float time2end, Vector2Mod startPoint, Vector2Mod startWaitPoint, Vector2Mod endWaitPoint, Vector2Mod endPoint)
    {
        this.speedMove = speedMove;
        this.speedWait = speedWait;
        this.recoil = recoil;
        this.size = size;
        this.bulletCounts = bulletCounts;
        this.bullet = bullet;

        this.time2move = time2move;
        this.time2charging = time2charging;
        this.time2shoot = time2shoot;
        this.time2wait = time2wait;
        this.time2end = time2end;

        this.startPoint = startPoint;
        this.startWaitPoint = startWaitPoint;
        this.endWaitPoint = endWaitPoint;
        this.endPoint = endPoint;
    }
}
public class SquareSnake : PrefabItem
{
    public FloatMod time4Spawn;
    public FloatMod angleSpeed;
    public FloatMod distance;
    public FloatMod scale;
    public AnimSprite animSprite;

    public Vector2Mod point = new Vector2Mod();
    public FloatMod angle;

    public SquareSnake(FloatMod time4Spawn, FloatMod angleSpeed, FloatMod distance, FloatMod scale, AnimSprite animSprite, Vector2Mod point, FloatMod angle)
    {
        this.time4Spawn = time4Spawn;
        this.angleSpeed = angleSpeed;
        this.distance = distance;
        this.scale = scale;
        this.animSprite = animSprite;

        this.point = point;
        this.angle = angle;
    }
}
public class Pillar : PrefabItem
{
    public float time2wait;
    public float time2exist;
    public CamMod camMod;

    public FloatMod width;
    public FloatMod progressTarget;
    public Direction direction;

    public Vector2Mod point = new Vector2Mod();

    public Pillar(FloatMod width, FloatMod progressTarget, Direction direction, float time2wait, float time2exist, CamMod camMod, Vector2Mod point)
    {
        this.width = width;
        this.progressTarget = progressTarget;
        this.direction = direction;
        this.time2wait = time2wait;
        this.time2exist = time2exist;
        this.camMod = camMod;

        this.point = point;
    }
}
public class AnimSprite : PrefabItem
{
    public SpriteRenderer sr { private get; set; }
    public SpriteRenderer GetSR() { return sr; }
    public string name;
    public AnimationCurve curve;
    public SpriteType spriteType;
    public Color color0;
    public Color color1;
    public FloatMod time2Start;
    public FloatMod time2Exist;
    public FloatMod time2End;

    public Vector2Mod pos = new Vector2Mod();
    public float angle;
    public Vector2 scale;

    public AnimSprite() { }
    public AnimSprite(AnimationCurve curve, SpriteType spriteType, Color color0, Color color1, FloatMod time2Start, FloatMod time2Exist, FloatMod time2End, Vector2Mod pos, float angle, Vector2 scale)
    {
        this.curve = curve;
        this.spriteType = spriteType;
        this.color0 = color0;
        this.color1 = color1;
        this.time2Start = time2Start;
        this.time2Exist = time2Exist;
        this.time2End = time2End;
        sr = null;

        this.pos = pos;
        this.angle = angle;
        this.scale = scale;
    }
    public AnimSprite(AnimationCurve curve, SpriteType spriteType, Color color0, Color color1, FloatMod time2Start, FloatMod time2Exist, FloatMod time2End, Vector2Mod pos, float angle)
    {
        this.curve = curve;
        this.spriteType = spriteType;
        this.color0 = color0;
        this.color1 = color1;
        this.time2Start = time2Start;
        this.time2Exist = time2Exist;
        this.time2End = time2End;
        sr = null;

        this.pos = pos;
        this.angle = angle;
        this.scale = new Vector2(1, 1);
    }
}
public class AnimText : PrefabItem
{
    public TextMesh tm { private get; set; }
    public TextMesh GetTM() { return tm; }

    public string text;
    public AnimationCurve curve;
    public Color color0;
    public Color color1;
    public FloatMod time2Start;
    public FloatMod time2Exist;
    public FloatMod time2End;

    public Vector2Mod pos = new Vector2Mod();
    public float angle;
    public Vector2 scale;

    public AnimText() { }

    public AnimText(string text, AnimationCurve curve, Color color0, Color color1, FloatMod time2Start, FloatMod time2Exist, FloatMod time2End, Vector2Mod pos, float angle, Vector2 scale)
    {
        this.text = text;
        this.curve = curve;
        this.color0 = color0;
        this.color1 = color1;
        this.time2Start = time2Start;
        this.time2Exist = time2Exist;
        this.time2End = time2End;
        tm = null;

        this.pos = pos;
        this.angle = angle;
        this.scale = scale;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Custom;

public enum Direction { Up = 1, Down = 2, Left = 3, Right = 4, Random = 5 }
public enum Swipe { Up = 1, Down = 2, Left = 3, Right = 4 }
public enum ModCam { None = 1, Shake = 2, ShakeAngle = 3, ShakeDeep = 4 }
public enum CalculateFunctionVector2 { OrigPos = 1, InnerPos = 2, OuterPos = 3, Velocity = 4, PlayerPos = 5 }
public enum CalculateFunctionNum { Standard = 1, Random = 2 }

public class BasicValue { }

[System.Serializable]
public class Vector2Mod : BasicValue
{
    public CalculateFunctionVector2 func = CalculateFunctionVector2.OrigPos;
    public Vector2 point;//OrigPos and end
    public Direction direction = Direction.Random;//OuterPos

    public Vector2Mod(float x, float y)
    { func = CalculateFunctionVector2.OrigPos; point = new Vector2(x, y); }
    public Vector2Mod(Vector2 xy)
    { func = CalculateFunctionVector2.OrigPos; point = xy; }
    public Vector2Mod(bool isPlayer = false)
    {
        if (isPlayer)
            func = CalculateFunctionVector2.PlayerPos;
        else
            func = CalculateFunctionVector2.InnerPos;
    }
    public Vector2Mod(CalculateFunctionVector2 func)
    { this.func = func; }
    public Vector2Mod(Direction direction)
    { func = CalculateFunctionVector2.OuterPos; this.direction = direction; }
}
[System.Serializable]
public class FloatMod : BasicValue
{
    public CalculateFunctionNum func = CalculateFunctionNum.Standard;
    public float instance = float.NaN;
    public float start = float.NaN;
    public float end = float.NaN;
    private bool isSeted = false;
    public FloatMod(float start) { instance = start; }
    public FloatMod(float start, float end) { this.start = start; this.end = end; func = CalculateFunctionNum.Random; }
    public float Get(bool generate = false)
    {
        if (!isSeted || generate)
        {
            isSeted = true;
            if (func == CalculateFunctionNum.Random)
            { instance = Random.Range(start, end); }
        }
        return instance;
    }
}
[System.Serializable]
public class IntMod : BasicValue
{
    public CalculateFunctionNum func = CalculateFunctionNum.Standard;
    public int instance = int.MaxValue;
    public int start;
    public int end;
    private bool isSeted = false;
    public IntMod(int start) { instance = start; }
    public IntMod(int start, int end) { this.start = start; this.end = end; func = CalculateFunctionNum.Random; }
    public int Get()
    {
        if (!isSeted)
        {
            isSeted = true;
            if (func == CalculateFunctionNum.Random)
            { instance = Random.Range(start, end); }
        }
        return instance;
    }
}
[System.Serializable]
public class CamMod : BasicValue
{
    public ModCam cam = ModCam.None;
    public FloatMod parameter = new FloatMod(0);
    public FloatMod radius = new FloatMod(0.1f);
    public CamMod(ModCam cam)
    {
        this.cam = cam;
    }

    public CamMod(ModCam cam, FloatMod parameter, FloatMod radius)
    {
        this.parameter = parameter;
        this.radius = radius;
        this.cam = cam;
    }
}
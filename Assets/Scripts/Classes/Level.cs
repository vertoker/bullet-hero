using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

#region Main save class
/// (need save THIS class in JSON)
[Serializable]
public class Level
{
    public LevelData data;//
    public List<Marker> markers;//
    public List<Checkpoint> checkpoints;//
    public List<NestedPrefab> nestedPrefabs;//
    public List<Prefab> prefabs;//
    public Effects effects;//

    public Level(LevelData data, List<Marker> markers, List<Checkpoint> checkpoints, List<NestedPrefab> nestedPrefabs, List<Prefab> prefabs, Effects effects)
    {
        this.data = data;
        this.markers = markers;
        this.checkpoints = checkpoints;
        this.nestedPrefabs = nestedPrefabs;
        this.prefabs = prefabs;
        this.effects = effects;
    }
}
#endregion

#region LevelData
// Include basic level information
[Serializable]
public class LevelData
{
    public string levelName;
    public int editor_version;//editor version
    public string music_title;
    public string music_author;
    public string level_author;
    public float startFadeOut;
    public float endFadeOut;

    public LevelData(string levelName, int editor_version, string music_title, string music_author, string level_author, float startFadeOut, float endFadeOut)
    {
        this.levelName = levelName;
        this.editor_version = editor_version;
        this.music_title = music_title;
        this.music_author = music_author;
        this.level_author = level_author;
        this.startFadeOut = startFadeOut;
        this.endFadeOut = endFadeOut;
    }
}
#endregion

#region Markers
// Timecode markup
[Serializable]
public class Marker
{
    public bool active;
    public int groupID;
    public string name;
    public float t;

    public Marker(bool active, int groupID, string name, float t)
    {
        this.active = active;
        this.groupID = groupID;
        this.name = name;
        this.t = t;
    }
}
#endregion

#region Checkpoint
// Save player in song
[Serializable]
public class Checkpoint
{
    public bool active;
    public string name;
    public float t;
    public float x;//spawn pos x
    public float y;//spawn pos y

    public Checkpoint(bool active, string name, float t, float x = 0, float y = 0)
    {
        this.active = active;
        this.name = name;
        this.x = x;
        this.t = t;
        this.y = y;
    }
}
#endregion

#region Prefab
//Original prefab with original data (animation)
[Serializable]
public class Prefab
{
    public string name;
    public bool active;
    public bool collider;
    public List<Pos> pos;
    public List<Sca> sca;
    public List<Rot> rot;
    public List<Clr> clr;
    public SpriteType st;//sprite type
    public int startFrame;
    public int frameTimeExist;
    public int id, idParent;
    public AnchorPresets anchor;
    public AnchorPresets pivot;
    public int height;//x
    public int thread;//z
    public int layer;

    public float GetMin()
    {
        float end = pos[0].t;
        if (sca[0].t < end) { end = sca[0].t; }
        if (rot[0].t < end) { end = rot[0].t; }
        if (clr[0].t < end) { end = clr[0].t; }
        return end;
    }

    public Prefab(string name, bool active, bool collider, List<Pos> pos, List<Sca> sca, List<Rot> rot, List<Clr> clr, SpriteType st, int startFrame, int frameTimeExist, int idParent, int id, AnchorPresets anchor, AnchorPresets pivot, int thread, int height, int layer)
    {
        this.name = name;
        this.active = active;
        this.collider = collider;
        this.pos = pos;
        this.sca = sca;
        this.rot = rot;
        this.clr = clr;

        this.st = st;
        this.startFrame = startFrame;
        this.frameTimeExist = frameTimeExist;
        this.idParent = idParent;
        this.id = id;

        this.anchor = anchor;
        this.pivot = pivot;
        this.height = height;
        this.thread = thread;
        this.layer = layer;
    }
}
#endregion

#region NestedPrefabs
[Serializable]
public class NestedPrefab
{
    public Prefab origPrefab;
    public List<Prefab> prefab;
}
#endregion

#region Effects
[Serializable]
public class Effects
{
    public CamData camZoom;//zoom

    public Effects(List<Zoom> zoom, List<Pos> pos, List<Rot> rot)
    {
        camZoom = new CamData
        {
            zoom = zoom,
            pos = pos,
            rot = rot
        };
    }
}

[Serializable]
public class CamData
{
    public List<Zoom> zoom;
    public List<Pos> pos;
    public List<Rot> rot;
    public List<Clr> clr;
}
#endregion

#region Class Events
public interface MarkerData { }
[Serializable]
public class Pos : MarkerData
{
    public float t; // time
    public VectorRandomType r;// type of random
    public float sx;// start x
    public float sy;// start y
    public float ex;// end x
    public float ey;// end y
    public float i; // internal

    public Pos(float t, VectorRandomType r, float sx, float sy, float ex = 0, float ey = 0, float i = 0)
    {
        this.t = t;
        this.r = r;
        this.sx = sx;
        this.sy = sy;
        this.ex = ex;
        this.ey = ey;
        this.i = i;
    }
}
[Serializable]
public class Sca : MarkerData
{
    public float t; // time
    public VectorRandomType r;// type of random
    public float sx;// start x
    public float sy;// start y
    public float ex;// end x
    public float ey;// end y
    public float i; // internal

    public Sca(float t, VectorRandomType r, float sx, float sy, float ex = 0, float ey = 0, float i = 0)
    {
        this.t = t;
        this.r = r;
        this.sx = sx;
        this.sy = sy;
        this.ex = ex;
        this.ey = ey;
        this.i = i;
    }
}
[Serializable]
public class Rot : MarkerData
{
    public float t; // time
    public FloatRandomType r;// type of random
    public float sa;// start angle
    public float ea;// end angle
    public float i; // internal

    public Rot(float t, FloatRandomType r, float sa, float ea = 0, float i = 0)
    {
        this.t = t;
        this.r = r;
        this.sa = sa;
        this.ea = ea;
        this.i = i;
    }
}
[Serializable]
public class Clr : MarkerData
{
    public float t; // time
    public ColorRandomType r;// type of random
    public float sr;// start r
    public float sg;// start g
    public float sb;// start b
    public float sa;// start a
    public float er;// end r
    public float eg;// end g
    public float eb;// end b
    public float ea;// end a
    public float i; // internal

    public Clr(float t, ColorRandomType r, float sr, float sg, float sb, float sa, float er = 0, float eg = 0, float eb = 0, float ea = 0, float i = 0)
    {
        this.t = t;
        this.r = r;
        this.sr = sr;
        this.sg = sg;
        this.sb = sb;
        this.sa = sa;
        this.er = er;
        this.eg = eg;
        this.eb = eb;
        this.ea = ea;
        this.i = i;
    }
}

public enum SpriteType
{
    None = -1, Square = 0, SquareBO = 1, SquareO = 2,
    Circle = 3, CircleBO = 4, CircleO = 5,
    HalfCircle = 6, HalfCircleBO = 7, HalfCircleO = 8,
    QuarterCircle = 9, QuarterCircleBO = 10, QuarterCircleO = 11,
    EighthCircle = 12, EighthCircleBO = 13, EighthCircleO = 14,
    Triangle = 15, TriangleBO = 16, TriangleO = 17,
    RightTriangle = 18, RightTriangleBO = 19, RightTriangleO = 20,
    Hexagon = 21, HexagonBO = 22, HexagonO = 23,
    HalfHexagon = 24, HalfHexagonBO = 25, HalfHexagonO = 26,
    Arrow = 27, CroppedArrow = 28
}
public enum AnchorPresets
{
    Left_Top = 0, Center_Top = 1, Right_Top = 2,
    Left_Middle = 3, Center_Middle = 4, Right_Middle = 5,
    Left_Bottom = 6, Center_Bottom = 7, Right_Bottom = 8
}
public enum VectorRandomType
{
    N = 0,// NONE - No random (2 nums)
    MM = 1,// MINMAX - Standard Random (4 nums + 1 num (internal))
    IMM = 2,// INSTANTMINMAX - Standard Random between 2 vectors (4 nums)
    C = 3,// CIRCLE - Circle Random (2 nums (pos) + 2 nums (random angle) + power)
    M = 4 // MULTIPLY - Random multiply (2 nums (pos) + 2 nums (random multiply num))
}
public enum FloatRandomType
{
    N = 0,// NONE - No random (1 num)
    MM = 1,// MINMAX - Standard Random (2 nums + 1 num (internal))
    IMM = 2,// INSTANTMINMAX - Standard Random between 2 floats (2 nums)
    M = 3 // MULTIPLY - Random multiply (1 nums (angle) + 2 nums (random multiply num))
}
public enum ColorRandomType
{
    N = 0,// NONE - No random (4 nums)
    MM = 1,// MINMAX - Standard Random (8 nums + 1 num (internal))
    IMM = 2 // INSTANTMINMAX - Standard Random between 2 vectors (8 nums)
}

//Effects
[Serializable]
public class Zoom
{
    public float t;
    public float s;//size

    public Zoom(float t, float s)
    {
        this.t = t;
        this.s = s;
    }
}
#endregion
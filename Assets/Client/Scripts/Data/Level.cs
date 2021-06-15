using System.Collections.Generic;
using System.Collections;
using UnityEngine.Events;
using UnityEngine;
using System;

#region Data
/// <summary>
/// Класс для унификации всех классов с данными
/// </summary>
public interface IData
{
    public string GetParameter(byte index);
}
#endregion

#region Level
/// Это класс нужно сохранять в JSON
/// Основной класс, хранящиц всю информацию об уровне
[Serializable]
public class Level : IData
{
    public static int CURRENT_VERSION = 1;

    [SerializeField] private LevelData ld;// Базовая информация об уровне (level data)
    [SerializeField] private List<Marker> m;// Маркеры для редактора (markers)
    [SerializeField] private List<Checkpoint> c;// Сохранения игрока на уровне (checkpoints)
    [SerializeField] private List<Prefab> p;// Все объекты на сцене (prefabs)
    [SerializeField] private List<EditorPrefab> ep;// Объекты редактора (editor prefabs)
    //[SerializeField] private Effects e;// Эффекты в игре (effects)

    public LevelData LevelData { get { return ld; } set { ld = value; } }
    public List<Marker> Markers { get { return m; } set { m = value; } }
    public List<Checkpoint> Checkpoints { get { return c; } set { c = value; } }
    public List<Prefab> Prefabs { get { return p; } set { p = value; } }
    public List<EditorPrefab> EditorPrefabs { get { return ep; } set { ep = value; } }
    //public Effects Effects { get { return e; } set { e = value; } }

    public Level(LevelData level_data, 
        List<Marker> markers, 
        List<Checkpoint> checkpoints, 
        List<Prefab> prefabs, 
        List<EditorPrefab> editor_prefabs)//, 
        //Effects effects)
    {
        ld = level_data;
        m = markers;
        c = checkpoints;
        p = prefabs;
        ep = editor_prefabs;
        //e = effects;
    }

    public Level()
    {

    }

    public string GetParameter(byte index)
    {
        switch (index)
        {
            case 0: return ld.ToString();
            case 1: return m.ToString();
            case 2: return c.ToString();
            case 3: return p.ToString();
            case 4: return ep.ToString();
            //case 5: return e.ToString();
        }
        return null;
    }
}
#endregion

#region LevelData
/// <summary>
/// Базовая информация об уровне
/// <summary>
[Serializable]
public class LevelData : IData
{
    [SerializeField] private string ln;// Название уровня (level name)
    [SerializeField] private int ev;// Версия редактора (editor version)
    [SerializeField] private string mt;// Название трека (music title)
    [SerializeField] private string ma;// Автор трека (music author)
    [SerializeField] private string la;// Автор уровня (level author)
    [SerializeField] private float sfo;// Начало убывания музыки (start fade out)
    [SerializeField] private float efo;// Конец убывания музыки (end fade out)

    public string LevelName { get { return ln; } set { ln = value; } }
    public int EditorVersion { get { return ev; } set { ev = value; } }
    public string MusicTitle { get { return mt; } set { mt = value; } }
    public string MusicAuthor { get { return ma; } set { ma = value; } }
    public string LevelAuthor { get { return la; } set { la = value; } }
    public float StartFadeOut { get { return sfo; } set { sfo = value; } }
    public float EndFadeOut { get { return efo; } set { efo = value; } }

    public LevelData(string level_name, 
        int editor_version, 
        string music_title, 
        string music_author, 
        string level_author,
        float start_fade_out, 
        float end_fade_out)
    {
        ln = level_name;
        ev = editor_version;
        mt = music_title;
        ma = music_author;
        la = level_author;
        sfo = start_fade_out;
        efo = end_fade_out;
    }

    public LevelData()
    {

    }

    public string GetParameter(byte index)
    {
        switch (index)
        {
            case 0: return ln.ToString();
            case 1: return ev.ToString();
            case 2: return mt.ToString();
            case 3: return ma.ToString();
            case 4: return sfo.ToString();
            case 5: return efo.ToString();
        }
        return null;
    }
}
#endregion

#region Markers
/// <summary>
/// Маркеры для редактора
/// <summary>
[Serializable]
public class Marker : IData
{
    [SerializeField] private string n;// Имя маркера (name)
    [SerializeField] private string d;// Описание маркера (description)
    [SerializeField] private int f;// На каком моменте времени находиться метка по кадрам (frame)
    [SerializeField] private float r;// Цвет маркера (красное) (red)
    [SerializeField] private float g;// Цвет маркера (синее) (green)
    [SerializeField] private float b;// Цвет маркера (зелёное) (blue)

    public string Name { get { return n; } set { n = value; } }
    public string Description { get { return d; } set { d = value; } }
    public int Frame { get { return f; } set { f = value; } }
    public float Red { get { return r; } set { r = value; } }
    public float Green { get { return g; } set { g = value; } }
    public float Blue { get { return b; } set { b = value; } }

    public Marker(string name, 
        string description, 
        int frame,
        float red,
        float green,
        float blue)
    {
        n = name;
        d = description;
        f = frame;
        r = red;
        g = green;
        b = blue;
    }

    public Marker()
    {

    }

    public string GetParameter(byte index)
    {
        switch (index)
        {
            case 0: return n.ToString();
            case 1: return d.ToString();
            case 2: return f.ToString();
            case 3: return r.ToString();
            case 4: return g.ToString();
            case 5: return b.ToString();
        }
        return null;
    }
}
#endregion

#region Checkpoint
/// <summary>
/// Сохранение игрока на уровне
/// <summary>
[Serializable]
public class Checkpoint : IData
{
    [SerializeField] private bool a;// Активность чекпоинта (active)
    [SerializeField] private string n;// Название чекпоинта (name)
    [SerializeField] private int f;// На каком моменте времени находиться чекпоинт по кадрам (frame)
    [SerializeField] private VectorRandomType r;// Тип рандома у позиции (type of random)
    // Параметры позиции
    [SerializeField] private float sx;// start x
    [SerializeField] private float sy;// start y
    [SerializeField] private float ex;// end x
    [SerializeField] private float ey;// end y
    [SerializeField] private float i; // internal

    public bool Active { get { return a; } set { a = value; } }
    public string Name { get { return n; } set { n = value; } }
    public int Frame { get { return f; } set { f = value; } }
    public VectorRandomType RandomType { get { return r; } set { r = value; } }
    public float SX { get { return sx; } set { sx = value; } }
    public float SY { get { return sy; } set { sy = value; } }
    public float EX { get { return ex; } set { ex = value; } }
    public float EY { get { return ey; } set { ey = value; } }
    public float Interval { get { return i; } set { i = value; } }

    public Checkpoint(bool active,
        string name,
        int frame,
        VectorRandomType random_type,
        float sx, float sy,
        float ex = 0, float ey = 0,
        float i = 0)
    {
        a = active;
        n = name;
        f = frame;
        r = random_type;

        this.sx = sx;
        this.sy = sy;
        this.ex = ex;
        this.ey = ey;
        this.i = i;
    }

    public Checkpoint()
    {

    }

    public string GetParameter(byte index)
    {
        switch (index)
        {
            case 0: return a.ToString();
            case 1: return n.ToString();
            case 2: return f.ToString();
            case 3: return r.ToString();
            case 4: return sx.ToString();
            case 5: return sy.ToString();
            case 6: return ex.ToString();
            case 7: return ey.ToString();
            case 8: return i.ToString();
        }
        return null;
    }
}
#endregion

#region Prefab
/// <summary>
/// Объекты в уровне с данными для игры
/// <summary>
[Serializable]
public class Prefab : IData
{
    [SerializeField] private string n;// Имя у объекта (name)
    [SerializeField] private bool a;// Активность объекта (active)
    [SerializeField] private bool c;// Наличие коллайдера у объекта (collider)
    [SerializeField] private List<Pos> pos;// Список с метками позиции (position)
    [SerializeField] private List<Sca> sca;// Список с метками размера (scale)
    [SerializeField] private List<Rot> rot;// Список с метками поворота (rotation)
    [SerializeField] private List<Clr> clr;// Список с метками цветов (color)
    [SerializeField] private SpriteType st;// Тип спрайта у объекта (sprite type)
    [SerializeField] private int sf;// На этом моменте времени создаёться объект (start frame)
    [SerializeField] private int ef;// На этом моменте времени удаляеться объект (end frame)
    [SerializeField] private AnchorPresets a2;// Якорь объекта (anchor)
    [SerializeField] private int id, idp;// ID объекта и его родителя (id, idp)
    [SerializeField] private int l;// Слой на котором находиться объект, относительно других (layer) (выше или ниже)
    [SerializeField] private int h;//На каком потоке (в редакторе) находиться объект (height)

    public string Name { get { return n; } set { n = value; } }
    public bool Active { get { return a; } set { a = value; } }
    public bool Collider { get { return c; } set { c = value; } }
    public List<Pos> Pos { get { return pos ?? new List<Pos>(); ; } set { pos = value; } }
    public List<Sca> Sca { get { return sca; } set { sca = value; } }
    public List<Rot> Rot { get { return rot; } set { rot = value; } }
    public List<Clr> Clr { get { return clr; } set { clr = value; } }
    public SpriteType SpriteType { get { return st; } set { st = value; } }
    public int StartFrame { get { return sf; } set { sf = value; } }
    public int EndFrame { get { return ef; } set { ef = value; } }
    public AnchorPresets Anchor { get { return a2; } set { a2 = value; } }
    public int ID { get { return id; } set { id = value; } }
    public int ParentID { get { return idp; } set { idp = value; } }
    public int Layer { get { return l; } set { l = value; } }
    public int Height { get { return h; } set { h = value; } }
    public int GetFrameLength { get { return ef - sf; } }

    public Prefab(string name, 
        bool active, 
        bool collider, 
        List<Pos> pos, 
        List<Sca> sca, 
        List<Rot> rot, 
        List<Clr> clr, 
        SpriteType st,
        int startFrame, 
        int endFrame,
        AnchorPresets anchor,
        int id, int idParent,
        int layer, 
        int height)
    {
        n = name;
        a = active;
        c = collider;
        this.pos = pos;
        this.sca = sca;
        this.rot = rot;
        this.clr = clr;

        this.st = st;
        sf = startFrame;
        ef = endFrame;

        idp = idParent;
        this.id = id;

        a2 = anchor;
        h = height;
        l = layer;
    }

    public Prefab()
    {

    }

    public string GetParameter(byte index)
    {
        switch (index)
        {
            case 0: return n.ToString();
            case 1: return (ef - sf).ToString();
            case 2: return a.ToString();
            case 3: return c.ToString();
            case 4: return pos.ToString();
            case 5: return sca.ToString();
            case 6: return rot.ToString();
            case 7: return clr.ToString();
            case 8: return st.ToString();
            case 9: return sf.ToString();
            case 10: return ef.ToString();
            case 11: return idp.ToString();
            case 12: return id.ToString();
            case 13: return a2.ToString();
            case 14: return h.ToString();
            case 15: return l.ToString();
        }
        return null;
    }
}
#endregion

#region Классы, хранящие данные об объекте
/// <summary>
/// Интерфейс для работы общих методов
/// <summary>
public interface MarkerData { }

[Serializable]
public struct Pos : MarkerData, IData
{
    [SerializeField] private int f; // Глобальная метка во времени по кадрам (frame)
    [SerializeField] private VectorRandomType r;// Тип рандома у позиции (type of random)
    [SerializeField] private EasingType e;// Тип функции (easing)
    // Параметры позиции
    [SerializeField] private float sx;// start x
    [SerializeField] private float sy;// start y
    [SerializeField] private float ex;// end x
    [SerializeField] private float ey;// end y
    [SerializeField] private float i; // internal

    public int Frame { get { return f; } set { f = value; } }
    public VectorRandomType RandomType { get { return r; } set { r = value; } }
    public EasingType Easing { get { return e; } set { e = value; } }
    public float SX { get { return sx; } set { sx = value; } }
    public float SY { get { return sy; } set { sy = value; } }
    public float EX { get { return ex; } set { ex = value; } }
    public float EY { get { return ey; } set { ey = value; } }
    public float Interval { get { return i; } set { i = value; } }

    public Pos(int frame, 
        VectorRandomType random_type, 
        EasingType easing, 
        float sx, float sy, 
        float ex = 0, float ey = 0, 
        float i = 0)
    {
        f = frame;
        r = random_type;
        e = easing;
        this.sx = sx;
        this.sy = sy;
        this.ex = ex;
        this.ey = ey;
        this.i = i;
    }

    public string GetParameter(byte index)
    {
        switch (index)
        {
            case 0: return f.ToString();
            case 1: return r.ToString();
            case 2: return e.ToString();
            case 3: return sx.ToString();
            case 4: return sy.ToString();
            case 5: return ex.ToString();
            case 6: return ey.ToString();
            case 7: return i.ToString();
        }
        return null;
    }
}

[Serializable]
public struct Sca : MarkerData, IData
{
    [SerializeField] private int f; // Глобальная метка во времени по кадрам (frame)
    [SerializeField] private VectorRandomType r;// Тип рандома у размера (type of random)
    [SerializeField] private EasingType e;// Тип функции (easing)
    // Параметры размера
    [SerializeField] private float sx;// start x
    [SerializeField] private float sy;// start y
    [SerializeField] private float ex;// end x
    [SerializeField] private float ey;// end y
    [SerializeField] private float i; // internal

    public int Frame { get { return f; } set { f = value; } }
    public VectorRandomType RandomType { get { return r; } set { r = value; } }
    public EasingType Easing { get { return e; } set { e = value; } }
    public float SX { get { return sx; } set { sx = value; } }
    public float SY { get { return sy; } set { sy = value; } }
    public float EX { get { return ex; } set { ex = value; } }
    public float EY { get { return ey; } set { ey = value; } }
    public float Interval { get { return i; } set { i = value; } }

    public Sca(int frame,
        VectorRandomType random_type,
        EasingType easing,
        float sx, float sy,
        float ex = 0, float ey = 0,
        float i = 0)
    {
        f = frame;
        r = random_type;
        e = easing;
        this.sx = sx;
        this.sy = sy;
        this.ex = ex;
        this.ey = ey;
        this.i = i;
    }

    public string GetParameter(byte index)
    {
        switch (index)
        {
            case 0: return f.ToString();
            case 1: return r.ToString();
            case 2: return e.ToString();
            case 3: return sx.ToString();
            case 4: return sy.ToString();
            case 5: return ex.ToString();
            case 6: return ey.ToString();
            case 7: return i.ToString();
        }
        return null;
    }
}

[Serializable]
public struct Rot : MarkerData, IData
{
    [SerializeField] private int f; // Глобальная метка во времени по кадрам (frame)
    [SerializeField] private FloatRandomType r;// Тип рандома у угла (type of random)
    [SerializeField] private EasingType e;// Тип функции (easing)
    // Параметры угла
    [SerializeField] private float sa;// start angle
    [SerializeField] private float ea;// end angle
    [SerializeField] private float i; // internal

    public int Frame { get { return f; } set { f = value; } }
    public FloatRandomType RandomType { get { return r; } set { r = value; } }
    public EasingType Easing { get { return e; } set { e = value; } }
    public float SA { get { return sa; } set { sa = value; } }
    public float EA { get { return ea; } set { ea = value; } }
    public float Interval { get { return i; } set { i = value; } }

    public Rot(int frame,
        FloatRandomType random_type,
        EasingType easing,
        float sa, float ea = 0,
        float i = 0)
    {
        f = frame;
        r = random_type;
        e = easing;
        this.sa = sa;
        this.ea = ea;
        this.i = i;
    }

    public string GetParameter(byte index)
    {
        switch (index)
        {
            case 0: return f.ToString();
            case 1: return r.ToString();
            case 2: return e.ToString();
            case 3: return sa.ToString();
            case 4: return ea.ToString();
            case 5: return i.ToString();
        }
        return null;
    }
}

[Serializable]
public struct Clr : MarkerData, IData
{
    [SerializeField] private int f; // Глобальная метка во времени по кадрам (frame)
    [SerializeField] private ColorRandomType r;// Тип рандома у угла (type of random)
    [SerializeField] private EasingType e;// Тип функции (easing)
    // Параметры угла
    [SerializeField] private float sr;// start r
    [SerializeField] private float sg;// start g
    [SerializeField] private float sb;// start b
    [SerializeField] private float sa;// start a
    [SerializeField] private float er;// end r
    [SerializeField] private float eg;// end g
    [SerializeField] private float eb;// end b
    [SerializeField] private float ea;// end a
    [SerializeField] private float i; // internal

    public int Frame { get { return f; } set { f = value; } }
    public ColorRandomType RandomType { get { return r; } set { r = value; } }
    public EasingType Easing { get { return e; } set { e = value; } }
    public float SR { get { return sr; } set { sr = value; } }
    public float SG { get { return sg; } set { sg = value; } }
    public float SB { get { return sb; } set { sb = value; } }
    public float SA { get { return sa; } set { sa = value; } }
    public float ER { get { return er; } set { er = value; } }
    public float EG { get { return eg; } set { eg = value; } }
    public float EB { get { return eb; } set { eb = value; } }
    public float EA { get { return ea; } set { ea = value; } }
    public float Interval { get { return i; } set { i = value; } }

    public Clr(int frame,
        ColorRandomType random_type,
        EasingType easing, 
        float sr, float sg, float sb, float sa, 
        float er = 0, float eg = 0, float eb = 0, float ea = 0, 
        float i = 0)
    {
        f = frame;
        r = random_type;
        e = easing;
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

    public string GetParameter(byte index)
    {
        switch (index)
        {
            case 0: return f.ToString();
            case 1: return r.ToString();
            case 2: return e.ToString();
            case 3: return sr.ToString();
            case 4: return sg.ToString();
            case 5: return sb.ToString();
            case 6: return sa.ToString();
            case 7: return er.ToString();
            case 8: return eg.ToString();
            case 9: return eb.ToString();
            case 10: return ea.ToString();
            case 11: return i.ToString();
        }
        return null;
    }
}
#endregion

#region EditorPrefab
[Serializable]
public class EditorPrefab : IData
{
    [SerializeField] private List<Prefab> p;//Объекты, находяящиеся в наследственности с главный объектом (главный объект на 0 позиции)

    public List<Prefab> Prefabs { get { return p; } set { p = value; } }
    public int CountObjects { get { return p.Count; } }
    public int GetFrameLength { get { return p[0].EndFrame - p[0].StartFrame; } }

    public EditorPrefab(List<Prefab> prefabs)
    {
        p = prefabs;
    }

    public EditorPrefab()
    {

    }

    public string GetParameter(byte index)
    {
        switch (index)
        {
            case 0: return p[0].ToString();
            case 1: return p.Count.ToString();
            case 2: return (p[0].EndFrame - p[0].StartFrame).ToString();
        }
        return null;
    }
}

public class ListEditorPrefab : IData
{
    [SerializeField] private List<EditorPrefab> p;

    public List<EditorPrefab> EditorPrefabs { get { return p; } set { p = value; } }
    public int CountObjects { get { return p.Count; } }

    public ListEditorPrefab(List<EditorPrefab> prefabs)
    {
        p = prefabs;
    }

    public ListEditorPrefab()
    {
        p = new List<EditorPrefab>();
    }

    public void Add(EditorPrefab prefab) { p.Add(prefab); }
    public void Remove(EditorPrefab prefab) { p.Remove(prefab); }
    public void RemoveAt(int index) { p.RemoveAt(index); }
    public string GetParameter(byte index)
    {
        switch (index)
        {
            case 0: return p[0].ToString();
            case 1: return p.Count.ToString();
        }
        return null;
    }
}
#endregion

#region Эффекты
[Serializable]
public class Effects
{
    [SerializeField] private CamData camZoom;//zoom

    public Effects(CamData camZoom)
    {
        this.camZoom = camZoom;
    }
}

[Serializable]
public class CamData
{
    [SerializeField] private List<Zoom> zoom;
    [SerializeField] private List<Pos> pos;
    [SerializeField] private List<Rot> rot;

    public CamData(List<Zoom> zoom, List<Pos> pos, List<Rot> rot)
    {
        this.zoom = zoom;
        this.pos = pos;
        this.rot = rot;
    }
}
#endregion

#region Классы эффектов
[Serializable]
public class Zoom
{
    [SerializeField] private float t;
    [SerializeField] private float s;//size

    public Zoom(float t, float s)
    {
        this.t = t;
        this.s = s;
    }
}
#endregion

#region Конвертер
public static class LevelConverter
{
    public static Converter<EditorPrefab, IData> EditorPrefabsConverter = 
        new Converter<EditorPrefab, IData>((EditorPrefab p) => { return p; });
    public static Converter<ListEditorPrefab, IData> ListEditorPrefabsConverter =
        new Converter<ListEditorPrefab, IData>((ListEditorPrefab p) => { return p; });
    public static Converter<Marker, IData> MarkersConverter =
        new Converter<Marker, IData>((Marker p) => { return p; });
    public static Converter<Checkpoint, IData> CheckpointsConverter =
        new Converter<Checkpoint, IData>((Checkpoint p) => { return p; });
    public static Converter<Prefab, IData> PrefabsConverter =
        new Converter<Prefab, IData>((Prefab p) => { return p; });
}
#endregion

#region Типы объектов
// Все возможные спрайты
public enum SpriteType
{
    None = 0, 
    Square = 1, SquareBO = 2, SquareO = 3,
    Circle = 4, CircleBO = 5, CircleO = 6,
    HalfCircle = 7, HalfCircleBO = 8, HalfCircleO = 9,
    QuarterCircle = 10, QuarterCircleBO = 11, QuarterCircleO = 12,
    EighthCircle = 13, EighthCircleBO = 14, EighthCircleO = 15,
    Triangle = 16, TriangleBO = 17, TriangleO = 18,
    RightTriangle = 19, RightTriangleBO = 20, RightTriangleO = 21,
    Hexagon = 22, HexagonBO = 23, HexagonO = 24,
    HalfHexagon = 25, HalfHexagonBO = 26, HalfHexagonO = 27,
    Arrow = 28, CroppedArrow = 29
}
// Место, от какой части экрана расчитывается позиция
// необходимо для local и global позиции, а также так как
// экраны телефонов бывают разными, а поэтому необходим инструмент
// для большей кастомизации позиции объектов
public enum AnchorPresets
{
    Left_Top = 0, Center_Top = 1, Right_Top = 2,
    Left_Middle = 3, Center_Middle = 4, Right_Middle = 5,
    Left_Bottom = 6, Center_Bottom = 7, Right_Bottom = 8
}
// Типы рандома для координат
public enum VectorRandomType//5
{
    N = 0,// NONE - без рандома (2 числа)
    IMM = 1,// INSTANTMINMAX - рандом (4 числа)
    MM = 2,// MINMAX - рандом с интервалом (4 числа + 1 число (интервал))
    C = 3,// CIRCLE - рандомная точка в круге (2 числа (стартовая позиция) + 2 числа (между ними выбирается рандомный угол) + дальность от центра)
    M = 4 // MULTIPLY - рандомная точка с умножением (2 числа (стартовая позиция) + 2 числа (между ними выбирается число))
}
// Типы рандома для числа
public enum FloatRandomType//4
{
    N = 0,// NONE - без рандома (1 число)
    IMM = 1,// INSTANTMINMAX - рандом (2 числа)
    MM = 2,// MINMAX - рандом с интервалом (2 числа + 1 число (интервал между ними))
    M = 3 // MULTIPLY - рандомное умножение (1 число (угол) + 2 числа (рандомная сила))
}
// Типы рандома для цвета
public enum ColorRandomType//3
{
    N = 0,// NONE - без рандома (4 числа)
    IMM = 1,// INSTANTMINMAX - рандом (8 чисел)
    MM = 2 // MINMAX - рандом с интервалом (8 чисел + 1 число (internal))
}
// Типы функций плавности
public enum EasingType
{
    Linear = 0, Constant = 1,// без изменений
    InSine = 2, OutSine = 3, InOutSine = 4,// синусовые функции
    InQuad = 5, OutQuad = 6, InOutQuad = 7,// 2-степенные функции (квадрат)
    InCubic = 8, OutCubic = 9, InOutCubic = 10,// 3-степенные функции (куб)
    InQuart = 11, OutQuart = 12, InOutQuart = 13,// 4-степенные функции (тессеракт)
    InQuint = 14, OutQuint = 15, InOutQuint = 16,// 5-степенные функции
    InExpo = 17, OutExpo = 18, InOutExpo = 19,// экспанинциальные функции
    InCirc = 20, OutCirc = 21, InOutCirc = 22,// круговые функции
    InBack = 23, OutBack = 24, InOutBack = 25,// инерциальные функции
    InElastic = 26, OutElastic = 27, InOutElastic = 28// эластичные функции
}
#endregion

#region Остальное
public class Second : IData
{
    private float s;//Секунда

    public float Seconds { get { return s; } set { s = value; } }

    public Second(float seconds)
    {
        s = seconds;
    }

    public string GetParameter(byte index)
    {
        return s.ToString();
    }
}
#endregion
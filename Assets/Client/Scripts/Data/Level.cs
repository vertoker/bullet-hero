using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;


#region Основной класс сохранения
/// Это класс можно сохранить в JSON
/// Основной класс, хранящиц всю информацию об уровне
[Serializable]
public class Level
{
    static int CURRENT_VERSION = 1;

    public LevelData data;// Базовая информация об уровне
    public List<Marker> markers;// Маркеры для редактора
    public List<Checkpoint> checkpoints;// Сохранение игрока на уровне
    public List<Prefab> prefabs;// 
    public List<GameEvent> gameEvents;// 
    public List<EditorPrefab> editorPrefabs;// 
    public Effects effects;// 

    public Level(LevelData data, List<Marker> markers, List<Checkpoint> checkpoints, List<Prefab> prefabs, List<GameEvent> gameEvents, List<EditorPrefab> editorPrefabs, Effects effects)
    {
        this.data = data;
        this.markers = markers;
        this.checkpoints = checkpoints;
        
        this.prefabs = prefabs;
        this.editorPrefabs = editorPrefabs;
        this.gameEvents = gameEvents;
        this.effects = effects;
    }
}
#endregion

#region LevelData
// Базовая информация об уровне
[Serializable]
public class LevelData
{
    public string levelName;
    public int editor_version;//editor version
    public string music_title;
    public string music_author;
    public string level_author;

    public int threadCount;
    public float startFadeOut;
    public float endFadeOut;

    public LevelData(string levelName, int editor_version, string music_title, string music_author, string level_author, int threadCount, float startFadeOut, float endFadeOut)
    {
        this.levelName = levelName;
        this.editor_version = editor_version;
        this.music_title = music_title;
        this.music_author = music_author;
        this.level_author = level_author;

        this.threadCount = threadCount;
        this.startFadeOut = startFadeOut;
        this.endFadeOut = endFadeOut;
    }
}
#endregion

#region Markers
// Маркеры для редактора
[Serializable]
public class Marker
{
    public int groupID;
    public string name;
    public string description;
    public float t;

    public Marker(int groupID, string name, string description, float t)
    {
        this.groupID = groupID;
        this.description = description;
        this.name = name;
        this.t = t;
    }
}
#endregion

#region Checkpoint
// Сохранение игрока на уровне
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
//Объекты в уровне с данными для игры
[Serializable]
public class Prefab
{
    public string name;// Имя у объекта
    public bool active;// Активность объекта
    public bool collider;// Наличие коллайдера у объекта
    public List<Pos> pos;// Список с метками позиции
    public List<Sca> sca;// Список с метками размера
    public List<Rot> rot;// Список с метками поворота
    public List<Clr> clr;// Список с метками цветов
    public SpriteType st;// Тип спрайта у объекта
    public int startFrame;// На этом моменте времени создавать объект
    public int frameTimeExist;// Столько объект должен существовать на сцене
    public int endFrame;// На этом моменте времени убирать объект
    public AnchorPresets pivot;// Якорь объекта
    public int id, idParent;// ID объекта и его родителя
    public int layer;// 
    public int height;//x
    public int thread;//z

    public Prefab(string name, bool active, bool collider, List<Pos> pos, List<Sca> sca, List<Rot> rot, List<Clr> clr, SpriteType st,
        int startFrame, int frameTimeExist, int endFrame, int idParent, int id, AnchorPresets pivot, int thread, int height, int layer)
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
        this.endFrame = endFrame;

        this.idParent = idParent;
        this.id = id;

        this.pivot = pivot;
        this.height = height;
        this.thread = thread;
        this.layer = layer;
    }
}
#endregion

#region Классы, хранящие данные об объекте
// Интерфейс для работы общих методов
public interface MarkerData { }
[Serializable]
// Класс для хранения позиции
public struct Pos : MarkerData
{
    public float t; // time
    public VectorRandomType r;// type of random
    public AnchorPresets a;// Якорь экрана
    public EasingType eas;// Тип функции
    public float sx;// start x
    public float sy;// start y
    public float ex;// end x
    public float ey;// end y
    public float i; // internal

    public Pos(float t, VectorRandomType r, AnchorPresets a, EasingType eas, float sx, float sy, float ex = 0, float ey = 0, float i = 0)
    {
        this.t = t;
        this.r = r;
        this.a = a;
        this.eas = eas;
        this.sx = sx;
        this.sy = sy;
        this.ex = ex;
        this.ey = ey;
        this.i = i;
    }
}
[Serializable]
public struct Sca : MarkerData
{
    public float t; // time
    public VectorRandomType r;// type of random
    public EasingType eas;// Тип функции
    public float sx;// start x
    public float sy;// start y
    public float ex;// end x
    public float ey;// end y
    public float i; // internal

    public Sca(float t, VectorRandomType r, EasingType eas, float sx, float sy, float ex = 0, float ey = 0, float i = 0)
    {
        this.t = t;
        this.r = r;
        this.eas = eas;
        this.sx = sx;
        this.sy = sy;
        this.ex = ex;
        this.ey = ey;
        this.i = i;
    }
}
[Serializable]
public struct Rot : MarkerData
{
    public float t; // time
    public FloatRandomType r;// type of random
    public EasingType eas;// Тип функции
    public float sa;// start angle
    public float ea;// end angle
    public float i; // internal

    public Rot(float t, FloatRandomType r, EasingType eas, float sa, float ea = 0, float i = 0)
    {
        this.t = t;
        this.r = r;
        this.eas = eas;
        this.sa = sa;
        this.ea = ea;
        this.i = i;
    }
}
[Serializable]
public struct Clr : MarkerData
{
    public float t; // time
    public ColorRandomType r;// type of random
    public EasingType eas;// Тип функции
    public float sr;// start r
    public float sg;// start g
    public float sb;// start b
    public float sa;// start a
    public float er;// end r
    public float eg;// end g
    public float eb;// end b
    public float ea;// end a
    public float i; // internal

    public Clr(float t, ColorRandomType r, EasingType eas, float sr, float sg, float sb, float sa, float er = 0, float eg = 0, float eb = 0, float ea = 0, float i = 0)
    {
        this.t = t;
        this.r = r;
        this.eas = eas;
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
#endregion

#region EditorPrefab
[Serializable]
public class EditorPrefab
{
    public List<Prefab> prefabs;

    public EditorPrefab(List<Prefab> prefabs)
    {
        this.prefabs = prefabs;
    }
}
#endregion

#region EditorPrefab
[Serializable]
public class GameEvent
{

}
#endregion

#region Эффекты
[Serializable]
public class Effects
{
    public CamData camZoom;//zoom

    public Effects(CamData camZoom)
    {
        this.camZoom = camZoom;
    }
}

[Serializable]
public class CamData
{
    public List<Zoom> zoom;
    public List<Pos> pos;
    public List<Rot> rot;

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
    public float t;
    public float s;//size

    public Zoom(float t, float s)
    {
        this.t = t;
        this.s = s;
    }
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
public enum VectorRandomType
{
    N = 0,// NONE - без рандома (2 числа)
    IMM = 1,// INSTANTMINMAX - рандом (4 числа)
    MM = 2,// MINMAX - рандом с интервалом (4 числа + 1 число (интервал))
    C = 3,// CIRCLE - рандомная точка в круге (2 числа (стартовая позиция) + 2 числа (между ними выбирается рандомный угол) + дальность от центра)
    M = 4 // MULTIPLY - рандомная точка с умножением (2 числа (стартовая позиция) + 2 числа (между ними выбирается число))
}
// Типы рандома для числа
public enum FloatRandomType
{
    N = 0,// NONE - без рандома (1 число)
    IMM = 1,// INSTANTMINMAX - рандом (2 числа)
    MM = 2,// MINMAX - рандом с интервалом (2 числа + 1 число (интервал между ними))
    M = 3 // MULTIPLY - рандомное умножение (1 число (угол) + 2 числа (рандомная сила))
}
// Типы рандома для цвета
public enum ColorRandomType
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

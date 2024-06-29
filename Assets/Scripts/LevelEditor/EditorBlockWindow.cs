using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class EditorBlockWindow : MonoBehaviour
{
    public BaseLevelEditor editor;
    [Header("Editor Left")]
    public RectTransform editorLeft;
    public RectTransform objectEditor;
    private ObjectBlock obj;

    public RectTransform createPrefab;
    public RectTransform createMarker;
    public RectTransform createCheckpoint;

    [Header("Editor Right")]
    public RectTransform editorRight;
    public RectTransform markerObjectEditor;
    private TMP_InputField time_IF;
    private Image[] imagesTime;
    private VectorBlock pos, scale;
    private FloatBlock rot;
    private ColorBlock color;

    public RectTransform threads;
    public RectTransform markerList;
    public RectTransform checkpointList;

    [Header("Editor Down")]
    public RectTransform editorDown;
    public RectTransform contentMarkers;
    public GameObject markerInstance;

    public Scrollbar scrollbarHorizontal;
    public RectTransform scrollView;
    public RectTransform secLine;
    public RectTransform secLineMarker;

    private Prefab prefab;
    private float secLength;
    private float secOffset;
    private float timelineLength;
    private float camAspect;
    private float width;
    private float halfScreen;
    private string actType;
    private int idMarker;

    public void Start()
    {
        #region Start Marker Object Editor
        time_IF = markerObjectEditor.GetChild(1).GetComponent<TMP_InputField>();
        imagesTime = new Image[]
        {
            markerObjectEditor.GetChild(2).GetChild(0).GetComponent<Image>(),
            markerObjectEditor.GetChild(2).GetChild(1).GetComponent<Image>(),
            markerObjectEditor.GetChild(2).GetChild(2).GetComponent<Image>(),
            markerObjectEditor.GetChild(2).GetChild(3).GetComponent<Image>(),
            markerObjectEditor.GetChild(2).GetChild(4).GetComponent<Image>()
        };

        //Pos
        RectTransform posTr = markerObjectEditor.GetChild(5).GetComponent<RectTransform>();
        RectTransform startPos = posTr.GetChild(2).GetComponent<RectTransform>();
        RectTransform endPos = posTr.GetChild(3).GetComponent<RectTransform>();
        RectTransform intervalPos = posTr.GetChild(4).GetComponent<RectTransform>();
        TMP_InputField[] inputFieldsPos = new TMP_InputField[]
        {
            markerObjectEditor.GetChild(1).GetComponent<TMP_InputField>(),
            startPos.GetChild(2).GetComponent<TMP_InputField>(),
            startPos.GetChild(3).GetComponent<TMP_InputField>(),
            endPos.GetChild(2).GetComponent<TMP_InputField>(),
            endPos.GetChild(3).GetComponent<TMP_InputField>(),
            intervalPos.GetChild(1).GetComponent<TMP_InputField>()
        };
        RectTransform butsTrPos = posTr.GetChild(1).GetComponent<RectTransform>();
        ButStandard[] butsPos = new ButStandard[]
        {
            new ButStandard(butsTrPos.GetChild(0).GetComponent<Image>(), butsTrPos.GetChild(0).GetChild(0).GetComponent<Image>()),
            new ButStandard(butsTrPos.GetChild(1).GetComponent<Image>(), butsTrPos.GetChild(1).GetChild(0).GetComponent<Image>()),
            new ButStandard(butsTrPos.GetChild(2).GetComponent<Image>(), butsTrPos.GetChild(2).GetChild(0).GetComponent<Image>()),
            new ButStandard(butsTrPos.GetChild(3).GetComponent<Image>(), butsTrPos.GetChild(3).GetChild(0).GetComponent<Image>()),
            new ButStandard(butsTrPos.GetChild(4).GetComponent<Image>(), butsTrPos.GetChild(4).GetChild(0).GetComponent<Image>())
        };

        //Scale
        RectTransform scaleTr = markerObjectEditor.GetChild(6).GetComponent<RectTransform>();
        RectTransform startScale = scaleTr.GetChild(2).GetComponent<RectTransform>();
        RectTransform endScale = scaleTr.GetChild(3).GetComponent<RectTransform>();
        RectTransform intervalScale = scaleTr.GetChild(4).GetComponent<RectTransform>();
        TMP_InputField[] inputFieldsScale = new TMP_InputField[]
        {
            markerObjectEditor.GetChild(1).GetComponent<TMP_InputField>(),
            startScale.GetChild(2).GetComponent<TMP_InputField>(),
            startScale.GetChild(3).GetComponent<TMP_InputField>(),
            endScale.GetChild(2).GetComponent<TMP_InputField>(),
            endScale.GetChild(3).GetComponent<TMP_InputField>(),
            intervalScale.GetChild(1).GetComponent<TMP_InputField>()
        };
        RectTransform butsTrScale = scaleTr.GetChild(1).GetComponent<RectTransform>();
        ButStandard[] butsScale = new ButStandard[]
        {
            new ButStandard(butsTrScale.GetChild(0).GetComponent<Image>(), butsTrScale.GetChild(0).GetChild(0).GetComponent<Image>()),
            new ButStandard(butsTrScale.GetChild(1).GetComponent<Image>(), butsTrScale.GetChild(1).GetChild(0).GetComponent<Image>()),
            new ButStandard(butsTrScale.GetChild(2).GetComponent<Image>(), butsTrScale.GetChild(2).GetChild(0).GetComponent<Image>()),
            new ButStandard(butsTrScale.GetChild(3).GetComponent<Image>(), butsTrScale.GetChild(3).GetChild(0).GetComponent<Image>()),
            new ButStandard(butsTrScale.GetChild(4).GetComponent<Image>(), butsTrScale.GetChild(4).GetChild(0).GetComponent<Image>())
        };

        //Rot
        RectTransform rotTr = markerObjectEditor.GetChild(7).GetComponent<RectTransform>();
        RectTransform startRot = rotTr.GetChild(2).GetComponent<RectTransform>();
        RectTransform endRot = rotTr.GetChild(3).GetComponent<RectTransform>();
        RectTransform intervalRot = rotTr.GetChild(4).GetComponent<RectTransform>();
        TMP_InputField[] inputFieldsRot = new TMP_InputField[]
        {
            markerObjectEditor.GetChild(1).GetComponent<TMP_InputField>(),
            startRot.GetChild(1).GetComponent<TMP_InputField>(),
            endRot.GetChild(1).GetComponent<TMP_InputField>(),
            intervalRot.GetChild(1).GetComponent<TMP_InputField>()
        };
        RectTransform butsTrRot = rotTr.GetChild(1).GetComponent<RectTransform>();
        ButStandard[] butsRot = new ButStandard[]
        {
            new ButStandard(butsTrRot.GetChild(0).GetComponent<Image>(), butsTrRot.GetChild(0).GetChild(0).GetComponent<Image>()),
            new ButStandard(butsTrRot.GetChild(1).GetComponent<Image>(), butsTrRot.GetChild(1).GetChild(0).GetComponent<Image>()),
            new ButStandard(butsTrRot.GetChild(2).GetComponent<Image>(), butsTrRot.GetChild(2).GetChild(0).GetComponent<Image>()),
            new ButStandard(butsTrRot.GetChild(3).GetComponent<Image>(), butsTrRot.GetChild(3).GetChild(0).GetComponent<Image>())
        };

        //Color
        RectTransform colorTr = markerObjectEditor.GetChild(8).GetComponent<RectTransform>();
        RectTransform startColor = colorTr.GetChild(2).GetComponent<RectTransform>();
        RectTransform endColor = colorTr.GetChild(3).GetComponent<RectTransform>();
        RectTransform intervalColor = colorTr.GetChild(4).GetComponent<RectTransform>();
        TMP_InputField time = markerObjectEditor.GetChild(1).GetComponent<TMP_InputField>();
        ButStandard[] butsColor = new ButStandard[]
        {
            new ButStandard(colorTr.GetChild(1).GetChild(0).GetComponent<Image>(), colorTr.GetChild(1).GetChild(0).GetChild(0).GetComponent<Image>()),
            new ButStandard(colorTr.GetChild(1).GetChild(1).GetComponent<Image>(), colorTr.GetChild(1).GetChild(1).GetChild(0).GetComponent<Image>()),
            new ButStandard(colorTr.GetChild(1).GetChild(2).GetComponent<Image>(), colorTr.GetChild(1).GetChild(2).GetChild(0).GetComponent<Image>())
        };
        Image startColorOut = startColor.GetChild(1).GetComponent<Image>();
        Image endColorOut = endColor.GetChild(1).GetComponent<Image>();
        Slider[] slidersColor = new Slider[]
        {
            startColorOut.transform.GetChild(0).GetComponent<Slider>(),
            startColorOut.transform.GetChild(1).GetComponent<Slider>(),
            startColorOut.transform.GetChild(2).GetComponent<Slider>(),
            startColorOut.transform.GetChild(3).GetComponent<Slider>(),
            endColorOut.transform.GetChild(0).GetComponent<Slider>(),
            endColorOut.transform.GetChild(1).GetComponent<Slider>(),
            endColorOut.transform.GetChild(2).GetComponent<Slider>(),
            endColorOut.transform.GetChild(3).GetComponent<Slider>(),
            intervalColor.GetChild(2).GetComponent<Slider>()
        };
        Text[] textsColor = new Text[]
        {
            startColorOut.transform.GetChild(4).GetComponent<Text>(),
            startColorOut.transform.GetChild(5).GetComponent<Text>(),
            startColorOut.transform.GetChild(6).GetComponent<Text>(),
            startColorOut.transform.GetChild(7).GetComponent<Text>(),
            endColorOut.transform.GetChild(4).GetComponent<Text>(),
            endColorOut.transform.GetChild(5).GetComponent<Text>(),
            endColorOut.transform.GetChild(6).GetComponent<Text>(),
            endColorOut.transform.GetChild(7).GetComponent<Text>(),
            intervalColor.GetChild(1).GetComponent<Text>()
        };

        pos = new VectorBlock(inputFieldsPos, butsPos, startPos.gameObject, endPos.gameObject, intervalPos.gameObject, posTr.gameObject);
        scale = new VectorBlock(inputFieldsScale, butsScale, startScale.gameObject, endScale.gameObject, intervalScale.gameObject, scaleTr.gameObject);
        rot = new FloatBlock(inputFieldsRot, butsRot, startRot.gameObject, endRot.gameObject, intervalRot.gameObject, rotTr.gameObject);
        color = new ColorBlock(time, butsColor, slidersColor, textsColor, startColorOut, endColorOut, startColor.gameObject, endColor.gameObject, intervalColor.gameObject, colorTr.gameObject);
        //pos.Open(new Pos(3, VectorRandomType.N, 2, 2, 5, 5, 1));
        //scale.Open(new Sca(3, VectorRandomType.N, 2, 2, 5, 5, 1));
        //rot.Open(new Rot(3, FloatRandomType.N, 2, 5, 1));
        //color.Open(new Clr(3, ColorRandomType.N, 0.5f, 0.5f, 0.5f, 1f, 1f, 0.2f, 0.3f, 1f, 0f));
        #endregion

        //Start Object Editor
        obj = new ObjectBlock(objectEditor, contentMarkers, markerInstance, pos, scale, rot, color);
        Prefab prefab = new Prefab("cube1", true, true,
            new List<Pos>() { new Pos(0, VectorRandomType.N, 0, 0), new Pos(1, VectorRandomType.N, -5, -5), new Pos(3, VectorRandomType.N, 5, -5), new Pos(5, VectorRandomType.N, 5, 5), new Pos(7, VectorRandomType.N, 5, -5) },
            new List<Sca>() { new Sca(0, VectorRandomType.N, 0, 0), new Sca(3, VectorRandomType.N, 1, 1), new Sca(5, VectorRandomType.N, 1, 2), new Sca(7, VectorRandomType.N, 2, 2), new Sca(9, VectorRandomType.N, 2, 1) },
            new List<Rot>() { new Rot(0, FloatRandomType.N, 0), new Rot(5, FloatRandomType.N, 0), new Rot(7, FloatRandomType.N, 180), new Rot(9, FloatRandomType.N, 90), new Rot(11, FloatRandomType.N, -90) },
            new List<Clr>() { new Clr(0, ColorRandomType.N, 1f, 0f, 0f, 1f), new Clr(11, ColorRandomType.N, 1f, 0.2f, 0.3f, 1f) },
            SpriteType.Square, 1, 15, 0, 1, AnchorPresets.Center_Middle, AnchorPresets.Center_Middle, 3, 1, 0);
        StartObjectEditor(prefab);

        //Aspect Update
        halfScreen = Screen.width / 2f;
        camAspect = editor.GetAspect();
        width = 540f * camAspect;
        AspectUpdate(true);
    }

    #region Marker Object Editor
    public void PosRandomizeButs(int id) { pos.RandomizeButs(id); UpdateMarker(); }
    public void PosUpdate() { UpdateMarker(); return; }
    public void PosButsModData(int id) { pos.ButsModData(id); UpdateMarker(); }
    public void ScaRandomizeButs(int id) { scale.RandomizeButs(id); UpdateMarker(); }
    public void ScaUpdate() { UpdateMarker(); return; }
    public void ScaButsModData(int id) { scale.ButsModData(id); UpdateMarker(); }
    public void RotRandomizeButs(int id) { rot.RandomizeButs(id); UpdateMarker(); }
    public void RotUpdate() { UpdateMarker(); return; }
    public void RotButsModData(int id) { rot.ButsModData(id); UpdateMarker(); }
    public void ClrRandomizeButs(int id) { color.RandomizeButs(id); UpdateMarker(); }
    public void ClrUpdate(int id) { color.Update(id); UpdateMarker(); return; }
    public void TimeButsModData(int id) 
    {
        switch (id)
        {
            case 1: if (float.Parse(time_IF.text) >= 1f) { time_IF.text = (float.Parse(time_IF.text) - 1f).ToString("0.000"); } break;
            case 2: if (float.Parse(time_IF.text) >= 0.1f) { time_IF.text = (float.Parse(time_IF.text) - 0.1f).ToString("0.000"); } break;
            case 3: time_IF.text = editor.secCurrent.ToString("0.000"); break;
            case 4: time_IF.text = (float.Parse(time_IF.text) + 0.1f).ToString("0.000"); break;
            case 5: time_IF.text = (float.Parse(time_IF.text) + 1f).ToString("0.000"); break;
        }
        UpdateMarker();
    }
    #endregion

    #region Object Editor
    public void ObjectOff() { obj.Off(); UpdatePrefab(); }
    public void ObjectUpdate() { UpdatePrefab(); }
    public void ObjectButsModData(int id) { obj.ButsModData(id, editor.secCurrent); UpdatePrefab(); }
    public void SelectSprite(int id) { obj.SelectSprite(id); UpdatePrefab(); }
    public void OpenObjects(int id) { obj.OpenObjects(id); UpdatePrefab(); }
    public void SelectAnchor(int id) { obj.SelectAnchor(id); UpdatePrefab(); }
    public void SelectPivot(int id) { obj.SelectPivot(id); UpdatePrefab(); }
    #endregion

    public void AspectUpdate(bool downRender)
    {
        if (false) { editor.downRender = BaseLevelEditor.EditorRenderType.None; }
        editorDown.gameObject.SetActive(downRender);
        if (downRender)
        {
            editor.downRender = BaseLevelEditor.EditorRenderType.EditorLRD;
            editorLeft.localPosition = new Vector2(0f, 150f);
            editorLeft.sizeDelta = new Vector2(270f * camAspect, 390f);
            editorRight.localPosition = new Vector2(270f * camAspect, 150f);
            editorRight.sizeDelta = new Vector2(270f * camAspect, 390f);
            editorDown.localPosition = new Vector2(0, 25f);
            editorDown.sizeDelta = new Vector2(540f * camAspect, 250f);
        }
        else
        {
            editor.downRender = BaseLevelEditor.EditorRenderType.EditorLR;
            editorLeft.localPosition = new Vector2(0f, -100f);
            editorLeft.sizeDelta = new Vector2(270f * camAspect, 640f);
            editorRight.localPosition = new Vector2(270f * camAspect, -100f);
            editorRight.sizeDelta = new Vector2(270f * camAspect, 640f);
        }
        return;
    }

    public void StartObjectEditor(Prefab prefab)
    {
        secLength = prefab.tExist;
        secOffset = prefab.startTime;
        timelineLength = 100f * secLength;
        contentMarkers.sizeDelta = new Vector2(timelineLength, 248f);
        obj.Open(prefab);
    }

    public void OpenMarkerEditor(string type, int id)
    {
        if (actType != "")
        {
            UpdatePrefab();
        }
        idMarker = id;
        actType = type;
        switch (type)
        {
            case "pos": pos.Open(obj.pos[id]); break;
            case "sca": scale.Open(obj.sca[id]); break;
            case "rot": rot.Open(obj.rot[id]); break;
            case "clr": color.Open(obj.clr[id]); break;
        }
    }

    public void UpdatePrefab()
    {
        prefab = obj.Get();
        Debug.Log(JsonUtility.ToJson(prefab));
    }

    public void UpdateMarker()
    {
        switch (actType)
        {
            case "pos": obj.UpdateMarker(actType, idMarker, pos.GetPos()); break;
            case "sca": obj.UpdateMarker(actType, idMarker, pos.GetPos()); break;
            case "rot": obj.UpdateMarker(actType, idMarker, pos.GetPos()); break;
            case "clr": obj.UpdateMarker(actType, idMarker, pos.GetPos()); break;
        }
    }

    public void SecLineDown()
    {
        editor.isTouchSecLine = true;
        float locWidth = timelineLength - width;
        float start = locWidth * scrollbarHorizontal.value;
        float posX_01 = (Input.mousePosition.x - halfScreen) / halfScreen;
        float posTimeline = start + posX_01 * width;
        float secLocalCurrent = posTimeline / timelineLength * secLength;
        if (secLocalCurrent < 0) { secLocalCurrent = 0; }
        else if (secLocalCurrent > secLength) { secLocalCurrent = secLength; }
        secLineMarker.localPosition = new Vector2(secLocalCurrent * 100 - timelineLength * scrollbarHorizontal.value, -25);
        editor.secCurrent = secOffset + secLocalCurrent;
        editor.timeline.RenderSecLine();
        editor.play.Timer(editor.secCurrent);
        if (editor.isPlay) { StopCoroutine(editor.timer); }
    }
    public void SecLineDrag()
    {
        float locWidth = timelineLength - width;
        float start = locWidth * scrollbarHorizontal.value;
        float posX_01 = (Input.mousePosition.x - halfScreen) / halfScreen;
        float posTimeline = start + posX_01 * width;
        float secLocalCurrent = posTimeline / timelineLength * secLength;
        if (secLocalCurrent < 0) { secLocalCurrent = 0; }
        else if (secLocalCurrent > secLength) { secLocalCurrent = secLength; }
        secLineMarker.localPosition = new Vector2(secLocalCurrent * 100 - timelineLength * scrollbarHorizontal.value, -25);
        editor.secCurrent = secOffset + secLocalCurrent;
        editor.timeline.RenderSecLine();
        editor.play.Timer(editor.secCurrent);
    }
    public void SecLineUp()
    {
        editor.isTouchSecLine = false;
        if (editor.isPlay) { editor.timer = StartCoroutine(editor.Timer()); }
    }
    public void RenderLocalSecMarker()
    {
        float locSec = editor.secCurrent - secOffset;
        if (locSec < 0f) { locSec = 0f; }
        else if (locSec > secLength) { locSec = secLength; }
        secLineMarker.localPosition = new Vector2(locSec * 100 - timelineLength * scrollbarHorizontal.value, -25);
        return;
    }
}
/// //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
public class ObjectBlock
{
    private GameObject main;
    private TMP_InputField name_IF, startTime_IF, lengthTime_IF;
    private Toggle active_T, collider_T;
    private Image parentImage;
    private Text nameParent;

    private Image[] shapesAnchor;
    private Image[] shapesPivot;
    private Image[] imagesButsModData;
    private Image[] shapesMainButs;
    private Image[] shapes;
    private Transform[] objectsSprites;

    private GameObject marker;
    private RectTransform contentMarkers;
    private List<RectTransform> posMarker = new List<RectTransform>();
    private List<RectTransform> scaMarker = new List<RectTransform>();
    private List<RectTransform> rotMarker = new List<RectTransform>();
    private List<RectTransform> clrMarker = new List<RectTransform>();
    public List<Pos> pos;
    public List<Sca> sca;
    public List<Rot> rot;
    public List<Clr> clr;
    private VectorBlock p;
    private VectorBlock s;
    private FloatBlock r;
    private ColorBlock c;

    private SpriteType spriteType = SpriteType.Square;
    private AnchorPresets anchor = AnchorPresets.Center_Middle;
    private AnchorPresets pivot = AnchorPresets.Center_Middle;
    private int activeObjID = 0;

    public ObjectBlock(RectTransform objectEditor, RectTransform contentMarkers, GameObject marker, VectorBlock p, VectorBlock s, FloatBlock r, ColorBlock c)
    {
        this.p = p;
        this.s = s;
        this.r = r;
        this.c = c;

        this.marker = marker;
        main = objectEditor.gameObject;
        this.contentMarkers = contentMarkers;
        RectTransform basics = objectEditor.GetChild(0).GetComponent<RectTransform>();
        RectTransform timings = objectEditor.GetChild(1).GetComponent<RectTransform>();
        RectTransform parent = objectEditor.GetChild(2).GetComponent<RectTransform>();
        RectTransform anchor_pivot = objectEditor.GetChild(3).GetComponent<RectTransform>();
        RectTransform shapeSelect = objectEditor.GetChild(4).GetComponent<RectTransform>();

        name_IF = basics.GetChild(0).GetComponent<TMP_InputField>();
        startTime_IF = timings.GetChild(0).GetComponent<TMP_InputField>();
        lengthTime_IF = timings.GetChild(1).GetComponent<TMP_InputField>();

        active_T = basics.GetChild(1).GetComponent<Toggle>();
        collider_T = basics.GetChild(2).GetComponent<Toggle>();
        imagesButsModData = new Image[]
        {
            timings.GetChild(2).GetChild(0).GetComponent<Image>(),
            timings.GetChild(2).GetChild(1).GetComponent<Image>(),
            timings.GetChild(2).GetChild(2).GetComponent<Image>(),
            timings.GetChild(2).GetChild(3).GetComponent<Image>(),
            timings.GetChild(2).GetChild(4).GetComponent<Image>(),
            timings.GetChild(3).GetChild(0).GetComponent<Image>(),
            timings.GetChild(3).GetChild(1).GetComponent<Image>(),
            timings.GetChild(3).GetChild(2).GetComponent<Image>(),
            timings.GetChild(3).GetChild(3).GetComponent<Image>()
        };

        parentImage = parent.GetChild(0).GetComponent<Image>();
        nameParent = parent.GetChild(0).GetChild(0).GetComponent<Text>();

        shapesAnchor = new Image[]
        {
            anchor_pivot.GetChild(0).GetChild(0).GetComponent<Image>(),
            anchor_pivot.GetChild(0).GetChild(1).GetComponent<Image>(),
            anchor_pivot.GetChild(0).GetChild(2).GetComponent<Image>(),
            anchor_pivot.GetChild(0).GetChild(3).GetComponent<Image>(),
            anchor_pivot.GetChild(0).GetChild(4).GetComponent<Image>(),
            anchor_pivot.GetChild(0).GetChild(5).GetComponent<Image>(),
            anchor_pivot.GetChild(0).GetChild(6).GetComponent<Image>(),
            anchor_pivot.GetChild(0).GetChild(7).GetComponent<Image>(),
            anchor_pivot.GetChild(0).GetChild(8).GetComponent<Image>()
        };
        shapesPivot = new Image[]
        {
            anchor_pivot.GetChild(1).GetChild(0).GetComponent<Image>(),
            anchor_pivot.GetChild(1).GetChild(1).GetComponent<Image>(),
            anchor_pivot.GetChild(1).GetChild(2).GetComponent<Image>(),
            anchor_pivot.GetChild(1).GetChild(3).GetComponent<Image>(),
            anchor_pivot.GetChild(1).GetChild(4).GetComponent<Image>(),
            anchor_pivot.GetChild(1).GetChild(5).GetComponent<Image>(),
            anchor_pivot.GetChild(1).GetChild(6).GetComponent<Image>(),
            anchor_pivot.GetChild(1).GetChild(7).GetComponent<Image>(),
            anchor_pivot.GetChild(1).GetChild(8).GetComponent<Image>()
        };

        objectsSprites = new Transform[] 
        {
            shapeSelect.GetChild(1),
            shapeSelect.GetChild(2),
            shapeSelect.GetChild(3),
            shapeSelect.GetChild(4),
            shapeSelect.GetChild(5)
        };
        shapesMainButs = new Image[] 
        {
            shapeSelect.GetChild(0).GetChild(0).GetComponent<Image>(),
            shapeSelect.GetChild(0).GetChild(1).GetComponent<Image>(),
            shapeSelect.GetChild(0).GetChild(2).GetComponent<Image>(),
            shapeSelect.GetChild(0).GetChild(3).GetComponent<Image>(),
            shapeSelect.GetChild(0).GetChild(4).GetComponent<Image>(),
            shapeSelect.GetChild(0).GetChild(5).GetComponent<Image>()
        };
        shapes = new Image[]
        {
            //Box
            objectsSprites[0].GetChild(0).GetComponent<Image>(),
            objectsSprites[0].GetChild(1).GetComponent<Image>(),
            objectsSprites[0].GetChild(2).GetComponent<Image>(),
            //Circle
            objectsSprites[1].GetChild(0).GetComponent<Image>(),
            objectsSprites[1].GetChild(1).GetComponent<Image>(),
            objectsSprites[1].GetChild(2).GetComponent<Image>(),
            objectsSprites[1].GetChild(3).GetComponent<Image>(),
            objectsSprites[1].GetChild(4).GetComponent<Image>(),
            objectsSprites[1].GetChild(5).GetComponent<Image>(),
            objectsSprites[1].GetChild(6).GetComponent<Image>(),
            objectsSprites[1].GetChild(7).GetComponent<Image>(),
            objectsSprites[1].GetChild(8).GetComponent<Image>(),
            objectsSprites[1].GetChild(9).GetComponent<Image>(),
            objectsSprites[1].GetChild(10).GetComponent<Image>(),
            objectsSprites[1].GetChild(11).GetComponent<Image>(),
            //Triangle
            objectsSprites[2].GetChild(0).GetComponent<Image>(),
            objectsSprites[2].GetChild(1).GetComponent<Image>(),
            objectsSprites[2].GetChild(2).GetComponent<Image>(),
            objectsSprites[2].GetChild(3).GetComponent<Image>(),
            objectsSprites[2].GetChild(4).GetComponent<Image>(),
            objectsSprites[2].GetChild(5).GetComponent<Image>(),
            //Hexagon
            objectsSprites[3].GetChild(0).GetComponent<Image>(),
            objectsSprites[3].GetChild(1).GetComponent<Image>(),
            objectsSprites[3].GetChild(2).GetComponent<Image>(),
            objectsSprites[3].GetChild(3).GetComponent<Image>(),
            objectsSprites[3].GetChild(4).GetComponent<Image>(),
            objectsSprites[3].GetChild(5).GetComponent<Image>(),
            //Arrow
            objectsSprites[4].GetChild(0).GetComponent<Image>(),
            objectsSprites[4].GetChild(1).GetComponent<Image>()
        };
        Off();
    }

    public void Off()
    {
        activeObjID = 0;
        spriteType = SpriteType.None;
        for (int i = 0; i < objectsSprites.Length; i++)
        {
            objectsSprites[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < shapesAnchor.Length; i++)
        {
            shapesAnchor[i].color = BaseLevelEditor.butDisableBG();
        }
        shapesAnchor[(int)anchor].color = BaseLevelEditor.butEnableBG();

        for (int i = 0; i < shapesPivot.Length; i++)
        {
            shapesPivot[i].color = BaseLevelEditor.butDisableBG();
        }
        shapesPivot[(int)pivot].color = BaseLevelEditor.butEnableBG();

        shapesMainButs[0].color = BaseLevelEditor.butEnableBG();
        for (int i = 1; i < shapesMainButs.Length; i++)
        {
            shapesMainButs[i].color = BaseLevelEditor.butDisableBG();
        }

        for (int i = 0; i < shapes.Length; i++)
        {
            shapes[i].color = BaseLevelEditor.butDisableBG();
        }
        return;
    }

    public void Open(Prefab prefab)
    {
        name_IF.text = prefab.name;
        active_T.isOn = prefab.active;
        collider_T.isOn = prefab.collider;
        pos = prefab.pos;
        sca = prefab.sca;
        rot = prefab.rot;
        clr = prefab.clr;

        for (int i = 0; i < pos.Count; i++)
        {
            GameObject p = Object.Instantiate(marker, contentMarkers.transform);
            p.name = "pos" + i; posMarker.Add(p.GetComponent<RectTransform>());
            posMarker[i].localPosition = new Vector2(pos[i].t * 100f, -45f);
        }
        for (int i = 0; i < sca.Count; i++)
        {
            GameObject s = Object.Instantiate(marker, contentMarkers.transform);
            s.name = "sca" + i; scaMarker.Add(s.GetComponent<RectTransform>());
            scaMarker[i].localPosition = new Vector2(sca[i].t * 100f, -95f);
        }
        for (int i = 0; i < rot.Count; i++)
        {
            GameObject r = Object.Instantiate(marker, contentMarkers.transform);
            r.name = "rot" + i; rotMarker.Add(r.GetComponent<RectTransform>());
            rotMarker[i].localPosition = new Vector2(rot[i].t * 100f, -145f);
        }
        for (int i = 0; i < clr.Count; i++)
        {
            GameObject c = Object.Instantiate(marker, contentMarkers.transform);
            c.name = "clr" + i; clrMarker.Add(c.GetComponent<RectTransform>());
            clrMarker[i].localPosition = new Vector2(clr[i].t * 100f, -195f);
        }

        startTime_IF.text = prefab.startTime.ToString();
        lengthTime_IF.text = prefab.tExist.ToString();
        SelectSprite((int)prefab.st);
        SelectAnchor((int)prefab.anchor);
        SelectPivot((int)prefab.pivot);
        main.SetActive(true);
    }

    public Prefab Get()
    {
        return new Prefab(
            name_IF.text,
            active_T.isOn,
            collider_T.isOn,
            pos, sca, rot, clr,
            spriteType,
            float.Parse(startTime_IF.text),
            float.Parse(lengthTime_IF.text),
            0, 0, anchor, pivot, 0, 0, 0);
    }
    public void UpdateMarker(string type, int id, MarkerData data)
    {
        switch (type)
        {
            case "pos": pos[id] = (Pos)data; break;
            case "sca": sca[id] = (Sca)data; break;
            case "rot": rot[id] = (Rot)data; break;
            case "clr": clr[id] = (Clr)data; break;
        }
    }

    public float GetStartTime()
    {
        return float.Parse(startTime_IF.text);
    }
    public void SelectSprite(int id)
    {
        if (spriteType != SpriteType.None) 
        { shapes[(int)spriteType].color = BaseLevelEditor.butDisableBG(); };
        shapes[id].color = BaseLevelEditor.butEnableBG();
        spriteType = (SpriteType)id;
    }

    public void OpenObjects(int id)
    {
        shapesMainButs[activeObjID].color = BaseLevelEditor.butDisableBG();
        shapesMainButs[activeObjID + 1].color = BaseLevelEditor.butDisableBG();
        objectsSprites[activeObjID].gameObject.SetActive(false);
        shapesMainButs[id + 1].color = BaseLevelEditor.butEnableBG();
        objectsSprites[id].gameObject.SetActive(true);
        activeObjID = id;
    }

    public void SelectAnchor(int id)
    {
        shapesAnchor[(int)anchor].color = BaseLevelEditor.butDisableBG();
        shapesAnchor[id].color = BaseLevelEditor.butEnableBG();
        anchor = (AnchorPresets)id;
    }
    public void SelectPivot(int id)
    {
        shapesPivot[(int)pivot].color = BaseLevelEditor.butDisableBG();
        shapesPivot[id].color = BaseLevelEditor.butEnableBG();
        pivot = (AnchorPresets)id;
    }
    public void ButsModData(int id, float secCurrent)
    {
        switch (id)
        {
            case 1: startTime_IF.text = (float.Parse(startTime_IF.text) - 1f).ToString("0.000"); break;
            case 2: startTime_IF.text = (float.Parse(startTime_IF.text) - 0.1f).ToString("0.000"); break;
            case 3: startTime_IF.text = (secCurrent + 0.1f).ToString("0.000"); break;
            case 4: startTime_IF.text = (float.Parse(startTime_IF.text) + 0.1f).ToString("0.000"); break;
            case 5: startTime_IF.text = (float.Parse(startTime_IF.text) + 1f).ToString("0.000"); break;

            case 6: lengthTime_IF.text = (float.Parse(lengthTime_IF.text) - 1f).ToString("0.000"); break;
            case 7: lengthTime_IF.text = (float.Parse(lengthTime_IF.text) - 0.1f).ToString("0.000"); break;
            case 8: lengthTime_IF.text = (float.Parse(lengthTime_IF.text) + 0.1f).ToString("0.000"); break;
            case 9: lengthTime_IF.text = (float.Parse(lengthTime_IF.text) + 1f).ToString("0.000"); break;
        }
    }
}
public class VectorBlock
{
    public ButStandard[] randomizeButs;
    public TMP_InputField time_IF, interval_IF;
    public TMP_InputField startVecX_IF, startVecY_IF;
    public TMP_InputField endVecX_IF, endVecY_IF;
    public GameObject startGroup;
    public GameObject endGroup;
    public GameObject intervalGroup;
    public GameObject main;
    private int idActive = 0;

    public VectorBlock(TMP_InputField[] inputFields, ButStandard[] buts, GameObject s, GameObject e, GameObject i, GameObject m)
    {
        randomizeButs = buts;

        time_IF = inputFields[0];
        startVecX_IF = inputFields[1];
        startVecY_IF = inputFields[2];
        endVecX_IF = inputFields[3];
        endVecY_IF = inputFields[4];
        interval_IF = inputFields[5];

        startGroup = s;
        endGroup = e;
        intervalGroup = i;
        main = m;

        for (int x = 0; x < randomizeButs.Length; x++)
        { randomizeButs[x].UpdateColor(BaseLevelEditor.butDisableBG(), BaseLevelEditor.butDisable()); }
    }

    public void Open(Pos pos)
    {
        time_IF.text = pos.t.ToString("0.000");
        startVecX_IF.text = pos.sx.ToString("0.000");
        startVecY_IF.text = pos.sy.ToString("0.000");
        endVecX_IF.text = pos.ex.ToString("0.000");
        endVecY_IF.text = pos.ey.ToString("0.000");
        interval_IF.text = pos.i.ToString("0.000");

        RandomizeButs((int)pos.r);
        main.SetActive(true);
        return;
    }
    public Pos GetPos()
    {
        return new Pos(
            float.Parse(time_IF.text),
            (VectorRandomType)idActive,
            float.Parse(startVecX_IF.text),
            float.Parse(startVecY_IF.text),
            float.Parse(endVecX_IF.text),
            float.Parse(endVecY_IF.text),
            float.Parse(interval_IF.text));
    }

    public void Open(Sca sca)
    {
        time_IF.text = sca.t.ToString("0.000");
        startVecX_IF.text = sca.sx.ToString("0.000");
        startVecY_IF.text = sca.sy.ToString("0.000");
        endVecX_IF.text = sca.ex.ToString("0.000");
        endVecY_IF.text = sca.ey.ToString("0.000");
        interval_IF.text = sca.i.ToString("0.000");

        RandomizeButs((int)sca.r);
        main.SetActive(true);
        return;
    }
    public Sca GetSca()
    {
        return new Sca(
            float.Parse(time_IF.text),
            (VectorRandomType)idActive,
            float.Parse(startVecX_IF.text),
            float.Parse(startVecY_IF.text),
            float.Parse(endVecX_IF.text),
            float.Parse(endVecY_IF.text),
            float.Parse(interval_IF.text));
    }

    public void Close()
    {
        main.SetActive(false);
        return;
    }

    public void RandomizeButs(int id)
    {
        randomizeButs[idActive].UpdateColor(BaseLevelEditor.butDisableBG(), BaseLevelEditor.butDisable());
        randomizeButs[id].UpdateColor(BaseLevelEditor.butEnableBG(), BaseLevelEditor.butEnable());
        idActive = id;

        startGroup.SetActive(true);
        endGroup.SetActive(idActive != 0);
        intervalGroup.SetActive(idActive != 0 && idActive != 2);
        return;
    }

    public void ButsModData(int id)
    {
        switch (id)
        {
            case 6: startVecX_IF.text = (float.Parse(startVecX_IF.text) - 1f).ToString("0.000"); break;
            case 7: startVecX_IF.text = (float.Parse(startVecX_IF.text) - 0.1f).ToString("0.000"); break;
            case 8: startVecX_IF.text = (float.Parse(startVecX_IF.text) + 0.1f).ToString("0.000"); break;
            case 9: startVecX_IF.text = (float.Parse(startVecX_IF.text) + 1f).ToString("0.000"); break;

            case 10: startVecY_IF.text = (float.Parse(startVecY_IF.text) - 1f).ToString("0.000"); break;
            case 11: startVecY_IF.text = (float.Parse(startVecY_IF.text) - 0.1f).ToString("0.000"); break;
            case 12: startVecY_IF.text = (float.Parse(startVecY_IF.text) + 0.1f).ToString("0.000"); break;
            case 13: startVecY_IF.text = (float.Parse(startVecY_IF.text) + 1f).ToString("0.000"); break;

            case 14: endVecX_IF.text = (float.Parse(endVecX_IF.text) - 1f).ToString("0.000"); break;
            case 15: endVecX_IF.text = (float.Parse(endVecX_IF.text) - 0.1f).ToString("0.000"); break;
            case 16: endVecX_IF.text = (float.Parse(endVecX_IF.text) + 0.1f).ToString("0.000"); break;
            case 17: endVecX_IF.text = (float.Parse(endVecX_IF.text) + 1f).ToString("0.000"); break;

            case 18: endVecY_IF.text = (float.Parse(endVecY_IF.text) - 1f).ToString("0.000"); break;
            case 19: endVecY_IF.text = (float.Parse(endVecY_IF.text) - 0.1f).ToString("0.000"); break;
            case 20: endVecY_IF.text = (float.Parse(endVecY_IF.text) + 0.1f).ToString("0.000"); break;
            case 21: endVecY_IF.text = (float.Parse(endVecY_IF.text) + 1f).ToString("0.000"); break;

            case 22: interval_IF.text = (float.Parse(interval_IF.text) - 1f).ToString("0.000"); break;
            case 23: interval_IF.text = (float.Parse(interval_IF.text) - 0.1f).ToString("0.000"); break;
            case 24: interval_IF.text = (float.Parse(interval_IF.text) + 0.1f).ToString("0.000"); break;
            case 25: interval_IF.text = (float.Parse(interval_IF.text) + 1f).ToString("0.000"); break;
        }
    }
}
public class FloatBlock
{
    public ButStandard[] randomizeButs;
    public TMP_InputField time_IF, interval_IF;
    public TMP_InputField startRot_IF, endRot_IF;
    public GameObject startGroup;
    public GameObject endGroup;
    public GameObject intervalGroup;
    public GameObject main;
    private int idActive = 0;

    public FloatBlock(TMP_InputField[] inputFields, ButStandard[] buts, GameObject s, GameObject e, GameObject i, GameObject m)
    {
        randomizeButs = buts;

        time_IF = inputFields[0];
        startRot_IF = inputFields[1];
        endRot_IF = inputFields[2];
        interval_IF = inputFields[3];

        startGroup = s;
        endGroup = e;
        intervalGroup = i;
        main = m;

        for (int x = 0; x < randomizeButs.Length; x++)
        { randomizeButs[x].UpdateColor(BaseLevelEditor.butDisableBG(), BaseLevelEditor.butDisable()); }
    }

    public void Open(Rot rot)
    {
        time_IF.text = rot.t.ToString("0.000");
        startRot_IF.text = rot.sa.ToString("0.000");
        endRot_IF.text = rot.ea.ToString("0.000");
        interval_IF.text = rot.i.ToString("0.000");

        RandomizeButs((int)rot.r);
        main.SetActive(true);
        return;
    }
    public Rot GetRot()
    {
        return new Rot(
            float.Parse(time_IF.text),
            (FloatRandomType)idActive,
            float.Parse(startRot_IF.text),
            float.Parse(endRot_IF.text),
            float.Parse(interval_IF.text));
    }

    public void Close()
    {
        main.SetActive(false);
        return;
    }

    public void RandomizeButs(int id)
    {
        randomizeButs[idActive].UpdateColor(BaseLevelEditor.butDisableBG(), BaseLevelEditor.butDisable());
        randomizeButs[id].UpdateColor(BaseLevelEditor.butEnableBG(), BaseLevelEditor.butEnable());
        idActive = id;

        startGroup.SetActive(true);
        endGroup.SetActive(idActive != 0);
        intervalGroup.SetActive(idActive != 0 && idActive != 2);
        return;
    }

    public void ButsModData(int id)
    {
        switch (id)
        {
            case 6: startRot_IF.text = (float.Parse(startRot_IF.text) - 1f).ToString("0.000"); break;
            case 7: startRot_IF.text = (float.Parse(startRot_IF.text) - 0.1f).ToString("0.000"); break;
            case 8: startRot_IF.text = (float.Parse(startRot_IF.text) + 0.1f).ToString("0.000"); break;
            case 9: startRot_IF.text = (float.Parse(startRot_IF.text) + 1f).ToString("0.000"); break;

            case 10: endRot_IF.text = (float.Parse(endRot_IF.text) - 1f).ToString("0.000"); break;
            case 11: endRot_IF.text = (float.Parse(endRot_IF.text) - 0.1f).ToString("0.000"); break;
            case 12: endRot_IF.text = (float.Parse(endRot_IF.text) + 0.1f).ToString("0.000"); break;
            case 13: endRot_IF.text = (float.Parse(endRot_IF.text) + 1f).ToString("0.000"); break;

            case 14: interval_IF.text = (float.Parse(interval_IF.text) - 1f).ToString("0.000"); break;
            case 15: interval_IF.text = (float.Parse(interval_IF.text) - 0.1f).ToString("0.000"); break;
            case 16: interval_IF.text = (float.Parse(interval_IF.text) + 0.1f).ToString("0.000"); break;
            case 17: interval_IF.text = (float.Parse(interval_IF.text) + 1f).ToString("0.000"); break;
        }
    }
}
public class ColorBlock
{
    public TMP_InputField time_IF;
    public ButStandard[] randomizeButs;

    public Image startIm, endIm;
    public Slider startR, startG, startB, startA;
    public Slider endR, endG, endB, endA;
    public Slider interval;
    public Text startTxtR, startTxtG, startTxtB, startTxtA;
    public Text endTxtR, endTxtG, endTxtB, endTxtA;
    public Text intervalTxt;

    public GameObject startGroup;
    public GameObject endGroup;
    public GameObject intervalGroup;
    public GameObject main;
    private int idActive = 0;

    public ColorBlock(TMP_InputField time_IF, ButStandard[] buts, Slider[] sliders, Text[] texts, Image startIm, Image endIm, GameObject s, GameObject e, GameObject i, GameObject m)
    {
        this.endIm = endIm;
        this.startIm = startIm;
        this.time_IF = time_IF;
        randomizeButs = buts;

        startR = sliders[0]; startTxtR = texts[0];
        startG = sliders[1]; startTxtG = texts[1];
        startB = sliders[2]; startTxtB = texts[2];
        startA = sliders[3]; startTxtA = texts[3];
        endR = sliders[4]; endTxtR = texts[4];
        endG = sliders[5]; endTxtG = texts[5];
        endB = sliders[6]; endTxtB = texts[6];
        endA = sliders[7]; endTxtA = texts[7];
        interval = sliders[8]; intervalTxt = texts[8];

        startGroup = s;
        endGroup = e;
        intervalGroup = i;
        main = m;

        for (int x = 0; x < randomizeButs.Length; x++)
        { randomizeButs[x].UpdateColor(BaseLevelEditor.butDisableBG(), BaseLevelEditor.butDisable()); }
    }

    public void Open(Clr clr)
    {
        time_IF.text = clr.t.ToString();
        startR.value = clr.sr; startTxtR.text = clr.sr.ToString("0.000");
        startG.value = clr.sg; startTxtG.text = clr.sg.ToString("0.000");
        startB.value = clr.sb; startTxtB.text = clr.sb.ToString("0.000");
        startA.value = clr.sa; startTxtA.text = clr.sa.ToString("0.000");
        endR.value = clr.er; endTxtR.text = clr.er.ToString("0.000");
        endG.value = clr.eg; endTxtG.text = clr.eg.ToString("0.000");
        endB.value = clr.eb; endTxtB.text = clr.eb.ToString("0.000");
        endA.value = clr.ea; endTxtA.text = clr.ea.ToString("0.000");
        interval.value = clr.i; intervalTxt.text = clr.i.ToString("0.000"); 

        startIm.color = new Color(startR.value, startG.value, startB.value, startA.value); 
        endIm.color = new Color(endR.value, endG.value, endB.value, endA.value);
        RandomizeButs((int)clr.r);
        main.SetActive(true);
        return;
    }

    public Clr GetColor()
    {
        return new Clr(
            float.Parse(time_IF.text),
            (ColorRandomType)idActive,
            startR.value, startG.value, startB.value, startA.value,
            endR.value, endG.value, endB.value, endA.value,
            interval.value);
    }

    public void Update(int id)
    {
        switch (id)
        {
            case 1: startTxtR.text = startR.value.ToString("0.000"); startIm.color = new Color(startR.value, startG.value, startB.value, startA.value); break;
            case 2: startTxtG.text = startG.value.ToString("0.000"); startIm.color = new Color(startR.value, startG.value, startB.value, startA.value); break;
            case 3: startTxtB.text = startB.value.ToString("0.000"); startIm.color = new Color(startR.value, startG.value, startB.value, startA.value); break;
            case 4: startTxtA.text = startA.value.ToString("0.000"); startIm.color = new Color(startR.value, startG.value, startB.value, startA.value); break;
            case 5: endTxtR.text = endR.value.ToString("0.000"); endIm.color = new Color(endR.value, endG.value, endB.value, endA.value); break;
            case 6: endTxtG.text = endG.value.ToString("0.000"); endIm.color = new Color(endR.value, endG.value, endB.value, endA.value); break;
            case 7: endTxtB.text = endB.value.ToString("0.000"); endIm.color = new Color(endR.value, endG.value, endB.value, endA.value); break;
            case 8: endTxtA.text = endA.value.ToString("0.000"); endIm.color = new Color(endR.value, endG.value, endB.value, endA.value); break;
            case 9: intervalTxt.text = interval.value.ToString("0.000"); break;
        }
        return;
    }

    public void Close()
    {
        main.SetActive(false);
        return;
    }

    public void RandomizeButs(int id)
    {
        randomizeButs[idActive].UpdateColor(BaseLevelEditor.butDisableBG(), BaseLevelEditor.butDisable());
        randomizeButs[id].UpdateColor(BaseLevelEditor.butEnableBG(), BaseLevelEditor.butEnable());
        idActive = id;

        startGroup.SetActive(true);
        endGroup.SetActive(idActive != 0);
        intervalGroup.SetActive(idActive != 0 && idActive != 2);
        return;
    }
}

public class ButStandard
{
    public Image butBG;
    public Image butIcon;

    public void UpdateColor(Color BG, Color icon)
    {
        butBG.color = BG;
        butIcon.color = icon;
    }

    public ButStandard(Image BG, Image icon)
    {
        butBG = BG;
        butIcon = icon;
    }
}
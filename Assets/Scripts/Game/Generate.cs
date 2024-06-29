using UnityEngine.Experimental.Rendering.Universal;
using System.Collections;
using UnityEngine.UI;/// /////////////////////////////////////
using UnityEngine;
using UnityEditor;
using Custom;

public class Generate : GlobalBehaviour
{
    [Header("Assets")]
    public Text debug;/// /////////////////////////////////////
    public Cam cam;
    public Menu menu;
    public DataGame data;
    public Transform player;
    public GameObject empty;
    public GameObject effect;
    public GameObject text;
    public GameObject bullet;
    public GameObject circleBullet;
    public GameObject divideBullet;
    public GameObject money;
    public GameObject line;
    public GameObject circle;
    public GameObject gun;
    [Header("Effects")]
    private float xScr, yScr;
    private float offsetXScr;
    private Transform content;
    private MainAnimator animator;

    #region Basic
    void Start()
    {
        animator = GetComponent<MainAnimator>();
        yScr = 10; xScr = yScr * cam.cam.aspect;
        offsetXScr = xScr - yScr * (16f / 9f);
        content = GetComponent<Transform>();
    }

    private bool isShadow = false;
    public void GraphicsSet(bool isShadow)
    {
        this.isShadow = isShadow;
    }
    public void ClearData()
    {
        animator.Items.Clear();
        for (int i = 0; i < content.childCount; i++)
        {
            Destroy(content.GetChild(i).gameObject);
        }
        return;
    }

    public void StartGame()
    {
        /*Track track = data.levels[PlayerPrefs.GetInt("level") - 1];
        menu.SetTrack(track);
        anim = Instantiate(track.animator, content).GetComponent<Animator>();
        anim.GetComponent<Interpretator>().StartSet(this);
        anim.gameObject.name = "MainAnimator";*/
    }

    public void PauseGame(bool enabled)
    {
        /*anim.enabled = enabled;*/
    }

    public void EndGame()
    {
        /*Destroy(anim.gameObject);*/
    }
    #endregion

    #region Spawn
    public void Money(Vector2 playerPos)
    {
        Vector2 spawnPos = InnerPos(0.5f);
        if (Vector2.Distance(playerPos, spawnPos) < 1f)
        {
            int angle = Random.Range(0, 360);
            Vector2 rotate = Quaternion.Euler(0, 0, angle) * Vector2.up;
            spawnPos = BorderPos(-playerPos - rotate, 0.5f);
        }

        Instantiate(money, spawnPos, Quaternion.identity, content);
    }

    public void Bullet(Bullet data)
    {
        Vector2 startPoint = GetPos(data.startPoint, data.size.Get());
        Vector2 size = new Vector2(data.size.Get(), data.size.Get());
        Vector2 border = new Vector2(xScr, yScr);
        Vector2 offset = data.startPoint.point - (Vector2)player.localPosition;
        float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg + 90f;
        Quaternion rot = Quaternion.Euler(0, 0, angle);

        Transform tr = Instantiate(bullet, startPoint, rot, content).transform;
        Vector3 vel = GetPos(data.velocity, angle) * data.speed.Get();
        tr.localScale = size; tr.name = "Bullet";
        tr.GetComponent<SpriteRenderer>().sprite = animator.GetSprite(data.sprite);
        animator.StartAnimPosVel(startPoint, vel, size + border, tr);
        StartCoroutine(NextFrame());

        IEnumerator NextFrame()
        {
            yield return new WaitForSeconds(Anim.fpsTime30);
            if (data.spinSpeed.Get() != 0) { animator.StartAnimRot(data.spinSpeed.Get(), border, tr); }
        }
    }

    public void BulletExplosion(BulletExplosion data)
    {
        Vector2 startPoint = GetPos(data.startPoint, data.size.Get());
        Transform tr = Instantiate(circleBullet, startPoint, Quaternion.identity, content).transform;
        SpriteRenderer sr = tr.GetComponent<SpriteRenderer>();
        tr.name = "Bullet Explosion"; sr.color = MainColor(0.5f);
        tr.GetComponent<SpriteRenderer>().sprite = animator.GetSprite(data.bomb);
        tr.localScale = new Vector2(data.size.Get(), data.size.Get());
        animator.StartAnimPos(data.time2point, startPoint, GetPos(data.endPoint, data.size.Get()), tr, new EndAnim(Explosion));
        if (data.spinSpeed.Get() != 0) { animator.StartAnimRot(data.time2point, data.spinSpeed.Get(), tr); }

        void Explosion()
        {
            DoCam(data.camMod);
            sr.color = MainColor(1f);
            tr.GetComponent<CircleCollider2D>().enabled = true;
            tr.GetComponent<ShadowCaster2D>().enabled = isShadow;
            animator.StartAnimScale(data.time2charging, data.size.Get(), data.sizeExp.Get(), tr, new EndAnim(Exist));
        }

        void Exist()
        {
            StartCoroutine(Timer());
            IEnumerator Timer()
            {
                yield return new WaitForSeconds(data.time2spread);
                animator.StartAnimColor(data.time2end, MainColor(1f), MainColor(0f), sr);
            }
        }
    }

    public void BulletDivision(BulletDivision data)
    {
        Bullet bullet = data.bulletDivide;
        Vector2 startPoint = GetPos(data.startPoint, data.size.Get());
        Vector2 endPoint = GetPos(data.endPoint, data.size.Get());
        Vector2 border = new Vector2(xScr + bullet.size.Get(), yScr + bullet.size.Get());

        Transform tr = Instantiate(divideBullet, startPoint, Quaternion.identity, content).transform;
        SpriteRenderer sr = tr.GetComponent<SpriteRenderer>();
        //sr.sprite = animator.GetSprite(data.sprite);
        tr.localScale = new Vector2(data.size.Get(), data.size.Get());
        tr.name = "Bullet Division"; sr.color = MainColor(1f);
        animator.StartAnimScale(data.time2point, 0.1f, data.size.Get(), tr);
        animator.StartAnimPos(data.time2point, startPoint, endPoint, tr, new EndAnim(Stay));
        animator.StartAnimRot(data.time2point + data.time2charging, data.spinSpeed.Get(), tr, new EndAnim(Explosion));

        void Stay()
        {
            tr.GetComponent<CircleCollider2D>().enabled = true;
        }

        void Explosion()
        {
            DoCam(data.camMod);
            Destroy(tr.gameObject);//Anim
            int id = 0; Spawn(id); id++;
            float time2spawn = data.time2spread / data.bulletCounts.Length;
            if (data.bulletCounts.Length > 1) { StartCoroutine(SpawnBullets()); }
            IEnumerator SpawnBullets()
            {
                yield return new WaitForSeconds(time2spawn);
                Spawn(id); id++;
                if (data.bulletCounts.Length > id) 
                { StartCoroutine(SpawnBullets()); }
            }
        }

        void Spawn(int id)
        {
            Vector3[] forces = ExplosionForce(data.bulletCounts[id].Get());
            for (int i = 0; i < data.bulletCounts[id].Get(); i++)
            {
                GameObject obj = Instantiate(circleBullet, startPoint, Quaternion.identity, content);
                obj.GetComponent<CircleCollider2D>().enabled = true;
                obj.transform.localScale = new Vector2(bullet.size.Get(), bullet.size.Get());
                animator.StartAnimPosVel(endPoint, forces[i] * bullet.speed.Get(), border, obj.transform);
            }
            return;
        }
    }

    public void BulletRow(BulletRow bulletRow)
    {
        float size = bulletRow.count.Get() * bulletRow.sizeBullet.Get();
        Vector2 startPoint = GetPos(bulletRow.startPoint, size);
        Vector2 a = (Vector2)player.localPosition - startPoint;
        float angle = Mathf.Atan2(a.y, a.x) * Mathf.Rad2Deg - 90f;
        Vector2 size2 = new Vector2(bulletRow.sizeBullet.Get(), bulletRow.sizeBullet.Get());

        Transform tr = Instantiate(empty, startPoint, Quaternion.Euler(0, 0, angle), content).transform;
        tr.name = "RowBullet c:" + bulletRow.count;
        for (int i = 0; i < bulletRow.count.Get(); i++)
        {
            Transform trChilds = Instantiate(bullet, tr).transform;
            float posY = (-bulletRow.count.Get() + 1f + i * 2) * bulletRow.sizeBullet.Get();
            trChilds.localPosition = new Vector3(0, posY, 0);
            trChilds.localScale = size2;
        }
        Vector2 border = new Vector2(xScr + size, yScr + size);
        animator.StartAnimPosVel(startPoint, GetPos(bulletRow.velocity, angle) * bulletRow.speed.Get(), border, tr);
    }

    public void LineSimple(LineSimple data)//x-start, y-action, z-end
    {
        Transform tr = Instantiate(line, GetPos(data.point, data.width.Get()), Quaternion.Euler(0, 0, data.angle.Get(true)), content).transform;
        SpriteRenderer sr = tr.GetComponent<SpriteRenderer>();
        tr.localScale = new Vector2(data.width.Get(), tr.localScale.y);
        tr.GetComponent<ShadowCaster2D>().enabled = false;
        animator.StartAnimColor(data.time2charging, MainColor(0), MainColor(0.5f), sr, new EndAnim(Action));

        void Action()
        {
            DoCam(data.camMod);
            tr.GetComponent<ShadowCaster2D>().enabled = isShadow;
            tr.GetComponent<BoxCollider2D>().enabled = true;
            tr.GetComponent<SpriteRenderer>().color = MainColor(1);
            StartCoroutine(Action2End());

            IEnumerator Action2End()
            {
                yield return new WaitForSeconds(data.time2exist);
                tr.GetComponent<BoxCollider2D>().enabled = false;
                animator.StartAnimColor(data.time2end, MainColor(1), MainColor(0), sr, new EndAnim(End));
            }
        }

        void End()
        {
            Destroy(tr.gameObject);
        }
    }

    public void CircleSimple(CircleSimple data)//x-start, y-action, z-end
    {
        Transform tr = Instantiate(circle, GetPos(data.point, data.size.Get()), Quaternion.identity, content).transform;
        SpriteRenderer sr = tr.GetComponent<SpriteRenderer>();
        tr.GetComponent<ShadowCaster2D>().enabled = false;
        animator.StartAnimColor(data.time2wait, MainColor(0), MainColor(1f), sr, new EndAnim(Action));
        animator.StartAnimRot(data.time2wait, 3f, tr);
        tr.localScale = new Vector2(data.size.Get(), data.size.Get());

        void Action()
        {
            DoCam(data.camMod);
            tr.GetComponent<ShadowCaster2D>().enabled = isShadow;
            tr.GetComponent<CircleCollider2D>().enabled = true;
            tr.GetComponent<SpriteRenderer>().sprite = animator.GetSprite(SpriteType.Circle);
            StartCoroutine(Action2End());

            IEnumerator Action2End()
            {
                yield return new WaitForSeconds(data.time2charging);
                tr.GetComponent<CircleCollider2D>().enabled = false;
                animator.StartAnimColor(data.time2end, MainColor(1), MainColor(0), sr, new EndAnim(End));
            }
        }

        void End()
        {
            Destroy(tr.gameObject);
        }
    }

    public void StandardGun(StandardGun data)
    {
        Bullet b = data.bullet;
        Vector2 startPoint = GetPos(data.startPoint, b.size.Get());
        Vector2 startWaitPoint = GetPos(data.startWaitPoint, b.size.Get());
        Vector2 endWaitPoint = GetPos(data.endWaitPoint, b.size.Get());
        Vector2 endPoint = GetPos(data.endPoint, b.size.Get());

        Vector2 a = endPoint - startPoint;
        Vector2 border = new Vector2(xScr + b.size.Get(), yScr + b.size.Get());
        float angle = Mathf.Atan2(a.y, a.x) * Mathf.Rad2Deg - 90f;
        float time2spawn = data.time2shoot / data.bulletCounts.Length;

        Transform tr = Instantiate(gun, startPoint, Quaternion.Euler(0, 0, angle), content).transform;
        SpriteRenderer sr = tr.GetComponent<SpriteRenderer>();
        animator.StartAnimPos(data.time2move, startPoint, startWaitPoint, tr, new EndAnim(Charging));

        void Charging()
        {
            animator.StartAnimColor(data.time2charging, MainColor(), Color.white, sr, new EndAnim(Shoot));
            animator.StartAnimPos(data.time2charging + data.time2shoot + data.time2wait, startWaitPoint, endWaitPoint, tr);
        }

        void Shoot()
        {
            sr.color = MainColor();
            int id = 0; Spawn(id); id++;
            if (data.bulletCounts.Length > 1) 
            { StartCoroutine(SpawnBullets()); }
            IEnumerator SpawnBullets()
            {
                yield return new WaitForSeconds(time2spawn);
                Spawn(id); id++;
                if (data.bulletCounts.Length > id)
                { StartCoroutine(SpawnBullets()); }
                else { End(); }
            }
        }

        void Spawn(int id)
        {
            float scale = tr.localScale.x;
            Vector2[] poses = PositionsBulletsGun(GunType.Standard);
            for (int i = 0; i < data.bulletCounts[id].Get(); i++)
            {
                Vector2Mod pos = new Vector2Mod(tr.localPosition + RotateVector(poses[i], angle) * scale);
                Transform trBullet = Instantiate(circleBullet, GetPos(pos, b.size.Get()), Quaternion.identity, content).transform;
                trBullet.GetComponent<CircleCollider2D>().enabled = true;
                trBullet.localScale = new Vector2(b.size.Get(), b.size.Get());
                animator.StartAnimPosVel(GetPos(pos, b.size.Get()), Quaternion.Euler(0, 0, angle) * Vector2.up * b.speed.Get(), border, trBullet);////////////
            }
            return;
        }
        void End() { animator.StartAnimPos(data.time2end, endWaitPoint, endPoint, tr, new EndAnim(DestroyObj)); }
        void DestroyObj() { Destroy(tr.gameObject); }
    }

    public void SquareSnake(SquareSnake data)
    {
        float distance = data.distance.Get();
        float scale = data.scale.Get(true);
        Vector2 posAct = GetPos(data.point, scale);
        float angleAct = data.angle.Get();

        AnimSprite animSprite = data.animSprite;
        Vector2 border = new Vector2(xScr + scale, yScr + scale);
        Spawn(); StartCoroutine(GenerateBlock());
        IEnumerator GenerateBlock()
        {
            yield return new WaitForSeconds(data.time4Spawn.Get(true));
            angleAct += data.angleSpeed.Get();
            Vector2 rotate = Quaternion.Euler(0, 0, angleAct) * Vector2.up * (distance + scale);
            posAct += rotate; Spawn();
            if (!Math.OutOfBorder(posAct, border))
            { StartCoroutine(GenerateBlock()); }
        }

        void Spawn()
        {
            Color color = animSprite.color0;
            Transform tr = Instantiate(bullet, posAct, Quaternion.Euler(0, 0, angleAct), content).transform;
            animSprite.sr = tr.GetComponent<SpriteRenderer>();
            animSprite.GetSR().color = new Color(color.r, color.g, color.b, 0f);
            animSprite.GetSR().sprite = animator.GetSprite(SpriteType.Square);
            tr.localScale = new Vector2(scale, scale);
            animator.StartAnimColor(animSprite);
            return;
        }
    }

    public void Pillar(Pillar pillar)
    {
        Vector2 point = GetPos(pillar.point, 0);
        Vector2 border = new Vector2(xScr, yScr);
        if (pillar.direction == Direction.Random) { pillar.direction = (Direction)Random.Range(1, 5); }
        Transform trEffect = Instantiate(effect, Vector2.zero, Quaternion.identity, content).transform;

        if (pillar.direction == Direction.Up || pillar.direction == Direction.Down) 
        { trEffect.localScale = new Vector2(pillar.width.Get(), yScr * 2); trEffect.localPosition = new Vector2(point.x, 0); }
        else { trEffect.localScale = new Vector2(xScr * 2, pillar.width.Get()); trEffect.localPosition = new Vector2(0, point.y); }

        SpriteRenderer srEffect = trEffect.GetComponent<SpriteRenderer>();
        srEffect.color = MainColor(0.5f);
        srEffect.sprite = animator.GetSprite(SpriteType.Square);

        Transform tr = Instantiate(line, point, Quaternion.identity, content).transform;
        tr.localScale = Vector3.zero; tr.GetComponent<BoxCollider2D>().enabled = true;
        SpriteRenderer sr = tr.GetComponent<SpriteRenderer>();
        sr.color = MainColor(1f); sr.sprite = animator.GetSprite(SpriteType.Square);
        animator.StartAnimFilled(pillar.time2wait, pillar.progressTarget.Get(), pillar.width.Get(), pillar.direction, point, border, tr, new EndAnim(Filled));

        void Filled()
        {
            DoCam(pillar.camMod);
            Destroy(trEffect.gameObject);
            if (pillar.direction == Direction.Up || pillar.direction == Direction.Down)
            { tr.localScale = new Vector2(pillar.width.Get(), yScr * 2); tr.localPosition = new Vector2(point.x, 0); }
            else { tr.localScale = new Vector2(xScr * 2, pillar.width.Get()); tr.localPosition = new Vector2(0, point.y); }

            StartCoroutine(FlashStart());
            IEnumerator FlashStart()
            {
                yield return new WaitForSeconds(Anim.fpsTime30);
                sr.color = Color.white;
                StartCoroutine(FlashEnd());
            }
            IEnumerator FlashEnd()
            {
                yield return new WaitForSeconds(Anim.fpsTime30);
                sr.color = MainColor(1f);
                StartCoroutine(Delete());
            }
        }

        IEnumerator Delete()
        {
            yield return new WaitForSeconds(pillar.time2exist);
            animator.StartAnimColor(0.1f, MainColor(1f), MainColor(0f), sr);
        }
    }

    public void AnimSprite(AnimSprite animSprite)
    {
        Color color = animSprite.color0;
        Transform tr = Instantiate(effect, GetPos(animSprite.pos, animSprite.scale.x), Quaternion.Euler(0, 0, animSprite.angle), content).transform;
        animSprite.sr = tr.GetComponent<SpriteRenderer>();
        animSprite.GetSR().color = new Color(color.r, color.g, color.b, 0f);
        animSprite.GetSR().sprite = animator.GetSprite(animSprite.spriteType);
        tr.localScale = animSprite.scale;
        animator.StartAnimColor(animSprite);
    }

    public void AnimText(AnimText animText)
    {
        Color color = animText.color0;
        Transform tr = Instantiate(text, GetPos(animText.pos, animText.scale.x), Quaternion.Euler(0, 0, animText.angle), content).transform;
        animText.tm = tr.GetComponent<TextMesh>();
        animText.GetTM().color = new Color(color.r, color.g, color.b, 0f);
        animText.GetTM().text = animText.text;
        tr.localScale = animText.scale;
        animator.StartAnimText(animText);
    }
    #endregion

    #region Calculate Functions
    public Vector2 GetPos(Vector2Mod vector, float parameter)
    {
        switch (vector.func)
        {
            case CalculateFunctionVector2.OrigPos: return OrigPos(vector.point);
            case CalculateFunctionVector2.InnerPos: return InnerPos(parameter);
            case CalculateFunctionVector2.OuterPos: return OuterPos(parameter, vector.direction);
            case CalculateFunctionVector2.Velocity: return vector.point;
            case CalculateFunctionVector2.PlayerPos: return player.localPosition;
            default: return Vector2.zero;
        }
    }
    public void DoCam(CamMod camMod)
    {
        switch (camMod.cam)
        {
            case ModCam.Shake: cam.Shake((int)camMod.parameter.Get(), camMod.radius.Get()); break;
            case ModCam.ShakeAngle: cam.ShakeDirection(camMod.parameter.Get(), camMod.radius.Get(), 10); break;
            case ModCam.ShakeDeep: cam.ShakeDeep(camMod.radius.Get(), (int)camMod.parameter.Get()); break;
        }
    }
    public Vector2 OrigPos(Vector2 point)
    {
        if (point.x > 0) { return new Vector2(point.x + offsetXScr, point.y); }
        if (point.x < 0) { return new Vector2(point.x - offsetXScr, point.y); }
        return point;
    }
    public Vector2 BorderPos(Vector2 pos, float offset = 0f)
    {
        float x = pos.x; float y = pos.y;
        if (pos.x > xScr - offset) { x = xScr - offset; }
        else if (pos.x < -xScr + offset) { x = -xScr + offset; }
        if (pos.y > yScr - offset) { y = yScr - offset; }
        else if (pos.y < -yScr + offset) { y = -yScr + offset; }
        return new Vector2(x, y);
    }
    public Vector2 InnerPos(float offset = 0f)
    {
        float x = Random.Range(-xScr + offset, xScr - offset);
        float y = Random.Range(-yScr + offset, yScr - offset);
        return new Vector2(x, y);
    }
    public Vector2 OuterPos(float dist, Direction direction = Direction.Random)
    {
        float hd = dist / 2;/////////////////////////////////////////////////////////////////////
        Vector2 borders = new Vector2(xScr - hd, yScr - hd);
        int num = (int)direction;
        if (num == 5) { num = Random.Range(1, 5); }

        switch (num)
        {
            case 1:
                return new Vector2(Random.Range(-borders.x, borders.x), yScr + dist);
            case 2:
                return new Vector2(Random.Range(-borders.x, borders.x), -yScr - dist);
            case 3:
                return new Vector2(-xScr - dist, Random.Range(-borders.y, borders.y));
            case 4:
                return new Vector2(xScr + dist, Random.Range(-borders.y, borders.y));
            default:
                float x = Random.Range(0f, 1f) > 0.5f ? 1 : -1;
                float y = Random.Range(0f, 1f) > 0.5f ? 1 : -1;
                return new Vector2((xScr + dist) * x, (yScr + dist) * y);
        }
    }
    public Vector3[] ExplosionForce(int count)
    {
        float random = Random.Range(0, 180);
        Vector3[] end = new Vector3[count];
        for (int i = 0; i < count; i++)
        {
            float angle = 360f / count * i + random;
            end[i] = Quaternion.Euler(0, 0, angle) * Vector3.up;
        }
        return end;
    }
    public Vector3 RotateVector(Vector2 a, float offsetAngle)//метод вращения объекта
    {
        float power = Mathf.Sqrt(a.x * a.x + a.y * a.y);//коэффициент силы
        float angle = Mathf.Atan2(a.y, a.x) * Mathf.Rad2Deg - 90f + offsetAngle;//угол из координат с offset'ом
        return Quaternion.Euler(0, 0, angle) * Vector2.up * power;
        //построение вектора из изменённого угла с коэффициентом силы
    }

    public float Direction2Angle(Direction direction)
    {
        switch (direction)
        {
            case Direction.Up: return 0;
            case Direction.Down: return 180;
            case Direction.Left: return 90;
            case Direction.Right: return -90;
            default: return 0;
        }
    }

#if UNITY_EDITOR
    public int CountAnim()
    {
        return animator.Items.Count;
    }
    public int LengthContent()
    {
        return content.childCount;
    }
#endif

    public readonly DestroyDelegate destroyDelegate = new DestroyDelegate(DestroyObj);
    static void DestroyObj(Object obj)
    {
        Destroy(obj);
    }
#endregion
}

#region Test Methods
#if UNITY_EDITOR
[CustomEditor(typeof(Generate))]
public class CustomButton : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        Generate generate = (Generate)target;

        if (GUILayout.Button("Shake Cam")) { generate.cam.Shake(5, 0.1f); }
        if (GUILayout.Button("Shake Direction Cam Random")) { generate.cam.ShakeDirection(Random.Range(0, 360), 0.5f, 10); }
        if (GUILayout.Button("Shake Deep Cam Random")) { generate.cam.ShakeDeep(-1, 10); }

        if (GUILayout.Button("Generate Bullet"))
        {
            FloatMod size = new FloatMod(0.2f, 2f);
            Vector2 spawn = generate.OuterPos(size.Get(), Direction.Right);
            Bullet bullet = new Bullet(new FloatMod(0.1f, 0.3f), size, SpriteType.Circle, new FloatMod(2f), new Vector2Mod(spawn), new Vector2Mod(-1, 0));
            generate.Bullet(bullet);
        }

        if (GUILayout.Button("Generate Bullet Explosion"))
        {
            FloatMod size = new FloatMod(0.1f);
            FloatMod sizeExp = new FloatMod(2f);
            BulletExplosion bullet = new BulletExplosion(size, new FloatMod(2), SpriteType.Circle, sizeExp, 0.5f, 0.5f, 1.5f, 0.5f, new CamMod(ModCam.None), new Vector2Mod(generate.OuterPos(size.Get())), new Vector2Mod(generate.InnerPos(sizeExp.Get())));
            generate.BulletExplosion(bullet);
        }

        if (GUILayout.Button("Generate Bullet Division"))
        {
            FloatMod size = new FloatMod(1.5f);
            Bullet bullet = new Bullet(new FloatMod(1), new FloatMod(0.5f), SpriteType.Circle, new FloatMod(0));
            BulletDivision division = new BulletDivision(size, new FloatMod(3), SpriteType.CircleBO, new IntMod[] { new IntMod(10) }, bullet, 0.5f, 1f, 0f, 0f, new CamMod(ModCam.None), new Vector2Mod(generate.OuterPos(size.Get())), new Vector2Mod(generate.InnerPos(size.Get())));
            generate.BulletDivision(division);
        }

        if (GUILayout.Button("Generate Row Bullet"))
        {
            IntMod count = new IntMod(10);
            FloatMod sizeBullet = new FloatMod(0.2f, 2f);
            Vector2 spawn = generate.OuterPos(sizeBullet.Get() * count.Get(), Direction.Right);
            BulletRow bulletRow = new BulletRow(count, new FloatMod(0.1f, 0.5f), sizeBullet, new Vector2Mod(spawn), new Vector2Mod(-1, 0));
            generate.BulletRow(bulletRow);
        }

        if (GUILayout.Button("Generate Money"))
        { generate.Money(generate.player.localPosition); }

        if (GUILayout.Button("Generate Line Simple"))
        {
            LineSimple line = new LineSimple(new FloatMod(0.5f), new FloatMod(0), 1.5f, 0.75f, 0.25f, new CamMod(ModCam.None), new Vector2Mod(generate.InnerPos(2f)), new FloatMod(Random.Range(0, 360)));
            generate.LineSimple(line);
        }

        if (GUILayout.Button("Generate Circle Simple"))
        {
            FloatMod size = new FloatMod(1f, 5f);
            CircleSimple circle = new CircleSimple(size, new FloatMod(0), new FloatMod(3), 2f, 0.75f, 0.25f, new CamMod(ModCam.None), new Vector2Mod(generate.InnerPos(size.Get())));
            generate.CircleSimple(circle);
        }

        if (GUILayout.Button("Generate Standard Gun"))
        {
            IntMod i = new IntMod(3);
            Vector2 spawnPos = generate.OuterPos(2f, Direction.Left);
            Bullet bullet = new Bullet(new FloatMod(1), new FloatMod(0.2f), SpriteType.Square, new FloatMod(0));
            StandardGun gun = new StandardGun(new FloatMod(2), new FloatMod(0.5f), new FloatMod(0.1f), new FloatMod(2), new IntMod[] { i, i, i, i, i, i}, bullet, 0.5f, 0.5f, 0.5f, 0.2f, 0.5f, new Vector2Mod(spawnPos.x, spawnPos.y), new Vector2Mod(-6f, spawnPos.y), new Vector2Mod(-1f, spawnPos.y), new Vector2Mod(-spawnPos.x, spawnPos.y));
            generate.StandardGun(gun);
        }

        if (GUILayout.Button("Generate Square Snake"))
        {
            AnimationCurve curve = new AnimationCurve(new Keyframe(0, 1), new Keyframe(0.3f, 0));
            AnimSprite anim = new AnimSprite(curve, SpriteType.Square, new Color(1, 0.2f, 0.3f, 1), new Color(1, 1, 1, 1), new FloatMod(0), new FloatMod(1), new FloatMod(0.1f), new Vector2Mod(Direction.Right), 90);
            SquareSnake snake = new SquareSnake(new FloatMod(0.1f), new FloatMod(2), new FloatMod(0.1f), new FloatMod(0.5f), anim, new Vector2Mod(Direction.Right), new FloatMod(90f));
            generate.SquareSnake(snake);
        }

        if (GUILayout.Button("Generate Pillar"))
        {
            Pillar pillar = new Pillar(new FloatMod(5), new FloatMod(0.2f), Direction.Random, 2f, 1f, new CamMod(ModCam.None), new Vector2Mod());
            generate.Pillar(pillar);
        }

        if (Application.isPlaying) 
        { 
            GUILayout.TextField("Items of anim: " + generate.CountAnim());
            GUILayout.TextField("Active gameobjects: " + generate.LengthContent());
        }
        else 
        {
            GUILayout.TextField("Items of anim: ");
            GUILayout.TextField("Active gameobjects: ");
        }
    }
}
#endif
#endregion
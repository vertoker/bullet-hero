/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Jobs;
using Unity.Mathematics;
using Unity.Jobs;
using Unity.Collections;
using Unity.Burst;

public class Testing : MonoBehaviour {

    public int count;
    public Transform block;
    public ColliderPlayer player;
    private List<Block> blockList;

    public class Block {
        public Transform transform;
        public bool isCollider;
        public SpriteRenderer sr;
        public float colorSpeed;
        public Vector2 moveSpeed;
        public float rotSpeed;
        public Vector2 scaSpeed;
    }

    private void Start() {
        blockList = new List<Block>();
        for (int i = 0; i < count; i++) {
            Transform tr = Instantiate(block, new Vector3(UnityEngine.Random.Range(-8f, 8f), UnityEngine.Random.Range(-4f, 4f)), Quaternion.identity);
            blockList.Add(new Block
            {
                transform = tr,
                sr = tr.GetComponent<SpriteRenderer>(), isCollider = true,
                colorSpeed = UnityEngine.Random.Range(-0.05f, 0.05f),
                moveSpeed = new Vector2(UnityEngine.Random.Range(-3f, 3f), UnityEngine.Random.Range(-3f, 3f)),
                rotSpeed = UnityEngine.Random.Range(-5f, 5f),
                scaSpeed = new Vector2(UnityEngine.Random.Range(-3f, 3f), UnityEngine.Random.Range(-3f, 3f))
            });
            blockList[i].sr.color = new Color(1f, UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), 1f);
        }
    }

    private void FixedUpdate()
    {
        NativeArray<bool> isColliderArray = new NativeArray<bool>(count, Allocator.TempJob);
        NativeArray<Vector2> moveSpeedArray = new NativeArray<Vector2>(count, Allocator.TempJob);
        NativeArray<float> rotSpeedArray = new NativeArray<float>(count, Allocator.TempJob);
        NativeArray<Vector2> scaleSpeedArray = new NativeArray<Vector2>(count, Allocator.TempJob);
        TransformAccessArray transformAccessArray = new TransformAccessArray(count);

        for (int i = 0; i < count; i++)
        {
            isColliderArray[i] = blockList[i].isCollider;
            moveSpeedArray[i] = blockList[i].moveSpeed;
            rotSpeedArray[i] = blockList[i].rotSpeed;
            scaleSpeedArray[i] = blockList[i].scaSpeed;
            transformAccessArray.Add(blockList[i].transform);
        }

        MainJobSystemParallelBlock reallyToughParallelJobTransforms = new MainJobSystemParallelBlock
        {
            deltaTime = Time.fixedDeltaTime, playerPos = player.GetPos(),
            moveSpeedArray = moveSpeedArray, rotSpeedArray = rotSpeedArray, 
            scaleSpeedArray = scaleSpeedArray, isColliderArray = isColliderArray
        };

        JobHandle jobHandle = reallyToughParallelJobTransforms.Schedule(transformAccessArray);
        jobHandle.Complete();

        bool isCollided = false;
        for (int i = 0; i < count; i++)
        {
            blockList[i].moveSpeed = moveSpeedArray[i];
            blockList[i].rotSpeed = rotSpeedArray[i];
            blockList[i].scaSpeed = scaleSpeedArray[i];

            if (!isCollided && isColliderArray[i]) 
            {
                isCollided = true;
                player.Trigger();
            }

            /// Not used C# Job System
            float speed = blockList[i].colorSpeed;
            float power = blockList[i].sr.color.g;
            if (power >= 1f) { blockList[i].colorSpeed = -math.abs(speed); }
            else if (power <= 0f) { blockList[i].colorSpeed = math.abs(speed); }
            blockList[i].sr.color = new Color(1f, power + speed, power + speed, 1f);
            /// Extremly lagging (seeing not actually)
        }

        isColliderArray.Dispose();
        moveSpeedArray.Dispose();
        rotSpeedArray.Dispose();
        scaleSpeedArray.Dispose();
        transformAccessArray.Dispose();
    }
}

[BurstCompile]
public struct MainJobSystemParallelBlock : IJobParallelForTransform
{
    public NativeArray<bool> isColliderArray;
    public NativeArray<Vector2> moveSpeedArray;
    public NativeArray<float> rotSpeedArray;
    public NativeArray<Vector2> scaleSpeedArray;
    [ReadOnly] public float deltaTime;
    [ReadOnly] public Vector3 playerPos;
    private const float Rad2Deg = 57.295779513f;

    public void Execute(int index, TransformAccess transform) 
    {
        //Position
        transform.localPosition += (Vector3)(moveSpeedArray[index] * deltaTime);
        Vector2 pos = moveSpeedArray[index];
        if (transform.localPosition.x > 11.5f) { pos = new Vector2(-math.abs(pos.x), pos.y); }
        else if (transform.localPosition.x < -11.5f) { pos = new Vector2(+math.abs(pos.x), pos.y); }
        if (transform.localPosition.y > 4.5f) { pos = new Vector2(pos.x, -math.abs(pos.y)); }
        else if (transform.localPosition.y < -4.5f) { pos = new Vector2(pos.x, +math.abs(pos.y)); }
        moveSpeedArray[index] = pos;

        //Rotation
        float rotZ = transform.localRotation.eulerAngles.z;
        transform.localRotation = Quaternion.Euler(0, 0, rotZ + rotSpeedArray[index] * deltaTime);

        //Scale
        transform.localScale += (Vector3)(scaleSpeedArray[index] * deltaTime);
        Vector2 sca = scaleSpeedArray[index];
        if (transform.localScale.x > 3f) { sca = new Vector2(-math.abs(sca.x), sca.y); }
        else if (transform.localScale.x < 0.3f) { sca = new Vector2(+math.abs(sca.x), sca.y); }
        if (transform.localScale.y > 3f) { sca = new Vector2(sca.x, -math.abs(sca.y)); }
        else if (transform.localScale.y < 0.3f) { sca = new Vector2(sca.x, +math.abs(sca.y)); }
        scaleSpeedArray[index] = sca;
        
        //Custom collider-triggers (holy shit it works, hahahahaha...)
        
        if (isColliderArray[index])
        {
            isColliderArray[index] = false;
            sca = new float2(transform.localScale.x, transform.localScale.y);
            if (CollisionDetectionCircleRectangle(playerPos, GetRadiusPlayer(), transform.localPosition, sca.x, sca.y, transform.localRotation.eulerAngles.z))
            { isColliderArray[index] = true; }
        }

        bool CollisionDetectionCircleRectangle(Vector2 player, float radius, Vector2 posLocal, float widthX, float heightY, float rotZLocal)// No angle
        {
            player = RotateVector(player - posLocal, -rotZLocal);

            float distanceX = Mathf.Abs(player.x);
            float distanceY = Mathf.Abs(player.y);

            if (distanceX > (widthX / 2 + radius)) { return false; }
            if (distanceY > (heightY / 2 + radius)) { return false; }
            if (distanceX <= (widthX / 2)) { return true; }
            if (distanceY <= (heightY / 2)) { return true; }

            float cDist_sqX = distanceX - widthX / 2;
            float cDist_sqY = distanceY - heightY / 2;
            float cDist_sq = cDist_sqX * cDist_sqX + cDist_sqY * cDist_sqY;

            return cDist_sq <= radius * radius;

            Vector2 RotateVector(Vector2 a, float offsetAngle)//метод вращения объекта
            {
                float power = Mathf.Sqrt(a.x * a.x + a.y * a.y);//коэффициент силы
                float angle = Mathf.Atan2(a.y, a.x) * Mathf.Rad2Deg - 90f + offsetAngle;//угол из координат с offset'ом
                return Quaternion.Euler(0, 0, angle) * Vector2.up * power;
                //построение вектора из изменённого угла с коэффициентом силы
            }
        }
        float GetRadiusPlayer() { return 0.5f; }
    }
}
using System.Collections;
using UnityEngine;
using Custom;

public class Cam : GlobalBehaviour
{
    public AnimationCurve curve;
    public Camera cam;
    private Transform tr;

    void Start()
    {
        tr = transform;
    }

    public void Shake(int countShakes, float radius, int fts = 1)//fts - Frame Per Shake
    {
        float sps = Anim.fpsTime60 * fts;//sps - Shake Per Seconds
        StartCoroutine(ShakeFrame());
        IEnumerator ShakeFrame()
        {
            yield return new WaitForSeconds(sps);
            Vector2 rotate = Quaternion.Euler(0, 0, Random.Range(0, 360)) * Vector2.up * radius;
            tr.localPosition = rotate;
            if (countShakes >= 0) 
            {
                countShakes--;
                StartCoroutine(ShakeFrame()); 
            }
            else { tr.localPosition = Vector2.zero; }
        }
    }

    public void ShakeDirection(float angle, float radius, int frames)
    {
        int counter = 0; 
        Vector2 direction = Quaternion.Euler(0, 0, angle) * Vector2.up * radius;
        StartCoroutine(ShakeFrame());
        IEnumerator ShakeFrame()
        {
            yield return new WaitForSeconds(Anim.fpsTime60);
            counter++; float progress = counter / (float)frames;
            tr.localPosition = direction * curve.Evaluate(progress);
            if (progress < 1f) { StartCoroutine(ShakeFrame()); }
            else { tr.localPosition = Vector2.zero; }
        }
    }

    public void ShakeDeep(float power, int frames)
    {
        int counter = 0;
        float size = cam.orthographicSize;
        StartCoroutine(ShakeFrame());
        IEnumerator ShakeFrame()
        {
            yield return new WaitForSeconds(Anim.fpsTime60);
            counter++; float progress = counter / (float)frames;
            cam.orthographicSize = size + curve.Evaluate(progress) * power;
            if (progress < 1f) { StartCoroutine(ShakeFrame()); }
            else { cam.orthographicSize = size; }
        }
    }
}

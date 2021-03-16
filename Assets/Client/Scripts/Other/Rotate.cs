using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float speed = 1f;
    private Transform tr;

    private void Awake()
    {
        tr = GetComponent<Transform>();
    }

    private void FixedUpdate()
    {
        tr.localEulerAngles = new Vector3(0f, 0f, tr.localEulerAngles.z + speed);
    }
}
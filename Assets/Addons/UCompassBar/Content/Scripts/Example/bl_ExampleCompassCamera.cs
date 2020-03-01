using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bl_ExampleCompassCamera : MonoBehaviour
{

    public AnimationCurve Curve;
    public float Speed = 1;
    private float value;
    private float time;

    private void Update()
    {
        Vector3 v = transform.eulerAngles;
        time += Time.deltaTime * Speed;
        value = Curve.Evaluate(time);
        float angle = 360 * value;
        v.y = angle;
        transform.eulerAngles = v;
    }
}
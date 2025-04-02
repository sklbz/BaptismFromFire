using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CameraShake : MonoBehaviour
{
    Transform cam;
    readonly float amplitude = 3f;
    float angle;
    void Awake()
    {
        cam = Camera.main.transform;
    }

    void FixedUpdate() {
        SingleShake();
    }

    public void Invert() {
        angle -= Mathf.PI;
    }

    public void Shake() {
        Vector3 direction = new(Mathf.Cos(angle), Mathf.Sin(angle), 0);

        Debug.Log(direction);

        cam.position += amplitude * Time.fixedDeltaTime * direction;
    }

    public void Randomize() {
        angle = Random.Range(-Mathf.PI, Mathf.PI);
    }

    public void SingleShake() {
        Randomize();
        Shake();
    }
}

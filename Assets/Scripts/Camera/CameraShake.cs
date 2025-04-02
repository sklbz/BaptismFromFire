using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CameraShake : MonoBehaviour
{
    Transform cam;
    float amplitude = 5f;
    float angle;
    void Awake()
    {
        cam = Camera.main.transform;
    }

    void FixedUpdate() {
        amplitude *= 0.5f;
        //SingleShake();

        amplitude *= 2;
        DoubleShake();
    }

    public void DoubleShake() {
        angle = Random.Range(-Mathf.PI, Mathf.PI);

        Shake();


        Shake();
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
        angle = Random.Range(-Mathf.PI, Mathf.PI);

        Shake();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonScript : MonoBehaviour
{
    [SerializeField]
    Vector2 direction;
    [SerializeField]
    float distance;
    bool isPlayerAligned;

    void Update()
    {
        isPlayerAligned = Physics2D.Raycast(transform.position, direction, distance);
    }
}

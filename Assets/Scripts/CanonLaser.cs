using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonLaser : MonoBehaviour
{
    [SerializeField]
    Vector2 direction;
    [SerializeField]
    float distance;
    LayerMask playerLayer;
    bool isPlayerAligned;

    private void Start() {
        playerLayer = LayerMask.GetMask("Player");
    }

    void Update()
    {
        isPlayerAligned = Physics2D.Raycast(transform.position, direction, distance, playerLayer);
    }
}

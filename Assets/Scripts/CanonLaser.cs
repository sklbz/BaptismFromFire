using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonLaser : MonoBehaviour
{
    [SerializeField]
    Vector2 direction;

    LayerMask playerLayer;
    bool isPlayerAligned;
    bool _isVisible;

    private void Start() {
        playerLayer = LayerMask.GetMask("Player");
    }

    void Update()
    {
        isPlayerAligned = Physics2D.Raycast(transform.position, direction, playerLayer);

        if (!_isVisible)
            return;

    }

    void OnBecameVisible() {
        _isVisible = true;
    }

    void OnBecameInvisible() {
        _isVisible = false;
    }
}

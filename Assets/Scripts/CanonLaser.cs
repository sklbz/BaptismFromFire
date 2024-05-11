using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CanonLaser : MonoBehaviour
{
    [SerializeField]
    Vector2 direction;

    [SerializeField]
    GameObject laserPrefab, particlePrefab;

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

        if (isPlayerAligned)

    }

    void OnBecameVisible() {
        _isVisible = true;
    }

    void OnBecameInvisible() {
        _isVisible = false;
    }

    void ShootingProcedure() {
         Instantiate(particlePrefab, transform.position, Quaternion.identity);
        Instantiate(laserPrefab, transform.position, Quaternion.identity);
    }
}

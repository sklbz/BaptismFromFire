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
    bool _isShooting;

    private void Start() {
        playerLayer = LayerMask.GetMask("Player");
    }

    void Update()
    {
        if (!_isVisible || _isShooting)
            return;

        isPlayerAligned = Physics2D.Raycast(transform.position, direction, playerLayer);

        if (isPlayerAligned)
            ShootingProcedure();
    }

    void OnBecameVisible() {
        _isVisible = true;
    }

    void OnBecameInvisible() {
        _isVisible = false;
    }

    void ShootingProcedure() {
        StartCoroutine(Cooldown());
        Quaternion rotation = Quaternion.FromToRotation(Vector2.zero, direction);
        Instantiate(particlePrefab, transform.position, rotation);
        Instantiate(laserPrefab, transform.position, rotation);
    }

    IEnumerator Cooldown() {
        _isShooting = true;
        yield return new WaitForSeconds( 5 * Time.timeScale );
        _isShooting = false;
    }
}

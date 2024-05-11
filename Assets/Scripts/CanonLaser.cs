using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CanonLaser : MonoBehaviour
{
    [SerializeField]
    Vector2 direction;

    [SerializeField]
    GameObject laserPrefab, particlePrefab, flashPrefab;

    Transform laserSpot;

    LayerMask playerLayer;
    bool isPlayerAligned;
    bool _isVisible;
    bool _isShooting;

    private void Start() {
        laserSpot = GetComponentInChildren<LaserSpot>().transform;
        playerLayer = LayerMask.GetMask("Player");
    }

    void Update()
    {
        Debug.Log(Physics2D.Raycast(transform.position, direction, playerLayer));

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
        Shoot();
    }

    void Shoot() {
        Quaternion rotation = Quaternion.identity;
        Vector2 position = laserSpot.position;
        GenerateParticle(position, rotation);
        StartCoroutine(GenerateLaser(position, rotation));
    }

    void GenerateParticle(Vector2 position, Quaternion rotation) {
        Instantiate(particlePrefab, position, rotation, laserSpot);
    }

    IEnumerator GenerateLaser(Vector2 position, Quaternion rotation) {
        yield return new WaitForSeconds(1.9f);
        
        Instantiate(flashPrefab, position, rotation, laserSpot);

        yield return new WaitForSeconds(.1f);

        LineRenderer laserBeam = Instantiate(laserPrefab, position, rotation, laserSpot).GetComponent<LineRenderer>();
        laserBeam.SetPosition(1, direction * 20);

        isPlayerAligned = Physics2D.Raycast(transform.position, direction, playerLayer);

        if (isPlayerAligned)
            Kill();
    }

    void Kill() {
        GameObject.FindObjectOfType<health>().Resurrect();
    }

    IEnumerator Cooldown() {
        _isShooting = true;
        yield return new WaitForSecondsRealtime(5);
        _isShooting = false;
    }
}

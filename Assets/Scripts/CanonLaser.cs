using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CanonLaser : MonoBehaviour
{
    [SerializeField]
    Vector2 direction;

    [SerializeField]
    GameObject laserPrefab, particlePrefab, flashPrefab, boomPrefab;

    Transform laserSpot, player;

    LayerMask playerLayer;
    bool isPlayerAligned;
    bool _isVisible;
    bool _isShooting;

    
    private void Start() {
        laserSpot = GetComponentInChildren<LaserSpot>().transform;
        playerLayer = LayerMask.GetMask("Player");
        player = FindObjectOfType<health>().transform;
    }

    void Update()
    {
        if (!_isVisible || _isShooting)
            return;

        isPlayerAligned = PlayerAligned(); 

        Debug.Log(isPlayerAligned);

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
        laserBeam.SetPosition(1, direction * 30);

        isPlayerAligned = PlayerAligned();

        if (isPlayerAligned)
            Kill();
    }

    void Kill() {
        player.gameObject.SetActive(false);
        Instantiate(boomPrefab, player.position, Quaternion.identity);

        StartCoroutine(Restart());
    }

    IEnumerator Restart() {
        yield return new WaitForSecondsRealtime(2f);
        player.GetComponent<health>().Resurrect();
    }

    IEnumerator Cooldown() {
        _isShooting = true;
        yield return new WaitForSecondsRealtime(5);
        _isShooting = false;
    }

    bool PlayerAligned() {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 300, playerLayer);
        bool RaycastSucces = (bool) hit;
        return RaycastSucces && hit.collider.CompareTag("Player");
    }
}

using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections.Generic;

public class Health : MonoBehaviour {
    int _healthPoints = 60;
    public int healthPoints {
        set {
            _healthPoints = Mathf.Clamp(value, 0, 60);
        }
        get {
            return _healthPoints;
        }
    }
    public float timer;
    Vector3[] restartPos = new Vector3[3];
    int _restartIndex = 0;
    int restartIndex {
        get {
            return _restartIndex;
        }
        set {
            originalRestart = value;
            _restartIndex = Mathf.Clamp(value, 0, 4);
        }
    }
    int originalRestart;

    Light2D lightsource;

    float maxLight = 2f, minLight = 0f, lightInterp = .5f;

    Rigidbody2D _rb;


    Image darkScreen;
    Color overlay;
    float overlayAlpha = 0f;

    Character character;
    CharacterController characterController;


    void Awake() {
        character = GetComponent<Character>();
        characterController = GetComponent<CharacterController>();
        _rb = GetComponent<Rigidbody2D>();
        lightsource = GetComponent<Light2D>();

        darkScreen = GameObject.Find("Dark Screen").GetComponent<Image>();
        overlay = darkScreen.color;
        overlay.a = overlayAlpha;
        darkScreen.color = overlay;


        restartPos[0] = new (-3, 3, 0);
        restartPos[1] = new (31, 55, 0);
        restartPos[2] = new (55, 145, 0);

    }

    void Update() {
        UpdateLight();
        if (healthPoints == 0)
        {
            Resurrect();
            return;
        }

        timer += Time.deltaTime;
        if (timer >= Time.timeScale)
        {
            RemoveHealth();
            timer = 0f;
        }
    }

    void UpdateLight() {
        if (healthPoints == 0)
            return;

        float desiredLight = Mathf.Lerp(minLight, maxLight, healthPoints * 0.017f);
        lightsource.intensity = Mathf.Lerp(lightsource.intensity, desiredLight, lightInterp);
    }

    public void Resurrect() {
        _rb.velocity = Vector2.zero;

        gameObject.SetActive(true);
        lightsource.intensity = 0;
        character.Die();

        StartCoroutine(RevivalAnim());
    }

    IEnumerator RevivalAnim() {
        characterController.canMove = false;

        yield return new WaitForSecondsRealtime(.2f);

        Retry();

        yield return new WaitForSecondsRealtime(.05f);

        DarkenScreen();


        while (overlayAlpha > 0.01f)
        {
            if (overlayAlpha < 0.3f)
                characterController.ReturnToIdle();

            FadeOverlay();
            yield return null;
        }

        ResetOverlay();
    }

    void RemoveHealth() {
        healthPoints--;
    }

    void OnTriggerEnter2D(Collider2D coll) {
        if (coll.gameObject.CompareTag("Heal"))
        {
            healthPoints += 10;
            Destroy(coll.gameObject);
        }

    }

    void OnTriggerStay2D(Collider2D coll) {
        if (coll.gameObject.CompareTag("Respawn"))
        {
            HandleHealZone();
        }
        if (coll.gameObject.CompareTag("CheckpointDoubleJump"))
        {
            HandleHealZone();

            characterController.canDoubleJump = true;

            if (originalRestart < 0 && restartIndex <= 1)
                restartIndex = 1;
        }
        if (coll.gameObject.CompareTag("CheckpointDash"))
        {
            HandleHealZone();

            if (restartIndex <= 1)
                restartIndex = 2;
        }
        if (coll.gameObject.CompareTag("CheckpointTripleJump"))
        {
            HandleHealZone();

            if (restartIndex < 2)
                restartIndex = 3;
        }
        if (coll.gameObject.CompareTag("CheckpointHover"))
        {
            HandleHealZone();

            if (restartIndex < 3)
                restartIndex = 4;
        }
    }

    void HandleHealZone() {
        healthPoints = 60;
        timer = 0f;
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.layer == 7)
            healthPoints = 0;
    }

    void DarkenScreen() {
        overlayAlpha = 1f;

        overlay.a = overlayAlpha;
        darkScreen.color = overlay;
    }

    void FadeOverlay() {
        overlayAlpha = Mathf.Lerp(overlayAlpha, 0f, Time.unscaledDeltaTime * 0.5f);
        float alpha = 1f - Mathf.Pow(1f - overlayAlpha, 2f);

        overlay.a = alpha;
        darkScreen.color = overlay;
    }

    void ResetOverlay() {
        overlayAlpha = 0f;

        overlay.a = overlayAlpha;
        darkScreen.color = overlay;
    }

    void ResetPosition() {
        transform.position = restartPos[restartIndex];
        healthPoints = 60;
    }

    void Retry() {
        ResetPosition();
        characterController.Spawn();
        character.Resurect();
        characterController.canMove = true;
    }
}

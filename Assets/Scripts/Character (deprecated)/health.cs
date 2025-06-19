using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Assertions.Must;

public class health : MonoBehaviour {
    public float totalTime;
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
    [SerializeField]
    Vector3[] restartPos;
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

    CharacterJump charJump;
    PlayerController charController;
    hover charHover;
    Light2D lightsource;

    float maxLight = 2f, minLight = 0f, lightInterp = .5f;

    [SerializeField]
    public Volume postProcess;
    Bloom bloom;

    Rigidbody2D _rb;

    Animator _anim;

    //CameraShake camera;

    Image darkScreen;
    Color overlay;
    float overlayAlpha = 0f;

    CharacterController characterController;

    void Awake() 
    {
        charJump = GetComponent<CharacterJump>();
        charController = GetComponent<PlayerController>();
        characterController = GetComponent<CharacterController>();
        charHover = GetComponent<hover>();

        postProcess.profile.TryGet(out bloom);

        _rb = GetComponent<Rigidbody2D>();

        _anim = GetComponent<Animator>();

        lightsource = GetComponent<Light2D>();

        //camera = Camera.main.GetComponent<CameraShake>();

        // darkScreen = GameObject.Find("Dark Screen").GetComponent<Image>();
        // darkScreen.color = darkScreen.color.WithAlpha(overlayAlpha); 
    }

    void Update()
    {
        totalTime += Time.unscaledDeltaTime;
        UpdateLight();
        if (healthPoints == 0)
        {
            Resurrect();
            return;
        }

        timer += Time.deltaTime;
        if(timer >= Time.timeScale)
        {
            RemoveHealth();
            timer = 0f;
        }
    }

    void UpdateLight() {
        float desiredLight = Mathf.Lerp(minLight, maxLight, healthPoints * 0.017f);
        lightsource.intensity = Mathf.Lerp(lightsource.intensity, desiredLight, lightInterp);
    }

    public void Resurrect() {
        totalTime = 0f;
        _rb.velocity = Vector2.zero;
        gameObject.SetActive(true);

        // This may be useful for something else,
        // such as the very beginning, or the very end,
        // but as is, it is too violent
        // StartCoroutine(LongTransition());

        // this one might be better
        //StartCoroutine(RevivalTransition());

        // this is probably the best
        StartCoroutine(RevivalAnim());

        // people seem to dislike this
        //restartIndex--;

    }

    IEnumerator LongTransition()
    {
        yield return new WaitForSecondsRealtime(0.2f);

        bloom.intensity.value = 15f;
        bloom.threshold.value = 0f;

        while (bloom.threshold.value < 0.5f)
        {
            bloom.intensity.value = Mathf.Lerp(bloom.intensity.value, 8f, Time.unscaledDeltaTime / 12f);
            bloom.threshold.value = Mathf.Lerp(bloom.threshold.value, 0.55f, Time.unscaledDeltaTime / 15f);
            yield return null;
        }

        bloom.intensity.value = 8f;
        bloom.threshold.value = 0.55f;
    }

    IEnumerator RevivalTransition() {

        bloom.intensity.value = 100f;
        bloom.threshold.value = 0f;

        while (bloom.threshold.value < 0.3f)
        {
            bloom.intensity.value = Mathf.Lerp(bloom.intensity.value, 8f, Time.unscaledDeltaTime * .2f);
            bloom.threshold.value = Mathf.Lerp(bloom.threshold.value, 0.55f, Time.unscaledDeltaTime * .1f);
            yield return null;
        }

        bloom.intensity.value = 8f;
        bloom.threshold.value = 0.55f;
    }

    IEnumerator RevivalAnim() {
        yield return new WaitForSecondsRealtime(.2f);

        transform.position = restartPos[restartIndex];
        healthPoints = 60;

        yield return new WaitForSecondsRealtime(.05f);

        overlayAlpha = 1f;
        // darkScreen.color = darkScreen.color.WithAlpha(overlayAlpha);


        while (overlayAlpha > 0.01f)
        {
            if (overlayAlpha < .5f)
                _anim.SetTrigger("Spawn");


            overlayAlpha = Mathf.Lerp(overlayAlpha, 0f, Time.unscaledDeltaTime * 0.5f);

            float alpha = 1f - Mathf.Pow(1f - overlayAlpha, 2f);

            // darkScreen.color = darkScreen.color.WithAlpha(alpha); 
            yield return null;
        }


        overlayAlpha = 0f;
        // darkScreen.color = darkScreen.color.WithAlpha(overlayAlpha);

        string currentAnim = _anim.GetCurrentAnimatorClipInfo(0)[0].clip.name;

        while (currentAnim == "Spawn")
        {
            currentAnim = _anim.GetCurrentAnimatorClipInfo(0)[0].clip.name;
            yield return null;
        }

        
    }

    void RemoveHealth()
    {
        healthPoints--;
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.gameObject.CompareTag("Heal")){
            healthPoints += 10;
            Destroy(coll.gameObject);
        }
        
    }

    void OnTriggerStay2D(Collider2D coll)
    {
        if(coll.gameObject.CompareTag("Spawnpoint")){
            HandleHealZone();
        }
        if(coll.gameObject.CompareTag("CheckpointDoubleJump")){
            HandleHealZone();

            if (originalRestart < 0 && restartIndex <= 1)
                restartIndex = 1;

            if (charJump.canDoubleJump == true)
                return;
            
            charJump.canDoubleJump = true;
            restartIndex = 1;
        }
        if(coll.gameObject.CompareTag("CheckpointDash")){
            HandleHealZone();

            if (restartIndex <= 1)
                restartIndex = 2;

            if (charController.canDash == true)
                return;
            
            charController.canDash = true;
        }
        if(coll.gameObject.CompareTag("CheckpointTripleJump")){
            HandleHealZone();

            if (restartIndex < 2)
                restartIndex = 3;

            if (charJump.canTripleJump == true)
                return;
            
            charJump.canTripleJump = true;
        }
        if(coll.gameObject.CompareTag("CheckpointHover")){
            HandleHealZone();

            if (restartIndex < 3)
                restartIndex = 4;

            if (charHover.canHover == true)
                return;
            
            charHover.canHover = true;
        }
    }

    void HandleHealZone() 
    {
        healthPoints = 60;
        timer = 0f;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 7)
            healthPoints = 0;
    }
}
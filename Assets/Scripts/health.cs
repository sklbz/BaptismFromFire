using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Assertions.Must;

public class health : MonoBehaviour
{
    public float Hello;
    int _healthPoints = 60;
    public int healthPoints{
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

    [SerializeField]
    public Volume postProcess;
    Bloom bloom;

    Rigidbody2D _rb;

    Animator _anim;

    void Awake() 
    {
        charJump = GetComponent<CharacterJump>();
        charController = GetComponent<PlayerController>();
        charHover = GetComponent<hover>();

        postProcess.profile.TryGet(out bloom);

        _rb = GetComponent<Rigidbody2D>();

        _anim = GetComponent<Animator>();
    }

    void Update()
    {
        Hello += Time.unscaledDeltaTime;
        if (healthPoints == 0)
        {
            Resurrect();
            return;
        }

        timer += Time.deltaTime;
        if(timer >= Time.timeScale)
        {
            removeHealth();
            timer = 0f;
        }
    }

    void Resurrect() {
        GetComponent<PlayerController>().canMove = false;
        Hello = 0f;
        _rb.velocity = Vector2.zero;
        StartCoroutine(RevivalTransition());
        transform.position = restartPos[restartIndex];
        healthPoints = 60;
        restartIndex--;
        _anim.SetTrigger("Spawn");
        GetComponent<PlayerController>().canMove = true;
    }

    IEnumerator RevivalTransition()
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

    void removeHealth()
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

            if (originalRestart < 0)
                restartIndex = 1;

            if (charJump.canDoubleJump == true)
                return;
            
            charJump.canDoubleJump = true;
            restartIndex = 1;
        }
        if(coll.gameObject.CompareTag("CheckpointDash")){
            HandleHealZone();

            if (restartIndex < 1)
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
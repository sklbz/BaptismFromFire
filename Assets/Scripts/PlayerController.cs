using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb2d;
    [SerializeField]
    float speed = 80f;
    public bool canDash = true, canMove = true;
    bool isDashing;
    public float dashingPower, interpolationValue = 100f;
    float dashingVelocity;
    float dashingTime = 0.2f;
    float dashingCooldown = 1f;

    CharacterJump characterJump;

    ChromaticAberration chromatic;
    MotionBlur motionBlur;

    Coroutine chromaticCoroutine;
    Coroutine dashCoroutine;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D> ();
        characterJump = GetComponent<CharacterJump>();

        GetComponent<health>().postProcess.profile.TryGet(out chromatic);
        GetComponent<health>().postProcess.profile.TryGet(out motionBlur);

    }

    void Update()
    {
        if (isDashing)
            return;
        
        if(Input.GetButtonDown("Dash") && canDash)
        {
            dashCoroutine = StartCoroutine(Dash());
        }
    }

    void FixedUpdate()
    {
        if (isDashing)
            return;

        if((Input.GetAxisRaw("Horizontal") != 0 || characterJump._state == CharacterJumpingState.STATE_GROUNDED) && canMove)
            HandleMove();

        if (rb2d.velocity.x < 0) {
            dashingVelocity = -dashingPower; // Se déplace vers la gauche
        } else if (rb2d.velocity.x > 0) {
            dashingVelocity = dashingPower; // Se déplace vers la droite
        } else {
            dashingVelocity = 0; // Aucun mouvement horizontal
        }

    }

    void HandleMove() {
        CharacterJumpingState state = characterJump._state;
        if (state == CharacterJumpingState.STATE_WALL_LEFT || state == CharacterJumpingState.STATE_WALL_RIGHT)
            return;

        float moveHorizontal = Input.GetAxis("Horizontal");

        /*Vector2 movement = new Vector2(moveHorizontal, 0);
        rb2d.AddForce(speed * rb2d.gravityScale * Time.deltaTime / Time.timeScale * movement, ForceMode2D.Force); */
        rb2d.velocity = new Vector2(Mathf.Clamp(speed * rb2d.gravityScale * Time.deltaTime / Time.timeScale * moveHorizontal/* + rb2d.velocity.x*/, -5f, 5f), rb2d.velocity.y);
    }

    private IEnumerator Dash()
    {
        StartCoroutine(MotionBlur());
        chromaticCoroutine = StartCoroutine(ChromaticAberration());
        canDash = false;
        isDashing = true;
        float originalGravity = rb2d.gravityScale;
        rb2d.gravityScale = 0f;
        rb2d.velocity = new Vector2(dashingVelocity * Time.unscaledDeltaTime, 0f);
        float dashTimer = 0f;
        while (dashTimer <= dashingTime)
        {
            if((characterJump._state == CharacterJumpingState.STATE_WALL_LEFT && dashingVelocity <0) || (characterJump._state == CharacterJumpingState.STATE_WALL_RIGHT && dashingVelocity > 0))
            {
                StartCoroutine(ForceEndDash(originalGravity));
                Debug.Log("sdjkfvnsdfjxvnsdfjxc");
                if(dashCoroutine != null)
                    StopCoroutine(dashCoroutine);
                else
                    Debug.Log("WTF");
                yield break;
            }
            dashTimer += Time.deltaTime;
            yield return null;   
        }
        // yield return new WaitForSeconds(dashingTime);
        rb2d.gravityScale = originalGravity;
        isDashing = false;
        StopCoroutine(chromaticCoroutine);
        chromatic.intensity.value = 0.08f;
        yield return new WaitForSeconds(dashingCooldown * Time.timeScale);
        canDash = true;
    }

    private IEnumerator ForceEndDash(float originalGravity)
    {
        rb2d.gravityScale = originalGravity;
        isDashing = false;
        StopCoroutine(chromaticCoroutine);
        chromatic.intensity.value = 0.4f;
        motionBlur.intensity.value = 0.2f;
        yield return new WaitForSeconds(dashingCooldown * Time.timeScale);
        canDash = true;
    }

    IEnumerator MotionBlur()
    {
        float blurValue = motionBlur.intensity.value;

        motionBlur.intensity.value = 1f;
        yield return new WaitForSeconds(dashingTime * Time.timeScale);

        motionBlur.intensity.value = blurValue;
    }

    IEnumerator ChromaticAberration() {
        float timerChromatic = 0f;
        float chromaticValue = chromatic.intensity.value;

        chromatic.intensity.value = 1f;
        yield return new WaitForSeconds(dashingTime * Time.timeScale + dashingCooldown / 2);

        while (chromatic.intensity.value > chromaticValue + 0.05f)
        {
            chromatic.intensity.value = Mathf.Lerp(chromatic.intensity.value, chromaticValue, Time.unscaledDeltaTime * interpolationValue);
            timerChromatic += Time.unscaledDeltaTime;
            yield return null;
        }
        chromatic.intensity.value = chromaticValue;
    }

}
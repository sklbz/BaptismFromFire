using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static StateName;
using static Direction;

[RequireComponent(typeof(Rigidbody2D), typeof(InputHandler))]
public class CharacterController : MonoBehaviour
{
    CharacterState characterState;
    new Rigidbody2D rigidbody;
    [SerializeField]
    LayerMask groundMask, wallMask;
    [SerializeField]
    List<Transform> groundChecks;
    [SerializeField]
    Transform leftCheck, rightCheck;
    float sqrJumpHeight = 2f, jumpForce, jumpFactor = .82f;
    float jumpVelocity, sideModifier = .45f;
    float doubleJumpFactor = 0.8f;
    float speed = 4.55f, speedFactor = 130f;
    float wallJumpDelay = .6f;

    InputHandler input;
    public InputHandler Input {
        get { return input; }
    }


    public bool canMove = true;
    public bool canDoubleJump = false;
    bool canExtraJump = true;

    void Start()
    {

        characterState = new IdleState();
        input = GetComponent<InputHandler>();
        rigidbody = GetComponent<Rigidbody2D>();

        jumpForce = Mathf.Sqrt(-2f * Physics2D.gravity.y * rigidbody.gravityScale * jumpFactor);
        jumpVelocity = sqrJumpHeight * jumpForce;
    }

    void FixedUpdate()
    {
        characterState = characterState.handleInput(this);

        // DEBUG INFO
        /* switch (characterState.getState())
        {
            case DEFAULT:
                Debug.Log("default");

                break;
            case GROUNDED:
                Debug.Log("grounded");

                break;
            case JUMPING:
                Debug.Log("jumping");

                break;
            case DOUBLE_JUMPING:
                Debug.Log("double jumping");

                break;
            case WALL_SLIDING:
                Debug.Log("wall sliding");

                break;
            case DASHING:
                Debug.Log("dashing");

                break;
            default:
                Debug.Log("Unhandled case");

                break;

        } */
    }

    public void Move(float motion) {
        if (!canMove)
            return;

        motion *= speed * speedFactor * Time.unscaledDeltaTime * rigidbody.gravityScale;

        /* if (!IsGrounded())
        {
            float interpolationRatio = Time.deltaTime * 20f;
            float currentVelocity = getVelocity();
            motion = Mathf.Lerp(motion, currentVelocity, interpolationRatio);
        } */

        rigidbody.velocity = new Vector2(motion, rigidbody.velocity.y);
    }

    public void VerticalJump() {
        if (!canMove) return;
        if (!canExtraJump) return;

        float currentVelocity = getVelocity();
        rigidbody.velocity = new Vector2(currentVelocity, jumpVelocity);

        StartCoroutine(ResetDoubleJump());
        StartCoroutine(PreventExtraJump());
    }

    public void DoubleJump() {
        if (!canDoubleJump) return;
        if (!canExtraJump) return;

        float currentVelocity = getVelocity();
        rigidbody.velocity = new Vector2(currentVelocity, jumpVelocity * doubleJumpFactor);
    }

    public void WallJump(Direction direction) {
        if (!canExtraJump) return;
        if (direction != LEFT && direction != RIGHT)
            return;

        StartCoroutine(ResetMove(wallJumpDelay));

        float horizontal_motion = direction == LEFT ? -jumpVelocity : jumpVelocity;

        Vector2 motion = new Vector2(horizontal_motion * sideModifier, jumpVelocity);
        rigidbody.velocity = motion;
    }

    /*
    IEnumerator ResetHorizontalVelocity() {
        while (rigidbody.velocity.y > 0)
            yield return null;

        while (Mathf.Abs(rigidbody.velocity.x) > .1)
        {
            rigidbody.velocity = Vector2.Lerp(rigidbody.velocity, new Vector2(0, rigidbody.velocity.y + Physics2D.gravity.y * Time.deltaTime), Time.deltaTime);
            yield return null;
        }

        rigidbody.velocity = new Vector2(0, rigidbody.velocity.y);
    }
    */


    public bool IsGrounded() {
        bool groundCheckDown = false;
        foreach (Transform groundCheck in groundChecks)
        {
            if (Physics2D.OverlapCircle(groundCheck.position, .1f, groundMask))
            {
                groundCheckDown = true;
                break;
            }
        };
        return groundCheckDown;
    }

    public Direction IsWalled() {
        if (Physics2D.OverlapCircle(leftCheck.position, .1f, wallMask))
            return LEFT;

        if (Physics2D.OverlapCircle(rightCheck.position, .1f, wallMask))
            return RIGHT;

        return NONE;
    }

    public StateName getState() {
        return characterState.getState();
    }

    public void Spawn() {
        characterState = new SpawnState();
    }

    public void ReturnToIdle() {
        if (getState() != DEFAULT)
            return;

        characterState = new IdleState();
    }

    float getVelocity() {
        float currentVelocity = rigidbody.velocity.x;
        return float.IsNaN(currentVelocity) ? 0 : currentVelocity;
    }


    IEnumerator ResetMove(float delay) {
        canMove = false;
        yield return new WaitForSeconds(delay * Time.timeScale);
        canMove = true;
    }

    IEnumerator ResetDoubleJump() {
        bool currentState = canDoubleJump;
        canDoubleJump = false;
        yield return new WaitForEndOfFrame();
        canDoubleJump = currentState;
    }

    IEnumerator PreventExtraJump() {
        canExtraJump = false;
        yield return new WaitForEndOfFrame();
        canExtraJump = true;
    }
}

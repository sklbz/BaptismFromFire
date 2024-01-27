using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterJump : MonoBehaviour
{
    [SerializeField]
    float _jumpForce = 4f, _doubleJumpModifier = .8f, _tripleJumpModifier = .3f, _sideModifier = 2f, _gravityModifier, wallSlidingSpeed, wallSlidingTime, wallJumpDelay = .2f;
    public float gravityModifier {
        get {
            return _gravityModifier;
        }
    }
    Rigidbody2D _rb;
    public CharacterJumpingState _state;
    public LayerMask groundMask;
    [SerializeField]
    Transform groundCheck, leftCheck, rightCheck;

    public bool _doubleJump, canDoubleJump, _tripleJump, canTripleJump;

    CharacterJumpingState prevState;

    PlayerController _controller;
    bool wallSlide, startedCoroutine;

    Animator _anim;

    void Start()
    {
        Debug.Log("jump");
        _rb = GetComponent<Rigidbody2D>();
        _rb.gravityScale = _gravityModifier;
        _controller = GetComponent<PlayerController>();
        _anim = GetComponent<Animator>();
    }

    void Update() 
    {
        if(Input.GetButtonDown("Jump"))
            HandleJump();
        if((_state == CharacterJumpingState.STATE_WALL_LEFT || _state == CharacterJumpingState.STATE_WALL_RIGHT) /*&& !wallSlide && !startedCoroutine*/)
            //wallRideCoroutine = StartCoroutine(WallSliding());
            WallSlide();
/* 
        if((prevState == CharacterJumpingState.STATE_WALL_LEFT || prevState == CharacterJumpingState.STATE_WALL_RIGHT) && wallSlide)
        {
            wallSlide = false;
            StopCoroutine(wallRideCoroutine);
            Debug.Log("Stoped");
        } */

        HandleAnim();
    }

    /*IEnumerator WallSliding()
    {
        startedCoroutine = true;
        wallSlide = true;
        Debug.Log("In");
        float timer = 0f;
        while(timer <= wallSlidingTime)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, Mathf.Clamp(_rb.velocity.y, Mathf.Lerp(wallSlidingSpeed, 0f, timer /wallSlidingTime), float.MaxValue));
            timer += Time.deltaTime;
            yield return null;
            Debug.Log("WallRideUp");
        }
        while(_state == CharacterJumpingState.STATE_WALL_LEFT || _state == CharacterJumpingState.STATE_WALL_RIGHT)
        {
            if ((Input.GetAxisRaw("Horizontal") == -1 && _state == CharacterJumpingState.STATE_WALL_LEFT) || (Input.GetAxisRaw("Horizontal") == 1 && _state == CharacterJumpingState.STATE_WALL_RIGHT))
                yield return null;
                
            _rb.velocity = new Vector2(_rb.velocity.x, Mathf.Clamp(_rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
            yield return null;

        }
        // while(true)
        // {
        // }
        // _rb.velocity = new Vector2(_rb.velocity.x, 0f);
        wallRideCoroutine = null;
        // // wallSlide = false;
        startedCoroutine = false;
    }*/

    private void WallSlide()
    {
        _rb.velocity = new Vector2(_rb.velocity.x, Mathf.Clamp(_rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
    }

    void FixedUpdate()
    {
        HandleJumpingState();
    }

    void HandleJump()
    {
        switch(_state) 
        {
            case CharacterJumpingState.STATE_GROUNDED:
                Jump(_jumpForce);
                prevState = _state;
                _state = CharacterJumpingState.STATE_JUMPING;

                _doubleJump = false;
                _tripleJump = false;
                wallSlide = false;

                break;

            case CharacterJumpingState.STATE_JUMPING:
                if(!canDoubleJump)
                    return;

                Jump(_jumpForce * _doubleJumpModifier);

                prevState = _state;
                _state = CharacterJumpingState.STATE_DOUBLE_JUMPING;
                wallSlide = false;
                _doubleJump = true;
                _tripleJump = false;

                break;

            case CharacterJumpingState.STATE_DOUBLE_JUMPING:
                if(!canTripleJump || _tripleJump)
                    return;

                Jump(_jumpForce * _tripleJumpModifier);

                wallSlide = false;
                _doubleJump = false;
                _tripleJump = true;
                
                break;

            case CharacterJumpingState.STATE_WALL_RIGHT:

                StartCoroutine(ResetMove());
                Jump(_jumpForce, -_jumpForce * _sideModifier);

                prevState = _state;
                _state = CharacterJumpingState.STATE_JUMPING;

                _doubleJump = false;
                _tripleJump = false;

                break;

            case CharacterJumpingState.STATE_WALL_LEFT:

                StartCoroutine(ResetMove());
                Jump(_jumpForce, _jumpForce * _sideModifier);

                prevState = _state;
                _state = CharacterJumpingState.STATE_JUMPING;

                _doubleJump = false;
                _tripleJump = false;

                break;
        }

        HandleAnim();
    }

    void HandleJumpingState()
    {
        if(IsGrounded())
        {
            _state = CharacterJumpingState.STATE_GROUNDED;
            return;
        }

        if(Physics2D.Raycast(leftCheck.position, Vector2.left, 0.1f, groundMask))
        {
            _state = CharacterJumpingState.STATE_WALL_LEFT;
            return;
        }

        if(Physics2D.Raycast(rightCheck.position, Vector2.right, 0.1f, groundMask))
        {
            _state = CharacterJumpingState.STATE_WALL_RIGHT;
            return;
        }

        if(_doubleJump || _tripleJump)
            _state = CharacterJumpingState.STATE_DOUBLE_JUMPING;
        else
            _state = CharacterJumpingState.STATE_JUMPING;
    }

    void Jump(float _jumpHeight, float _sideJump = 0f) {

        float velocityY = Mathf.Sqrt(_jumpHeight * -2f * Physics2D.gravity.y * _rb.gravityScale);
        float velocityX = (_sideJump != 0) ? _sideJump / Mathf.Abs(_sideJump) * Mathf.Sqrt(Mathf.Abs(_sideJump) * 2f) : 0f;
        _rb.velocity = new Vector2(velocityX, velocityY);

        JumpAnim();

        StartCoroutine(ResetHorizontalVelocity());
    }

    void JumpAnim()
    {
        Debug.Log("JumpAnim");
        _anim.SetTrigger("Jump");
    }

    void HandleAnim()
    {
        ResetAnim();
        switch(_state)
        {
            case CharacterJumpingState.STATE_GROUNDED:
                if (_rb.velocity.y != 0)
                    _anim.SetBool("isMoving", true);
                else
                    _anim.SetBool("isIdling", true);

                break;
            case CharacterJumpingState.STATE_WALL_LEFT:
                _anim.SetBool("isWallSlidingLeft", true);

                break;
            case CharacterJumpingState.STATE_WALL_RIGHT:
                _anim.SetBool("isWallSlidingRight", true);

                break;
        }
    }

    void ResetAnim() 
    {
        _anim.SetBool("isMoving", false);
        _anim.SetBool("isWallSlidingLeft", false);
        _anim.SetBool("isWallSlidingRight", false);
        _anim.SetBool("isIdling", false);
    }

    IEnumerator ResetHorizontalVelocity() {
        while(_rb.velocity.y > 0)
            yield return null;
            
        while(Mathf.Abs(_rb.velocity.x) > .1)
        {
            _rb.velocity = Vector2.Lerp(_rb.velocity, new Vector2(0, _rb.velocity.y + Physics2D.gravity.y * Time.deltaTime), Time.deltaTime);
            yield return null;
        }
        
        _rb.velocity = new Vector2(0, _rb.velocity.y);
    }

    IEnumerator ResetMove() {
        _controller.canMove = false;
        yield return new WaitForSeconds(wallJumpDelay * Time.timeScale);
        _controller.canMove = true;

    }

    bool IsGrounded()
    {
        bool groundCheckDown = Physics2D.OverlapCircle(groundCheck.position, .1f, groundMask);
        return groundCheckDown;
    }
}

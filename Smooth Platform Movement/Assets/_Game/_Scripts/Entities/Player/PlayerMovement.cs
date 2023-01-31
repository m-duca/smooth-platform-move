using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Unity Access Fields
    [Header("Horizontal Movement:")]
    [SerializeField] private float maxSpeed;
    [SerializeField] private float acceleration;
    
    [Header("Jump:")] 
    [SerializeField] private float jumpForce;

    [SerializeField] private bool canJump;
    
    // Components
    private Rigidbody2D _rb;
    private SpriteRenderer _spr;
    
    // Horizontal Movement
    private float _moveInput;

    // Jump
    private bool _jumped = false;

    #region Unity
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _spr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        GetMoveInput();
        GetJumpInput();
        FlipSprite();
    }

    private void FixedUpdate()
    {
        ApplyMove();
        if (_jumped)
        {
            _jumped = false;
            ApplyJump();
        }
    }

    private void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Floor")) canJump = true;
    }
    #endregion

    #region Custom
    private void GetMoveInput()
    {
        _moveInput = Input.GetAxisRaw("Horizontal");
    }

    private float SetVelocity(float goal, float curVelocity, float accel)
    {
        var velDifference = goal - curVelocity;

        if (velDifference > accel) return curVelocity + accel;
        if (velDifference < -accel) return curVelocity - accel;

        return goal;
    }
    
    private void ApplyMove()
    {
        _rb.velocity = new Vector2(SetVelocity(maxSpeed * _moveInput, _rb.velocity.x, acceleration), _rb.velocity.y);
    }

    private void GetJumpInput()
    {
        if (Input.GetButton("Jump") && canJump)
        {
            canJump = false;
            _jumped = true;
        }
    }

    private void ApplyJump()
    {
        _rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);   
    }

    private void FlipSprite()
    {
        if (_moveInput == -1f) _spr.flipX = true;
        else if (_moveInput == 1f) _spr.flipX = false;
    }
    #endregion
}

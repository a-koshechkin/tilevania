using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

using static Constants;

public class PlayerMovement : MonoBehaviour
{
    #region Fields

    [SerializeField] private float _runSpeed = 5f;
    [SerializeField] private float _jumpSpeed = 5f;
    [SerializeField] private float _climbSpeed = 5f;
    [SerializeField] private float _deathKick = 10f;
    [SerializeField] private Bullet _bullet;

    private Vector2 _moveInput;
    private Rigidbody2D _rb;
    private Animator _animator;
    private CapsuleCollider2D _bodyCollider;
    private BoxCollider2D _feetCollider;

    private float _defaultGravityScale;
    private readonly float _climbingGravityScale = 0f;

    private PlayerState _playerState = PlayerState.Idling;

    #endregion

    #region MonoBehaviour

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _bodyCollider = GetComponent<CapsuleCollider2D>();
        _feetCollider = GetComponent<BoxCollider2D>();

        _defaultGravityScale = _rb.gravityScale;
    }

    private void Update()
    {
        if (_playerState == PlayerState.Dying)
        {
            return;
        }
        CheckIfFalling(PlayerState.Falling);
        CheckIfRunning(PlayerState.Running);
        CheckIfClimbing(PlayerState.Climbing);
        CheckIfDied();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            var potentialDeathCollider = collision.GetComponent<DeathCollider>();
            if (potentialDeathCollider != null)
            {
                potentialDeathCollider.OnColliderHit(gameObject);
            }
        }
    }

    #endregion

    #region Methods

    private void CheckIfDied()
    {
        if (IsTouchingLayer(new List<Layer>() { Layer.Enemies, Layer.Hazard }, true))
        {
            _bodyCollider.enabled = false;
            _feetCollider.enabled = false;
            _rb.velocity = new Vector2(0, _deathKick);
            _animator.SetTrigger(PlayerStateTags[PlayerState.Dying]);
            UpdateState(PlayerState.Dying);
        }
    }

    private void UpdateState(PlayerState currentPlayerState)
    {
        if (_playerState == currentPlayerState)
        {
            return;
        }

        _playerState = currentPlayerState;
        if (_playerState == PlayerState.Dying)
        {
            return;
        }

        var ignoreStates = new List<PlayerState>() { PlayerState.Idling, PlayerState.Dying, PlayerState.Shooting, currentPlayerState };
        foreach (var state in PlayerStateTags)
        {
            if (!ignoreStates.Contains(state.Key))
            {
                _animator.SetBool(state.Value, false);
            }
        }

        if (currentPlayerState != PlayerState.Idling)
        {
            _animator.SetBool(PlayerStateTags[currentPlayerState], true);
        }
    }

    private void CheckIfClimbing(PlayerState actionState)
    {
        if (IsTouchingLayer(new List<Layer>() { Layer.Climbing }))
        {
            _rb.gravityScale = _climbingGravityScale;
            _rb.velocity = new Vector2(_rb.velocity.x, _moveInput.y * _climbSpeed);
            if (Mathf.Abs(_rb.velocity.y) > Mathf.Epsilon)
            {
                UpdateState(actionState);
                return;
            }
        }
        else
        {
            _rb.gravityScale = _defaultGravityScale;
        }
        ResetToIdleFromSelected(actionState);
    }

    private void ResetToIdleFromSelected(PlayerState playerState)
    {
        if (_playerState == playerState)
        {
            UpdateState(PlayerState.Idling);
        }
    }

    private void CheckIfFalling(PlayerState actionState)
    {
        if (!IsTouchingLayer(new List<Layer>() { Layer.Ground, Layer.Climbing }) && _rb.velocity.y < -Mathf.Epsilon)
        {
            UpdateState(actionState);
            return;
        }
        ResetToIdleFromSelected(actionState);
    }

    private void CheckIfRunning(PlayerState actionState)
    {
        _rb.velocity = new Vector2(_moveInput.x * _runSpeed, _rb.velocity.y);

        if (Mathf.Abs(_rb.velocity.x) > Mathf.Epsilon)
        {
            transform.localScale = new Vector2(Mathf.Abs(transform.localScale.x) * Mathf.Sign(_rb.velocity.x), transform.localScale.y);
            if (_playerState != PlayerState.Falling)
            {
                UpdateState(actionState);
            }
            return;
        }
        ResetToIdleFromSelected(actionState);
    }

    private bool IsTouchingLayer(List<Layer> layers, bool wholeBody = false)
    {
        var valueArray = layers.Select(key => LayerTags[key]).ToArray();
        if (wholeBody)
        {
            return _bodyCollider.IsTouchingLayers(LayerMask.GetMask(valueArray))
            || _feetCollider.IsTouchingLayers(LayerMask.GetMask(valueArray));
        }
        return _feetCollider.IsTouchingLayers(LayerMask.GetMask(valueArray));
    }

    #endregion

    #region PlayerInput

    private void OnMove(InputValue value)
    {
        if (_playerState == PlayerState.Dying)
        {
            return;
        }
        _moveInput = value.Get<Vector2>();
    }

    private void OnJump(InputValue value)
    {
        if (_playerState == PlayerState.Dying)
        {
            return;
        }
        if (!IsTouchingLayer(new List<Layer>() { Layer.Ground, Layer.Climbing, Layer.Bouncing }))
        {
            return;
        }
        if (value.isPressed)
        {
            _rb.velocity += new Vector2(0, _jumpSpeed);
        }
    }

    private void OnFire(InputValue value)
    {
        if (_playerState == PlayerState.Dying)
        {
            return;
        }
        if (value.isPressed)
        {
            _animator.SetTrigger(PlayerStateTags[PlayerState.Shooting]);
            UpdateState(PlayerState.Shooting);
            var gun = transform.GetChild(0).transform;
            var bullet = Instantiate(_bullet, gun.position, Quaternion.identity);
            bullet.transform.localScale = transform.localScale;
        }
    }

    #endregion
}

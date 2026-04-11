using System;
using UnityEngine;
using UnityEngine.Serialization;

public class BirdController : MonoBehaviour
{
    [Header("Basic flying")]
    [SerializeField] private Transform _bird;
    [SerializeField] private float _moveSpeed = 3.0f;
    [SerializeField] private float _acceleration = 3.0f;
    [SerializeField] private float _turnSpeed = 2.0f;
    
    [Header("Yoinking")]
    [SerializeField] private float _diveSpeed = 1.5f;
    [SerializeField] private float _diveHeight = 1.6f;

    private float _cruiseHeight;
    private Vector3 _moveDirection;
    private SpriteRenderer _birdSprite;

    private void Start()
    {
        _birdSprite = GetComponentInChildren<SpriteRenderer>();
        
        _cruiseHeight = _bird.position.y;
        _moveDirection = _bird.forward;
    }

    private void Update()
    {
        UpdateRotation();
        MoveForward();
        UpdateYoink();
    }

    private void UpdateRotation()
    {
        Vector2 moveInput = InputManager.Instance.GetMoveDirection();

        if (moveInput == Vector2.zero)
        {
            return;
        }

        Quaternion goalRotation = Quaternion.LookRotation(new Vector3(moveInput.x, 0.0f, moveInput.y), Vector3.up);
        _bird.rotation = Quaternion.Slerp(_bird.rotation, goalRotation, _turnSpeed * Time.deltaTime);

        // Flip sprite based on orientation
        _birdSprite.flipY = Vector3.Dot(_bird.forward, Vector3.right) > 0.0f;
    }

    private void MoveForward()
    {
        Vector3 forwardVector = _bird.forward;

        _moveDirection = Vector3.Lerp(_moveDirection, _bird.forward, _acceleration * Time.deltaTime);
        transform.Translate(_moveDirection * (_moveSpeed * Time.deltaTime));
    }

    private void UpdateYoink()
    {
        if (InputManager.Instance.GetIsYoinking())
        {
            if (_bird.position.y > (_cruiseHeight - _diveHeight))
            {
                Vector3 position = _bird.position;
                position.y -= _diveSpeed * Time.deltaTime;
                _bird.position = position;
            }
        }
        else if (_bird.position.y < _cruiseHeight)
        {
            Vector3 position = _bird.position;
            position.y += _diveSpeed * Time.deltaTime;

            if (position.y > _cruiseHeight)
            {
                position.y = _cruiseHeight;
            }
            
            _bird.position = position;
        }
    }
}

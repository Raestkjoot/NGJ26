using System;
using UnityEngine;
using UnityEngine.Serialization;

public class BirdController : MonoBehaviour
{
    [Header("Basic flying")]
    [SerializeField] private Transform _birdSprite;
    [SerializeField] private float _rotationSpeed = 160.0f;
    [SerializeField] private float _moveSpeed = 3.0f;
    
    [Header("Yoinking")]
    [SerializeField] private float _diveSpeed = 1.5f;
    [SerializeField] private float _diveHeight = 1.6f;

    private float _cruiseHeight;

    private void Start()
    {
        _cruiseHeight = _birdSprite.position.y;
    }

    private void Update()
    {
        UpdateRotation();
        MoveForward();
        UpdateYoink();
    }

    private void UpdateRotation()
    {
        float sidewayInput = InputManager.Instance.Gameplay.Move.ReadValue<Vector2>().x;

        if (sidewayInput > 0.1f)
        {
            _birdSprite.Rotate(Vector3.up, _rotationSpeed * Time.deltaTime, Space.World);
        }
        else if (sidewayInput < -0.1f)
        {
            _birdSprite.Rotate(Vector3.up, -_rotationSpeed * Time.deltaTime, Space.World);
        }
    }

    private void MoveForward()
    {
        Vector3 forwardVector = _birdSprite.up;
        transform.Translate(forwardVector * (_moveSpeed * Time.deltaTime));
    }

    private void UpdateYoink()
    {
        if (InputManager.Instance.Gameplay.Jump.IsPressed())
        {
            if (_birdSprite.position.y > (_cruiseHeight - _diveHeight))
            {
                Vector3 position = _birdSprite.position;
                position.y -= _diveSpeed * Time.deltaTime;
                _birdSprite.position = position;
            }
        }
        else if (_birdSprite.position.y < _cruiseHeight)
        {
            Vector3 position = _birdSprite.position;
            position.y += _diveSpeed * Time.deltaTime;

            if (position.y > _cruiseHeight)
            {
                position.y = _cruiseHeight;
            }
            
            _birdSprite.position = position;
        }
    }
}

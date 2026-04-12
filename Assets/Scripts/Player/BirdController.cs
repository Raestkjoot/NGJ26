using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdController : MonoBehaviour
{
    [Header("Basic flying")]
    [SerializeField] private Transform _bird;
    [SerializeField] private Transform _birdRotationHandler;
    [SerializeField] private float _moveSpeed = 3.0f;
    [SerializeField] private float _acceleration = 3.0f;
    [SerializeField] private float _turnSpeed = 2.0f;
    
    [Header("Yoinking")]
    [SerializeField] private float _diveSpeed = 1.5f;
    [SerializeField] private float _diveHeight = 1.6f;

    [Header("Visuals")]
    [SerializeField] private List<Sprite> _sprites;
    
    private float _cruiseHeight;
    private Vector3 _moveDirection;
    private SpriteRenderer _birdSprite;
    private bool _isYoinking = false;

    private void Start()
    {
        _birdSprite = GetComponentInChildren<SpriteRenderer>();
        
        _cruiseHeight = _bird.position.y;
        _moveDirection = GetForwardVector();
    }

    private void Update()
    {
        UpdateRotation();
        MoveForward();
        UpdateYoink();
        
        if (InputManager.Instance.GetWasEscPressed())
        {
            MenuButtons.Instance.Reload();
        }
    }

    private void UpdateRotation()
    {
        Vector2 moveInput = InputManager.Instance.GetMoveDirection();

        if (moveInput == Vector2.zero)
        {
            return;
        }

        Quaternion goalRotation = Quaternion.LookRotation(new Vector3(moveInput.x, 0.0f, moveInput.y), Vector3.up);
        _birdRotationHandler.rotation = Quaternion.Slerp(_birdRotationHandler.rotation, goalRotation, _turnSpeed * Time.deltaTime);

        // Update sprite
        float forwardness = Vector3.Dot(_birdRotationHandler.forward, Vector3.forward);
        float rightedness = Vector3.Dot(_birdRotationHandler.forward, Vector3.right);
            
        //_birdSprite.flipX = forwardness > 0.0f;

        if (Mathf.Abs(forwardness) >= Mathf.Abs(rightedness))
        {
            if (forwardness < 0.0f)
            {
                _birdSprite.sprite = _sprites[1];
            }
            else
            {
                _birdSprite.sprite = _sprites[0];
            }
        }
        else
        {
            if (rightedness < 0.0f)
            {
                _birdSprite.sprite = _sprites[3];
            }
            else
            {
                _birdSprite.sprite = _sprites[2];
            }
        }
        
        Debug.Log("Orientation: " + Vector3.Dot(_birdRotationHandler.forward, Vector3.forward));
    }

    private void MoveForward()
    {
        _moveDirection = Vector3.Lerp(_moveDirection, GetForwardVector(), _acceleration * Time.deltaTime);
        transform.Translate(_moveDirection * (_moveSpeed * Time.deltaTime));
    }

    private void UpdateYoink()
    {
        if (!_isYoinking && InputManager.Instance.GetIsYoinking())
        {
            StartCoroutine(Yoink());
        }
    }

    private IEnumerator Yoink()
    {
        _isYoinking = true;
        Vector3 position = _bird.position;
        
        while (_bird.position.y > (_cruiseHeight - _diveHeight))
        {
            position = _bird.position;
            position.y -= _diveSpeed * Time.deltaTime;
            _bird.position = position;
            yield return null;
        }

        while (_bird.position.y < _cruiseHeight)
        {
            position = _bird.position;
            position.y += _diveSpeed * Time.deltaTime;
            
            _bird.position = position;
            yield return null;
        }
        
        position.y = _cruiseHeight;
        _bird.position = position;
        _isYoinking = false;
    }

    private Vector3 GetForwardVector()
    {
        return _birdRotationHandler.forward;
    }
}

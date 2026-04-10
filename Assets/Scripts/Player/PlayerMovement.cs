using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Transform _birdSprite;
    [SerializeField] private float _rotationSpeed = 160.0f;
    [SerializeField] private float _moveSpeed = 10.0f;
    
    private void Update()
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

        Vector3 forwardVector = _birdSprite.up;
        transform.Translate(forwardVector * (_moveSpeed * Time.deltaTime));
    }
}

using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Nest : MonoBehaviour
{
    [SerializeField] private Transform _nestPosition;
    [SerializeField] private float _nestRadius;
    [SerializeField] private float _nestHeight;

    private void OnValidate()
    {
        VisualDebug.Instance.DrawSphere(_nestPosition.position, _nestRadius, Color.rebeccaPurple, 1.0f);
        VisualDebug.Instance.DrawSphere(_nestPosition.position, _nestHeight, Color.brown, 1.0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.TryGetComponent<Yoinker>(out var yoinker) 
                && yoinker.IsCarryingThing())
            {
                Transform thing = yoinker.TakeYoinkable();

                Vector2 randomCircle = Random.insideUnitCircle * _nestRadius;
                Vector3 nestOffsetPosition = new Vector3(randomCircle.x, _nestHeight, randomCircle.y);
                thing.position = _nestPosition.position + nestOffsetPosition;
                thing.parent = transform;
            }
        }
    }
}

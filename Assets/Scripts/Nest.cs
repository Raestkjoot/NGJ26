using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Nest : MonoBehaviour
{
    [SerializeField] private Transform _nestPosition;
    [SerializeField] private float _nestRadius;
    [SerializeField] private float _nestHeight;

    [SerializeField] private Transform _min;
    [SerializeField] private Transform _max;
    [SerializeField] private float _maxThings = 20.0f;
    [SerializeField] private float _sideVariance = 1.0f;
    
    private float _progress = 0.0f;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.TryGetComponent<Yoinker>(out var yoinker) 
                && yoinker.IsCarryingThing())
            {
                AddYoinkable(yoinker.TakeYoinkable());
            }
        }
    }

    private void AddYoinkable(Transform thing)
    {
        _progress++;
        float t = _progress / _maxThings;
        Vector3 nestPoint = Vector3.Lerp(_min.position, _max.position, t);
        nestPoint.x += Random.Range(-_sideVariance, _sideVariance);
        
        thing.position = nestPoint;
        thing.parent = transform;
    }
}

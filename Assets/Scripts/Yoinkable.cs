using System;
using UnityEngine;

public class Yoinkable : MonoBehaviour
{
    public enum YoinkSize
    {
        Small = 0,
        Medium,
        Big,
        VeryBig,
        Biggest
    }

    [SerializeField] private YoinkSize _size;

    public YoinkSize Size => _size;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.TryGetComponent<Yoinker>(out var yoinker) 
                && yoinker.YoinkLevel >= _size)
            {
                yoinker.Yoink(transform);
            }
        }
    }
}

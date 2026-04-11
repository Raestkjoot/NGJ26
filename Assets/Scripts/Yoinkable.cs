using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.TryGetComponent<Yoinker>(out var yoinker) 
                && yoinker.YoinkLevel >= _size)
            {
                Debug.Log("Get yoinked!");
                yoinker.AttachYoinkable(transform);
            }
            else
            {
                Debug.Log("Couldn't yoink ;_;");
            }
        }
    }
}

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
            if (other.transform.parent.TryGetComponent<BirdController>(out var bird) 
                && bird.YoinkLevel >= _size)
            {
                Debug.Log("Get yoinked!");
            }
            else
            {
                Debug.Log("Couldn't yoink ;_;");
            }
        }
    }
}

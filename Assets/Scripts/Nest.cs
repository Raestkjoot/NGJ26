using UnityEngine;

public class Nest : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.TryGetComponent<Yoinker>(out var yoinker) 
                && yoinker.IsCarryingThing())
            {
                Transform thing = yoinker.TakeYoinkable();

                thing.parent = transform;
                thing.localPosition = Vector3.zero;
            }
        }
    }
}

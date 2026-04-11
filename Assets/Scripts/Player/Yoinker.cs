using System;
using UnityEngine;

public class Yoinker : MonoBehaviour
{
    [SerializeField] private Yoinkable.YoinkSize _yoinkLevel;
    [SerializeField] private Transform _yoinkAttachementPoint;

    private Transform _attachedYoinkable = null;
    
    public Yoinkable.YoinkSize YoinkLevel => _yoinkLevel;

    /// <summary>
    /// 
    /// </summary>
    /// <returns> True if successfully yoinked and attached. If already carrying something it will not
    /// carry more and will return false instead. </returns>
    public bool Yoink(Transform yoinkedObject)
    {
        if (IsCarryingThing())
        {
            return false;
        }

        yoinkedObject.GetComponent<Collider>().enabled = false;
        _attachedYoinkable = yoinkedObject;
        _attachedYoinkable.parent = _yoinkAttachementPoint;
        _attachedYoinkable.localPosition = Vector3.zero;
            
        return true;
    }

    public Transform TakeYoinkable()
    {
        Transform yoinkable = null;
        
        if (IsCarryingThing())
        {
            yoinkable = _attachedYoinkable;
            _attachedYoinkable.parent = null;
            _attachedYoinkable = null;
        }
        
        return yoinkable;
    }

    public bool IsCarryingThing()
    {
        return _attachedYoinkable != null;
    }
}

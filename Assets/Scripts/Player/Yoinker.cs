using System;
using UnityEngine;

public class Yoinker : MonoBehaviour
{
    [SerializeField] private Yoinkable.YoinkSize _yoinkLevel;
    [SerializeField] private Transform _yoinkAttachementPoint;

    [SerializeField] private int _smallsToLevelUp = 10;
    [SerializeField] private int _mediumsToLevelUp = 10;

    private int _smallsRegistered = 0;
    private int _mediumsRegistered = 0;
    
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
            var size = _attachedYoinkable.gameObject.GetComponent<Yoinkable>().Size;
            RegisterYoinkedThing(size);
            _attachedYoinkable.parent = null;
            _attachedYoinkable = null;
        }
        
        return yoinkable;
    }

    public bool IsCarryingThing()
    {
        return _attachedYoinkable != null;
    }

    private void RegisterYoinkedThing(Yoinkable.YoinkSize size)
    {
        switch (size)
        {
            case Yoinkable.YoinkSize.Small:
                ++_smallsRegistered;
                if (_smallsRegistered >= _smallsToLevelUp)
                {
                    ++_yoinkLevel;
                }
                break;
            case Yoinkable.YoinkSize.Medium:
                ++_mediumsRegistered;
                if (_mediumsRegistered >= _mediumsToLevelUp)
                {
                    ++_yoinkLevel;
                }
                break;
            default:
                break;
        }
    }
}

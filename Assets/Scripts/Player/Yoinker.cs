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
    public bool AttachYoinkable(Transform yoinkedObject)
    {
        if (IsCarryingThing())
        {
            return false;
        }

        _attachedYoinkable = yoinkedObject;
        _attachedYoinkable.parent = _yoinkAttachementPoint;
        _attachedYoinkable.gameObject.GetComponent<Collider>().enabled = false;
        _attachedYoinkable.gameObject.GetComponent<Rigidbody>().useGravity = false;
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

    public bool DropYoinkable()
    {
        if (!IsCarryingThing())
        {
            return false;
        }

        _attachedYoinkable.gameObject.GetComponent<Collider>().enabled = true;
        _attachedYoinkable.gameObject.GetComponent<Rigidbody>().useGravity = true;
        _attachedYoinkable.parent = null;
        _attachedYoinkable = null;
        return true;
    }

    public bool IsCarryingThing()
    {
        return _attachedYoinkable != null;
    }
}

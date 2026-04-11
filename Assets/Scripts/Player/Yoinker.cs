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
        if (_attachedYoinkable != null)
        {
            return false;
        }

        _attachedYoinkable = yoinkedObject;
        _attachedYoinkable.parent = _yoinkAttachementPoint;
        _attachedYoinkable.localPosition = Vector3.zero;
            
        return true;
    }

    public bool DropYoinkable()
    {
        if (_attachedYoinkable == null)
        {
            return false;
        }

        _attachedYoinkable.parent = null;
        _attachedYoinkable = null;
        return true;
    }
}

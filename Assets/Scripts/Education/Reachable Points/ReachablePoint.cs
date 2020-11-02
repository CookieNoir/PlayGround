using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Collider))]
public class ReachablePoint : MonoBehaviour
{
    public string targetTag = "Player";
    public Transform targetObject = null;

    protected int objectsAtPoint = 0;
    protected delegate bool CheckForMatchesDelegate(ref Transform other);
    protected CheckForMatchesDelegate CheckForMatches;
    protected List<Transform> transforms;

    private void Awake()
    {
        transforms = new List<Transform>();
        Classify();
    }

    private void Classify()
    {
        if (targetObject) CheckForMatches = CheckGameObject;
        else CheckForMatches = CheckTag;
    }

    public void Reclassify(bool useObject)
    {
        if (useObject) CheckForMatches = CheckGameObject;
        else CheckForMatches = CheckTag;
    }

    private void OnEnable()
    {
        ResetReached();
    }

    private bool CheckTag(ref Transform other)
    {
        return other.tag == targetTag;
    }

    private bool CheckGameObject(ref Transform other)
    {
        while (other && other != targetObject)
        {
            other = other.parent;
        }
        return other;
    }

    public bool IsReached()
    {
        return objectsAtPoint > 0;
    }

    public virtual void ResetReached()
    {
        objectsAtPoint = 0;
        if (transforms != null) transforms.Clear();
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        Transform otherTransform = other.transform;
        if (CheckForMatches(ref otherTransform) && !transforms.Contains(otherTransform))
        {
            objectsAtPoint++;
            transforms.Add(otherTransform);
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        Transform otherTransform = other.transform;
        if (CheckForMatches(ref otherTransform) && transforms.Contains(otherTransform))
        {
            objectsAtPoint--;
            transforms.Remove(otherTransform);
        }
    }
}

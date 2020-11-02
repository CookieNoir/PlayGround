using System.Collections;
using UnityEngine;
[AddComponentMenu("_Accessory/Default")]
public class Accessory : AttachableObject
{
    protected Example example;
    protected bool equipped;

    protected Vector3 fixedDistance;
    protected Quaternion fixedRotation;
    protected IEnumerator setFixedDistance;

    protected virtual void Awake()
    {
        rigidbodyHandler.centerOfMass = centerOfMass;
        rigidbodyHandler.mass = mass;

        fixedDistance = new Vector3(0.0005f, 0, 0);
        fixedRotation = Quaternion.Euler(0, -90, 0);
        setFixedDistance = SetFixedDistance();
    }

    protected virtual void Update()
    {
    }

    public virtual void FirstAction()
    {
    }

    public virtual void SecondAction()
    {
    }

    public virtual void Equip(Transform parent, Example ex)
    {
        transform.parent = parent;
        example = ex;
        LeaveHandler(example);

        StopCoroutine(setFixedDistance);
        setFixedDistance = SetFixedDistance();
        StartCoroutine(setFixedDistance);
    }

    public virtual void Unequip()
    {
        ReturnToHandler(example);

        example = null;
        equipped = false;
    }

    public void SetFixedDistanceAndRotation()
    {
        StopCoroutine(setFixedDistance);
        transform.localPosition = fixedDistance;
        transform.localRotation = fixedRotation;
        equipped = true;
    }

    protected IEnumerator SetFixedDistance()
    {
        Vector3 startPosition = transform.localPosition;
        Quaternion startRotation = transform.localRotation;
        for (float f = 0.1f; f <= 1; f += 0.1f)
        {
            transform.localPosition = Vector3.Lerp(startPosition, fixedDistance, f);
            transform.localRotation = Quaternion.Lerp(startRotation, fixedRotation, f);
            yield return new WaitForFixedUpdate();
        }        
        equipped = true;
    }
}

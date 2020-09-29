﻿using UnityEngine;

public class DestroyingArea : ReachablePoint
{
    private int requiredObjectsAmount;
    private Grab grab;

    public void SetRequiredObjectsAmount(int value) => requiredObjectsAmount = value;
    public void SetGrab(Grab grab) => this.grab = grab;

    public bool IsEnoughObjects()
    {
        if (objectsAtPoint >= requiredObjectsAmount)
            return true;
        else
            return false;
    }

    public override void ResetReached()
    {
        objectsAtPoint = 0;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (CheckForMatches(other))
        {
            AttachableObject ao = other.GetComponent<AttachableObject>();
            if (ao)
            {
                if (grab.GetAttachedObject() != ao)
                {
                    objectsAtPoint++;
                    Destroy(ao.gameObject);
                }
            }
            else
            {
                ao = other.transform.parent.GetComponent<AttachableObject>();
                if (ao)
                {
                    if (grab.GetAttachedObject() != ao)
                    {
                        objectsAtPoint++;
                        Destroy(ao.gameObject);
                    }
                }
            }
        }
    }

    protected override void OnTriggerExit(Collider other) { }
}

using UnityEngine;

public class AttachableObject : CenterOfMass
{
    public Rigidbody rigidbodyHandler;
    public GameObject antiSelfIntersectionBox;

    public void LeaveHandler(Example ex)
    {
        rigidbodyHandler.useGravity = false;
        rigidbodyHandler.constraints = RigidbodyConstraints.FreezeAll;
        if (antiSelfIntersectionBox) ex.ChangeAntiIntersectionBox(antiSelfIntersectionBox);
        ex.generalCenterOfMass.list.Add(this);
    }

    public void ReturnToHandler(Example ex)
    {
        rigidbodyHandler.transform.position = transform.position;
        rigidbodyHandler.transform.rotation = transform.rotation;
        transform.parent = rigidbodyHandler.gameObject.transform;
        rigidbodyHandler.useGravity = true;
        rigidbodyHandler.constraints = RigidbodyConstraints.None;
        if (antiSelfIntersectionBox) ex.SetDefaultAntiIntersectionBox();
        ex.generalCenterOfMass.list.Remove(this);
    }

    private void OnDestroy()
    {
        if (rigidbodyHandler) Destroy(rigidbodyHandler.gameObject);
    }
}

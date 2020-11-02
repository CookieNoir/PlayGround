using System.Collections.Generic;
using UnityEngine;
[AddComponentMenu("_Accessory/Grab")]
public class Grab : Accessory
{
    public GameObject leftClaw;
    public GameObject rightClaw;
    public float speed;
    public float topRotationBorderDegrees;
    public float lowRotationBorderDegrees;
    public List<AttachableObject> Left;
    public List<AttachableObject> Right;

    private bool attachedAttachableObject;
    private AttachableObject attachedObject;
    private float rot; // Угол вращения относительно оси Y
    private float attachedRot; // Угол, при котором произошел захват

    public AttachableObject GetAttachedObject()
    {
        return attachedObject;
    }

    protected override void Awake()
    {
        rigidbodyHandler.centerOfMass = centerOfMass;
        rigidbodyHandler.mass = mass;

        Left = new List<AttachableObject>();
        Right = new List<AttachableObject>();

        fixedDistance = new Vector3(0.0005f, 0, 0);
        fixedRotation = Quaternion.Euler(0, -90, 0);
        setFixedDistance = SetFixedDistance();

        attachedAttachableObject = false;
    }

    protected override void Update()
    {
        if (equipped&&!attachedAttachableObject)
        {
            foreach (AttachableObject rb_left in Left)
            {
                foreach (AttachableObject rb_right in Right)
                {
                    if (rb_left && rb_left == rb_right && rb_left != example.rb)
                    {
                        SetAttached(rb_left);

                        Left.Remove(rb_left);
                        Right.Remove(rb_right);
                        break;
                    }
                }
                if (attachedAttachableObject) break;
            }
        }
    }

    public float GetClawsRotation()
    {
        return rot;
    }

    public override void FirstAction()
    {
        if (!attachedAttachableObject)
        {
            rot += Time.deltaTime * speed;
            RotateClaws();
        }
    }

    public override void SecondAction()
    {
        rot -= Time.deltaTime * speed;
        RotateClaws();
        if (attachedAttachableObject)
        {
            if (rot + 0.5f < attachedRot || rot == lowRotationBorderDegrees)
                SetFree();
        }
    }

    private void RotateClaws()
    {
        if (rot > topRotationBorderDegrees) rot = topRotationBorderDegrees;
        else if (rot < lowRotationBorderDegrees) rot = lowRotationBorderDegrees;
        leftClaw.transform.localRotation = Quaternion.Euler(0, rot, 0);
        rightClaw.transform.localRotation = Quaternion.Euler(0, -rot, 0);
    }

    public void SetAttached(AttachableObject ao)
    {
        ao.transform.parent = transform;
        ao.LeaveHandler(example);
        attachedObject = ao;
        attachedAttachableObject = true;
        attachedRot = rot;
    }

    public void SetFree()
    {
        attachedObject.ReturnToHandler(example);
        attachedObject = null;
        attachedAttachableObject = false;
    }

    public override void Equip(Transform parent, Example ex)
    {
        transform.parent = parent;
        example = ex;
        LeaveHandler(example);

        StopCoroutine(setFixedDistance);
        setFixedDistance = SetFixedDistance();
        StartCoroutine(setFixedDistance);
    }

    public override void Unequip()
    {
        if (attachedAttachableObject) SetFree();
        ReturnToHandler(example);

        example = null;
        equipped = false;
    }
}

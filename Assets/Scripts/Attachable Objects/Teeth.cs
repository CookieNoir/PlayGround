using UnityEngine;

public class Teeth : MonoBehaviour
{
    public Grab grab;
    public bool isLeft;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Caterpillar")) return;
        AttachableObject ao = other.GetComponent<AttachableObject>();
        if (ao)
        {
            if (isLeft) grab.Left.Add(ao);
            else grab.Right.Add(ao);
        }
        else
        {
            if (other.transform.parent)
            {
                ao = other.transform.parent.GetComponent<AttachableObject>();
                if (ao)
                {
                    if (isLeft) grab.Left.Add(ao);
                    else grab.Right.Add(ao);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Caterpillar")) return;
        AttachableObject ao = other.GetComponent<AttachableObject>();
        if (ao)
        {
            if (isLeft) grab.Left.Remove(ao);
            else grab.Right.Remove(ao);
        }
        {
            if (other.transform.parent)
            {
                ao = other.transform.parent.GetComponent<AttachableObject>();
                if (ao)
                {
                    if (isLeft) grab.Left.Remove(ao);
                    else grab.Right.Remove(ao);
                }
            }
        }
    }

}

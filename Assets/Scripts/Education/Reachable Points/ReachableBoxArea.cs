using UnityEngine;
[RequireComponent(typeof(BoxCollider))]
public class ReachableBoxArea : ReachablePoint
{
    public Vector3 globalScale;
    public int requiredObjectsAmount;
    public int resultValue = 1;

    public int IsEnoughObjects()
    {
        if (objectsAtPoint >= requiredObjectsAmount)
            return resultValue;
        else
            return 0;
    }

    private void OnValidate()
    {
        transform.localScale = new Vector3(globalScale.x / transform.parent.transform.localScale.x,
                                            globalScale.y / transform.parent.transform.localScale.y,
                                            globalScale.z / transform.parent.transform.localScale.z);
    }
}

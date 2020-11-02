
using UnityEngine;

public class Arrow : Task
{
    // Start is called before the first frame update// Start is called before the first frame update
    public bool showInstructions = true;
    [Header("Объекты, связанные с задачей")]
    public Transform main;
    public GameObject armTemp;
    public Transform[] mainArm;
    private Transform[] tempArm = new Transform[6];
    private GameObject obj;
    int i = 1;

    protected override void EnableTaskGameObjects()
    {

        obj = Instantiate(armTemp);
        obj.transform.SetParent(main,false);
        tempArm[0] = armTemp.transform;
        i = 1;
        Child(armTemp.transform);
    }

    protected override void DisableTaskGameObjects()
    {
        Destroy(obj);
    }

    void Child(Transform parent)
    {

        foreach (Transform child in parent.transform)
        {
            if (child.tag == "Transform")
            {
                tempArm[i++] = child.transform;
                Child(child.transform);
            }
        }
    }


    protected override int Task_0()
    {
        // Transform transform = armMain.transform;
        bool[] t = new bool[7];
        for (int j = 0; j < 7; j++)
        {
            float dist1 = Vector3.Distance(mainArm[j].localPosition, tempArm[j].localPosition);
            float dist2 = Quaternion.Angle(mainArm[j].localRotation, tempArm[j].localRotation)/1000;

            if (dist1 + dist2  < 0.02f)
                t[j] = true;
            else
                t[j] = false;

        }
        bool tm = true;
        for (int j = 0; j < 7; j++)
            tm = tm && t[j];
        if (tm)
        {
            SetStage(1, Task_1, showInstructions);
            return 1;
        }
        return 0;
    }
    private int Task_1()
    {
        if (Input.GetKey(KeyCode.Escape))
            SetStage(1, EndTask, showInstructions);
        return 0;
    }


}

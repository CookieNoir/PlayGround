using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveArrow : Task
{

    public bool showInstructions = true;
    [Header("Объекты, связанные с задачей")]
    public Transform body;
    public PointOfInterest joinPoint;
    public GameObject armTemp;
    public Transform[] mainArm;
    private Transform[] tempArm = new Transform[7];
    private GameObject obj;
    private PointOfInterest joinpoint;
    int i = 1;

    protected override void EnableTaskGameObjects()
    {
        joinpoint = Instantiate(joinPoint);
        joinpoint.gameObject.SetActive(true);
        
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
    protected override void DisableTaskGameObjects()
    {
        Destroy(obj);
        Destroy(joinpoint);
    }


    protected override int Task_0() // Достичь  области
    {
        if (joinpoint.IsReached())
        {

            // добавление руки
            if (!obj)
            {
                obj = Instantiate(armTemp);
                obj.transform.SetParent(body, false);
                tempArm[0] = armTemp.transform;
                i = 1;
                Child(armTemp.transform);
            }
            joinpoint.gameObject.SetActive(false);
            SetStage(1, Task_1, false);
            return 1;
        }
        else return 0;
    }
    protected  int Task_1() // проверка на совпадение конфигураций
    {
        bool[] t = new bool[7];
        for (int j = 0; j < 7; j++)
        {
            float dist1 = Vector3.Distance(mainArm[j].localPosition, tempArm[j].localPosition);
            float dist2 = Quaternion.Angle(mainArm[j].localRotation, tempArm[j].localRotation) / 1000;

            if (dist1 + dist2 < 0.02f)
                t[j] = true;
            else
                t[j] = false;

        }
        bool tm = true;
        for (int j = 0; j < 7; j++)
            tm = tm && t[j];
        if (tm)
        {
            SetStage(2, Task_2, showInstructions);
            return 1;
        }
        return 0;
    }
    private int Task_2()
    {
        if (Input.GetKey(KeyCode.Escape))
            SetStage(3, EndTask, showInstructions);
        return 0;
    }

    // Update is called once per frame
}

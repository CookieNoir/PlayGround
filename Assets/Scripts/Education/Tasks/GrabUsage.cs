using UnityEngine;

public class GrabUsage : Task
{
    [Header("Объекты, связанные с задачей")]
    public Transform grabTransform;
    public Grab grab;
    public Transform grabDefaultPoint;
    public GameObject[] otherObjects;

    private float highestAngle;
    private float lowestAngle;

    protected override void EnableTaskGameObjects()
    {
        foreach (GameObject obj in otherObjects)
        {
            obj.SetActive(true);
        }
        grabTransform.gameObject.SetActive(true);
        grabTransform.transform.position = grabDefaultPoint.position;
        grabTransform.transform.rotation = grabDefaultPoint.rotation;
        highestAngle = grab.topRotationBorderDegrees - 0.5f;
        lowestAngle = grab.lowRotationBorderDegrees + 0.5f;
    }

    protected override void DisableTaskGameObjects()
    {
        foreach (GameObject obj in otherObjects)
        {
            obj.SetActive(false);
        }
        mainGameObject.UnequipAccessory();
        grabTransform.gameObject.SetActive(false);
    }

    protected override int Task_0() // Присоединить оборудование
    {
        if (mainGameObject.GetEquipped())
        {
            SetStage(1, Task_1, true);
            return 1;
        }
        else return 0;
    }

    private int Task_1() // Соединить клешни (максимальный угол Захвата)
    {
        if (grab.GetClawsRotation() >= highestAngle)
        {
            SetStage(2, Task_2, true);
            return 1;
        }
        else return 0;
    }

    private int Task_2() // Развести клешни на наибольший угол (минимальный угол Захвата)
    {
        if (grab.GetClawsRotation() <= lowestAngle)
        {
            SetStage(3, EndTask, false);
            return 1;
        }
        else return 0;
    }
}
